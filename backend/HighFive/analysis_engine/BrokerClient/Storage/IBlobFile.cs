using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace broker_analysis_client.Storage
{
    public interface IBlobFile
    {
        public BlobProperties Properties { get; }
        public string Name { get; }
        public void AddMetadata(string key, string value);
        public string GetMetaData(string key);
        public Task UploadFileFromStream(Stream stream, string contentType="");
        public Task UploadFileFromByteArray(byte[] array, string contentType = "");
        public Task UploadText(string text);
        public Task Delete();
        public Task<bool> Exists();
        public Task<byte[]> ToByteArray();
        public Task<Stream> ToStream();
        public Task<string> ToText();
        public string GetUrl();
    }
}
