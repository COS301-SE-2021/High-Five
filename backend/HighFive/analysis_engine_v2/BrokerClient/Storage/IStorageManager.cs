using System.Collections.Generic;
using System.Threading.Tasks;

namespace broker_analysis_client.Storage
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
        public Task DeleteAllFilesInContainer(string container);
    }
}
