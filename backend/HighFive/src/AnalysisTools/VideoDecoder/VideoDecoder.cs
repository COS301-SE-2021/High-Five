using System;
using System.Collections.Generic;
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
        private readonly string _ffMpegPath = Directory.GetCurrentDirectory() + "\\ffmpegs\\ffmpeg\\bin";//@"D:\ffmpeg\bin";//TODO: Add ffmpeg path here
        private readonly string _ffMpegSharedPath = Directory.GetCurrentDirectory() + "\\ffmpegs\\ffmpegshared\\bin";//@"D:\ffmpegshared\bin";

        public VideoDecoder()
        {
            if (!_ffmpegLoaded)
            {
                Xabe.FFmpeg.FFmpeg.SetExecutablesPath(_ffMpegPath);
                FFmpegLoader.FFmpegPath = _ffMpegSharedPath;
                _ffmpegLoaded = true;
            }
        }
        
        [SuppressMessage("ReSharper.DPA", "DPA0003: Excessive memory allocations in LOH", MessageId = "type: System.Byte[]")]
        public List<Stream> GetFramesFromVideo(Stream videoStream)
        {
            var frameList = new List<Stream>();
            
            var file = MediaFile.Open(videoStream);
            while (file.Video.TryGetNextFrame( out var imageData))
            {
                var bmp = ToBitmap(imageData);
                var ms = new MemoryStream();
                bmp.Save(ms, ImageFormat.Bmp);
                frameList.Add(ms);
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

        public byte[] EncodeVideoFromFrames(List<byte[]> frameList, Stream originalVideoStream)
        {
            var originalVideo = MediaFile.Open(originalVideoStream);
            var avgFrameRate = originalVideo.Video.Info.AvgFrameRate;
            var height = originalVideo.Video.Info.FrameSize.Height;
            var width = originalVideo.Video.Info.FrameSize.Width;
            const FFMediaToolkit.Encoding.VideoCodec codec = FFMediaToolkit.Encoding.VideoCodec.H264;

            var settings = new VideoEncoderSettings(width, height, (int) avgFrameRate, codec)
            {
                EncoderPreset = EncoderPreset.Fast, CRF = 17
            };

            var basePath = Path.GetTempPath();
            using var file = MediaBuilder.CreateContainer(basePath + "\\analyzedVideo.mp4").WithVideo(settings)
                .Create();
            foreach (var frame in frameList)
            {
                using var stream = new MemoryStream(frame);
                var bmp = Image.FromStream(stream) as Bitmap;
                if (bmp == null) continue;
                var rect = new Rectangle(Point.Empty, bmp.Size);
                var bitLock = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                var imgData = ImageData.FromPointer(bitLock.Scan0, ImagePixelFormat.Bgr24, bmp.Size);
                file.Video.AddFrame(imgData);
                bmp.UnlockBits(bitLock);
            }

            file.Dispose();
            var fileStream = File.Open(basePath + "\\analyzedVideo.mp4", FileMode.Open);
            var videoBytes = new byte[fileStream.Length];
            fileStream.Read(videoBytes, 0, videoBytes.Length);
            return videoBytes;
        }

        private static unsafe Bitmap ToBitmap(ImageData bitmap)
        {
            fixed(byte* p = bitmap.Data)
            {
                var map = new Bitmap(bitmap.ImageSize.Width, bitmap.ImageSize.Height, bitmap.Stride, PixelFormat.Format24bppRgb, new IntPtr(p));
                return map;
            }
        }

    }
}