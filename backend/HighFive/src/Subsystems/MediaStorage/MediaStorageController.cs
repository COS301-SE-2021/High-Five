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

        public override async Task<IActionResult> StoreVideo(IFormFile file)
        {
            StoreVideoResponse response = new StoreVideoResponse
            {
                Message = "Video stored successfully", Success = true
            };
           /* String filePath = Directory.GetCurrentDirectory() + "\\Subsystems\\MediaStorage\\Videos\\" + file.FileName;
            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }*/
           _mediaStorageService.StoreVideo(file);
           return StatusCode(200, response);
        }
        
        public override IActionResult RetrieveVideos()
        {
            List<RetrieveVideosResponse> responseList = new List<RetrieveVideosResponse>();
            
            return StatusCode(200, responseList.ToArray());
        }
    }
}