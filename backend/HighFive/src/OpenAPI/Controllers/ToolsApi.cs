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
    public abstract class ToolsApiController : ControllerBase
    { 
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Create Meta Data Type use case</remarks>
        /// <param name="name"></param>
        /// <param name="file"></param>
        /// <response code="200">All tool types have been returned</response>
        [HttpPost]
        [Route("/tools/createMetaDataType")]
        [Consumes("multipart/form-data")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(EmptyObject))]
        public abstract IActionResult CreateMetaDataType([FromForm (Name = "name")]string name, IFormFile file);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Delete Tool use case</remarks>
        /// <param name="deleteToolRequest"></param>
        /// <response code="200">The Tool has been deleted</response>
        [HttpPost]
        [Route("/tools/deleteTool")]
        [Consumes("application/json")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(EmptyObject))]
        public abstract IActionResult DeleteTool([FromBody]DeleteToolRequest deleteToolRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Get Meta Data Types use case</remarks>
        /// <response code="200">Returns all meta data types</response>
        [HttpGet]
        [Route("/tools/getMetaDataTypes")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(GetToolMetaDataTypes))]
        public abstract IActionResult GetMetaDataTypes();

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Get Tool Types use case</remarks>
        /// <response code="200">All tool types have been returned</response>
        [HttpGet]
        [Route("/tools/getToolTypes")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(GetToolTypesResponse))]
        public abstract IActionResult GetToolTypes();

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Delete Tool use case</remarks>
        /// <response code="200">All tools have been returned</response>
        [HttpGet]
        [Route("/tools/getAllTools")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(GetAllToolsResponse))]
        public abstract IActionResult GetTools();

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Upload Analysis Tool use case</remarks>
        /// <param name="sourceCode"></param>
        /// <param name="model"></param>
        /// <param name="metadataType"></param>
        /// <param name="toolName"></param>
        /// <response code="200">The Analysis Tool has been uploaded</response>
        [HttpPost]
        [Route("/tools/uploadAnalysisTool")]
        [Consumes("multipart/form-data")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(EmptyObject))]
        public abstract IActionResult UploadAnalysisTool(IFormFile sourceCode, IFormFile model, [FromForm (Name = "metadataType")]string metadataType, [FromForm (Name = "toolName")]string toolName);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Upload Drawing Tool use case</remarks>
        /// <param name="sourceCode"></param>
        /// <param name="metadataType"></param>
        /// <param name="toolName"></param>
        /// <response code="200">The Drawing Tool has been uploaded</response>
        [HttpPost]
        [Route("/tools/uploadDrawingTool")]
        [Consumes("multipart/form-data")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(EmptyObject))]
        public abstract IActionResult UploadDrawingTool(IFormFile sourceCode, [FromForm (Name = "metadataType")]string metadataType, [FromForm (Name = "toolName")]string toolName);
    }
}
