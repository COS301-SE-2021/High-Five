using System;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Org.OpenAPITools.Models;
using src.AnalysisTools.MiscTools;
using src.Subsystems.Analysis;

namespace src.AnalysisTools.AnalysisThread
{
    public class ToolRunner
    {
        private BlockingCollection<byte[]> _frames = new();
        private readonly List<Tool> _tools;
        private BlockingCollection<byte[]> _outputQueue;
        private readonly ImageFormat _frameFormat = ImageFormat.Jpeg;//Format of output is changed here
        private List<BlockingCollection<AnalysisOutput>> _toolOutputs;
        private List<ToolThreadAllocator> _toolThreadAllocators;

        public ToolRunner(List<Tool> tools, BlockingCollection<byte[]> outputQueue)
        {
            _outputQueue = outputQueue;
            _tools = tools;


            var thread = new Thread(OnStart);
            thread.IsBackground = true;
            thread.Start();

            _toolOutputs = new List<BlockingCollection<AnalysisOutput>>();
            _toolThreadAllocators = new List<ToolThreadAllocator>();
            foreach (var tool in _tools)
            {
                _toolOutputs.Add(new BlockingCollection<AnalysisOutput>());
                _toolThreadAllocators.Add(new ToolThreadAllocator(tool, _toolOutputs.Last()));
            }

        }

        public void Enqueue(byte[] frame)
        {
            //Used by main thread to add new frames
            _frames.Add(frame);
        }
 
        private void OnStart()
        {
            
            //Loop keeps checking queue for new frames to analyse
            foreach (var frame in _frames.GetConsumingEnumerable(CancellationToken.None))
            {
                foreach (var toolAlloc in _toolThreadAllocators)
                {
                    toolAlloc.Enqueue(frame);
                }
                if (frame.Length == 1) break;
                List<AnalysisOutput> frameOutput = new List<AnalysisOutput>();
                for (var i = 0; i < _toolOutputs.Count; i++)
                {
                    foreach (var output in _toolOutputs[i].GetConsumingEnumerable(CancellationToken.None))
                    {
                        frameOutput.Add(output);
                        break;
                    }
                }
                
                
                
                var image = BoxDrawer.DrawBoxes(frame, frameOutput);
                byte[] outputFrame;
                using (var ms = new MemoryStream())
                {
                    image.Save(ms, _frameFormat);
                    outputFrame =  ms.ToArray();
                }
                
                //Add to output queue
                _outputQueue.Add(outputFrame);
            }
            
        }
    }
}