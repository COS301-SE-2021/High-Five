using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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
            var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var fileBytes = ms.ToArray();
            var generatedName = HashMd5(file.FileName);
            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(generatedName);
            var salt = "";
            while (await cloudBlockBlob.ExistsAsync())
            {
                salt += RandomString();
                generatedName = HashMd5(file.FileName+salt);
                cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(generatedName);
            }
            cloudBlockBlob.Properties.ContentType = file.ContentType;
            cloudBlockBlob.Metadata.Add(new KeyValuePair<string, string>("originalName", file.FileName));
            if (!IsNullOrEmpty(salt))
            {
                cloudBlockBlob.Metadata.Add(new KeyValuePair<string, string>("salt", salt));
            }
            cloudBlockBlob.Metadata.Add(new KeyValuePair<string, string>("duration", "int"));
            cloudBlockBlob.Metadata.Add(new KeyValuePair<string, string>("thumbnail", "byte array"));
            await cloudBlockBlob.UploadFromByteArrayAsync(fileBytes,0,(int)file.Length);
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
            
            var totalFiles = blobResultSegment.Results;
            var resultList = new List<VideoMetaData>();
            foreach(var listBlobItem in totalFiles)
            {
                var file = (CloudBlob) listBlobItem;
                if(!file.Name.Contains(".mp4"))
                    continue;
                var currentVideo = new VideoMetaData {Name = file.Name};
                if (file.Properties.LastModified != null)
                    currentVideo.DateStored = file.Properties.LastModified.Value.DateTime;
                resultList.Add(currentVideo);
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
             
            for(int i =0; i<_alphanumeric.Length; i++)
            {
                int a = _random.Next(_alphanumeric.Length);
                str = str + _alphanumeric.ElementAt(a);
            }

            return str;
        }

    }
}