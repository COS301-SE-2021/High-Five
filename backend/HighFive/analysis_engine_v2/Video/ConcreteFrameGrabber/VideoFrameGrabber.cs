using System;
using Emgu.CV;
using Emgu.CV.Structure;

namespace analysis_engine.Video
{
    public class VideoFrameGrabber : FrameGrabber
    {
        public override void Init(string url)
        {
            Capture = new VideoCapture(url);
        }

        public override Image<Rgb, byte> GetNextFrame()
        {
            var frame = Capture.QueryFrame();
            return frame?.ToImage<Rgb, byte>();
        }
    }
}