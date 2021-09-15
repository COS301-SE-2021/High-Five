using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VideoCodec = Xabe.FFmpeg.VideoCodec;

namespace src.AnalysisTools.VideoDecoder
{
    public class VideoDecoder: IVideoDecoder
    {
        private static bool _ffmpegLoaded;
        private readonly string _ffMpegPath = Directory.GetCurrentDirectory() + @"..\..\..\ffmpeg\bin";

        public VideoDecoder()
        {
            if (!_ffmpegLoaded)
            {
                Xabe.FFmpeg.FFmpeg.SetExecutablesPath(_ffMpegPath);
                _ffmpegLoaded = true;
            }
        }
        
        public async Task GetThumbnailFromVideo(string videoPath, string thumbnailPath)
        {
            var info = await Xabe.FFmpeg.FFmpeg.GetMediaInfo(videoPath).ConfigureAwait(false);
            var videoStream = info.VideoStreams.First()?.SetCodec(VideoCodec.h264);

            var conversionResult = await Xabe.FFmpeg.FFmpeg.Conversions.New()
                .AddStream(videoStream)
                .ExtractNthFrame(1, s => thumbnailPath)
                .Start();
        }

    }
}
