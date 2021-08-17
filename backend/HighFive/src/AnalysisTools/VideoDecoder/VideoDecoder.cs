using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FFMediaToolkit.Decoding;
using FFMediaToolkit.Graphics;
using Xabe.FFmpeg;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace src.AnalysisTools.VideoDecoder
{
    public class VideoDecoder: IVideoDecoder
    {

        public VideoDecoder()
        {
            const string ffMpegPath = @"D:\ffmpeg\bin";//TODO: Add ffmpeg path here
            const string ffMpegSharedPath = @"D:\ffmpegshared\bin";
            Xabe.FFmpeg.FFmpeg.SetExecutablesPath(ffMpegPath, "ffmpeg");
            FFMediaToolkit.FFmpegLoader.FFmpegPath = ffMpegSharedPath;
        }
        
        [SuppressMessage("ReSharper.DPA", "DPA0003: Excessive memory allocations in LOH", MessageId = "type: System.Byte[]")]
        public List<Bitmap> GetFramesFromVideo(Stream videoStream)
        {
            var frameList = new List<Bitmap>();
            
            var file = MediaFile.Open(videoStream);
            while (file.Video.TryGetNextFrame( out var imageData))
            {
                frameList.Add(ToBitmap(imageData));
            }

            return frameList;
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
        
        private static unsafe Bitmap ToBitmap(ImageData bitmap)
        {
            fixed(byte* p = bitmap.Data)
            {
                return new Bitmap(bitmap.ImageSize.Width, bitmap.ImageSize.Height, bitmap.Stride, PixelFormat.Format24bppRgb, new IntPtr(p));
            }
        }
    }
}