using System.Threading;
using System.Collections.Concurrent;

namespace src.AnalysisTools.AnalysisThread
{
    public class ToolRunner
    {
        private BlockingCollection<object> _frames = new BlockingCollection<object>();
        private ITool Tool;
        private object OutputQueue;//Type may change
 
        public ToolRunner(ITool tool, object outputQueue)
        {
            OutputQueue = outputQueue;
            Tool = tool;
            var thread = new Thread(new ThreadStart(OnStart));
            thread.IsBackground = true;
            thread.Start();
        }
 
        public void Enqueue(object frame)
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
                var analysedFrame=Tool.AnalyseFrame(frame);
                //Add to output queue
                //OutputQueue.Enqueue(analysedFrame);
            }
        }
    }
}