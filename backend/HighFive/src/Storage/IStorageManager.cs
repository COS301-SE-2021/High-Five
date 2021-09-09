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
        public Task<IBlobFile> GetFile(string fileName, string container, bool create=false);
        public Task<List<IBlobFile>> GetAllFilesInContainer(string container);
        public Task<IBlobFile> CreateNewFile(string name, string container);
        public string RandomString(int length=5);
        public string HashMd5(string source);
        public Task<bool> SetBaseContainer(string container);
        public bool IsContainerSet();
        public string GetCurrentContainer();
        public void StoreUserInfo(string id, string displayName, string email);
        public Task<List<User>> GetAllUsers();
        public Task DeleteAllFilesInContainer(string container);
    }
}
