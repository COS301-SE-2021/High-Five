using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using FFMediaToolkit.Graphics;
using Org.OpenAPITools.Models;
using src.AnalysisTools.AnalysisThread;
using src.Subsystems.Analysis;

namespace src.AnalysisTools
{
    public class AnalyserImpl: IAnalyser
    {
        private List<BlockingCollection<byte[]>> _outputQueues;
        private List<Tool> _tools;
        private List<ToolRunner> _toolRunners = new();
        private int _frameCount;

        public void StartAnalysis(Pipeline pipeline, IAnalysisModels analysisModels)
        {
            _outputQueues = new List<BlockingCollection<byte[]>>();
            _tools = new List<Tool>();
            
            var count = 1;
            foreach (var toolName in pipeline.Tools)
            {
                _tools.Add(analysisModels.GetTool(toolName));
                if (analysisModels.GetTool(toolName).SeparateOutput) count++;
            }

            for (var i = 0; i < count; i++)
            {
                _outputQueues.Add(new BlockingCollection<byte[]>());
            }

            count = 1;
            var baseTools = new List<Tool>();
            foreach (var tool in _tools)
            {
                if (tool.SeparateOutput)
                {
                    var tempList = new List<Tool>();
                    tempList.Add(tool);
                    _toolRunners.Add(new ToolRunner(tempList, _outputQueues[count]));
                    count++;
                }
                else
                {
                    baseTools.Add(tool);
                }
            }
            _toolRunners.Insert(0, new ToolRunner(baseTools, _outputQueues[0]));

            _frameCount = 0;
        }

        public void FeedFrame(byte[] frame)
        {
            _frameCount++;
            foreach (var toolRunner in _toolRunners)
            {
                toolRunner.Enqueue(frame);
            }
        }

        public void FeedFrame(Bitmap frame)
        {
            byte[] inputFrame;
            
            var rect = new Rectangle(Point.Empty, frame.Size);
            var bitLock = frame.LockBits(rect, ImageLockMode.ReadWrite, frame.PixelFormat);
            
            using (var ms = new MemoryStream())
            {
                frame.Save(ms, ImageFormat.Png);
                inputFrame =  ms.ToArray();
            }
            
            frame.UnlockBits(bitLock);
            
            
            _frameCount++;
            foreach (var toolRunner in _toolRunners)
            {
                toolRunner.Enqueue(inputFrame);
            }
        }

        public List<List<byte[]>> GetFrames()
        {
            var output = new List<List<byte[]>>();
            
            for (var i = 0; i < _outputQueues.Count; i++)
            {
                output.Add(new List<byte[]>());
                if (_frameCount == 0) break;
                var count = 0;
                foreach (var frame in _outputQueues[i].GetConsumingEnumerable(CancellationToken.None))
                {
                    output[i].Add(frame);
                    count++;
                    if (count == _frameCount) break;
                }
            }

            return output;
        }

        public void EndAnalysis()
        {
            foreach (var toolRunner in _toolRunners)
            {
                toolRunner.Enqueue(new byte[] { 0 });
            }
        }
    }
}