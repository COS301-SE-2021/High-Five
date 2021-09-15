using System.Drawing;
using Emgu.CV;

namespace analysis_engine.Video.ConcreteFrameEncoder
{
    public class StreamFrameEncoder : FrameEncoder
    {
        private readonly VideoWriter _videoWriter;
        private readonly string _url;

        public StreamFrameEncoder(string url, Size size)
        {
            _url = url;
            _videoWriter = new VideoWriter(_url, VideoWriter.Fourcc('m', 'p', '4', 'v'), 30,
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
    }
}