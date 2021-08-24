using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Org.OpenAPITools.Models;
using src.Storage;

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
        public bool SetBaseContainer(string containerName);
        public IBlobFile GetImage(string imageId);
        public IBlobFile GetVideo(string videoId);
        public GetAnalyzedImagesResponse GetAnalyzedImages();
        public GetAnalyzedVideosResponse GetAnalyzedVideos();
        public Task<bool> DeleteAnalyzedImage(DeleteImageRequest request);
        public Task<bool> DeleteAnalyzedVideo(DeleteVideoRequest request);
        public void StoreUserInfo(string id, string displayName, string email);
    }
}