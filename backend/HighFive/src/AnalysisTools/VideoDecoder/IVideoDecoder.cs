using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace src.AnalysisTools.VideoDecoder
{
    public interface IVideoDecoder
    {
        public List<Bitmap> GetFramesFromVideo(Stream video);
        public Task GetThumbnailFromVideo(string videoPath, string thumbnailPath);
    }
}