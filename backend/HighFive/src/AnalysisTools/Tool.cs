using src.AnalysisTools.AnalysisThread;

namespace src.AnalysisTools
{
    public abstract class Tool
    {
        public bool SeparateOutput;
        public abstract AnalysisOutput AnalyseFrame(byte[] frame);
    }
}