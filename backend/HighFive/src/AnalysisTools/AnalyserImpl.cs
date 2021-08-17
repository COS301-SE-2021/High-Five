using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Org.OpenAPITools.Models;
using src.AnalysisTools.AnalysisThread;
using src.Subsystems.Analysis;

namespace src.AnalysisTools
{
    public class AnalyserImpl: IAnalyser
    {
        private List<BlockingCollection<byte[]>> _outputQueues;
        private List<Tool> _tools;
        private List<ToolRunner> _toolRunners;
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

            foreach (var tool in _tools)
            {
                //_toolRunners.Add(new ToolRunner(tool,_outputQueues[0]));
            }

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
            foreach (var queue in _outputQueues)
            {
                queue.Add(new byte[] { 0 });
            }
        }
    }
}