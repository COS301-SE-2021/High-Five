using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Org.OpenAPITools.Models;

namespace src.Utils.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public abstract class GetVideoApiController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint for Get Videos use case</remarks>
        /// <param name="videoId"></param>
        /// <param name="getVideoRequest"></param>
        /// <response code="200">Video Data</response>
        /// <response code="400">Invalid video id provided</response>
        [HttpGet]
        [Route("/media/getVideo/{videoId}")]
        public abstract FileContentResult GetVideo([FromRoute(Name = "videoId")] [Required]
            string videoId);
    }
}