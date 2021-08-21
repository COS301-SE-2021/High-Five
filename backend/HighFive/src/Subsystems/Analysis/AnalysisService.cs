﻿using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Accord.IO;
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

        public async Task<AnalyzedImageMetaData> AnalyzeImage(AnalyzeImageRequest request)
        {
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
            
            var rawImage = _mediaStorageService.GetImage(request.ImageId);
            if (rawImage == null)
            {
                return null;//Invalid imageId provided
            }

            var rawImageByteArray = rawImage.ToByteArray().Result;

            //-----------------------------ANALYSIS IS DONE HERE HERE--------------------------------
            var analyser = new AnalyserImpl();
            analyser.StartAnalysis(analysisPipeline,_analysisModels);
            analyser.FeedFrame(rawImageByteArray);
            analyser.EndAnalysis();
            var analyzedImageData = analyser.GetFrames()[0][0];//TODO add functionality to save multiple images:analyser.GetFrames()[i][0]
            //---------------------------------------------------------------------------------------

            var analyzedFile = _storageManager.CreateNewFile(analyzedMediaName, storageContainer).Result;
            analyzedFile.AddMetadata("imageId", request.ImageId);
            analyzedFile.AddMetadata("pipelineId", request.PipelineId);
            const string contentType = "image/jpg";
            await analyzedFile.UploadFileFromByteArray(analyzedImageData, contentType);

            if (analyzedFile.Properties.LastModified != null)
                response.DateAnalyzed = analyzedFile.Properties.LastModified.Value.DateTime;
            response.Id = analyzedMediaName;
            response.Url = analyzedFile.GetUrl();
            return response;
        }

        public async Task<AnalyzedVideoMetaData> AnalyzeVideo(AnalyzeVideoRequest request)
        {
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
                response.Id = analyzedMediaName;
                response.Url = testFile.GetUrl();
                return response;
            }
            
            var rawVideo = _mediaStorageService.GetVideo(request.VideoId);
            if (rawVideo == null)
            {
                return null;//Invalid videoId provided
            }
            var rawVideoStream = rawVideo.ToStream().Result;
            var watch = new Stopwatch();
            watch.Reset();
            watch.Start();
            var frameList = _videoDecoder.GetFramesFromVideo(rawVideoStream);
            watch.Stop();
            Console.WriteLine("Convert video to frames: " + watch.ElapsedMilliseconds + "ms");

            //-----------------------------ANALYSIS IS DONE HERE HERE--------------------------------
            var analyser = new AnalyserImpl();
            watch.Reset();
            watch.Start();
            analyser.StartAnalysis(analysisPipeline,_analysisModels);
            foreach (var frameStream in frameList)
            {
                var bytes = ((MemoryStream) frameStream).ToArray();
                analyser.FeedFrame(bytes);
            }
            analyser.EndAnalysis();
            var analyzedFrameData = analyser.GetFrames()[0];
            watch.Stop();
            Console.WriteLine("From StartAnalysis to GetFrames: " + watch.ElapsedMilliseconds + "ms");
            //---------------------------------------------------------------------------------------

            rawVideoStream.Seek(0, SeekOrigin.Begin);
            watch.Reset();
            watch.Start();
            var analyzedVideoData = _videoDecoder.EncodeVideoFromFrames(analyzedFrameData, rawVideoStream);
            watch.Stop();
            Console.WriteLine("Convert from frames to video: " + watch.ElapsedMilliseconds + "ms");
            
            var analyzedFile = _storageManager.CreateNewFile(analyzedMediaName, storageContainer).Result;
            analyzedFile.AddMetadata("videoId", request.VideoId);
            analyzedFile.AddMetadata("pipelineId", request.PipelineId);
            const string contentType = "video/mp4";
            await analyzedFile.UploadFileFromByteArray(analyzedVideoData, contentType);

            if (analyzedFile.Properties.LastModified != null)
                response.DateAnalyzed = analyzedFile.Properties.LastModified.Value.DateTime;
            response.Id = analyzedFile.Name;
            response.Url = analyzedFile.GetUrl();
            return response;
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
