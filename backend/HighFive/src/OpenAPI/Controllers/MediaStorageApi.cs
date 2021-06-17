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
using System.Threading.Tasks;
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
    public abstract class MediaStorageApiController : ControllerBase
    { 
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Get Video Names use case</remarks>
        /// <response code="200">Returns a list of metadata objects of all the videos in the blob storage</response>
        [HttpPost]
        [Route("/media/getAllVideos")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(List<GetAllVideosResponse>))]
        public abstract IActionResult GetAllVideos();

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Get Videos use case</remarks>
        /// <param name="getVideoRequest"></param>
        /// <response code="200">Returns a list of the filenames of all videos</response>
        /// <response code="400">Invalid video id provided</response>
        [HttpPost]
        [Route("/media/getVideo")]
        [Consumes("application/json")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(GetVideoResponse))]
        [ProducesResponseType(statusCode: 400, type: typeof(EmptyObject))]
        public abstract IActionResult GetVideo([FromBody]GetVideoRequest getVideoRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Store Video use case</remarks>
        /// <param name="file"></param>
        /// <response code="200">Video has been stored</response>
        [HttpPost]
        [Route("/media/storeVideo")]
        [Consumes("multipart/form-data")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(StoreVideoResponse))]
        public abstract Task<IActionResult> StoreVideo(IFormFile file);
    }
}
