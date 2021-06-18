using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using Microsoft.AspNetCore.Cors;
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

        public StorageManager(IConfiguration config)
        {
            _configuration = config;
            String connectionString = _configuration.GetConnectionString("StorageConnection");
            _cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
        }

        public async Task<CloudBlockBlob> GetFile(string fileName, string container, bool create=false)
        {
            var cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference(container);
            if (create)//NOTE: Does not test if it actually exists or not
            {
                return cloudBlobContainer.GetBlockBlobReference(fileName);
            }
            if (!await cloudBlobContainer.ExistsAsync())
            {
                return null;
            }
            var file = cloudBlobContainer.GetBlockBlobReference(fileName);
            if (await file.ExistsAsync())
            {
                return file;
            }
            return null;
        }

        public async Task<List<CloudBlockBlob>> GetAllFilesInContainer(string container)
        {
            var cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference(container);
            if (await cloudBlobContainer.ExistsAsync())
            {
                await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions()
                    {PublicAccess = BlobContainerPublicAccessType.Off});
            }
            var subdirectory = "";
            var blobResultSegment = await cloudBlobContainer.ListBlobsSegmentedAsync(subdirectory, true, BlobListingDetails.All,
                int.MaxValue, null, null, null);
            var allFiles = blobResultSegment.Results;

            return allFiles.Cast<CloudBlockBlob>().ToList();
        }

        public async Task<CloudBlockBlob> CreateNewFile(string name, string container)
        {
            CloudBlockBlob newFile = GetFile(name, container, true).Result;
            if (await newFile.ExistsAsync())
            {
                return null;
            }
            return newFile;
        }
        
    }
}