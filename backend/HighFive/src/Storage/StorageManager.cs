﻿using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace src.Storage
{
    public class StorageManager: IStorageManager
    {
        private readonly IConfiguration _configuration;
        private readonly CloudStorageAccount _cloudStorageAccount;
        IConfiguration IStorageManager.Configuration => _configuration;
        CloudStorageAccount IStorageManager.CloudStorageAccount => _cloudStorageAccount;

        public StorageManager(IConfiguration config)
        {
            _configuration = config;
            String connectionString = _configuration.GetConnectionString("StorageConnection");
            _cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
        }

        public async Task UploadVideo(IFormFile video)
        {
            var cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference("demo2videos");

            if (await cloudBlobContainer.CreateIfNotExistsAsync())
            {
                await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions()
                    {PublicAccess = BlobContainerPublicAccessType.Off});
            }

            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(video.FileName);
            var ms = new MemoryStream();
            await video.CopyToAsync(ms);
            var fileBytes = ms.ToArray();
            await cloudBlockBlob.UploadFromByteArrayAsync(fileBytes,0,(int)video.Length);
        }

        public void RetrieveVideo(string videoName)
        {
            throw new NotImplementedException();
        }

        public void GetVideoStrings()
        {
            throw new NotImplementedException();
        }

    }
}