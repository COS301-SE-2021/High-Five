using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace src.Storage
{
    public class MockStorageManager: IStorageManager
    {
        /*
         *      Description:
         * This class is a mocked implementation of the StorageManager that will work with a mocked in-memory
         * storage instead of the actual cloud storage. Will be used for testing.
         *
         *      Attributes:
         *-> _mockContainer: an array containing all mocked blob files. Will only be used when _mocked is true.
         * -> _random: this is a random object that is used to generate unique id's for uploaded files.
         * -> Alphanumeric: this is a simple alphanumeric string used to generate salt during the process
         *      where uploaded files are granted unique id's.
         */
        
        private List<IBlobFile> _mockContainer;
        private readonly Random _random;
        private const string Alphanumeric = "abcdefghijklmnopqrstuvwxyz0123456789";
        
        public MockStorageManager()
        {
            /*
             *      Description:
             * The default constructor of the class will instantiate a new mockContainer list that will
             * serve as the mocked cloud storage.
             */
            
            _mockContainer = new List<IBlobFile>();
            _random = new Random();
        }
        public async Task<IBlobFile> GetFile(string fileName, string container, bool create = false)
        {
            /*
             *      Description:
             * This function returns a reference to an existing blob file in the mocked storage,
             * or null if the searched file does not exist in the storage. A MockBlobFile object is returned if
             * a new file is being created or if the file exists in the mocked storage.
             *
             *      Parameters:
             * -> fileName: this is the name of the file that is being retrieved. I.e. "video1.mp4".
             * -> container: this is the name of the storage container to be searched for the file. This will
             *      be ignored during testing in the mocked implementation of the storage manager.
             * -> create: this flag is only used internally by the CreateNewFile function to indicate to
             *      this function that it does not need to check if the blob file exists, but that it should
             *      rather instantiate the BlobFile with a reference to a CloudBlockBlob object that may
             *      or may not be in mocked storage. The creation of the file itself will be handled by the
             *      CreateNewFile function.
             */

            if (container.Equals("public"))
            {
                var toolsFile = new MockBlobFile(new List<IBlobFile>(), "tools.txt");
                await toolsFile.UploadText("CarRecognitions\nCarFollowing");
                return toolsFile;
            }
            
            IBlobFile file = null;
            foreach (var blobFile in _mockContainer)
            {
                if (!blobFile.Name.Equals(fileName)) continue;
                file = blobFile;
                break;
            }

            if (file != null) return file;
            return create ? new MockBlobFile(_mockContainer, fileName) : null;
        }

        public async Task<List<IBlobFile>> GetAllFilesInContainer(string container)
        {
            /*
             *      Description:
             * This function will return all the mocked files stored in the mocked container.
             *
             *      Parameters:
             * -> container: the name of the container which will contain all the blob files returned. It will
             *      be ignored in this mocked implementation.
             */

            return _mockContainer;
        }

        public async Task<IBlobFile> CreateNewFile(string name, string container)
        {
            /*
             *      Description:
             * This function will attempt to create a new blob file in temporary memory. It returns null
             * if the provided name already exists in the mocked storage, otherwise it returns the
             * MockBlobFile object that does not yet exist in the mocked container.
             *
             *       Parameters:
             * -> name: this is the name of the file to be created.
             * -> container: the name of the cloud storage container where the file should be created. Will
             *      be ignored in this mocked implementation.
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

        public bool SetBaseContainer(string container)
        {
            /*
             *      Description:
             * This function will set the base container of the StorageManager. It will usually be set to the
             * user's unique Azure Active Directory ID (obtained from the JWT authentication token), or it
             * will be set to "public" if publicly accessible data must be retrieved.
             * The function will return true if the provided container exists and false if the provided base
             * container does not exist.
             *
             *      Parameters:
             * -> container: the name of the new base container.
             */

            return true;
        }

        public bool IsContainerSet()
        {
            /*
             *      Description:
             * This function returns whether or not the storage manager's base container has been set yet.
             */            
            return true;
        }

        public string GetCurrentContainer()
        {
            /*
             *      Description:
             * Returns the current baseContainer. Or in the case of this mock, simply an empty string.
             */
            return "";
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