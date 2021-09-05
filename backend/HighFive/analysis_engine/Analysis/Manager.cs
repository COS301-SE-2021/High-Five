using System.Threading.Tasks;
using analysis_engine.Analysis.Pipeline.PipelineBuilder;
using analysis_engine.Analysis.Util.Data;

namespace analysis_engine.Analysis
{
    public class Manager
    {
        private Pipeline.Pipeline _pipeline;
        private PipelineBuilderDirector _builderDirector;
        private DataPool _dataPool;
        public Manager()
        {
        }

        public void CreatePipeline(string type, string pipelineString)
        {
            if (type.Equals("linear"))
            {
                _builderDirector = new PipelineBuilderDirector(new LinearPipelineBuilder());
            }
            else
            {
                //TODO parallel builder implementation
            }
            _dataPool = new DataPool(5000, new DataFactory());
            _pipeline = _builderDirector.Construct(pipelineString);
        }

        private Data GetNextFrame()
        {
            return _dataPool.GetData();
        }

        private void ReturnAnalyzedFrame(Data data)
        {
            _dataPool.ReleaseData(data);
        }
/*
 * This function calls the pipeline Init function to start all the Tool threads.
 * It the starts 2 tasks in seperate Threads. The first task is responsible for feeding the pipeline.
 * The second task is responsible for fetching analyzed frames from the pipeline.
 */
        public void StartAnalysis()
        {
            _pipeline.Init();
            Task.Factory.StartNew(() =>
            {
                _pipeline.Source.Push(GetNextFrame());
            });

            Task.Factory.StartNew(() =>
            {
                Data temp = _pipeline.Drain.Pop();
                if (temp != null)
                {
                    ReturnAnalyzedFrame(temp);
                }
            });
        }
    }
}