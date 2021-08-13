using System;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Org.OpenAPITools.Models;
using src.Subsystems.Analysis;

namespace src.AnalysisTools.AnalysisThread
{
    public class ToolRunner
    {
        private BlockingCollection<byte[]> _frames = new();
        private readonly AnalysisModels _tools;
        private Pipeline _pipeline;
        private object _outputQueue;//Type may change
 
        public ToolRunner(AnalysisModels tools, Pipeline analysisPipeline, BlockingCollection<byte[]> outputQueue, Pipeline pipeline)
        {
            _outputQueue = outputQueue;
            _pipeline = pipeline;
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
                var outputs = _pipeline.Tools.Select(tool => _tools.GetTool(tool).AnalyseFrame(frame)).ToList();
                
                //Draw Boxes
                var image = DrawBoxes(frame, outputs);

                //Add to output queue
                //OutputQueue.Enqueue(analysedFrame);
            }
        }

        private Image DrawBoxes(byte[] frame,List<AnalysisOutput> outputs)
        {
            Image outputFrame;
            using var ms = new MemoryStream(frame);
            outputFrame = Image.FromStream(ms);
            
            var oldWidth = outputFrame.Width;
            var oldHeight = outputFrame.Height;
            //initialise drawing tools
            var penWidth = Convert.ToInt32(Math.Max(oldHeight, oldWidth) * (1.0 / 445));
            var fontSize = Convert.ToSingle(Math.Max(oldHeight, oldWidth) * (1.0 / 89));
            var pen = new Pen(Color.Red,penWidth);
            var brush = Brushes.Red;
            var font = new Font(FontFamily.GenericSansSerif,fontSize);

            foreach (var output in outputs)
            {
                for (var i = 0; i < output.Classes.Count; i++)
                {
                    using var g = Graphics.FromImage(outputFrame);
                    var box = new Rectangle(Convert.ToInt32(output.Boxes[i * 4]),
                        Convert.ToInt32(output.Boxes[i * 4 + 1]),
                        Convert.ToInt32(output.Boxes[i * 4 + 2]),
                        Convert.ToInt32(output.Boxes[i * 4 + 3]));
                    g.DrawRectangle(pen, box);
                    g.DrawString(output.Classes[i], font, brush, Convert.ToSingle(output.Boxes[i * 4]),
                        Convert.ToSingle(output.Boxes[i * 4 + 1]) - 75);
                }
            }
            
            
            return outputFrame;
        }
    }
}