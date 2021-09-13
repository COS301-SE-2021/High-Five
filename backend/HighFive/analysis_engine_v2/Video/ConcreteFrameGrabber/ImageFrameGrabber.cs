using Emgu.CV;
using Emgu.CV.Structure;

namespace analysis_engine.Video
{
    public class ImageFrameGrabber : FrameGrabber
    {
        private Image<Rgb, byte> _image;
        public override void Init(string url)
        {
            _image=new Image<Rgb, byte>(url);
        }

        public override Image<Rgb, byte> GetNextFrame()
        {
            var temp = _image;
            _image = null;
            return temp;
        }
    }
}