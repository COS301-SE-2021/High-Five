using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Org.OpenAPITools.Models;

namespace src.Subsystems.MediaStorage
{
    public interface IMediaStorageService
    {
        public Task StoreVideo(IFormFile video);
        public GetVideoResponse GetVideo(GetVideoRequest request);
        public List<VideoMetaData> GetAllVideos();
        public Task<bool> DeleteVideo(DeleteVideoRequest request);
    }
}