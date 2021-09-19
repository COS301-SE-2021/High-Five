using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;

namespace analysis_engine.Video
{
    public abstract class FrameGrabber
    {
        protected VideoCapture Capture;

        public abstract void Init(string url);
        
        public abstract void Init(Stream input);

        public abstract Image<Rgb, byte> GetNextFrame();
    }
}