using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.OpenAPITools.Controllers;
using Org.OpenAPITools.Models;

namespace src.Subsystems.MediaStorage
{
    public class MediaStorageController : MediaStorageApiController
    {
        public override IActionResult StoreVideo(IFormFile file)
        {
            StoreVideoResponse Response = new StoreVideoResponse();
            Response.Message = "Video stored successfully";
            Response.Success = true;
            return StatusCode(200, Response);
        }
        
        public override IActionResult RetrieveVideos()
        {
            List<RetrieveVideosResponse> responseList = new List<RetrieveVideosResponse>();
            
            return StatusCode(200, responseList.ToArray());
        }
    }
}