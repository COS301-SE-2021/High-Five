namespace src.AnalysisTools.AnalysisThread
{
    public interface IToolRunner
    {
        public void Enqueue(byte[] frame);
    }
}