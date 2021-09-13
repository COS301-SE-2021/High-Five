using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AzureFunctionsToolkit.Portable.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Org.OpenAPITools.Models;
using src.Storage;
using src.Subsystems.Livestreaming;
using src.Subsystems.MediaStorage;
using src.Subsystems.Pipelines;
using src.Websockets;

namespace src.Subsystems.Analysis
{
    public class AnalysisService: IAnalysisService
    {
        /*
         *      Description:
         * This service class manages all the service contracts of the Analysis subsystem. It is responsible
         * for receiving a media- and pipeline Id and then analyzing the provided media with the tools present
         * in the provided pipeline.
         *
         *      Attributes:
         * -> _storageManager: a reference to the storage manager, used to access the blob storage.
         * -> _mediaStorageService: service used to retrieve raw media and store analyzed media.
         * -> _pipelineService: service used to retrieve tools from a provided pipeline.
         * -> _analysisModels: this singleton contains all the models/tools that will be used during analysis.
         */

        private readonly IStorageManager _storageManager;
        private readonly IMediaStorageService _mediaStorageService;
        private readonly IPipelineService _pipelineService;
        private readonly IConfiguration _configuration;
        private readonly IWebSocketClient _analysisSocket;
        private readonly ILivestreamingService _livestreamingService;
        private string _brokerToken;
        private string _userId;
        private bool _brokerConnection;

        public AnalysisService(IStorageManager storageManager, IMediaStorageService mediaStorageService,
            IPipelineService pipelineService, IConfiguration configuration, ILivestreamingService livestreamingService)
        {
            _storageManager = storageManager;
            _mediaStorageService = mediaStorageService;
            _livestreamingService = livestreamingService;
            _pipelineService = pipelineService;
            _configuration = configuration;
            _analysisSocket = new WebSocketClient();
            _brokerConnection = false;
        }

        public async Task<AnalyzedImageMetaData> AnalyzeImage(SocketRequest fullRequest)
        {
            var request = JsonConvert.DeserializeObject<AnalyzeImageRequest>(fullRequest.Body.Serialise());
            var pipelineSearchRequest = new GetPipelineRequest {PipelineId = request.PipelineId};
            var analysisPipeline = _pipelineService.GetPipeline(pipelineSearchRequest).Result;
            if (analysisPipeline == null)
            {
                return null; //invalid pipelineId provided
            }

            /* First, check if the Media and Pipeline combination has already been analyzed and stored before.
             * If this is the case, no analysis needs to be done. Simply return the already analyzed
             * media
             */
            
            analysisPipeline.Tools.Sort();
            const string storageContainer = "analyzed/image";
            const string fileExtension = ".img";
            var analyzedMediaName = _storageManager.HashMd5(request.ImageId + "|" + string.Join(",",analysisPipeline.Tools)) + fileExtension;
            var testFile = _storageManager.GetFile(analyzedMediaName, storageContainer).Result;
            var response = new AnalyzedImageMetaData
            {
                ImageId = request.ImageId,
                PipelineId = request.PipelineId
            };
            if (testFile != null) //This means the media has already been analyzed with this pipeline combination
            {
                if (testFile.Properties is {LastModified: { }})
                    response.DateAnalyzed = testFile.Properties.LastModified.Value.DateTime;
                response.Id = testFile.Name;
                response.Url = testFile.GetUrl();
                return response;
            }

            var brokerRequest = new BrokerSocketRequest(fullRequest, _userId) {Authorization = _brokerToken};
            await _analysisSocket.Send(JsonConvert.SerializeObject(brokerRequest));
            var responseString = _analysisSocket.Receive().Result;
            response = JsonConvert.DeserializeObject<AnalyzedImageMetaData>(responseString);

            return response;
        }

