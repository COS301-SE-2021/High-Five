using System;
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
         * where data is stored. This manager is primarily responsible for retrieving blobs from the storage
         * and constructing BlobFile objects to return to external callers.
         *
         *      Attributes:
         * -> _cloudStorageAccount: this is the object that contains a reference to the Azure Storage account.
         *      being used. It is initialised in the constructor through a connection string that can be
         *      retrieved from the Azure portal.
         * -> _random: this is a random object that is used to generate unique id's for uploaded files.
         * -> Alphanumeric: this is a simple alphanumeric string used to generate salt during the process
         *      where uploaded files are granted unique id's.
         * -> _mocked: this boolean parameter indicated whether the storage manager should be working with the
         *      actual cloud storage or with a mocked in-memory storage. It will only be set to true when the
         *      default constructor of the StorageManager is called.
         * -> _mockContainer: an array containing all mocked blob files. Will only be used when _mocked is true.
         */
        
        private readonly CloudStorageAccount _cloudStorageAccount;
        private readonly Random _random;
        private const string Alphanumeric = "abcdefghijklmnopqrstuvwxyz0123456789";
        
        //variables used during cloud storage mocking
        private bool _mocked;
        private List<MockBlobFile> _mockContainer;

        public StorageManager(IConfiguration config)
        {
            var connectionString = config.GetConnectionString("StorageConnection");
            _cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            _random = new Random();
            _mocked = false;
        }

        public StorageManager()
        {
            /*
             *      Description:
             * The default constructor of the class will enable the manager's "Mock Mode" in which it will
             * be working with a mocked in-memory storage as opposed to the actual cloud storage. Used for
             * testing purposes.
             */
            
            _mocked = true;
            _mockContainer = new List<MockBlobFile>();
            _random = new Random();
        }

        public async Task<IBlobFile> GetFile(string fileName, string container, bool create=false)
        {
            /*
             *      Description:
             * This function returns a reference to an existing blob file in some container within the storage,
             * or null if the searched file does not exist in the storage. A BlobFile object is returns which
             * contains the CloudBlockBlob itself.
             *
             *      Parameters:
             * -> fileName: this is the name of the file that is being retrieved. I.e. "video1.mp4".
             * -> container: this is the name of the storage container to be searched for the file.
             * -> create: this flag is only used internally by the CreateNewFile function to indicate to
             *      this function that it does not need to check if the blob file exists, but that it should
             *      rather instantiate the BlobFile with a reference to a CloudBlockBlob object that may
             *      or may not be in storage. The creation of the file itself will be handled by the
             *      CreateNewFile function.
             */

            var cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference(container);
            if (create)
            {
                return new BlobFile(cloudBlobContainer.GetBlockBlobReference(fileName));
            }
            
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

        public async Task<List<IBlobFile>> GetAllFilesInContainer(string container)
        {
            /*
             *      Description:
             * This function will return all the blob files stored in a provided container.
             *
             *      Parameters:
             * -> container: the name of the container which will contain all the blob files returned.
             */
            
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
            var blobFileList = new List<IBlobFile>();
            foreach (var listBlobItem in allFiles)
            {
                var blob = (CloudBlockBlob) listBlobItem;
                blobFileList.Add(new BlobFile(blob));
            }
            return blobFileList;
        }

        public async Task<IBlobFile> CreateNewFile(string name, string container)
        {
            /*
             *      Description:
             * This function will attempt to create a new blob file in temporary memory. It returns null
             * if the provided name already exists in the cloud storage, otherwise it returns the
             * BlobFile object with a reference to a CloudBlockBlob object that does not exist in the
             * cloud storage.
             *
             *       Parameters:
             * -> name: this is the name of the file to be created.
             * -> container: the name of the cloud storage container where the file should be created.
             */
            
            var newFile = GetFile(name, container, true).Result;
            if (await newFile.Exists())
            {
                return null;
            }
            return newFile;
        }
        
        public string HashMd5(string source)
        {
            /*
             *      Description:
             * This function is primarily used to generate id's for files stored in blob storage that are
             * guaranteed to be unique. An MD5 hash will be applied to the string passed to this function.
             *
             *      Parameters:
             * -> source: the string to be hashed.
             */
            
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
            /*
             *      Description:
             * This function returns a 5-character string consisting of randomly selected characters from
             * the Alphanumeric constant. It is mainly used during unique name generation of files, in
             * particular when salt needs to be added to the string to be hashed.
             */
            
            var str = "";
            for(var i =0; i<5; i++)
            {
                var a = _random.Next(Alphanumeric.Length);
                str += Alphanumeric.ElementAt(a);
            }
            return str;
        }
        
    }
}