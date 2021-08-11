using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.OpenAPITools.Controllers;
using Org.OpenAPITools.Models;

namespace src.Subsystems.Analysis
{
    [Authorize]
    public class AnalysisController: AnalysisApiController
    {
        public override IActionResult AnalyzeMedia(AnalyzeMediaRequest analyzeMediaRequest)
        {
            throw new System.NotImplementedException();
        }
    }
}