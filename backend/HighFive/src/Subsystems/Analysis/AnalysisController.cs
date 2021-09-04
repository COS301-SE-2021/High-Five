using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.OpenAPITools.Controllers;
using Org.OpenAPITools.Models;
using src.Websockets;

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
            
            return StatusCode(200, null);
        }

        public override IActionResult AnalyzeVideo(AnalyzeVideoRequest analyzeVideoRequest)
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }

            return StatusCode(200, null);
        }

        public override IActionResult GetLiveAnalysisToken()
        {
            var tokenString = HttpContext.GetTokenAsync("access_token").Result;
            if (tokenString == null)    //this means a mock instance is currently being run (integration tests)
            {
                return StatusCode(200, null);
            }
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = (JwtSecurityToken) handler.ReadToken(tokenString);
            var response = _analysisService.GetLiveAnalysisToken(jsonToken.Subject);
            return StatusCode(200, response);
        }
    }
}