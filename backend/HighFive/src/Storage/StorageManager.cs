﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Org.OpenAPITools.Models;
using static System.String;

namespace src.Storage
{
    public class StorageManager: IStorageManager
    {
        private readonly IConfiguration _configuration;
        private readonly CloudStorageAccount _cloudStorageAccount;
        IConfiguration IStorageManager.Configuration => _configuration;
        CloudStorageAccount IStorageManager.CloudStorageAccount => _cloudStorageAccount;
        private Random _random;
        private readonly string _alphanumeric = "abcdefghijklmnopqrstuvwxyz0123456789";

        public StorageManager(IConfiguration config)
        {
            _configuration = config;
            String connectionString = _configuration.GetConnectionString("StorageConnection");
            _cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            _random = new Random();
        }

        public async Task UploadFile(IFormFile file)
        {
            var cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference("demo2videos");
            if (await cloudBlobContainer.CreateIfNotExistsAsync())
            {
                await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions()
                    {PublicAccess = BlobContainerPublicAccessType.Off});
            }
            
            //create storage name for file
            var generatedName = HashMd5(file.FileName);
            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(generatedName + ".mp4");
            var salt = "";
            while (await cloudBlockBlob.ExistsAsync())
            {
                salt += RandomString();
                generatedName = HashMd5(file.FileName+salt + ".mp4");
                cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(generatedName);
            }
            cloudBlockBlob.Metadata.Add(new KeyValuePair<string, string>("originalName", file.FileName));
            if (!IsNullOrEmpty(salt))
            {
                cloudBlockBlob.Metadata.Add(new KeyValuePair<string, string>("salt", salt));
            }
            
            //create local temp copy of video file and thumbnail
            var thumbnailPath = Directory.GetCurrentDirectory() + "\\Subsystems\\MediaStorage\\Videos\\thumbnail.jpg";
            var videoPath = Directory.GetCurrentDirectory() + "\\Subsystems\\MediaStorage\\Videos\\" + file.FileName;
            try
            {
                await using var stream = new FileStream(videoPath, FileMode.Create);
                await file.CopyToAsync(stream);

                //get video thumbnail and store as separate blob
                var inputFile = new MediaFile {Filename = videoPath};
                var thumbnail = new MediaFile {Filename = thumbnailPath};
                using (var engine = new Engine())
                {
                    engine.GetMetadata(inputFile);
                    var options = new ConversionOptions {Seek = TimeSpan.FromSeconds(1)};
                    engine.GetThumbnail(inputFile, thumbnail, options);
                }
                var thumbnailBlockBlob = cloudBlobContainer.GetBlockBlobReference(generatedName + "-thumbnail.jpg");
                thumbnailBlockBlob.Properties.ContentType = "image/jpg";
                await thumbnailBlockBlob.UploadFromFileAsync(thumbnailPath);
                
                //get video duration in seconds
                var seconds = Math.Truncate(inputFile.Metadata.Duration.TotalSeconds);
                cloudBlockBlob.Metadata.Add(new KeyValuePair<string, string>("duration", seconds.ToString()));

                //upload to Azure Blob Storage
                var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                var fileBytes = ms.ToArray();
                cloudBlockBlob.Properties.ContentType = file.ContentType;
                await cloudBlockBlob.UploadFromByteArrayAsync(fileBytes, 0, (int) file.Length);
            }
            finally
            {
                File.Delete(videoPath);
                File.Delete(thumbnailPath);
            }
        }

        public void RetrieveVideo(string videoName)
        {
            throw new NotImplementedException();
        }

        public async Task<List<VideoMetaData>> GetAllVideos()
        {
            var cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference("demo2videos");
            var subdirectory = "";
            var blobResultSegment = await cloudBlobContainer.ListBlobsSegmentedAsync(subdirectory, true, BlobListingDetails.All,
                int.MaxValue, null, null, null);
            
            var allFiles = blobResultSegment.Results;
            var resultList = new List<VideoMetaData>();
            var currentVideo = new VideoMetaData();
            foreach(var listBlobItem in allFiles)//NOTE: Assuming here that a thumbnail will be immediately followed by its corresponding mp4 file
            {
                var file = (CloudBlob) listBlobItem;
                if (file.Name.Contains("thumbnail"))
                {
                    currentVideo = new VideoMetaData();
                    var thumbnail = new byte[file.Properties.Length];
                    for (int k = 0; k < file.Properties.Length; k++)
                    {
                        thumbnail[k] = 0x20;
                    }

                    currentVideo.Thumbnail = thumbnail;
                    await file.DownloadToByteArrayAsync(thumbnail, 0);
                }
                else
                {
                    currentVideo.Id = file.Name.Replace(".mp4", "");
                    if (file.Properties.LastModified != null)
                        currentVideo.DateStored = file.Properties.LastModified.Value.DateTime;
                    file.Metadata.TryGetValue("duration", out var time);
                    currentVideo.Duration = int.Parse(time ?? Empty);
                    file.Metadata.TryGetValue("originalName", out var oldName);
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
            String str = "";
            for(int i =0; i<5; i++)
            {
                int a = _random.Next(_alphanumeric.Length);
                str = str + _alphanumeric.ElementAt(a);
            }
            return str;
        }

    }
}