using System.Threading;
using System.Collections.Concurrent;

namespace src.AnalysisTools.AnalysisThread
{
    public class ToolRunner
    {
        private BlockingCollection<object> _frames = new BlockingCollection<object>();
 
        public ToolRunner()
        {
            var thread = new Thread(new ThreadStart(OnStart));
            thread.IsBackground = true;
            thread.Start();
        }
 
        public void Enqueue(object job)
        {
            _frames.Add(job);
        }
 
        private void OnStart()
        {
            //Load model here
            foreach (var frame in _frames.GetConsumingEnumerable(CancellationToken.None))
            {
                //Analyse images
                //Add to output queue
            }
        }
    }
}