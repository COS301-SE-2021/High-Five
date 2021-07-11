using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using BlobProperties = Microsoft.WindowsAzure.Storage.Blob.BlobProperties;
using System;

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
         * -> _file: this variable stores the contents of an uploaded file as a stream.
         * -> _metaData: an in-memory mock of the meta-data belonging to this mocked file.
         * -> _container: a reference to the container in which this mocked file is located. Will be handled
         *      externally by the StorageManager
        */
        
        public BlobProperties Properties { get; }
        private Stream _file;
        public string Name { get; }
        
        //mock variables
        private Hashtable _metaData;
        private List<IBlobFile> _container;

        public MockBlobFile(List<IBlobFile> container, string name)
        {
            _container = container;
            _metaData = new Hashtable();
            Properties = null;
            _file = null;
            Name = name;
        }

        ~MockBlobFile()
        {
            /*
             *      Description:
             * The MockBlobFile's finalizer is responsible for closing the filestream of the temporary mock
             * object in the case that it contained data.
             */
            
            _file?.Close();
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

            var ms = new MemoryStream();
            await newFile.CopyToAsync(ms);
            _file = ms;
            
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

            _file = File.OpenRead(path);

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
            
            var baseDirectory = Path.GetTempFileName();

            _file = File.Create(baseDirectory);
            var writer = new StreamWriter(_file);
            await writer.WriteAsync(text);
            writer.Close();
            _file = File.OpenRead(baseDirectory);
            
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
             * This function converts the contents of the stored file into a byte array and returns it.
             */

            if (_file == null)
            {
                return Array.Empty<byte>();
            }

            if (!_file.CanSeek)
            {
                
            }
            _file.Seek(0, SeekOrigin.Begin);
            var array = new byte[_file.Length];
            await _file.ReadAsync(array.AsMemory(0, array.Length));
            _file.Seek(0, SeekOrigin.Begin);
            return array;
        }

        public async Task<string> ToText()
        {
            /*
             *      Description:
             * This function converts the contents of a blob file to text. It is usually to return data
             * from text or json files. In the mocked case it will return a simple string. The return value
             * is the specified json format of a stored pipeline.
             */

            if (_file == null)
            {
                return "{\"name\":\"Mock\",\"id\":\""+Name.Split(".")[0]+"\",\"tools\":[]}";
            }

            _file.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(_file);
            return await reader.ReadToEndAsync();
        }
    }
}