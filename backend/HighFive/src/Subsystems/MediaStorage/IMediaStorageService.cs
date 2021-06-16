using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace src.Subsystems.MediaStorage
{
    public interface IMediaStorageService
    {
        public Task StoreVideo(IFormFile video);
        public void RetrieveVideo(String videoName);
        public void GetVideoStrings();
    }
}