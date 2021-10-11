using Emgu.CV;
using High5SDK;

namespace analysis_engine.Video.ConcreteFrameEncoder
{
    public class ImageFrameEncoder : FrameEncoder
    {
        private string _url;
        public ImageFrameEncoder(string url)
        {
            _url = url;
        }
        public override void AddFrame(Data data)
        {
            if (data != null)
            {
                CvInvoke.Imwrite(_url, data.Frame.Image);
            }
        }

        public override string GetVideo()
        {
            return _url;
        }

        public override void Dispose()
        {
            
        }
    }
}