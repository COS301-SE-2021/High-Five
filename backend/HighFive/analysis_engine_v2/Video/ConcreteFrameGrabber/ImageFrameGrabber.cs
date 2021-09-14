using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace analysis_engine.Video
{
    public class ImageFrameGrabber : FrameGrabber
    {
        private Image<Rgb, byte> _image;
        public override void Init(string url)
        {
            _image=new Image<Rgb, byte>(url);
            if (_image.Width % 4 != 0)
            {
                _image=_image.Resize(_image.Width+(4-_image.Width%4), _image.Height, Inter.Area);
            }
        }

        public override Image<Rgb, byte> GetNextFrame()
        {
            var temp = _image;
            _image = null;
            return temp;
        }
    }
}