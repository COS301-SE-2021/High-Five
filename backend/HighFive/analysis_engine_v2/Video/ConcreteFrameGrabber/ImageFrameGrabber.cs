using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
            // _image=CvInvoke.Imread(url).ToImage<Rgb,byte>();
            // _image = new Image<Rgb, byte>(url);
            Capture = new VideoCapture(url);
            Capture.Start();
            _image = Capture.QueryFrame().ToImage<Rgb, byte>();
            Capture.Stop();
            if (_image.Width % 4 != 0)
            {
                _image=_image.Resize(_image.Width+(4-_image.Width%4), _image.Height, Inter.Area);
            }
        }
        public override void Init(Stream input)
        {
            // _image=CvInvoke.Imread(url).ToImage<Rgb,byte>();
            // _image = new Image<Rgb, byte>(url);
            var bitmapImage = new Bitmap(input);

            Rectangle rectangle = new Rectangle(0, 0, bitmapImage.Width, bitmapImage.Height);//System.Drawing
            BitmapData bmpData = bitmapImage.LockBits(rectangle, ImageLockMode.ReadWrite, bitmapImage.PixelFormat);//System.Drawing.Imaging

            _image = new Image<Rgb, byte>(bitmapImage.Width, bitmapImage.Height, bmpData.Stride, bmpData.Scan0);
            
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