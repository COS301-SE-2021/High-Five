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
    }
}