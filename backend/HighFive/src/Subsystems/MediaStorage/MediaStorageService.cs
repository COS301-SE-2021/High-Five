using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
        
        public async Task StoreVideo(IFormFile video)
        {
            /*var filePath = Directory.GetCurrentDirectory() + "\\Subsystems\\MediaStorage\\Videos\\" + video.FileName;
            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await video.CopyToAsync(stream);
            }*/
            await _storageManager.UploadFile(video);
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