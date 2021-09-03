using analysis_engine.Analysis.Util.Data;
using analysis_engine.Util;

namespace analysis_engine.Analysis.Tools
{
    public interface Tool
    {
        public static Buffer Buffer;
        public Data Process(Data data);
        public void Init();
    }
}