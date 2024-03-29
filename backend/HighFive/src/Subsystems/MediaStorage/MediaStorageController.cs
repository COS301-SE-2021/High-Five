﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Org.OpenAPITools.Controllers;
using Org.OpenAPITools.Models;
using src.Websockets;

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
            
            var resultList = _mediaStorageService.GetAllImages();
            var result = new GetAllImagesResponse
            {
                Images = resultList
            };
            return StatusCode(200, result);
        }

        public override IActionResult GetAllVideos()
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            var resultList = _mediaStorageService.GetAllVideos();
            var result = new GetAllVideosResponse
            {
                Videos = resultList
            };
            return StatusCode(200, result);
        }

        public override IActionResult GetAnalyzedImages()
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            
            var resultList = _mediaStorageService.GetAnalyzedImages()?.Images;
            var result = new GetAnalyzedImagesResponse
            {
                Images = resultList
            };
            return StatusCode(200, result);

        }

        public override IActionResult GetAnalyzedVideos()
        {
            if (!_baseContainerSet)
            {
                ConfigureStorageManager();
            }
            var resultList = _mediaStorageService.GetAnalyzedVideos().Videos;
            var result = new GetAnalyzedVideosResponse
            {
                Videos = resultList
            };
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

                 var response = await _mediaStorageService.StoreImage(file);
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
                
                var response = await _mediaStorageService.StoreVideo(file);
                return StatusCode(200, response);
            }
            catch (Exception e)
            {
                var response500 = new EmptyObject() {Success = false, Message = e.ToString()};
                return StatusCode(500, response500);
            }
        }

         public override IActionResult DeleteAnalyzedImage(DeleteImageRequest deleteImageRequest)
         {
             if (!_baseContainerSet)
             {
                 ConfigureStorageManager();
             }
             var response = new EmptyObject {Success = true};
             if (_mediaStorageService.DeleteAnalyzedImage(deleteImageRequest).Result) return StatusCode(200, response);
             response.Success = false;
             response.Message = "Image could not be deleted.";
             return StatusCode(400, response);
         }

         public override IActionResult DeleteAnalyzedVideo(DeleteVideoRequest deleteVideoRequest)
         {
             if (!_baseContainerSet)
             {
                 ConfigureStorageManager();
             }
             var response = new EmptyObject {Success = true};
             if (_mediaStorageService.DeleteAnalyzedVideo(deleteVideoRequest).Result) return StatusCode(200, response);
             response.Success = false;
             response.Message = "Video could not be deleted.";
             return StatusCode(400, response);
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
                return;
            }
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = (JwtSecurityToken) handler.ReadToken(tokenString);
            var alreadyExisted = _mediaStorageService.SetBaseContainer(jsonToken.Subject);
            var id = jsonToken.Subject;
            var displayName = jsonToken.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
            var email = jsonToken.Claims.FirstOrDefault(x => x.Type == "emails")?.Value;
            _mediaStorageService.StoreUserInfo(id,displayName,email);
            _baseContainerSet = true;
        }

    }
}
