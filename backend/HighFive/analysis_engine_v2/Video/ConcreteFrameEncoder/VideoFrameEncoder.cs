using System.Drawing;
using System.Security.Policy;
using Emgu.CV;

namespace analysis_engine.Video.ConcreteFrameEncoder
{
    public class VideoFrameEncoder : FrameEncoder
    {
        private readonly VideoWriter _videoWriter;
        private readonly string _url;

        public VideoFrameEncoder(string url, int width, int height)
        {
            _url = url;
            _videoWriter = new VideoWriter(_url, VideoWriter.Fourcc('M', 'P', '4', 'V'), 30,
                new Size(width, height), true);
        }

        public override void AddFrame(Data data)
        {
            _videoWriter.Write(data.Frame.Image);
        }

        public override string GetVideo()
        {
            return _url;
        }
    }
}