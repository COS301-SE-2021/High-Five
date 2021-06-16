using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.OpenAPITools.Controllers;
using Org.OpenAPITools.Models;

namespace src.Subsystems.MediaStorage
{
    [ApiController]
    public class MediaStorageController : MediaStorageApiController
    {
        public override IActionResult StoreVideo(IFormFile file)
        {
            StoreVideoResponse Response = new StoreVideoResponse();
            Response.Message = "Video stored successfully";
            Response.Success = true;
            return StatusCode(200, Response);
        }
    }
}