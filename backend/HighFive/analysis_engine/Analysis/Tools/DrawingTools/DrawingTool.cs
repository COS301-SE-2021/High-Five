using analysis_engine.Analysis.Util.Data;
using Emgu.CV;
using Emgu.CV.Structure;

namespace analysis_engine.Analysis.Tools.DrawingTools
{
    public interface DrawingTool
    {
        public Image<Rgb, byte> Draw(Image<Rgb, byte> image, Data data);
    }
}