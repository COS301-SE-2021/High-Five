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
        
        public override IActionResult AnalyzeMedia(AnalyzeMediaRequest analyzeMediaRequest)
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }

            var url = _analysisService.AnalyzeMedia(analyzeMediaRequest).Result;
            if (url.Equals(string.Empty))
            {
                return StatusCode(400, null);
            }

            var response = new AnalyzeMediaResponse {Url = url};
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
            _analysisService.SetBaseContainer(jsonToken.Subject);
            _baseContainerSet = true;
        }
    }
}