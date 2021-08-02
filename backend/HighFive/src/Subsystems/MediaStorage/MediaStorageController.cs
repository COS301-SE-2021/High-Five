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
        private readonly IMediaStorageService _mediaStorageService;
        
        public MediaStorageController(IMediaStorageService mediaStorageService)
        {
            _mediaStorageService = mediaStorageService;
        }

        public override IActionResult GetAllImages()
        {
            throw new NotImplementedException();
        }

        public override IActionResult GetAllVideos()
        {
            var result = _mediaStorageService.GetAllVideos();
            return StatusCode(200, result);
        }

         public override IActionResult GetVideo(GetVideoRequest getVideoRequest)
         {
             var response = _mediaStorageService.GetVideo(getVideoRequest);
             if (response != null) return StatusCode(200, response);
            var fail = new EmptyObject
             {
                 Success = false,
                 Message = "No video exists associated with video id: " + getVideoRequest.Id
             };
             return StatusCode(400, fail);
         }

         public override IActionResult StoreImage(StoreImageRequest storeImageRequest)
         {
             throw new NotImplementedException();
         }

         public override async Task<IActionResult> StoreVideo(IFormFile file)
        {
            try
            {
                if (file == null)
                {
                    var response400 = new EmptyObject() {Success = false, Message = "The uploaded file is null."};
                    return StatusCode(400, response400);
                }

                var response = new StoreVideoResponse
                {
                    Message = "Video stored successfully", Success = true
                };
                await _mediaStorageService.StoreVideo(file);
                return StatusCode(200, response);
            }
            catch (Exception e)
            {
                var response500 = new EmptyObject() {Success = false, Message = e.ToString()};
                return StatusCode(500, response500);
            }
        }

        public override IActionResult DeleteImage(DeleteImageRequest deleteImageRequest)
        {
            throw new NotImplementedException();
        }

        public override IActionResult DeleteVideo(DeleteVideoRequest deleteVideoRequest)
        {
            var response = new EmptyObject {Success = true};
            if (_mediaStorageService.DeleteVideo(deleteVideoRequest).Result) return StatusCode(200, response);
            response.Success = false;
            response.Message = "Video could not be deleted.";
            return StatusCode(400, response);
        }

    }
}