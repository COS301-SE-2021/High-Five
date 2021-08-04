﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.OpenAPITools.Controllers;
using Org.OpenAPITools.Models;

namespace src.Subsystems.MediaStorage
{
    [Authorize]
    public class MediaStorageController : MediaStorageApiController
    {
        private readonly IMediaStorageService _mediaStorageService;
        private bool _baseContainerSet;
        
        public MediaStorageController(IMediaStorageService mediaStorageService)
        {
            _mediaStorageService = mediaStorageService;
            _baseContainerSet = false;
        }

        public override IActionResult GetAllImages()
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            var result = _mediaStorageService.GetAllImages();
            return StatusCode(200, result);
        }

        public override IActionResult GetAllVideos()
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            var result = _mediaStorageService.GetAllVideos();
            return StatusCode(200, result);
        }

        public override async Task<IActionResult> StoreImage(IFormFile file)
         {
             if (!_baseContainerSet)
             {
                 ConfigureStorageManager();
             }
             try
             {
                 if (file == null)
                 {
                     var response400 = new EmptyObject() {Success = false, Message = "The uploaded file is null."};
                     return StatusCode(400, response400);
                 }

                 var response = new StoreVideoResponse
                 {
                     VideoId = "Image stored successfully", Success = true
                 };
                 await _mediaStorageService.StoreImage(file);
                 return StatusCode(200, response);
             }
             catch (Exception e)
             {
                 var response400 = new EmptyObject() {Success = false, Message = "Invalid format provided."};
                 return StatusCode(400, response400);
             }
         }

         public override async Task<IActionResult> StoreVideo(IFormFile file)
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            try
            {
                if (file == null)
                {
                    var response400 = new EmptyObject() {Success = false, Message = "The uploaded file is null."};
                    return StatusCode(400, response400);
                }

                var response = new StoreVideoResponse
                {
                    VideoId = "Video stored successfully.", Success = true
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
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            var response = new EmptyObject {Success = true};
            if (_mediaStorageService.DeleteImage(deleteImageRequest).Result) return StatusCode(200, response);
            response.Success = false;
            response.Message = "Video could not be deleted.";
            return StatusCode(400, response);
        }

        public override IActionResult DeleteVideo(DeleteVideoRequest deleteVideoRequest)
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            var response = new EmptyObject {Success = true};
            if (_mediaStorageService.DeleteVideo(deleteVideoRequest).Result) return StatusCode(200, response);
            response.Success = false;
            response.Message = "Video could not be deleted.";
            return StatusCode(400, response);
        }

        private void ConfigureStorageManager()
        {
            var tokenString = HttpContext.GetTokenAsync("access_token").Result;
            if (tokenString == null)    //this means a mock instance is currently being run (integration tests)
            {
                _mediaStorageService.SetBaseContainer("demo2"); // This line of code is for contingency's sake, to not break code still working on the old Storage system.
                //TODO: Remove above code when front-end is compatible with new storage structure.
                return;
            }
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = (JwtSecurityToken) handler.ReadToken(tokenString);
            _mediaStorageService.SetBaseContainer(jsonToken.Subject);
            _baseContainerSet = true;
        }

    }
}