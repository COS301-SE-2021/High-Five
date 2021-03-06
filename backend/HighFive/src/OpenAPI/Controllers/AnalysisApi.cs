/*
 * High Five
 *
 * The OpenAPI specification for High Five's controllers
 *
 * The version of the OpenAPI document: 0.0.1
 * 
 * Generated by: https://openapi-generator.tech
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Org.OpenAPITools.Attributes;
using Org.OpenAPITools.Models;

namespace Org.OpenAPITools.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public abstract class AnalysisApiController : ControllerBase
    { 
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Analyze Image use case</remarks>
        /// <param name="analyzeImageRequest"></param>
        /// <response code="200">A url of the analyzed media is returned</response>
        [HttpPost]
        [Route("/analysis/analyzeImage")]
        [Consumes("application/json")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(AnalyzedImageMetaData))]
        public abstract IActionResult AnalyzeImage([FromBody]AnalyzeImageRequest analyzeImageRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Analyze Video use case</remarks>
        /// <param name="analyzeVideoRequest"></param>
        /// <response code="200">A url of the analyzed media is returned</response>
        [HttpPost]
        [Route("/analysis/analyzeVideo")]
        [Consumes("application/json")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(AnalyzedVideoMetaData))]
        public abstract IActionResult AnalyzeVideo([FromBody]AnalyzeVideoRequest analyzeVideoRequest);
    }
}
