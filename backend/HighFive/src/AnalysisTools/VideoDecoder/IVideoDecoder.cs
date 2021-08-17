using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace src.AnalysisTools.VideoDecoder
{
    public interface IVideoDecoder
    {
        public List<byte[]> GetFramesFromVideo(Stream video);
        public Task GetThumbnailFromVideo(string videoPath, string thumbnailPath);
    }
}