using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.Blob;

namespace src.Storage
{
    public class BlobFile
    {
        /*
         *      Description:
         * This is a wrapper class for Azure SDK's CloudBlockBlob object, which will contain the data of a file
         * retrieved from Azure Blob Storage. Usage of this class prohibits the services from modifying, saving
         * or deleting any data from the CloudBlockBlob in an unprotected manner.
         *
         *      Attributes:
         * -> _file - this is a reference to the Blob from storage that is currently being handled.
         * -> Properties - this variable contains the properties of the CloudBlockBlob from file
        */
        
        private readonly CloudBlockBlob _file;
        public BlobProperties Properties { get; }

        public BlobFile(CloudBlockBlob file)
        {
            /*
             *      Description:
             * The constructor of the class that initializes the file passed through.
             *
             *      Parameters:
             * -> file - the CloudBlockBlob file that this BlobFile object wraps.
             */
            
            _file = file;
            Properties = file.Properties;
        }
        
        public void AddMetadata(string key, string value)
        {
            /*
             *      Description:
             * The AddMetaData function adds a key-value pair as meta-data to the blob file.
             *
             *      Parameters:
             * -> key - this parameter represents the key in the key-value pair being added as meta-data.
             * -> value - this parameter represents the value in the key-value pair being added as meta-data.
             */
            
            _file.Metadata.Add(new KeyValuePair<string, string>(key, value));;
        }
        
        public async Task UploadFile(IFormFile newFile)
        {
            /*
             *      Description:
             * This function will upload a new file to the Blob in the Azure Blob Storage associated
             * with the file contained within this BlobFile. This function overwrites any data currently stored
             * in the CloudBlockBlob file and uploads it to the storage directly.
             *
             *      Parameters:
             * -> newFile - this parameter is the new file to be uploaded to the blob storage.
             */
            
            var ms = new MemoryStream();
            await newFile.CopyToAsync(ms);
            var fileBytes = ms.ToArray();
            _file.Properties.ContentType = newFile.ContentType;
            await _file.UploadFromByteArrayAsync(fileBytes, 0, (int) newFile.Length);
        }

        public async Task UploadFile(string path)
        {
            /*
             *      Description:
             * This function will upload a new file from a directory to the cloud in the Azure Blob Storage associated
             * with the file contained within this BlobFile. This function overwrites any data currently stored
             * in the CloudBlockBlob file and uploads it to the storage directly.
             *
             *      Parameters:
             * -> path - the full path pointing to where the file is stored.
             */
            
            await _file.UploadFromFileAsync(path);
        }

        public void UploadText(string text)
        {
            /*
             *      Description:
             * This function will upload a new text file to the Blob in the Azure Blob Storage associated
             * with the file contained within this BlobFile. This function overwrites any data currently stored
             * in the CloudBlockBlob file and uploads it to the storage directly.
             *
             *      Parameters:
             * -> text - the text file stored as a single string to be uploaded to the blob storage.
             */
            
            _file.UploadTextAsync(text);
        }

        public async Task Delete()
        {
            /*
             *      Description:
             * This function will remove the blob file from the storage completely.
             */
            
            await _file.DeleteAsync();
        }

        public async Task<bool> Exists()
        {
            /*
             *      Description:
             * This function returns a boolean indicating whether or not the contained CloudBlockBlob object
             * exists.
             */
            
            return await _file.ExistsAsync();
        }

        public async Task<byte[]> ToByteArray()
        {
            /*
             *      Description:
             * This function converts the contents of the blob storage into a byte array and returns it.
             */
            
            var byteArray = new byte[Properties.Length];
            for (var k = 0; k < Properties.Length; k++)
            {
                byteArray[k] = 0x20;
            }
            await _file.DownloadToByteArrayAsync(byteArray, 0);
            return byteArray;
        }

        public async Task<string> ToText()
        {
            /*
             *      Description:
             * This function converts the contents of a blob file to text. It is usually to return data
             * from text or json files.
             */
            
            return await _file.DownloadTextAsync();
        }
    }
}