namespace analysis_engine
{
    public abstract class AnalysisTool : Tool
    {
        public abstract Data Process(Data data);
        public abstract void Init();
    }
}