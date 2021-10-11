using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Accord.IO;
using FFMediaToolkit;
using FFMediaToolkit.Decoding;
using FFMediaToolkit.Encoding;
using FFMediaToolkit.Graphics;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using VideoCodec = Xabe.FFmpeg.VideoCodec;

namespace src.AnalysisTools.VideoDecoder
{
    public class VideoDecoder: IVideoDecoder
    {
        private static bool _ffmpegLoaded;
        private readonly string _ffMpegPath = Directory.GetCurrentDirectory() + "\\ffmpeg\\bin";//@"D:\ffmpeg\bin";

        public VideoDecoder()
        {
            if (!_ffmpegLoaded)
            {
                Xabe.FFmpeg.FFmpeg.SetExecutablesPath(_ffMpegPath);
                FFmpegLoader.FFmpegPath = _ffMpegPath;
                _ffmpegLoaded = true;
            }
        }
        
        public async Task GetThumbnailFromVideo(string videoPath, string thumbnailPath)
        {
            var info = await Xabe.FFmpeg.FFmpeg.GetMediaInfo(videoPath).ConfigureAwait(false);
            var videoStream = info.VideoStreams.First()?.SetCodec(VideoCodec.png);

            var conversionResult = await Xabe.FFmpeg.FFmpeg.Conversions.New()
                .AddStream(videoStream)
                .ExtractNthFrame(1, s => thumbnailPath)
                .Start();
        }

    }
}
