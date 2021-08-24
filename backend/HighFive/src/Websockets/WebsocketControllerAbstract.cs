using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Org.OpenAPITools.Attributes;
using Org.OpenAPITools.Models;

namespace src.Websockets
{
    [ApiController]
    public abstract class WebsocketControllerAbstract: ControllerBase
    {
        public static bool UploadVideo { get; set; }
        public static bool UploadImage { get; set; }
        public static bool AnalyzeVideo { get; set; }
        public static bool AnalyzeImage { get; set; }
        public static bool Close { get; set; }

        protected WebsocketControllerAbstract()
        {
            UploadImage = false;
            UploadVideo = false;
            AnalyzeVideo = false;
            AnalyzeImage = false;
            Close = false;
        }

        [HttpGet("/ws")]
        public abstract Task Get();
    }
}