using System.Collections.Concurrent;
using System.Threading;

namespace src.AnalysisTools.AnalysisThread
{
    public class ToolThreadAllocator
    {
        private BlockingCollection<byte[]> _frames = new();
        
        public ToolThreadAllocator(Tool tool, BlockingCollection<AnalysisOutput> outputQueue)
        {
            var thread = new Thread(OnStart);
            thread.IsBackground = true;
            thread.Start();
        }
        
        public void Enqueue(byte[] frame)
        {
            //Used by main thread to add new frames
            _frames.Add(frame);
        }

        private void OnStart()
        {
            // foreach (var frame in _frames.GetConsumingEnumerable(CancellationToken.None))
            // {
            //     //ByteArray of length 1 indicates end of input
            //     if (frame.Length == 1) break;
            //     
            //     //Analyse images
            //     var outputs = _tools.Select(tool => tool.AnalyseFrame(frame)).ToList();
            //     
            //     //Draw Boxes
            //     var image = BoxDrawer.DrawBoxes(frame, outputs);
            //     byte[] outputFrame;
            //     using (var ms = new MemoryStream())
            //     {
            //         image.Save(ms, _frameFormat);
            //         outputFrame =  ms.ToArray();
            //     }
            //     
            //     //Add to output queue
            //     _outputQueue.Add(outputFrame);
            // }
        }
    }
}