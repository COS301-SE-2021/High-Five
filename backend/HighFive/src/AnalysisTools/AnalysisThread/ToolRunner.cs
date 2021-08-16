using System;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Org.OpenAPITools.Models;
using src.AnalysisTools.MiscTools;
using src.Subsystems.Analysis;

namespace src.AnalysisTools.AnalysisThread
{
    public class ToolRunner:IToolRunner
    {
        private BlockingCollection<byte[]> _frames = new();
        private readonly List<Tool> _tools;
        private BlockingCollection<byte[]> _outputQueue;
        private readonly ImageFormat _frameFormat = ImageFormat.Jpeg;//Format of output is changed here
 
        public ToolRunner(List<Tool> tools, BlockingCollection<byte[]> outputQueue)
        {
            _outputQueue = outputQueue;
            _tools = tools;
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
            //Loop keeps checking queue for new frames to analyse
            foreach (var frame in _frames.GetConsumingEnumerable(CancellationToken.None))
            {
                //ByteArray of length 1 indicates end of input
                if (frame.Length == 1) break;
                
                //Analyse images
                var outputs = _tools.Select(tool => tool.AnalyseFrame(frame)).ToList();
                
                //Draw Boxes
                var image = BoxDrawer.DrawBoxes(frame, outputs);
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