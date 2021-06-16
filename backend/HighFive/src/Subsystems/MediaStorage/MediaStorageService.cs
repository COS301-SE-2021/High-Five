using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using src.Storage;

namespace src.Subsystems.MediaStorage
{
    public class MediaStorageService: IMediaStorageService
    {
        private IStorageManager _storageManager;

        public MediaStorageService(IStorageManager storageManager)
        {
            _storageManager = storageManager;
        }
        
        public void StoreVideo(IFormFile video)
        {
            _storageManager.UploadVideo(video);
        }

        public void RetrieveVideo(string videoName)
        {
            throw new System.NotImplementedException();
        }

        public void GetVideoStrings()
        {
            throw new System.NotImplementedException();
        }
    }
}