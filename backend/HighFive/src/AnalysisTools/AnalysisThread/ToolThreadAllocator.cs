using System.Collections.Concurrent;
using System.Threading;

namespace src.AnalysisTools.AnalysisThread
{
    public class ToolThreadAllocator
    {
        private PreprocessThread _inputThread;

        public ToolThreadAllocator(Tool tool, BlockingCollection<AnalysisOutput> outputQueue)
        {
            var postProcessThread = new PostprocessThread(tool, outputQueue);
            var processThread = new ProcessThread(tool, postProcessThread);
            _inputThread = new PreprocessThread(tool, processThread);
        }
        
        public void Enqueue(byte[] frame)
        {
            //Used by main thread to add new frames
            _inputThread.Enqueue(frame);
        }
    }
}