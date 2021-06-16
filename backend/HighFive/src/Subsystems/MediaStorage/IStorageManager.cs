using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;

namespace src.Subsystems.MediaStorage
{
    public interface IStorageManager
    {
        protected IConfiguration Configuration { get; }
        protected CloudStorageAccount CloudStorageAccount { get; }

        public Task UploadVideo(IFormFile video);
        public void RetrieveVideo(String videoName);
        public void GetAllVideos();
    }
}