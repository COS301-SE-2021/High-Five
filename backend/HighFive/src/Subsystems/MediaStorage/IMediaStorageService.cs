using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Org.OpenAPITools.Models;

namespace src.Subsystems.MediaStorage
{
    public interface IMediaStorageService
    {
        public Task StoreVideo(IFormFile video);
        public List<VideoMetaData> GetAllVideos();
        public Task<bool> DeleteVideo(DeleteVideoRequest request);
        public Task StoreImage(IFormFile image);
        public List<ImageMetaData> GetAllImages();
        public Task<bool> DeleteImage(DeleteImageRequest request);
        public void SetBaseContainer(string containerName);
    }
}