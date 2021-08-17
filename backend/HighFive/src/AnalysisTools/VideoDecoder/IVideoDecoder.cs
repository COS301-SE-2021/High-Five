using System.Collections.Generic;

namespace src.AnalysisTools.VideoDecoder
{
    public interface IVideoDecoder
    {
        public List<byte[]> GetFramesFromVideo(string path);
        public void GetThumbnailFromVideo(string videoPath, string thumbnailPath);
    }
}