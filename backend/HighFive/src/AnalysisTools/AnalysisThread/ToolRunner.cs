using System.Threading;
using System.Collections.Concurrent;
using src.Subsystems.Analysis;

namespace src.AnalysisTools.AnalysisThread
{
    public class ToolRunner
    {
        private BlockingCollection<byte[]> _frames = new();
        private AnalysisModels _tools;
        private object _outputQueue;//Type may change
 
        public ToolRunner(AnalysisModels tools, BlockingCollection<byte[]> outputQueue)
        {
            _outputQueue = outputQueue;
            _tools = tools;
            var thread = new Thread(new ThreadStart(OnStart));
            thread.IsBackground = true;
            thread.Start();
        }
 
        public void Enqueue(byte[] frame)//This function may not work if called from another thread and we will have to access the queue directly
        {
            //Used by main thread to add new frames
            _frames.Add(frame);
        }
 
        private void OnStart()
        {
            //Loop keeps checking queue for new frames to analyse
            foreach (var frame in _frames.GetConsumingEnumerable(CancellationToken.None))
            {
                //Analyse images
                // var analysedFrame=Tool.AnalyseFrame(frame);
                //Add to output queue
                //OutputQueue.Enqueue(analysedFrame);
            }
        }
    }
}