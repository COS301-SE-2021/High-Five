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
        private Random _random;
        private readonly string _alphanumeric = "abcdefghijklmnopqrstuvwxyz0123456789";
        private string _container = "demo2videos";

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
            var cloudBlobContainer = cloudBlobClient.GetContainerReference(_container);
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
                generatedName = HashMd5(file.FileName+salt);
                cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(generatedName + ".mp4");
            }
            cloudBlockBlob.Metadata.Add(new KeyValuePair<string, string>("originalName", file.FileName));
            if (!IsNullOrEmpty(salt))
            {
                cloudBlockBlob.Metadata.Add(new KeyValuePair<string, string>("salt", salt));
            }
            
            //create local temp copy of video file and thumbnail
            //var baseDirectory = "d:\\local\\";
            var baseDirectory = Directory.GetCurrentDirectory() + "\\";
            var thumbnailPath = baseDirectory + "thumbnail.jpg";
            var videoPath = baseDirectory + file.Name;
            await using var stream = new FileStream(videoPath, FileMode.Create);
            await file.CopyToAsync(stream);

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
            var thumbnailBlockBlob = cloudBlobContainer.GetBlockBlobReference(generatedName + "-thumbnail.jpg");
            thumbnailBlockBlob.Properties.ContentType = "image/jpg";
            await thumbnailBlockBlob.UploadFromFileAsync(thumbnailPath);
                
            //get video duration in seconds
            //var seconds = Math.Truncate(inputFile.Metadata.Duration.TotalSeconds);
            var seconds = 0;
            cloudBlockBlob.Metadata.Add(new KeyValuePair<string, string>("duration", seconds.ToString()));

            //upload to Azure Blob Storage
            var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var fileBytes = ms.ToArray();
            cloudBlockBlob.Properties.ContentType = file.ContentType;
            await cloudBlockBlob.UploadFromByteArrayAsync(fileBytes, 0, (int) file.Length);
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

        public async Task<CloudBlockBlob> GetFile(string fileName, string container)
        {
            var cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference(container);
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
            var cloudBlobContainer = cloudBlobClient.GetContainerReference(_container);
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

    }
}