namespace analysis_engine
{
    public abstract class DrawingTool : Tool
    {
        public abstract Data Process(Data data);
        public abstract void Init();
    }
}