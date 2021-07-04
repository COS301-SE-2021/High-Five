﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
        /*
         *      Description:
         * The StorageManager class serves as a general manager that interfaces with the Azure Cloud Storage
         * where data is stored. This manager is primarily responsible for retrieving blobs from the storage.
         *
         *      Attributes:
         * -> _cloudStorageAccount - this is the object that contains a reference to the Azure Storage account.
         *      being used. It is initialised in the constructor through a connection string that can be
         *      retrieved from the Azure portal.
         * -> _random - this is a random object that is used to generate unique id's for uploaded files.
         * -> Alphanumeric - this is a simple alphanumeric string used to generate salt during the process
         *      where uploaded files are granted unique id's.
         */
        
        private readonly CloudStorageAccount _cloudStorageAccount;
        private readonly Random _random;
        private const string Alphanumeric = "abcdefghijklmnopqrstuvwxyz0123456789";

        public StorageManager(IConfiguration config)
        {
            var connectionString = config.GetConnectionString("StorageConnection");
            _cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            _random = new Random();
        }

        public StorageManager(string connectionString)
        {
            /*
             *      Description:
             * The constructor of the class that initializes the CloudStorageAccount based on an
             * appropriate connection string passed through.
             *
             *      Parameters:
             * -> connectionString - the connection string of a Cloud Storage Client that can be retrieved.
             *      from the Azure portal.
             */
            
            _cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            _random = new Random();
        }

        public async Task<BlobFile> GetFile(string fileName, string container)
        {
            /*
             *      Description:
             * This function returns a reference to an existing blob file in some container within the storage,
             * or null if the searched file does not exist in the storage. A BlobFile object is returns which
             * contains the CloudBlockBlob itself.
             *
             *      Parameters:
             * -> fileName - this is the name of the file that is being retrieved. I.e. "video1.mp4".
             * -> container - this is the name of the storage container to be searched for the file.
             */
            
            var cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference(container);

            if (!await cloudBlobContainer.ExistsAsync())
            {
                return null;
            }
            var file = cloudBlobContainer.GetBlockBlobReference(fileName);
            if (await file.ExistsAsync())
            {
                return new BlobFile(file);
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
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(source);
            var hashBytes = md5.ComputeHash(inputBytes);
            
            var sb = new StringBuilder();
            foreach (var t in hashBytes)
            {
                sb.Append(t.ToString("X2"));
            }
            return sb.ToString();
        }

        public string RandomString()
        {
            var str = "";
            for(var i =0; i<5; i++)
            {
                var a = _random.Next(Alphanumeric.Length);
                str = str + Alphanumeric.ElementAt(a);
            }
            return str;
        }
        
    }
}