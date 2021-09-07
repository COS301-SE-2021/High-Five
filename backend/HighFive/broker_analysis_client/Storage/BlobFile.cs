using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace broker_analysis_client.Storage
{
    public class BlobFile: IBlobFile
    {
        /*
         *      Description:
         * This is a wrapper class for Azure SDK's CloudBlockBlob object, which will contain the data of a file
         * retrieved from Azure Blob Storage. Usage of this class prohibits the services from modifying, saving
         * or deleting any data from the CloudBlockBlob in an unprotected manner.
         *
         *      Attributes:
         * -> _file: this is a reference to the Blob from storage that is currently being handled.
         * -> Properties: this variable contains the properties of the CloudBlockBlob from file.
         * -> Name: this variable contains the name of the file.
        */

        private readonly CloudBlockBlob _file;
        public BlobProperties Properties { get; }
        public string Name { get; }

        public BlobFile(CloudBlockBlob file)
        {
            /*
             *      Description:
             * The constructor of the class that initializes the file passed through.
             *
             *      Parameters:
             * -> file: the CloudBlockBlob file that this BlobFile object wraps.
             */

            _file = file;
            Properties = file.Properties;
            var splitName = file.Name.Split('/');
            if (splitName.Length >= 2)
            {
                Name = splitName[splitName.Length-1];
            }
        }

        public void AddMetadata(string key, string value)
        {
            /*
             *      Description:
             * The AddMetaData function adds a key-value pair as meta-data to the blob file.
             *
             *      Parameters:
             * -> key: this parameter represents the key in the key-value pair being added as meta-data.
             * -> value: this parameter represents the value in the key-value pair being added as meta-data.
             */

            _file.Metadata.Add(new KeyValuePair<string, string>(key, value));;
        }

        public string GetMetaData(string key)
        {
            /*
             *      Description:
             * This function will attempt to return the value associated with a provided key, if such a
             * key-value pair exists within the CloudBlockBlob.
             *
             *      Parameters:
             * -> key: the key that may or may not belong to a key-value pair in the file's meta-data
             */

            _file.Metadata.TryGetValue(key, out var value);
            return value;
            //TODO: verify that if meta-data does not exist, empty string is returned
        }
        
        public async Task UploadFileFromStream(Stream stream, string contentType="")
        {
            if (!contentType.Equals(""))
            {
                _file.Properties.ContentType = contentType;
            }
            await _file.UploadFromStreamAsync(stream);
        }

        public async Task UploadFileFromByteArray(byte[] array, string contentType = "")
        {
            var ms = new MemoryStream(array);
            await UploadFileFromStream(ms, contentType);
        }

        public async Task UploadText(string text)
        {
            /*
             *      Description:
             * This function will upload a new text file to the Blob in the Azure Blob Storage associated
             * with the file contained within this BlobFile. This function overwrites any data currently stored
             * in the CloudBlockBlob file and uploads it to the storage directly.
             *
             *      Parameters:
             * -> text: the text file stored as a single string to be uploaded to the blob storage.
             */

            await _file.UploadTextAsync(text);
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
             * exists in the cloud storage.
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

        public async Task<Stream> ToStream()
        {
            var stream = new MemoryStream();
            await _file.DownloadToStreamAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
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

        public string GetUrl()
        {
            /*
             *      Description:
             * This function will generate a SAS token for this blob file and return a temporary URL with
             * the token to allow temporary viewing of the file.
             */

            var sasPermissions = new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessStartTime = DateTimeOffset.Now,
                SharedAccessExpiryTime = DateTimeOffset.Now.AddHours(3)
            };
            var token = _file.GetSharedAccessSignature(sasPermissions);
            var uri = _file.Uri + token;
            return uri;
        }
    }
}
