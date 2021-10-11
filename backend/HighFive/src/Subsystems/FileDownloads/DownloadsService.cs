using Org.OpenAPITools.Models;
using src.Storage;

namespace src.Subsystems.FileDownloads
{
    public class DownloadsService: IDownloadsService
    {
        private readonly IStorageManager _storageManager;

        public DownloadsService(IStorageManager storageManager)
        {
            _storageManager = storageManager;
        }
        
        public FileDownload DownloadApk()
        {
            var currentContainer = _storageManager.GetCurrentContainer();
            _storageManager.SetBaseContainer("public");
            var apkFile = _storageManager.GetFile("HighFive.apk", "sdk").Result;
            _storageManager.SetBaseContainer(currentContainer);
            var response = new FileDownload {FileUrl = apkFile.GetUrl()};
            return response;
        }

        public FileDownload DownloadSdkManual()
        {
            var currentContainer = _storageManager.GetCurrentContainer();
            _storageManager.SetBaseContainer("public");
            var sdkManual = _storageManager.GetFile("HighFiveSdkManual.pdf", "sdk").Result;
            _storageManager.SetBaseContainer(currentContainer);
            var response = new FileDownload {FileUrl = sdkManual.GetUrl()};
            return response;
        }

        public DownloadSdkFilesResponse DownloadSdkFiles()
        {
            var currentContainer = _storageManager.GetCurrentContainer();
            _storageManager.SetBaseContainer("public");
            var sdkFiles = _storageManager.GetFile("high5_sdk.zip", "sdk").Result;
            _storageManager.SetBaseContainer(currentContainer);
            var response = new DownloadSdkFilesResponse
            {
                SdkZip = new FileDownload{FileUrl = sdkFiles.GetUrl()}
            };
            return response;
        }
    }
}