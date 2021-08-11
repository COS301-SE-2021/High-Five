﻿using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Org.OpenAPITools.Models;
using src.Storage;
using static System.String;

namespace src.Subsystems.MediaStorage
{
    public class MediaStorageService: IMediaStorageService
    {
        /*
         *      Description:
         * This service class manages all the service contracts of the MediaStorage subsystem. It is responsible
         * for retrieving, creating and deleting videos from a user's blob storage.
         *
         *      Attributes:
         * -> _storageManager: a reference to the storage manager, used to access the blob storage.
         * -> _containerName: the name of the container in which a user's videos are stored.
         */

        private readonly IStorageManager _storageManager;
        private const string VideoContainerName = "video";
        private const string ImageContainerName = "image";

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
             * -> video: the video that will be stored on the cloud storage.
             */

            if (video == null)
            {
                return;
            }
            //create storage name for file
            var generatedName = _storageManager.HashMd5(video.FileName);
            var videoBlob = _storageManager.CreateNewFile(generatedName + ".mp4", VideoContainerName).Result;
            var salt = "";
            while (videoBlob == null)
            {
                salt += _storageManager.RandomString();
                generatedName = _storageManager.HashMd5(video.FileName+salt);
                videoBlob = _storageManager.CreateNewFile(generatedName + ".mp4", VideoContainerName).Result;
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
            var thumbnailBlob = _storageManager.CreateNewFile(generatedName + "-thumbnail.jpg", VideoContainerName).Result;
            await thumbnailBlob.UploadFile(thumbnailPath);

            //get video duration in seconds
            //var seconds = Math.Truncate(inputFile.Metadata.Duration.TotalSeconds);
            var seconds = 0;
            videoBlob.AddMetadata("duration", seconds.ToString());

            //upload to Azure Blob Storage
            await videoBlob.UploadFile(video);
        }

        public List<VideoMetaData> GetAllVideos()
        {
            /*
             *      Description:
             * This function will return all videos that a user has stored in the cloud storage.
             */

            var allFiles = _storageManager.GetAllFilesInContainer(VideoContainerName).Result;
            if (allFiles == null)
            {
                return new List<VideoMetaData>();
            }
            var resultList = new List<VideoMetaData>();
            var currentVideo = new VideoMetaData();
            foreach(var listBlobItem in allFiles)//NOTE: Assuming here that a thumbnail will be immediately followed by its corresponding mp4 file
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
                    if (listBlobItem.Properties != null && listBlobItem.Properties.LastModified != null)
                        currentVideo.DateStored = listBlobItem.Properties.LastModified.Value.DateTime;
                    var oldName = listBlobItem.GetMetaData("originalName");
                    currentVideo.Name = oldName;
                    currentVideo.Url = listBlobItem.GetUrl();
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
             * -> request: the request object for this service contract.
             */

            var videoFile = _storageManager.GetFile(request.Id + ".mp4",VideoContainerName).Result;
            if (videoFile == null)
            {
                return false;
            }

            var thumbnail = _storageManager.GetFile(request.Id + "-thumbnail.jpg", VideoContainerName).Result;
            await videoFile.Delete();
            await thumbnail.Delete();
            return true;
        }

        public async Task StoreImage(IFormFile image)
        {
             /*
             *      Description:
             * This function will create a new blob file, containing the data from a provided image, and store
             * it to the cloud storage.
             *
             *      Parameters:
             * -> image: the image that will be stored on the cloud storage.
             */

            if (image == null)
            {
                return;
            }
            //create storage name for file
            var generatedName = _storageManager.HashMd5(image.FileName);
            var splitName = image.FileName.Split('.');
            if (splitName.Length < 2)
            {
                throw new InvalidDataException("No file extension provided.");
            }
            var extension = "." + splitName[1];
            if(!(extension.Equals(".jpg") || extension.Equals(".jpeg") || extension.Equals(".png")))
            {
                throw new InvalidDataException("Invalid extension provided.");
            }
            var imageBlob = _storageManager.CreateNewFile(generatedName + ".img", ImageContainerName).Result;
            var salt = "";
            while (imageBlob == null)
            {
                salt += _storageManager.RandomString();
                generatedName = _storageManager.HashMd5(image.FileName+salt);
                imageBlob = _storageManager.CreateNewFile(generatedName + ".img", ImageContainerName).Result;
            }

            imageBlob.AddMetadata("originalName", image.FileName);
            if (!IsNullOrEmpty(salt))
            {
                imageBlob.AddMetadata("salt", salt);
            }

            //upload to Azure Blob Storage
            await imageBlob.UploadFile(image);
        }

        public List<ImageMetaData> GetAllImages()
        {
            /*
             *      Description:
             * This function will return all images that a user has stored in the cloud storage.
             */

            var allFiles = _storageManager.GetAllFilesInContainer(ImageContainerName).Result;
            if (allFiles == null)
            {
                return new List<ImageMetaData>();
            }
            var resultList = new List<ImageMetaData>();
            foreach(var listBlobItem in allFiles)
            {
                var currentImage = new ImageMetaData {Id = listBlobItem.Name.Replace(".img", "")};
                if (listBlobItem.Properties is {LastModified: { }})
                    currentImage.DateStored = listBlobItem.Properties.LastModified.Value.DateTime;
                currentImage.Name = listBlobItem.GetMetaData("originalName");
                currentImage.Url = listBlobItem.GetUrl();
                resultList.Add(currentImage);
            }
            return resultList;
        }

        public async Task<bool> DeleteImage(DeleteImageRequest request)
        {
            /*
             *      Description:
             * This function will attempt to delete an image with details as passed through by the request object.
             * True is returned if the image was deleted successfully, false is returned if there exists no image
             * with the details specified in the request object.
             *
             *      Parameters:
             * -> request: the request object for this service contract.
             */

            var imageFile = _storageManager.GetFile(request.Id + ".img",ImageContainerName).Result;
            if (imageFile == null)
            {
                return false;
            }

            await imageFile.Delete();
            return true;
        }

        public void SetBaseContainer(string containerName)
        {
            /*
             *      Description:
             * This function tests if a baseContainer has been set yet, it will be called before any of the
             * other StorageManager method code executes. If a base container has already been set, this code
             * will do nothing, else it will set the base container to the user's Azure AD B2C unique object
             * id - hence pointing towards the user's own container within the storage.
             *
             *      Parameters:
             * -> containerName: the user's id that will be used as the container name.
             */
            if (!_storageManager.IsContainerSet())
            {
                _storageManager.SetBaseContainer(containerName);
            }
        }

    }
}
