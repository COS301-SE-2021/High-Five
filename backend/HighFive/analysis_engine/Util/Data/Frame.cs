namespace analysis_engine.Util
{
    public class Frame
    {
        public byte[] Bitmap {get; set; }
        public int FrameID {get; set; }
        
        public Frame(byte[] bitmap, int frameId)
        {
            RefreshFrame(bitmap, frameId);
        }

        public Frame()
        {
        }

        public void RefreshFrame(byte[] bitmap, int frameId)
        {
            this.Bitmap = bitmap;
            FrameID = frameId;
        }

    }
}