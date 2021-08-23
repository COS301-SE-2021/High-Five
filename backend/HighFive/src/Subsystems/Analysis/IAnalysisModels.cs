using src.AnalysisTools;

namespace src.Subsystems.Analysis
{
    public interface IAnalysisModels
    {
        public Tool GetTool(string toolName);
    }
}