using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.OpenAPITools.Controllers;
using Org.OpenAPITools.Models;

namespace src.Subsystems.Analysis
{
    [Authorize]
    public class AnalysisController: AnalysisApiController
    {
        private readonly IAnalysisService _analysisService;
        private bool _baseContainerSet;
        public AnalysisController(IAnalysisService analysisService)
        {
            _analysisService = analysisService;
            _baseContainerSet = false;
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
            _analysisService.SetBaseContainer(jsonToken.Subject);
            _baseContainerSet = true;
        }

        public override IActionResult AnalyzeImage(AnalyzeImageRequest analyzeImageRequest)
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            
            var response = _analysisService.AnalyzeImage(analyzeImageRequest).Result;
            if (response == null)
            {
                return StatusCode(400, null);
            }
            
            return StatusCode(200, response);
        }

        public override IActionResult AnalyzeVideo(AnalyzeVideoRequest analyzeVideoRequest)
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            
            var response = _analysisService.AnalyzeVideo(analyzeVideoRequest).Result;
            if (response == null)
            {
                return StatusCode(400, null);
            }
            
            return StatusCode(200, response);
        }
    }
}