﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.Blob;

namespace src.Storage
{
    /*
     *      Description:
     * This is a wrapper class for Azure SDK's CloudBlockBlob object, which will contain the data of a file
     * retrieved from Azure Blob Storage. Usage of this class prohibits the services from modifying, saving
     * or deleting any data from the CloudBlockBlob in an unprotected manner.
     *
     * Attributes:
     * _file - this is a reference to the Blob from storage that is currently being handled
     */
    public class BlobFile
    {
        private CloudBlockBlob _file;

        public BlobFile(CloudBlockBlob file)
        {
            _file = file;
        }
        
        public void AddMetadata(string key, string value)
        {
            /*
             *      Description:
             * The AddMetaData function adds a key-value pair as meta-data to the blob file.
             *
             *      Parameters:
             * key - this parameter represents the key in the key-value pair being added as meta-data
             * value - this parameter represents the value in the key-value pair being added as meta-data
             */
            
            _file.Metadata.Add(new KeyValuePair<string, string>(key, value));;
        }
        
        public async Task Upload(IFormFile newFile)
        {
            /*
             *      Description:
             * The Upload function will upload a new file to the Blob in the Azure Blob Storage assosciated
             * with the file contained within this BlobFile. This function overwrites any data currently stored
             * in the CloudBlockBlob file and uploads it to the storage directly.
             *
             *      Parameters:
             * newFile - this parameter is the new file to be uploaded to the blob storage
             */
            
            var ms = new MemoryStream();
            await newFile.CopyToAsync(ms);
            var fileBytes = ms.ToArray();
            _file.Properties.ContentType = newFile.ContentType;
            await _file.UploadFromByteArrayAsync(fileBytes, 0, (int) newFile.Length);
        }
        
    }
}