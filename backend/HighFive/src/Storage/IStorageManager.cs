using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Org.OpenAPITools.Models;

namespace src.Storage
{
    public interface IStorageManager
    {
        protected IConfiguration Configuration { get; }
        protected CloudStorageAccount CloudStorageAccount { get; }

        public Task UploadFile(IFormFile file);
        public Task<List<VideoMetaData>> GetAllVideos();
        public Task<GetVideoResponse> GetVideo(string videoId);
    }
}