using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Microsoft.ML.OnnxRuntime;

namespace src.AnalysisTools.AnalysisThread
{
    public class ProcessThread
    {
        private BlockingCollection<List<NamedOnnxValue>> _inputs = new();
        private Tool _tool;
        private PostprocessThread _outputThread;
        
        public ProcessThread(Tool tool, PostprocessThread outputThread)
        {
            var thread = new Thread(OnStart);
            thread.IsBackground = true;
            thread.Start();
            _tool = tool;
            _outputThread = outputThread;
        }
        
        public void Enqueue(List<NamedOnnxValue> input)
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
                    _outputThread.Enqueue(null);
                    break;
                }
                
                _outputThread.Enqueue(_tool.ProcessFrame(input));
            }
        }
    }
}