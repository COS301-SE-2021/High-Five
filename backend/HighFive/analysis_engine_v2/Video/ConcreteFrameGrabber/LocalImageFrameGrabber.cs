using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;

namespace analysis_engine.Video
{
    public class LocalImageFrameGrabber : FrameGrabber
    {
        private Image<Rgb, byte> _image;
        public override void Init(string url)
        {
            _image = CvInvoke.Imread(url).ToImage<Rgb, byte>();
        }

        public override void Init(Stream input)
        {
            throw new System.NotImplementedException();
        }

        public override Image<Rgb, byte> GetNextFrame()
        {
            var temp = _image;
            _image = null;
            return temp;
        }
    }
}