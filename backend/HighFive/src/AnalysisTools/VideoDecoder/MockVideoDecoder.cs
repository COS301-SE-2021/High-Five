using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace src.AnalysisTools.VideoDecoder
{
    public class MockVideoDecoder: IVideoDecoder
    {
        public List<Stream> GetFramesFromVideo(Stream video)
        {
            return new();
        }

        public async Task GetThumbnailFromVideo(string videoPath, string thumbnailPath)
        {
        }

        public byte[] EncodeVideoFromFrames(List<byte[]> frameList, Stream originalVideo)
        {
            return new byte[5];
        }
    }
}