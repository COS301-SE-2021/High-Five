using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FFMediaToolkit.Decoding;
using Xabe.FFmpeg;

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
        public List<byte[]> GetFramesFromVideo(Stream videoStream)
        {
            var frameList = new List<byte[]>();
            
            var file = MediaFile.Open(videoStream);
            while (file.Video.TryGetNextFrame( out var imageData))
            {
                frameList.Add(imageData.Data.ToArray());
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
    }
}