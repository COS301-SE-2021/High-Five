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
        private const string ContainerName = "demo2videos";
        private readonly Random _random;
        private readonly string _alphanumeric = "abcdefghijklmnopqrstuvwxyz0123456789";

        public MediaStorageService(IStorageManager storageManager)
        {
            _storageManager = storageManager;
            _random = new Random();
        }
        
        public async Task StoreVideo(IFormFile video)
        {
            //create storage name for file
            var generatedName = HashMd5(video.FileName);
            var cloudBlockBlob = _storageManager.CreateNewFile(generatedName + ".mp4", ContainerName).Result;
            var salt = "";
            while (cloudBlockBlob == null)
            {
                salt += RandomString();
                generatedName = HashMd5(video.FileName+salt);
                cloudBlockBlob = _storageManager.CreateNewFile(generatedName + ".mp4", ContainerName).Result;
            }
            cloudBlockBlob.Metadata.Add(new KeyValuePair<string, string>("originalName", video.FileName));
            if (!IsNullOrEmpty(salt))
            {
                cloudBlockBlob.Metadata.Add(new KeyValuePair<string, string>("salt", salt));
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
            var thumbnailBlockBlob = _storageManager.CreateNewFile(generatedName + "-thumbnail.jpg", ContainerName).Result;
            thumbnailBlockBlob.Properties.ContentType = "image/jpg";
            await thumbnailBlockBlob.UploadFromFileAsync(thumbnailPath);
                
            //get video duration in seconds
            //var seconds = Math.Truncate(inputFile.Metadata.Duration.TotalSeconds);
            var seconds = 0;
            cloudBlockBlob.Metadata.Add(new KeyValuePair<string, string>("duration", seconds.ToString()));

            //upload to Azure Blob Storage
            var ms = new MemoryStream();
            await video.CopyToAsync(ms);
            var fileBytes = ms.ToArray();
            cloudBlockBlob.Properties.ContentType = video.ContentType;
            await cloudBlockBlob.UploadFromByteArrayAsync(fileBytes, 0, (int) video.Length);
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
                    currentVideo.Duration = int.Parse(time ?? Empty);
                    listBlobItem.Metadata.TryGetValue("originalName", out var oldName);
                    currentVideo.Name = oldName;
                    resultList.Add(currentVideo); 
                }
            }
            return resultList;
        }
        
        private string HashMd5(string source)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(source);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
     
            // Step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private string RandomString()
        {
            var str = "";
            for(var i =0; i<5; i++)
            {
                var a = _random.Next(_alphanumeric.Length);
                str = str + _alphanumeric.ElementAt(a);
            }
            return str;
        }
    }
}