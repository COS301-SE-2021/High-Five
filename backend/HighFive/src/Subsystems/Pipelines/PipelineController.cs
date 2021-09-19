using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.OpenAPITools.Controllers;
using Org.OpenAPITools.Models;

namespace src.Subsystems.Pipelines
{
    [Authorize]
    public class PipelineController: PipelinesApiController
    {
        private readonly IPipelineService _pipelineService;
        private bool _baseContainerSet;

        public PipelineController(IPipelineService pipelineService)
        {
            _pipelineService = pipelineService;
            _baseContainerSet = false;
        }

        public override IActionResult AddTools(AddToolsRequest addToolsRequest)
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            var response = new EmptyObject {Success = true};
            if (_pipelineService.AddTools(addToolsRequest).Result)
            {
                return StatusCode(200, response);
            }

            response.Success = false;
            response.Message = "Addition of tools to pipeline failed";
            return StatusCode(400, response);
        }

        public override IActionResult CreatePipeline(CreatePipelineRequest createPipelineRequest)
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            var response = _pipelineService.CreatePipeline(createPipelineRequest).Result;
            return StatusCode(200, response);
        }

        public override IActionResult DeletePipeline(DeletePipelineRequest deletePipelineRequest)
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            var response = new EmptyObject() {Success = true};
            if (_pipelineService.DeletePipeline(deletePipelineRequest).Result) return StatusCode(200, response);
            response.Success = false;
            response.Message = "Pipeline could not be deleted";
            return StatusCode(400, response);
        }

        public override IActionResult GetAllTools()
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            var response = _pipelineService.GetAllTools();
            return StatusCode(200, response);
        }

        public override IActionResult GetLivePipeline()
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }

            var response = _pipelineService.GetLivePipeline().Result;
            return StatusCode(200, response);
        }

        public override IActionResult GetPipeline(GetPipelineRequest request)
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            var response = _pipelineService.GetPipeline(request).Result;
            return response == null ? StatusCode(404, null) : StatusCode(200, response);
        }

        public override IActionResult GetPipelineIds()
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            var response = _pipelineService.GetPipelineIds();
            return StatusCode(200, response);
        }

        public override IActionResult GetPipelines()
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            var response = _pipelineService.GetPipelines();
            return StatusCode(200, response);
        }

        public override IActionResult RemoveTools(RemoveToolsRequest removeToolsRequest)
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            var response = new EmptyObject {Success = true};
            if (_pipelineService.RemoveTools(removeToolsRequest).Result)
            {
                return StatusCode(200, response);
            }

            response.Success = false;
            response.Message = "Removal of tools from pipeline failed";
            return StatusCode(400, response);
        }

        public override IActionResult SetLivePipeline(GetPipelineRequest getPipelineRequest)
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            var pipelineSet = _pipelineService.SetLivePipeline(getPipelineRequest).Result;
            var response = new EmptyObject
            {
                Success = pipelineSet,
                Message = "Live pipeline set."
            };
            return StatusCode(200, response);
        }

        private void ConfigureStorageManager()
        {
            var tokenString = HttpContext.GetTokenAsync("access_token").Result;
            if (tokenString == null)    //this means a mock instance is currently being run (integration tests)
            {
                return;
            }
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = (JwtSecurityToken) handler.ReadToken(tokenString);
            var alreadyExisted = _pipelineService.SetBaseContainer(jsonToken.Subject);
            var id = jsonToken.Subject;
            var displayName = jsonToken.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
            var email = jsonToken.Claims.FirstOrDefault(x => x.Type == "emails")?.Value;
            _pipelineService.StoreUserInfo(id,displayName,email);
            _baseContainerSet = true;
        }
    }
}
