using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace analysis_engine
{
    public class Manager
    {
        private Pipeline _pipeline;
        private PipelineBuilderDirector _builderDirector;
        private DataPool _dataPool;
        private int _frameCount;
        public Manager()
        {
            _frameCount = 0;
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
            _pipeline = _builderDirector.Construct("analysis:animals1");
        }

        private Data GetNextFrame()
        {
            Data temp = _dataPool.GetData();
            var image = CvInvoke.Imread("C:\\Users\\Bieldt\\OneDrive\\Pictures\\cows.jpg", ImreadModes.Unchanged).ToImage<Rgb,byte>();
            temp.Frame.Image = image;
            temp.Frame.FrameID = _frameCount;
            _frameCount++;
            return temp;
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