using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace src.AnalysisTools.VideoDecoder
{
    public interface IVideoDecoder
    {
        public List<Stream> GetFramesFromVideo(Stream video);
        public Task GetThumbnailFromVideo(string videoPath, string thumbnailPath);
        public byte[] EncodeVideoFromFrames(List<byte[]> frameList, Stream originalVideo);
    }
}