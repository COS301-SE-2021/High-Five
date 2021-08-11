using System;
using Org.OpenAPITools.Models;
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
         * -> _mediaStorageService: service used to retrieve raw media and store analyzed media.
         * -> _pipelineService: service used to retrieve tools from a provided pipeline.
         */
        
        private readonly IMediaStorageService _mediaStorageService;
        private readonly IPipelineService _pipelineService;

        public AnalysisService(IStorageManager storageManager, IMediaStorageService mediaStorageService,
            IPipelineService pipelineService)
        {
            _mediaStorageService = mediaStorageService;
            _pipelineService = pipelineService;
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

            return request.MediaType switch
            {
                AnalyzeMediaRequest.MediaTypeEnum.ImageEnum => AnalyzeImage(request.MediaId, analysisPipeline),
                AnalyzeMediaRequest.MediaTypeEnum.VideoEnum => AnalyzeVideo(request.MediaId, analysisPipeline),
                _ => string.Empty //invalid media type provided
            };
        }
        
        private string AnalyzeImage(string imageId, Pipeline analysisPipeline)
        {
            /*
             *      Description:
             * This function will call the functions necessary to analyze an image belonging to imageId with
             * the analysis tools present within analysisPipeline.
             *
             *      Parameters:
             * -> imageId: the id of the image to be analyzed
             * -> analysisPipeline: the pipeline object containing the tools that will be applied to the image
             *      during the analysis phase.
             */
            
            throw new System.NotImplementedException();
        }

        private string AnalyzeVideo(string videoId ,Pipeline analysisPipeline)
        {
            /*
             *      Description:
             * This function will call the functions necessary to analyze a video belonging to videoId with
             * the analysis tools present within analysisPipeline.
             *
             *      Parameters:
             * -> videoId: the id of the video to be analyzed
             * -> analysisPipeline: the pipeline object containing the tools that will be applied to the video
             *      during the analysis phase.
             */
            
            throw new System.NotImplementedException();
        }
    }
}