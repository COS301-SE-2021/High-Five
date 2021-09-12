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
            var boxCoordinateFile = _storageManager.GetFile("BoxCoordinateData.cs", "sdk").Result;
            var toolInterfaceFile = _storageManager.GetFile("Tool.cs", "sdk").Result;
            var analysisToolAbstractClassFile = _storageManager.GetFile("AnalysisTool.cs", "sdk").Result;
            var drawingToolAbstractClassFile = _storageManager.GetFile("DrawingTool.cs", "sdk").Result;
            var metaDataClassFile = _storageManager.GetFile("MetaData.cs", "sdk").Result;
            var dataClassFile = _storageManager.GetFile("Data.cs", "sdk").Result;
            _storageManager.SetBaseContainer(currentContainer);
            var response = new DownloadSdkFilesResponse
            {
                BoxCoordinatesClass = new FileDownload{FileUrl = boxCoordinateFile.GetUrl()},
                ToolInterface = new FileDownload{FileUrl = toolInterfaceFile.GetUrl()},
                AnalysisToolAbstractClass = new FileDownload{FileUrl = analysisToolAbstractClassFile.GetUrl()},
                DrawingToolAbstractClass = new FileDownload{FileUrl = drawingToolAbstractClassFile.GetUrl()},
                MetaDataClass = new FileDownload{FileUrl = metaDataClassFile.GetUrl()},
                DataClass = new FileDownload{FileUrl = dataClassFile.GetUrl()}
            };
            return response;
        }
    }
}