using Emgu.CV;
using Emgu.CV.Structure;

namespace analysis_engine.Video
{
    public class StreamFrameGrabber : FrameGrabber
    {
        public override void Init(string url)
        {
            Capture = new VideoCapture(url);
        }

        public override Image<Rgb, byte> GetNextFrame()
        {
            return Capture.QueryFrame().ToImage<Rgb, byte>();
        }
    }
}