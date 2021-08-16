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
        private List<Tool> _tools;
        private List<IToolRunner> _toolRunners;

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
        }

        public void FeedFrame(byte[] frame)
        {
            foreach (var queue in _outputQueues)
            {
                queue.Add(frame);
            }
        }

        public List<List<byte[]>> GetFrames()
        {
            throw new System.NotImplementedException();
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