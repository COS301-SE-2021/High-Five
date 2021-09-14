using System.Threading.Tasks;
using IronPython.Modules;
using Org.OpenAPITools.Models;
using src.Subsystems.Livestreaming;
using src.Websockets;

namespace src.Subsystems.Analysis
{
    public interface IAnalysisService
    {
        public Task<AnalyzedImageMetaData> AnalyzeImage(SocketRequest fullRequest);
        public Task<AnalyzedVideoMetaData> AnalyzeVideo(SocketRequest fullRequest);
        public void SetBaseContainer(string containerName);
        public void SetBrokerToken(string userId);
        public void CloseBrokerSocket();
        public Task<bool> StartLiveStream(string userId);
        public Task<bool> Synchronise(SocketRequest fullRequest);
    }
}