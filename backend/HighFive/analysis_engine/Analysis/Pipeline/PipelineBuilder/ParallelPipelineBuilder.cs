using analysis_engine.Util;
using analysis_engine.Util.Factories;

namespace analysis_engine.Analysis.Pipeline.PipelineBuilder
{
    public class ParallelPipelineBuilder : PipelineBuilder
    {
        private Pipe[] _channelInputs;
        private Pipe _channelsOutput;
        public ParallelPipelineBuilder(int channels)
        {
            _pipeFactories = new PipeFactory[3];
            _pipeFactories[0] = new ConcurrentQueuePipeFactory();
            _pipeFactories[1] = new InputSplitterPipeFactory();
            _pipeFactories[2] = new InputMergerPipeFactory(3);
            _channelInputs = new Pipe[channels];
        }

        public override void BuildPipeline()
        {
            Pipeline = new ParallelPipeline();
        }

        public override void BuildSource(Pipe source)
        {
            Pipeline.Source = _pipeFactories[1].getPipe();
            InputSplitter temp = (InputSplitter)Pipeline.Source;
            foreach (var channelInput in _channelInputs)
            {
                temp.AddInput(channelInput);
            }
        }

        public override void BuildDrain(Pipe drain)
        {
            Pipeline.Drain = _pipeFactories[0].getPipe();
            _channelsOutput = _pipeFactories[2].getPipe();
        }

        public override void AddFilter(analysis_engine.Filter.Filter filter)
        {
            Pipeline.Filters.Add(filter);
        }

        public override Pipeline GetPipeline()
        {
            return Pipeline;
        }
    }
}