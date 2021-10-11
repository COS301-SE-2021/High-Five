using Emgu.CV;
using Emgu.CV.Structure;

namespace High5SDK
{
    public class Frame
    {
        public Image<Rgb, byte> Image { get; set; }
        public int FrameID { get; set; }

        public Frame(Image<Rgb, byte> image, int frameId)
        {
            RefreshFrame(image, frameId);
        }

        public Frame()
        {
        }

        public void RefreshFrame(Image<Rgb, byte> image, int frameId)
        {
            this.Image = image;
            FrameID = frameId;
        }

    }
}