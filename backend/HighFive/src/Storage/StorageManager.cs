using System;
using System.Collections.Generic;
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
        private readonly Random _random;
        private readonly string _alphanumeric = "abcdefghijklmnopqrstuvwxyz0123456789";

        public StorageManager(IConfiguration config)
        {
            _configuration = config;
            String connectionString = _configuration.GetConnectionString("StorageConnection");
            _cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            _random = new Random();
        }

        public StorageManager(String connectionString)
        {
            _cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            _random = new Random();
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
        
        public string HashMd5(string source)
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

        public string RandomString()
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