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
        public AnalysisController(IAnalysisService analysisService)
        {
            _analysisService = analysisService;
        }

        public override IActionResult AnalyzeImage(AnalyzeImageRequest analyzeImageRequest)
        {
            return StatusCode(501, null);
        }

        public override IActionResult AnalyzeVideo(AnalyzeVideoRequest analyzeVideoRequest)
        {
            return StatusCode(501, null);
        }
    }
}