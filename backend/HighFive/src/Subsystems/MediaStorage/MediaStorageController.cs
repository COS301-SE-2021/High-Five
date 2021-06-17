using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.OpenAPITools.Controllers;
using Org.OpenAPITools.Models;

namespace src.Subsystems.MediaStorage
{
    public class MediaStorageController : MediaStorageApiController
    {
        private IMediaStorageService _mediaStorageService;
        
        public MediaStorageController(IMediaStorageService mediaStorageService)
        {
            _mediaStorageService = mediaStorageService;
        }

        public override IActionResult GetAllVideos()
        {
            var result = _mediaStorageService.GetAllVideos();
            return StatusCode(200, result);
        }

        public override IActionResult GetVideo(GetVideoRequest getVideoRequest)
        {
            throw new NotImplementedException();
        }

        public override async Task<IActionResult> StoreVideo(IFormFile file)
        {
            StoreVideoResponse response = new StoreVideoResponse
            {
                Message = "Video stored successfully", Success = true
            };
            await _mediaStorageService.StoreVideo(file);
           return StatusCode(200, response);
        }
        
    }
}