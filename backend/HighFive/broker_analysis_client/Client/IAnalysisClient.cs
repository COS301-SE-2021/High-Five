using broker_analysis_client.Client.Models;

namespace broker_analysis_client.Client
{
    public interface IAnalysisClient
    {
        public AnalyzedImageMetaData StoreImage(byte[] image);
        public AnalyzedVideoMetaData StoreVideo(byte[] video);
        public byte[] GetVideo(string videoId);
        public byte[] GetImage(string imageId);

        public string GetAnalysisTool();
        public string GetDrawingTool();
    }
}