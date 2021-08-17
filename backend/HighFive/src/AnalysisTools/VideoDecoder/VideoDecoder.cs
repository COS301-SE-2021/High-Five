using System;
using System.Collections.Generic;
using System.IO;
using NReco.VideoConverter;

namespace src.AnalysisTools.VideoDecoder
{
    public class VideoDecoder: IVideoDecoder
    {
        private FFMpegConverter _ffmpeg;
        
        public VideoDecoder()
        {
            _ffmpeg = new FFMpegConverter();
        }
        
        public List<byte[]> GetFramesFromVideo(string path)
        {
            var frameList = new List<byte[]>();
            try
            {
                for (var timeStamp = 0.0f; timeStamp > 0; timeStamp += 0.03f)
                {
                    var ms = new MemoryStream();
                    _ffmpeg.GetVideoThumbnail(path, ms, timeStamp);
                    frameList.Add(ms.ToArray());
                }
            }
            catch (Exception e)
            {
                // ignored
            }

            return frameList;
        }

        public void GetThumbnailFromVideo(string videoPath, string thumbnailPath)
        {
            _ffmpeg.GetVideoThumbnail(videoPath, thumbnailPath);
        }
    }
}