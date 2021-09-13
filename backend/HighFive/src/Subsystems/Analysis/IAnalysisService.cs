using System.Threading.Tasks;
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
        public string ListenForMessage();
        public Task<LiveStreamingLinks> StartLiveStream(string userId);
    }
}