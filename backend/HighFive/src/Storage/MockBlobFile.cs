using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using BlobProperties = Microsoft.WindowsAzure.Storage.Blob.BlobProperties;

namespace src.Storage
{
    public class MockBlobFile: IBlobFile
    {
        /*
         *      Description:
         * This is mock instantiation of the IBlobFile interface that will allow unit and integration tests
         * to be employed using a mocked storage.
         *
         *      Attributes:
         * -> Properties: this variable contains the properties of the CloudBlockBlob from file
         * -> Name: this variable contains the name of the file.
         * -> _metaData: an in-memory mock of the meta-data belonging to this mocked file.
         * -> _container: a reference to the container in which this mocked file is located. Will be handled
         *      externally by the StorageManager
        */
        
        public BlobProperties Properties { get; }
        public string Name { get; }
        
        //mock variables
        private Hashtable _metaData;
        private List<IBlobFile> _container;

        public MockBlobFile(List<IBlobFile> container, string name)
        {
            _container = container;
            _metaData = new Hashtable();
            Properties = null;
            Name = name;
        }
        public void AddMetadata(string key, string value)
        {
            /*
             *      Description:
             * The AddMetaData function adds a key-value pair as meta-data to the mocked blob file.
             *
             *      Parameters:
             * -> key: this parameter represents the key in the key-value pair being added as meta-data.
             * -> value: this parameter represents the value in the key-value pair being added as meta-data.
             */
            
            _metaData.Add(key, value);
        }

        public string GetMetaData(string key)
        {
            /*
             *      Description:
             * This function will attempt to return the value associated with a provided key, if such a
             * key-value pair exists within the mocked blob file.
             *
             *      Parameters:
             * -> key: the key that may or may not belong to a key-value pair in the file's meta-data
             */

            if (_metaData.ContainsKey(key))
            {
                return (string) _metaData[key];
            }

            return string.Empty;
        }

        public async Task UploadFile(IFormFile newFile)
        {
            /*
             *      Description:
             * This function will upload a new file from a directory to the mocked cloud storage.
             *
             *      Parameters:
             * -> newFile: this parameter is the new file to be uploaded to the blob storage.
             */
            
            if (!_container.Contains(this))
            {
                _container.Add(this);
            }
        }

        public async Task UploadFile(string path)
        {
            /*
             *      Description:
             * This function will upload a new file from a directory to the mocked cloud storage.
             *
             *      Parameters:
             * -> path: the full path pointing to where the file is stored.
             */
            
            if (!_container.Contains(this))
            {
                _container.Add(this);
            }
        }

        public async Task UploadText(string text)
        {
            /*
             *      Description:
             * This function will upload a new file from a directory to the mocked cloud storage.
             *
             *      Parameters:
             * -> text: the text file stored as a single string to be uploaded to the blob storage.
             */
            
            if (!_container.Contains(this))
            {
                _container.Add(this);
            }
        }

        public async Task Delete()
        {
            /*
             *      Description:
             * This function will remove the mocked file from the mocked storage completely.
             */
            
            _container.Remove(this);
        }

        public async Task<bool> Exists()
        {
            /*
             *      Description:
             * This function returns a boolean indicating whether or not the contained CloudBlockBlob object
             * exists in the mocked storage.
             */

            return _container.Contains(this);
        }

        public async Task<byte[]> ToByteArray()
        {
            /*
             *      Description:
             * This function converts the contents of the blob storage into a byte array and returns it.
             */
                        
            var arr = new byte[5];
            return arr;
        }

        public async Task<string> ToText()
        {
            /*
             *      Description:
             * This function converts the contents of a blob file to text. It is usually to return data
             * from text or json files. In the mocked case it will return a simple string. The return value
             * is the specified json format of a stored pipeline.
             */
                        
            return "{\"name\":\"Mock\",\"id\":\""+Name.Split(".")[0]+"\",\"tools\":[]}";
        }
    }
}