using System.Collections.Concurrent;
using System.Threading;
using Microsoft.ML.OnnxRuntime;

namespace src.AnalysisTools.AnalysisThread
{
    public class PostprocessThread
    {
        private BlockingCollection<IDisposableReadOnlyCollection<DisposableNamedOnnxValue>> _inputs = new();
        private Tool _tool;
        private BlockingCollection<AnalysisOutput> _outputQueue;
        
        public PostprocessThread(Tool tool, BlockingCollection<AnalysisOutput> outputQueue)
        {
            var thread = new Thread(OnStart);
            thread.IsBackground = true;
            thread.Start();
            _tool = tool;
            _outputQueue = outputQueue;
        }
        
        public void Enqueue(IDisposableReadOnlyCollection<DisposableNamedOnnxValue> input)
        {
            //Used by main thread to add new frames
            _inputs.Add(input);
        }

        private void OnStart()
        {
            foreach (var input in _inputs.GetConsumingEnumerable(CancellationToken.None))
            {
                //ByteArray of length 1 indicates end of input
                if (input==null)
                {
                    break;
                }
                
                _outputQueue.Add(_tool.PostprocessFrame(input));
            }
        }
    }
}