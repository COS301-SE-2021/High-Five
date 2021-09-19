using System;
using System.IO;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace analysis_engine.Video
{
    public class VideoFrameGrabber : FrameGrabber
    {
        public override void Init(string url)
        {
            Capture = new VideoCapture(url);
        }
        
        public override void Init(Stream input)
        {
        }

        public override Image<Rgb, byte> GetNextFrame()
        {
            var frame = Capture.QueryFrame();
            if (frame == null) return null;
            var image=frame.ToImage<Rgb, byte>();
            if (image.Width % 4 != 0)
            {
                image=image.Resize(image.Width+(4-image.Width%4), image.Height, Inter.Area);
            }

            return image;
        }
    }
}