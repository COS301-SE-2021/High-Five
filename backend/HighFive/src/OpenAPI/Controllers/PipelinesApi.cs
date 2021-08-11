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
    public abstract class PipelinesApiController : ControllerBase
    { 
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Add Tools use case</remarks>
        /// <param name="addToolsRequest"></param>
        /// <response code="200">Tools have been added to pipeline</response>
        [HttpPost]
        [Route("/pipelines/addTools")]
        [Consumes("application/json")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(EmptyObject))]
        public abstract IActionResult AddTools([FromBody]AddToolsRequest addToolsRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Create Pipeline use case</remarks>
        /// <param name="createPipelineRequest"></param>
        /// <response code="200">Pipeline has been created</response>
        [HttpPost]
        [Route("/pipelines/createPipeline")]
        [Consumes("application/json")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(CreatePipelineResponse))]
        public abstract IActionResult CreatePipeline([FromBody]CreatePipelineRequest createPipelineRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Delete Pipeline use case</remarks>
        /// <param name="deletePipelineRequest"></param>
        /// <response code="200">Tools have been removed from pipeline</response>
        [HttpPost]
        [Route("/pipelines/deletePipeline")]
        [Consumes("application/json")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(EmptyObject))]
        public abstract IActionResult DeletePipeline([FromBody]DeletePipelineRequest deletePipelineRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Get All Tools use case</remarks>
        /// <param name="deletePipelineRequest"></param>
        /// <response code="200">All existing tools have been returned</response>
        [HttpPost]
        [Route("/pipelines/getAllTools")]
        [Consumes("application/json")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(List<string>))]
        public abstract IActionResult GetAllTools();

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Get Pipeline use case</remarks>
        /// <param name="getPipelineRequest"></param>
        /// <response code="200">Returns a pipeline for a given Id</response>
        [HttpPost]
        [Route("/pipelines/getPipeline")]
        [Consumes("application/json")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(Pipeline))]
        public abstract IActionResult GetPipeline([FromBody]GetPipelineRequest getPipelineRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Get Pipeline Ids use case</remarks>
        /// <response code="200">All pipeline Id&#39;s have been returned</response>
        [HttpPost]
        [Route("/pipelines/getPipelineIds")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(GetPipelineIdsResponse))]
        public abstract IActionResult GetPipelineIds();

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Get Pipelines use case</remarks>
        /// <response code="200">All pipelines have been returned</response>
        [HttpPost]
        [Route("/pipelines/getPipelines")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(GetPipelinesResponse))]
        public abstract IActionResult GetPipelines();

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Remove Tools use case</remarks>
        /// <param name="removeToolsRequest"></param>
        /// <response code="200">Tools have been removed from pipeline</response>
        [HttpPost]
        [Route("/pipelines/removeTools")]
        [Consumes("application/json")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(EmptyObject))]
        public abstract IActionResult RemoveTools([FromBody]RemoveToolsRequest removeToolsRequest);
    }
}
