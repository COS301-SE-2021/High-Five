using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using analysis_engine;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace broker_analysis_client.Storage
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
         * -> _baseContainer: the name of the base container from which the blobs will be retrieved. It will
         *      usually be the user's Azure AD B2C object Id, or "public". All other "container" parameters that
         *      are passed through in methods will be sub-containers within the baseContainer. Sub-container in
         *      this context simply refers to a prefix naming convention of the blob files.
         */

        private static CloudStorageAccount _cloudStorageAccount;
        private readonly Random _random;
        private const string Alphanumeric = "abcdefghijklmnopqrstuvwxyz0123456789";
        private string _baseContainer;
        private CloudBlobContainer _cloudBlobContainer;

        public StorageManager(string userId)
        {
            _cloudStorageAccount ??= CloudStorageAccount.Parse(ConfigStrings.StorageConnectionString);
            SetBaseContainer(userId);
            _random = new Random();
        }

        public async Task<IBlobFile> GetFile(string fileName, string container, bool create=false)
        {
            /*
             *      Description:
             * This function returns a reference to an existing blob file in some container within the storage,
             * or null if the searched file does not exist in the storage. A BlobFile object is returned which
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
            if (!container.Equals(""))
            {
                fileName = container + "/" + fileName;
            }

            if (create)
            {
                return new BlobFile(_cloudBlobContainer.GetBlockBlobReference(fileName));
            }

            if (!await _cloudBlobContainer.ExistsAsync())
            {
                return null;
            }
            var file = _cloudBlobContainer.GetBlockBlobReference(fileName);
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
            
            if (await _cloudBlobContainer.ExistsAsync())
            {
                await _cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions()
                    {PublicAccess = BlobContainerPublicAccessType.Off});
            }
            var subdirectory = "";
            var blobResultSegment = await _cloudBlobContainer.ListBlobsSegmentedAsync(subdirectory, true, BlobListingDetails.All,
                int.MaxValue, null, null, null);
            var allFiles = blobResultSegment.Results;
            var blobFileList = new List<IBlobFile>();
            foreach (var listBlobItem in allFiles)
            {
                var blob = (CloudBlockBlob) listBlobItem;
                if (blob.Name.Contains(container) && !blob.Name.Replace(container + "/", string.Empty).Contains("/"))
                {
                    blobFileList.Add(new BlobFile(blob));
                }
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

        public async Task<bool> SetBaseContainer(string container)
        {
            /*
             *      Description:
             * This function will set the base container of the StorageManager. It will usually be set to the
             * user's unique Azure Active Directory ID (obtained from the JWT authentication token), or it
             * will be set to "public" if publicly accessible data must be retrieved.
             * The function will return true if the provided container exists and false if the provided base
             * container does not initially exist.
             * If the container does not initially exist, a new container will be made for this user in the
             * cloud storage.
             *
             *      Parameters:
             * -> container: the name of the new base container.
             */
            
            _baseContainer = container;
            if (_baseContainer.Equals("unset"))
            {
                return false;
            }
            var cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();
            _cloudBlobContainer = cloudBlobClient.GetContainerReference(container);

            var created = _cloudBlobContainer.ExistsAsync().Result;
            if (created) return true;
            
            /*the following code will create a new container for the user and will be called when a new user first
                tries to access the cloud storage. */
            await _cloudBlobContainer.CreateAsync();
            return false;
        }

        public bool IsContainerSet()
        {
            /*
             *      Description:
             * This function returns whether or not the storage manager's base container has been set yet.
             */
            return !_baseContainer.Equals("unset");
        }

        public string GetCurrentContainer()
        {
            /*
             *      Description:
             * Returns the current baseContainer;
             */
            
            return _baseContainer;
        }

        public void StoreUserInfo(string id, string displayName, string email)
        {
            var userInfoString = id + "\n" + displayName + "\n" + email;
            var userInfoFile = CreateNewFile("user_info.txt", "").Result;
            userInfoFile.UploadText(userInfoString);

            var toolsFile = CreateNewFile("tools.txt", "").Result;
            toolsFile.UploadText("");
        }

        public string RandomString(int length=5)
        {
            /*
             *      Description:
             * This function returns a 5-character string consisting of randomly selected characters from
             * the Alphanumeric constant. It is mainly used during unique name generation of files, in
             * particular when salt needs to be added to the string to be hashed.
             */

            var str = "";
            for(var i =0; i<length; i++)
            {
                var a = _random.Next(Alphanumeric.Length);
                str += Alphanumeric.ElementAt(a);
            }
            return str;
        }

        public async Task DeleteAllFilesInContainer(string container)
        {
            var cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference(container);
            var blobResultSegment = await cloudBlobContainer.ListBlobsSegmentedAsync("", true, BlobListingDetails.All,
                int.MaxValue, null, null, null);
            var allFiles = blobResultSegment.Results;
            foreach (var blob in allFiles)
            {
                var blobFile = (CloudBlockBlob) blob;
                if (blobFile.Name.Equals("user_info.txt")) continue;
                await blobFile.DeleteIfExistsAsync();
            }
        }

    }
}
