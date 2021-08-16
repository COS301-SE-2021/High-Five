using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Org.OpenAPITools.Models;
using src.AnalysisTools.AnalysisThread;
using src.Subsystems.Analysis;

namespace src.AnalysisTools
{
    public class AnalyserImpl: IAnalyser
    {
        private List<BlockingCollection<byte[]>> _outputQueues;
        private List<List<Tool>> _tools;
        private List<IToolRunner> _toolRunners;

        public void StartAnalysis(Pipeline pipeline, IAnalysisModels analysisModels)
        {
            _outputQueues = new List<BlockingCollection<byte[]>>();
            _tools = new List<List<Tool>>();
            
            var count = 1;
            _tools.Add(new List<Tool>());
            foreach (var toolName in pipeline.Tools)
            {
                if (analysisModels.GetTool(toolName).SeparateOutput)
                {
                    _tools.Add(new List<Tool>());
                    _tools[count].Add(analysisModels.GetTool(toolName));
                    count++;
                }
                else
                {
                    _tools[0].Add(analysisModels.GetTool(toolName));
                }
            }

            for (var i = 0; i < count; i++)
            {
                _outputQueues.Add(new BlockingCollection<byte[]>());
                _toolRunners.Add(new ToolRunner(_tools[i], _outputQueues[i]));
            }
        }

        public void FeedFrame(byte[] frame)
        {
            foreach (var queue in _outputQueues)
            {
                queue.Add(frame);
            }
        }

        public List<byte[]> GetFrames()
        {
            throw new System.NotImplementedException();
        }

        public void EndAnalysis()
        {
            foreach (var queue in _outputQueues)
            {
                queue.Add(new byte[]{0});
            }
        }
    }
}