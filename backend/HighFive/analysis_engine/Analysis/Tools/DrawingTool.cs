using analysis_engine.Analysis.Tools;
using analysis_engine.Analysis.Util.Data;

namespace analysis_engine.Tools
{
    public abstract class DrawingTool : Tool
    {
        public abstract Data Process(Data data);
        public abstract void Init();
    }
}