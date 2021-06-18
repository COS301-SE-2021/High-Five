using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Org.OpenAPITools.Models;

namespace src.Storage
{
    public interface IStorageManager
    {
        protected IConfiguration Configuration { get; }
        protected CloudStorageAccount CloudStorageAccount { get; }
        
        public Task<CloudBlockBlob> GetFile(string fileName, string container, bool create=false);
        public Task<List<CloudBlockBlob>> GetAllFilesInContainer(string container);
        public Task<CloudBlockBlob> CreateNewFile(string name, string container);
        public String RandomString();
        public String HashMd5(string source);
    }
}