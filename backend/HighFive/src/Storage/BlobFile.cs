using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.Blob;

namespace src.Storage
{
    /*
     * This is a wrapper class for Azure SDK's CloudBlockBlob object, which will contain the data of a file
     * retrieved from Azure Blob Storage. Usage of this class prohibits the services from modifying, saving
     * or deleting any data from the CloudBlockBlob in an unprotected manner.
     */
    public class BlobFile
    {
        private CloudBlockBlob _file;

        public BlobFile(CloudBlockBlob file)
        {
            _file = file;
        }

        /*
         * The AddMetaData function adds a key-value pair as meta-data to the blob file.
         *
         * Parameters:
         * key - this parameter represents the key in the key-value pair being added as meta-data
         * value - this parameter represents the value in the key-value pair being added as meta-data
         */
        public void AddMetadata(string key, string value)
        {
            _file.Metadata.Add(new KeyValuePair<string, string>(key, value));;
        }
        
        public async Task Upload(IFormFile newFile)
        {
            var ms = new MemoryStream();
            await newFile.CopyToAsync(ms);
            var fileBytes = ms.ToArray();
            _file.Properties.ContentType = newFile.ContentType;
            await _file.UploadFromByteArrayAsync(fileBytes, 0, (int) newFile.Length);
        }
        
    }
}