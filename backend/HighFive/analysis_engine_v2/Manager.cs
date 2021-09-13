using System.Threading.Tasks;
using analysis_engine.Video;
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
        private FrameGrabber _frameGrabber;
        private string _outputUrl;
        private bool _isStream;
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
            _pipeline = _builderDirector.Construct(pipelineString);
        }

        public void GiveLinkToFootage(string mediaType, string url, string outputUrl="")
        {
            switch (mediaType)
            {
                case "video":
                    _frameGrabber = new VideoFrameGrabber();
                    _frameGrabber.Init(url);
                    _isStream = false;
                    break;
                case "stream":
                    _frameGrabber = new StreamFrameGrabber();
                    _frameGrabber.Init(url);
                    _isStream = true;
                    break;
                case "image":
                    _frameGrabber = new ImageFrameGrabber();
                    _frameGrabber.Init(url);
                    _isStream = false;
                    break;
                default:
                    _frameGrabber = new VideoFrameGrabber();
                    _frameGrabber.Init(url);
                    _isStream = false;
                    break;
            }
            
            _outputUrl = outputUrl;
        }

        private Data GetNextFrame()
        {
            Data temp = _dataPool.GetData();
            var image = _frameGrabber.GetNextFrame();
            if (image == null)
            {
                return null;
            }
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
            var drawFilter=_pipeline.Init();
            Task.Factory.StartNew(() =>
            {
                var data = GetNextFrame();
                _pipeline.Source.Push(data);
                while (data!=null){
                    if (_frameCount % 3 == 0)
                    {
                        data = GetNextFrame();
                        _pipeline.Source.Push(data);
                    }
                    else
                    {
                        var temp = data.Meta;
                        data = GetNextFrame();
                        data.Meta = temp;
                        drawFilter.Input.Push(data);
                    }
                }
            });

            Task.Factory.StartNew(() =>
            {
                var data = _pipeline.Drain.Pop();
                while (data != null)
                {
                    ReturnAnalyzedFrame(data);
                    data = _pipeline.Drain.Pop();
                }
                //TODO Finish encoding
            });
        }
    }
}