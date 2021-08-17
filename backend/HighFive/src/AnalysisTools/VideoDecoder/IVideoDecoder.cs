using System.Collections.Generic;
using System.Threading.Tasks;

namespace src.AnalysisTools.VideoDecoder
{
    public interface IVideoDecoder
    {
        public List<byte[]> GetFramesFromVideo(string path);
        public Task GetThumbnailFromVideo(string videoPath, string thumbnailPath);
    }
}