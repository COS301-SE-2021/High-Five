using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace src.Subsystems.MediaStorage
{
    public interface IMediaStorageService
    {
        public void StoreVideo(IFormFile video);
        public void RetrieveVideo(String videoName);
        public void GetVideoStrings();
    }
}