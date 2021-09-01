namespace analysis_engine.Util
{
    public class Frame
    {
        private byte[] bitmap;
        private int frameID;
        public Frame(byte[] bitmap, int frameId)
        {
            refreshFrame(bitmap, frameId);
        }

        public Frame()
        {
        }

        public void refreshFrame(byte[] bitmap, int frameId)
        {
            this.bitmap = bitmap;
            frameID = frameId;
        }

    }
}