        public async Task<AnalyzedVideoMetaData> AnalyzeVideo(SocketRequest fullRequest)
        {
            var request = JsonConvert.DeserializeObject<AnalyzeVideoRequest>(fullRequest.Body.Serialise());
            var pipelineSearchRequest = new GetPipelineRequest {PipelineId = request.PipelineId};
            var analysisPipeline = _pipelineService.GetPipeline(pipelineSearchRequest).Result;
            if (analysisPipeline == null)
            {
                return null; //invalid pipelineId provided
            }

            /* First, check if the Media and Pipeline combination has already been analyzed and stored before.
             * If this is the case, no analysis needs to be done. Simply return the already analyzed
             * media
             */
            analysisPipeline.Tools.Sort();
            const string storageContainer = "analyzed/video";
            const string fileExtension = ".mp4";
            var analyzedMediaName = _storageManager.HashMd5(request.VideoId + "|" + string.Join(",",analysisPipeline.Tools)) + fileExtension;
            var testFile = _storageManager.GetFile(analyzedMediaName, storageContainer).Result;
            var response = new AnalyzedVideoMetaData
            {
                VideoId = request.VideoId,
                PipelineId = request.PipelineId
            };
            if (testFile != null) //This means the media has already been analyzed with this pipeline combination
            {
                if (testFile.Properties is {LastModified: { }})
                    response.DateAnalyzed = testFile.Properties.LastModified.Value.DateTime;
                response.Id = analyzedMediaName.Replace(fileExtension, "");
                response.Url = testFile.GetUrl();
                var thumbnailFile = _storageManager.GetFile(analyzedMediaName.Replace(".mp4", "-thumbnail.jpg"), storageContainer).Result;
                response.Thumbnail = thumbnailFile.GetUrl();
                return response;
            }
            
            var brokerRequest = new BrokerSocketRequest(fullRequest, _userId) {Authorization = _brokerToken};
            await _analysisSocket.Send(JsonConvert.SerializeObject(brokerRequest));
            var responseString = _analysisSocket.Receive().Result;
            Console.WriteLine("Response string: " +responseString);
            response = JsonConvert.DeserializeObject<AnalyzedVideoMetaData>(responseString);
            
            return response;
            
            /*rawVideoStream.Seek(0, SeekOrigin.Begin);
            watch.Reset();
            watch.Start();
            var analyzedVideoData = _videoDecoder.EncodeVideoFromFrames(analyzedFrameData, rawVideoStream);
            watch.Stop();
            Console.WriteLine("Convert from frames to video: " + watch.ElapsedMilliseconds + "ms");

            watch.Reset();
            watch.Start();
            //create thumbnail from analyzed video
            var thumbnail =
                _storageManager.CreateNewFile(analyzedMediaName.Replace(fileExtension, "") + "-thumbnail.jpg",
                    storageContainer).Result;
            await thumbnail.UploadFileFromByteArray(analyzedFrameData[0], "image/jpg");//uploads synchronously

            var analyzedFile = _storageManager.CreateNewFile(analyzedMediaName, storageContainer).Result;
            analyzedFile.AddMetadata("videoId", request.VideoId);
            analyzedFile.AddMetadata("pipelineId", request.PipelineId);
            const string contentType = "video/mp4";
            await analyzedFile.UploadFileFromByteArray(analyzedVideoData, contentType);
            watch.Stop();
            Console.WriteLine("Store video & thumbnail to cloud: " + watch.ElapsedMilliseconds + " ms");

            if (analyzedFile.Properties.LastModified != null)
                response.DateAnalyzed = analyzedFile.Properties.LastModified.Value.DateTime;
            response.Id = analyzedMediaName.Replace(fileExtension, "");
            response.Url = analyzedFile.GetUrl();
            response.Thumbnail = thumbnail.GetUrl();*/
        }

        public void SetBaseContainer(string containerName)
        {
            /*
             *      Description:
             * This function tests if a baseContainer has been set yet, it will be called before any of the
             * other StorageManager method code executes. If a base container has already been set, this code
             * will do nothing, else it will set the base container to the user's Azure AD B2C unique object
             * id - hence pointing towards the user's own container within the storage.
             *
             *      Parameters:
             * -> containerName: the user's id that will be used as the container name.
             */

            if (_storageManager.IsContainerSet()) return;

            _storageManager.SetBaseContainer(containerName);
            _pipelineService.SetBaseContainer(containerName);
            _mediaStorageService.SetBaseContainer(containerName);
        }

        public void SetBrokerToken(string userId)
        {
            _userId = userId;
            ConnectToBroker();
            var key = _configuration["BrokerJWTSecret"];
            const string issuer = "https://high5api.azurewebsites.net";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));    
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var permClaims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), new Claim("userId", userId)
            };

            var token = new JwtSecurityToken(issuer, //Issure    
                issuer,  //Audience    
                permClaims,    
                expires: DateTime.Now.AddDays(1),    
                signingCredentials: credentials);    
            _brokerToken = new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string ListenForMessage()
        {
            return _analysisSocket.Receive().Result;
        }

        public async Task StartLiveStream(string userId)
        {
            await _livestreamingService.AuthenticateUser();
            var appName = _livestreamingService.CreateApplication(userId).Result;
            await _livestreamingService.UpdateApplicationSettings(appName);
            var streamingId = _livestreamingService.CreateStreamingUrl(appName).Result;
            var publishToken = _livestreamingService.CreateOneTimeToken(appName, streamingId, "publish").Result;
            var playingToken = _livestreamingService.CreateOneTimeToken(appName, streamingId, "play").Result;

            var response = new LiveStreamingLinks
            {
                PublishLink = _configuration["LivestreamUri"].Replace("https", "rtmp") +
                              "/" + appName + "/" + streamingId + "?token=" + publishToken,
                PlayLink = _configuration["LivestreamUri"] + ":5443/" + appName + "/play.html?name=" +
                           streamingId + "&token=" + playingToken,
                StreamId = streamingId
            };
            
            var brokerRequest = new BrokerSocketRequest
            {
                Authorization = _brokerToken,
                UserId = _userId,
                Request = "StartLiveAnalysis",
                Body = response
            };
            await _analysisSocket.Send(JsonConvert.SerializeObject(brokerRequest));
        }

        private void ConnectToBroker()
        {
            if (!_brokerConnection)
            {
                _analysisSocket.Connect(_configuration["BrokerUri"], _userId);
                _brokerConnection = true;
            }
        }
        
    }
}
