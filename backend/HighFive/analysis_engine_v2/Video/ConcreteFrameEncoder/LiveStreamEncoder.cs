using System.Reflection;
using High5SDK;

namespace analysis_engine.Video.ConcreteFrameEncoder
{
    public class LiveStreamEncoder : FrameEncoder
    {
        private Buffer _buffer;
        private string _destination;
        private bool _running;
        
        public LiveStreamEncoder(string url)
        {
            _destination = url;
            _buffer = new RingBuffer(500);
            _running = true;
        }
        public override void AddFrame(Data data)
        {
            _buffer.Push(data);
        }

        public override string GetVideo()
        {
            throw new System.NotImplementedException();
        }

        public override void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}