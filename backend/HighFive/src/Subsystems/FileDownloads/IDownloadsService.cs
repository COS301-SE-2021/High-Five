using Org.OpenAPITools.Models;

namespace src.Subsystems.FileDownloads
{
    public interface IDownloadsService
    {
        public FileDownload DownloadApk();
        public FileDownload DownloadSdkManual();
        public DownloadSdkFilesResponse DownloadSdkFiles();
    }
}