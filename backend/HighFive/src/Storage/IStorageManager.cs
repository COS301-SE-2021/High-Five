using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;

namespace src.Storage
{
    public interface IStorageManager
    {
        protected IConfiguration Configuration { get; }
        protected CloudStorageAccount CloudStorageAccount { get; }

        public Task UploadFile(IFormFile file);
        public void RetrieveVideo(String videoName);
        public void GetVideoStrings();
    }
}