using analysis_engine.Analysis.Util.Data;
using Emgu.CV;
using Emgu.CV.Structure;

namespace analysis_engine.Analysis.Tools.DrawingTools
{
    public interface DrawingTool
    {
        public Data Draw(Data data);
    }
}