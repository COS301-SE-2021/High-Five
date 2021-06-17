using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Org.OpenAPITools.Models;
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
        
        public async Task StoreVideo(IFormFile video)
        {
            await _storageManager.UploadFile(video);
        }

        public void RetrieveVideo(string videoName)
        {
            throw new System.NotImplementedException();
        }

        public List<VideoMetaData> GetAllVideos()
        {
            return _storageManager.GetAllVideos().Result;
        }
    }
}