using System;
using System.Collections.Generic;
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
        
        private List<MockBlobFile> _mockContainer;
        private readonly Random _random;
        private const string Alphanumeric = "abcdefghijklmnopqrstuvwxyz0123456789";
        
        public MockStorageManager()
        {
            /*
             *      Description:
             * The default constructor of the class will instantiate a new mockContainer list that will
             * serve as the mocked cloud storage.
             */
            
            _mockContainer = new List<MockBlobFile>();
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
            throw new System.NotImplementedException();
        }

        public async Task<IBlobFile> CreateNewFile(string name, string container)
        {
            throw new System.NotImplementedException();
        }

        public string RandomString()
        {
            throw new System.NotImplementedException();
        }

        public string HashMd5(string source)
        {
            throw new System.NotImplementedException();
        }
    }
}