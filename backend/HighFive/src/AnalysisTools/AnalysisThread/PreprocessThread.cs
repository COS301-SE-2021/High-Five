using System.Collections.Concurrent;
using System.Threading;

namespace src.AnalysisTools.AnalysisThread
{
    public class PreprocessThread
    {
        private BlockingCollection<byte[]> _frames = new();
        private Tool _tool;
        private ProcessThread _outputThread;
        
        public PreprocessThread(Tool tool, ProcessThread outputThread)
        {
            var thread = new Thread(OnStart);
            thread.IsBackground = true;
            thread.Start();
            _tool = tool;
            _outputThread = outputThread;
        }
        
        public void Enqueue(byte[] frame)
        {
            //Used by main thread to add new frames
            _frames.Add(frame);
        }

        private void OnStart()
        {
            foreach (var frame in _frames.GetConsumingEnumerable(CancellationToken.None))
            {
                //ByteArray of length 1 indicates end of input
                if (frame.Length == 1)
                {
                    _outputThread.Enqueue(null);
                    break;
                }
                
                _outputThread.Enqueue(_tool.PreprocessFrame(frame));
            }
        }
    }
}