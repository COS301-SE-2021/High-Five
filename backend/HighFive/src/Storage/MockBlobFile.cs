using Azure.Storage.Blobs.Models;

namespace src.Storage
{
    public class MockBlobFile
    {
        /*
         *      Description:
         * This is mock instantiation of the IBlobFile interface that will allow unit and integration tests
         * to be employed using a mocked storage.
         *
         *      Attributes:
         * -> _file: this is a reference to the Blob from storage that is currently being handled.
         * -> Properties: this variable contains the properties of the CloudBlockBlob from file
        */
        
        public BlobProperties Properties { get; }
        public string Name { get; }

    }
}