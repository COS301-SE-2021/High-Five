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
        /// <remarks>Endpoint for Delete Analyzed Image use case</remarks>
        /// <param name="deleteImageRequest"></param>
        /// <response code="200">Deletes analyzed image</response>
        [HttpPost]
        [Route("/media/deleteAnalyzedImage")]
        [Consumes("application/json")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(EmptyObject))]
        public abstract IActionResult DeleteAnalyzedImage([FromBody]DeleteImageRequest deleteImageRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Delete Analyzed Video use case</remarks>
        /// <param name="deleteVideoRequest"></param>
        /// <response code="200">Deletes analyzed video</response>
        [HttpPost]
        [Route("/media/deleteAnalyzedVideo")]
        [Consumes("application/json")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(EmptyObject))]
        public abstract IActionResult DeleteAnalyzedVideo([FromBody]DeleteVideoRequest deleteVideoRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Delete Image use case</remarks>
        /// <param name="deleteImageRequest"></param>
        /// <response code="200">Image successfully deleted</response>
        [HttpPost]
        [Route("/media/deleteImage")]
        [Consumes("application/json")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(EmptyObject))]
        public abstract IActionResult DeleteImage([FromBody]DeleteImageRequest deleteImageRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Delete Video use case</remarks>
        /// <param name="deleteVideoRequest"></param>
        /// <response code="200">Video successfully deleted</response>
        [HttpPost]
        [Route("/media/deleteVideo")]
        [Consumes("application/json")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(EmptyObject))]
        public abstract IActionResult DeleteVideo([FromBody]DeleteVideoRequest deleteVideoRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Get All Images use case</remarks>
        /// <response code="200">Returns a list of images in the blob storage</response>
        [HttpGet]
        [Route("/media/getAllImages")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(GetAllImagesResponse))]
        public abstract IActionResult GetAllImages();

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Get All Videos use case</remarks>
        /// <response code="200">Returns a list of metadata objects of all the videos in the blob storage</response>
        [HttpGet]
        [Route("/media/getAllVideos")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(GetAllVideosResponse))]
        public abstract IActionResult GetAllVideos();

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Get Analyzed Images use case</remarks>
        /// <response code="200">All previously analyzed images are returned</response>
        [HttpGet]
        [Route("/media/getAnalyzedImages")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(GetAnalyzedImagesResponse))]
        public abstract IActionResult GetAnalyzedImages();

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Get Analyzed Videos use case</remarks>
        /// <response code="200">All previously analyzed videos are returned</response>
        [HttpGet]
        [Route("/media/getAnalyzedVideos")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(GetAnalyzedVideosResponse))]
        public abstract IActionResult GetAnalyzedVideos();

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Store Image use case</remarks>
        /// <param name="file"></param>
        /// <response code="200">Image has been stored</response>
        [HttpPost]
        [Route("/media/storeImage")]
        [Consumes("multipart/form-data")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(ImageMetaData))]
        public abstract Task<IActionResult> StoreImage(IFormFile file);

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
        [ProducesResponseType(statusCode: 200, type: typeof(VideoMetaData))]
        public abstract Task<IActionResult> StoreVideo(IFormFile file);
    }
}
