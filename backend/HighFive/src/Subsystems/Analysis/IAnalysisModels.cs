using src.AnalysisTools;

namespace src.Subsystems.Analysis
{
    public interface IAnalysisModels
    {
        public ITool GetTool(string toolName);
    }
}