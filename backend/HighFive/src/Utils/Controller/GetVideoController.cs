using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;
using Org.OpenAPITools.Models;
using src.Storage;
using src.Subsystems.MediaStorage;

namespace src.Utils.Controller
{
    public class GetVideoController : GetVideoApiController
    {
        private readonly IStorageManager _storageManager;
        private const string ContainerName = "demo2videos";
        
        public GetVideoController(IStorageManager storageManager)
        {
            _storageManager = storageManager;
        }
        
        
        public override FileContentResult GetVideo(string videoId)
        {
            var file = GetFileBlob(videoId).Result ?? Array.Empty<byte>();
            return File(file, "application/octet-stream");
            //byte[] decodedByteArray =Convert.FromBase64String (Encoding.ASCII.GetString (file));
            //return decodedByteArray;
        }

        private async Task<byte[]> GetFileBlob(string vidId)
        {
            var videoId = vidId + ".mp4";
            var file = _storageManager.GetFile(videoId, ContainerName).Result;
            var videoFile = file?.ToByteArray().Result;
            return videoFile;
        }
    }
}