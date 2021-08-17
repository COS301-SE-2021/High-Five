using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Org.OpenAPITools.Models;
using src.AnalysisTools;
using src.AnalysisTools.AnalysisThread;
using src.AnalysisTools.VideoDecoder;
using src.Storage;
using src.Subsystems.MediaStorage;
using src.Subsystems.Pipelines;

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
        private readonly IAnalysisModels _analysisModels;
        private readonly IVideoDecoder _videoDecoder;

        public AnalysisService(IStorageManager storageManager, IMediaStorageService mediaStorageService,
            IPipelineService pipelineService, IAnalysisModels analysisModels, IVideoDecoder videoDecoder)
        {
            _storageManager = storageManager;
            _mediaStorageService = mediaStorageService;
            _pipelineService = pipelineService;
            _analysisModels = analysisModels;
            _videoDecoder = videoDecoder;
        }
        
        public string AnalyzeMedia(AnalyzeMediaRequest request)
        {
            /*
             *      Description:
             * This function will determine the type of media that needs to be analysed and call the
             * corresponding helper function to perform the analysis, but not before the provided
             * analysis pipeline will be searched for and saved for the forthcoming analysis.
             *
             *      Parameters:
             * -> request: the request object for this function containing all the necessary id's.
             */
            
            var pipelineSearchRequest = new GetPipelineRequest {PipelineId = request.PipelineId};
            var analysisPipeline = _pipelineService.GetPipeline(pipelineSearchRequest).Result;
            if (analysisPipeline == null)
            {
                return string.Empty; //invalid pipelineId provided
            }
            
            /* First, check if the Media and Pipeline combination has already been analyzed and stored before.
             * If this is the case, no analysis needs to be done. Simply return the url of the already analyzed
             * media
             */
            analysisPipeline.Tools.Sort();
            var storageContainer = "analyzed/" + request.MediaType;
            var fileExtension = request.MediaType switch
            {
                "image" => ".img",
                "video" => ".mp4",
                _ => string.Empty
            };
            var analyzedMediaName = _storageManager.HashMd5(request.MediaId + "|" + analysisPipeline.Tools) + fileExtension;
            var testFile = _storageManager.GetFile(analyzedMediaName, storageContainer).Result;
            if (testFile != null) //This means the media has already been analyzed with this pipeline combination
            {
                return testFile.GetUrl();
            }
            
            //else this media and tool combination has not yet been analyzed. Proceed to the analysis phase.
            
            var analyzedMediaTemporaryLocation = string.Empty;
            switch (request.MediaType)
            {
                case "image":
                    analyzedMediaTemporaryLocation = AnalyzeImage(request.MediaId, analysisPipeline);
                    break;
                case "video":
                    analyzedMediaTemporaryLocation = AnalyzeVideo(request.MediaId, analysisPipeline);
                    break;
            }

            var analyzedFile = _storageManager.CreateNewFile(analyzedMediaName, storageContainer).Result;
            analyzedFile.AddMetadata("mediaId", request.MediaId);
            analyzedFile.AddMetadata("pipelineId", request.PipelineId);
            analyzedFile.UploadFile(analyzedMediaTemporaryLocation);
            return analyzedFile.GetUrl();
        }

        private string AnalyzeImage(string imageId, Pipeline analysisPipeline)
        {
            /*
             *      Description:
             * This function will call the functions necessary to analyze an image belonging to imageId with
             * the analysis tools present within analysisPipeline.
             * A temporary file will be created in local storage containing the contents of the analyzed
             * image, the path to this file will be returned.
             *
             *      Parameters:
             * -> imageId: the id of the image to be analyzed
             * -> analysisPipeline: the pipeline object containing the tools that will be applied to the image
             *      during the analysis phase.
             */
            
            var rawImage = _mediaStorageService.GetImage(imageId);
            var rawImageByteArray = rawImage.ToByteArray().Result;
            
            
            //-----------------------------ANALYSIS IS DONE HERE HERE--------------------------------
            var analyser = new AnalyserImpl();
            analyser.StartAnalysis(analysisPipeline,_analysisModels);
            analyser.FeedFrame(rawImageByteArray);
            analyser.EndAnalysis();
            var analyzedImageData = analyser.GetFrames()[0][0];//TODO add functionality to save multiple images:analyser.GetFrames()[i][0]
            //---------------------------------------------------------------------------------------

            var oldName = rawImage.GetMetaData("originalName");
            var tempDirectory = Path.GetTempPath() + "\\analyzed" + oldName;
            File.WriteAllBytes(tempDirectory, analyzedImageData);
            return tempDirectory;
        }

        private string AnalyzeVideo(string videoId ,Pipeline analysisPipeline)
        {
            /*
             *      Description:
             * This function will call the functions necessary to analyze a video belonging to videoId with
             * the analysis tools present within analysisPipeline.
             * A temporary file will be created in local storage containing the contents of the analyzed
             * video, the path to this file will be returned.
             * 
             *      Parameters:
             * -> videoId: the id of the video to be analyzed
             * -> analysisPipeline: the pipeline object containing the tools that will be applied to the video
             *      during the analysis phase.
             */

            var rawVideo = _mediaStorageService.GetVideo(videoId);
            var rawVideoStream = rawVideo.ToStream().Result;
            var frameList = _videoDecoder.GetFramesFromVideo(rawVideoStream);

            //-----------------------------ANALYSIS IS DONE HERE HERE--------------------------------
            var analyser = new AnalyserImpl();
            analyser.StartAnalysis(analysisPipeline,_analysisModels);
            foreach (var frameBytes in frameList)
            {
                analyser.FeedFrame(frameBytes);
            }
            analyser.EndAnalysis();
            var analyzedFrameData = analyser.GetFrames()[0]; //TODO add functionality to save multiple images:analyser.GetFrames()[i][0]

            //---------------------------------------------------------------------------------------


            throw new System.NotImplementedException();
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
    }
}