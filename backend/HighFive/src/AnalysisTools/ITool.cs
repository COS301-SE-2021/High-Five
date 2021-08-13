using src.AnalysisTools.AnalysisThread;

namespace src.AnalysisTools
{
    public interface ITool
    {
        public AnalysisOutput AnalyseFrame(byte[] frame);
    }
    
    
}