using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Org.OpenAPITools.Models;
using src.Storage;
using static System.String;

namespace src.Subsystems.MediaStorage
{
    public class MediaStorageService: IMediaStorageService
    {
        private readonly IStorageManager _storageManager;
        private string _containerName = "demo2videos";

        public MediaStorageService(IStorageManager storageManager)
        {
            _storageManager = storageManager;
        }
        
        public async Task StoreVideo(IFormFile video)
        {
            /*
             *      Description:
             * This function will create a new blob file, containing the data from a provided video, and store
             * it to the cloud storage.
             *
             *      Parameters:
             * -> video - the video that will be stored on the cloud storage.
             */
            
            if (video == null)
            {
                return;
            }
            //create storage name for file
            var generatedName = _storageManager.HashMd5(video.FileName);
            var videoBlob = _storageManager.CreateNewFile(generatedName + ".mp4", _containerName).Result;
            var salt = "";
            while (videoBlob == null)
            {
                salt += _storageManager.RandomString();
                generatedName = _storageManager.HashMd5(video.FileName+salt);
                videoBlob = _storageManager.CreateNewFile(generatedName + ".mp4", _containerName).Result;
            }

            videoBlob.AddMetadata("originalName", video.FileName);
            if (!IsNullOrEmpty(salt))
            {
                videoBlob.AddMetadata("salt", salt);
            }

            //create local temp copy of video file and thumbnail
            //var baseDirectory = "d:\\local\\";
            var baseDirectory = Directory.GetCurrentDirectory() + "\\";
            var thumbnailPath = baseDirectory + "thumbnail.jpg";
            var videoPath = baseDirectory + video.Name;
            await using var stream = new FileStream(videoPath, FileMode.Create);
            await video.CopyToAsync(stream);

            //get video thumbnail and store as separate blob
            /*var inputFile = new MediaFile {Filename = videoPath};
            var thumbnail = new MediaFile {Filename = thumbnailPath};
            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);
                var options = new ConversionOptions {Seek = TimeSpan.FromSeconds(1), VideoSize = VideoSize.Cif};
                engine.GetThumbnail(inputFile, thumbnail, options);
            }*/
            if (!File.Exists(thumbnailPath))
            {
                File.Create(thumbnailPath).Close();
            }
            var thumbnailBlob = _storageManager.CreateNewFile(generatedName + "-thumbnail.jpg", _containerName).Result;
            await thumbnailBlob.UploadFile(thumbnailPath);
                
            //get video duration in seconds
            //var seconds = Math.Truncate(inputFile.Metadata.Duration.TotalSeconds);
            var seconds = 0;
            videoBlob.AddMetadata("duration", seconds.ToString());

            //upload to Azure Blob Storage
            await videoBlob.UploadFile(video);
        }

        public async Task<GetVideoResponse> GetVideo(GetVideoRequest request)
        {
            /*
             *      Description:
             * This function will attempt to retrieve a video from blob storage and return the video if it
             * exists, null otherwise.
             *
             *      Parameters:
             * -> request - the request object for this service contract.
             */
            
            var videoId = request.Id + ".mp4";
            var file = _storageManager.GetFile(videoId, _containerName).Result;
            if (file == null) return null;
            var videoFile = file.ToByteArray().Result;
            var response = new GetVideoResponse {File = videoFile};
            return response;
            //else cloudBlockBlob does not exist
        }

        public async Task<List<VideoMetaData>> GetAllVideos()
        {
            /*
             *      Description:
             * This function will return all videos that a user has stored in the cloud storage.
             */
            
            var allFiles = _storageManager.GetAllFilesInContainer(_containerName);
            if (allFiles.Result == null)
            {
                return new List<VideoMetaData>();
            }
            var resultList = new List<VideoMetaData>();
            var currentVideo = new VideoMetaData();
            foreach(var listBlobItem in allFiles.Result)//NOTE: Assuming here that a thumbnail will be immediately followed by its corresponding mp4 file
            {
                if (listBlobItem.Name.Contains("thumbnail"))
                {
                    currentVideo = new VideoMetaData();
                    var thumbnail = listBlobItem.ToByteArray().Result;
                    currentVideo.Thumbnail = thumbnail;
                }
                else
                {
                    currentVideo.Id = listBlobItem.Name.Replace(".mp4", "");
                    if (listBlobItem.Properties.LastModified != null)
                        currentVideo.DateStored = listBlobItem.Properties.LastModified.Value.DateTime;
                    var time = listBlobItem.GetMetaData("duration");
                    currentVideo.Duration = int.Parse(time ?? Empty);
                    var oldName = listBlobItem.GetMetaData("originalName");
                    currentVideo.Name = oldName;
                    resultList.Add(currentVideo); 
                }
            }
            return resultList;
        }

        public async Task<bool> DeleteVideo(DeleteVideoRequest request)
        {
            /*
             *      Description:
             * This function will attempt to delete a video with details as passed through by the request object.
             * True is returned if the video was deleted successfully, false is returned if there exists no video
             * with the details specified in the request object.
             *
             *      Parameters:
             * -> request - the request object for this service contract.
             */
            
            var videoFile = _storageManager.GetFile(request.Id + ".mp4",_containerName).Result;
            if (videoFile == null)
            {
                return false;
            }

            var thumbnail = _storageManager.GetFile(request.Id + "-thumbnail.jpg", _containerName).Result;
            await videoFile.Delete();
            await thumbnail.Delete();
            return true;
        }

        public void SetContainer(string container)
        {
            _containerName = container;
        }
    }
}