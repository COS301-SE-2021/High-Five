using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.Blob;

namespace src.Storage
{
    public interface IBlobFile
    {
        public BlobProperties Properties { get; }
        public string Name { get; }
        public void AddMetadata(string key, string value);
        public string GetMetaData(string key);
        public Task UploadFile(IFormFile newFile);
        public Task UploadFile(string path, string contentType="");
        public Task UploadText(string text);
        public Task Delete();
        public Task<bool> Exists();
        public Task<byte[]> ToByteArray();
        public Task<Stream> ToStream();
        public Task<string> ToText();
        public string GetUrl();
    }
}
