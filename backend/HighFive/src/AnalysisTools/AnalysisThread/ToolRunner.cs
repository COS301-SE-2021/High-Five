using System;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Org.OpenAPITools.Models;
using src.Subsystems.Analysis;

namespace src.AnalysisTools.AnalysisThread
{
    public class ToolRunner
    {
        private BlockingCollection<byte[]> _frames = new();
        private readonly IAnalysisModels _tools;
        private Pipeline _pipeline;
        private BlockingCollection<byte[]> _outputQueue;
        private readonly ImageFormat _frameFormat = ImageFormat.Jpeg;//Format of output is changed here
 
        public ToolRunner(IAnalysisModels tools, BlockingCollection<byte[]> outputQueue, Pipeline pipeline)
        {
            _outputQueue = outputQueue;
            _pipeline = pipeline;
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
                var outputs = _pipeline.Tools.Select(tool => _tools.GetTool(tool).AnalyseFrame(frame)).ToList();
                
                //Draw Boxes
                var image = DrawBoxes(frame, outputs);
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

        private static Image DrawBoxes(byte[] frame, IReadOnlyList<AnalysisOutput> outputs)
        {
            Image outputFrame;
            using var ms = new MemoryStream(frame);
            outputFrame = Image.FromStream(ms);
            
            var oldWidth = outputFrame.Width;
            var oldHeight = outputFrame.Height;
            //initialise drawing tools
            var penWidth = Convert.ToInt32(Math.Max(oldHeight, oldWidth) * (1.0 / 445));
            var fontSize = Convert.ToSingle(Math.Max(oldHeight, oldWidth) * (1.3 / 89));
            var boxHeadingHeight = Convert.ToSingle(Math.Max(oldHeight, oldWidth) * (2.0 / 89));
            var countTextHeight = Convert.ToSingle(Math.Max(oldHeight, oldWidth) * (11.0 / 445));
            var pen = new Pen(Color.Red,penWidth);
            var brush = Brushes.Red;
            var countFont = new Font(FontFamily.GenericSansSerif, fontSize * 2 / 1.3f);
            var font = new Font(FontFamily.GenericSansSerif,fontSize);

            for (var index = 0; index < outputs.Count; index++)
            {
                var output = outputs[index];
                for (var i = 0; i < output.Classes.Count; i++)
                {
                    using var g = Graphics.FromImage(outputFrame);
                    var box = new Rectangle(Convert.ToInt32(output.Boxes[i * 4] * oldWidth),
                        Convert.ToInt32(output.Boxes[i * 4 + 1] * oldHeight),
                        Convert.ToInt32(output.Boxes[i * 4 + 2] * oldWidth),
                        Convert.ToInt32(output.Boxes[i * 4 + 3] * oldHeight));
                    g.DrawRectangle(pen, box);
                    g.DrawString(char.ToUpper(output.Classes[i][0]) + output.Classes[i].Substring(1), font, brush,
                        Convert.ToSingle(output.Boxes[i * 4] * oldWidth),
                        Convert.ToSingle(output.Boxes[i * 4 + 1] * oldHeight) - boxHeadingHeight);
                }
                
                Graphics.FromImage(outputFrame).DrawString(output.Purpose + " Count: " + output.Classes.Count, countFont,
                    brush, 10, 10 + index * countTextHeight);
            }//char.ToUpper(str[0]) + str.Substring(1)


            return outputFrame;
        }
    }
}