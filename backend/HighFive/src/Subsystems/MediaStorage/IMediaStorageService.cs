using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Org.OpenAPITools.Models;

namespace src.Subsystems.MediaStorage
{
    public interface IMediaStorageService
    {
        public Task StoreVideo(IFormFile video);
        public void RetrieveVideo(String videoName);
        public List<VideoMetaData> GetAllVideos();
    }
}