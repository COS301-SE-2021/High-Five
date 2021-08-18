using System.Collections.Generic;
using Microsoft.ML.OnnxRuntime;
using src.AnalysisTools.AnalysisThread;

namespace src.AnalysisTools
{
    public abstract class Tool
    {
        public bool SeparateOutput;
        public abstract AnalysisOutput AnalyseFrame(byte[] frame);

        public abstract IDisposableReadOnlyCollection<DisposableNamedOnnxValue> ProcessFrame(
            List<NamedOnnxValue> modelInput);

        public abstract List<NamedOnnxValue> PreprocessFrame(byte[] frame);

        public abstract AnalysisOutput PostprocessFrame(IDisposableReadOnlyCollection<DisposableNamedOnnxValue> result);
    }
}