namespace analysis_engine.Video
{
    public abstract class FrameEncoder
    {
        public abstract void AddFrame(Data data);

        public abstract string GetVideo();

        public abstract void Dispose();
    }
}