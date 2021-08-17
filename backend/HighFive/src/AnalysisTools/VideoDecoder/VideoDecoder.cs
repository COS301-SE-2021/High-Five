using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace src.AnalysisTools.VideoDecoder
{
    public class VideoDecoder: IVideoDecoder
    {

        public VideoDecoder()
        {
            const string ffMpegPath = @"D:\ffmpeg\bin";//TODO: Add ffmpeg path here
            FFmpeg.SetExecutablesPath(ffMpegPath, "ffmpeg");
        }
        
        public List<byte[]> GetFramesFromVideo(string path)
        {
            var frameList = new List<byte[]>();

            

            return frameList;
        }

        public async void GetThumbnailFromVideo(string videoPath, string thumbnailPath)
        {
            var info = await FFmpeg.GetMediaInfo(videoPath).ConfigureAwait(false);
            var videoStream = info.VideoStreams.First()?.SetCodec(VideoCodec.png);

            var conversionResult = await FFmpeg.Conversions.New()
                .AddStream(videoStream)
                .ExtractNthFrame(1, s => thumbnailPath)
                .Start();
        }
    }
}