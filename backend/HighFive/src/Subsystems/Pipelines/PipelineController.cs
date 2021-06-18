using Microsoft.AspNetCore.Mvc;
using Org.OpenAPITools.Controllers;
using Org.OpenAPITools.Models;
using src.Subsystems.MediaStorage;

namespace src.Subsystems.Pipelines
{
    public class PipelineController: PipelinesApiController
    {
        private readonly IMediaStorageService _mediaStorageService;
        
        public PipelineController(IMediaStorageService mediaStorageService)
        {
            _mediaStorageService = mediaStorageService;
        }
        
        public override IActionResult AddTools(AddToolsRequest addToolsRequest)
        {
            throw new System.NotImplementedException();
        }

        public override IActionResult CreatePipeline(CreatePipelineRequest createPipelineRequest)
        {
            throw new System.NotImplementedException();
        }

        public override IActionResult DeletePipeline(DeletePipelineRequest deletePipelineRequest)
        {
            throw new System.NotImplementedException();
        }

        public override IActionResult GetPipelines()
        {
            throw new System.NotImplementedException();
        }

        public override IActionResult RemoveTools(RemoveToolsRequest removeToolsRequest)
        {
            throw new System.NotImplementedException();
        }
    }
}