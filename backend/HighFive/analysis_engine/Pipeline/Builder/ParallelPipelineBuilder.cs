
namespace analysis_engine
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

        public override void BuildSource()
        {
            Pipeline.Source = _pipeFactories[1].GetPipe();
            InputSplitter temp = (InputSplitter)Pipeline.Source;
            foreach (var channelInput in _channelInputs)
            {
                temp.AddInput(channelInput);
            }
        }

        public override void BuildDrain()
        {
            Pipeline.Drain = _pipeFactories[0].GetPipe();
            _channelsOutput = _pipeFactories[2].GetPipe();
        }

        public override void BuildFilters(string filterString)
        {
            
        }

        public override Pipeline GetPipeline()
        {
            return Pipeline;
        }
    }
}