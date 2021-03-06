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
    public abstract class DownloadsApiController : ControllerBase
    { 
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint to download the mobile app&#39;s apk</remarks>
        /// <response code="200">Returns an apk to install the mobile application</response>
        [HttpGet]
        [Route("/tools/downloadApk")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(FileDownload))]
        public abstract IActionResult DownloadApk();

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint to download the sdk&#39;s required files</remarks>
        /// <response code="200">Returns the sdk required files as .cs files.</response>
        [HttpGet]
        [Route("/tools/downloadSdkFiles")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(DownloadSdkFilesResponse))]
        public abstract IActionResult DownloadSdkFiles();

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Endpoint to download the sdk&#39;s user manual</remarks>
        /// <response code="200">Returns the sdk user manual</response>
        [HttpGet]
        [Route("/tools/downloadSdkManual")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(FileDownload))]
        public abstract IActionResult DownloadSdkManual();
    }
}
