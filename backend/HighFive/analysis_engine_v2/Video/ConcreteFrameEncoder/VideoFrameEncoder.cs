using System.Drawing;
using System.Security.Policy;
using Emgu.CV;

namespace analysis_engine.Video.ConcreteFrameEncoder
{
    public class VideoFrameEncoder : FrameEncoder
    {
        private readonly VideoWriter _videoWriter;
        private readonly string _url;

        public VideoFrameEncoder(string url, Size size)
        {
            _url = url;
            _videoWriter = new VideoWriter(_url, VideoWriter.Fourcc('h', '2', '6', '4'), 30,
                size, true);
        }

        public override void AddFrame(Data data)
        {
            _videoWriter.Write(data.Frame.Image);
        }

        public override string GetVideo()
        {
            return _url;
        }

        public override void Dispose()
        {
            _videoWriter.Dispose();
        }
    }
}