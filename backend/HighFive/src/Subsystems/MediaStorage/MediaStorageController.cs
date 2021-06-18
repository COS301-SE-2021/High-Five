﻿using System;
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
        
    }
}