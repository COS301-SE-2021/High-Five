using System;
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
        private readonly IStorageManager _storageManager;
        private const string ContainerName = "demo2videos";

        public MediaStorageService(IStorageManager storageManager)
        {
            _storageManager = storageManager;
        }
        
        public async Task StoreVideo(IFormFile video)
        {
            await _storageManager.UploadFile(video);
        }

        public async Task<GetVideoResponse> GetVideo(string videoId)
        {
            videoId += ".mp4";
            var file = _storageManager.GetFile(videoId, ContainerName).Result;
            if (file != null)
            {
                var videoFile = new byte[file.Properties.Length];
                for (int k = 0; k < file.Properties.Length; k++)
                {
                    videoFile[k] = 0x20;
                }
                await file.DownloadToByteArrayAsync(videoFile, 0);
                GetVideoResponse response = new GetVideoResponse {File = videoFile};
                return response;
            }
            //else cloudBlockBlob does not exist
            return null;
        }

        public async Task<List<VideoMetaData>> GetAllVideos()
        {
            var allFiles = _storageManager.GetAllFilesInContainer(ContainerName);
            if (allFiles.Result == null)
            {
                return null;
            }
            var resultList = new List<VideoMetaData>();
            var currentVideo = new VideoMetaData();
            foreach(var listBlobItem in allFiles.Result)//NOTE: Assuming here that a thumbnail will be immediately followed by its corresponding mp4 file
            {
                if (listBlobItem.Name.Contains("thumbnail"))
                {
                    currentVideo = new VideoMetaData();
                    var thumbnail = new byte[listBlobItem.Properties.Length];
                    for (var k = 0; k < listBlobItem.Properties.Length; k++)
                    {
                        thumbnail[k] = 0x20;
                    }
                    await listBlobItem.DownloadToByteArrayAsync(thumbnail, 0);
                    currentVideo.Thumbnail = thumbnail;
                }
                else
                {
                    currentVideo.Id = listBlobItem.Name.Replace(".mp4", "");
                    if (listBlobItem.Properties.LastModified != null)
                        currentVideo.DateStored = listBlobItem.Properties.LastModified.Value.DateTime;
                    listBlobItem.Metadata.TryGetValue("duration", out var time);
                    currentVideo.Duration = int.Parse(time ?? string.Empty);
                    listBlobItem.Metadata.TryGetValue("originalName", out var oldName);
                    currentVideo.Name = oldName;
                    resultList.Add(currentVideo); 
                }
            }
            return resultList;
        }
    }
}