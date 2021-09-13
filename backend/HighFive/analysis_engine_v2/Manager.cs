using System.Threading;
using System.Threading.Tasks;
using analysis_engine.Video;
using analysis_engine.Video.ConcreteFrameEncoder;
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
        private FrameEncoder _frameEncoder=null;
        private string _mediaType;
        private AnalysisObserver _analysisObserver;
        public Manager(AnalysisObserver analysisObserver)
        {
            _frameCount = 0;
            _analysisObserver=analysisObserver;
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
            _mediaType = mediaType;
            switch (mediaType)
            {
                case "video":
                    _frameGrabber = new VideoFrameGrabber();
                    _frameGrabber.Init(url);
                    break;
                case "stream":
                    _frameGrabber = new StreamFrameGrabber();
                    _frameGrabber.Init(url);
                    break;
                case "image":
                    _frameGrabber = new ImageFrameGrabber();
                    _frameGrabber.Init(url);
                    break;
                default:
                    _frameGrabber = new VideoFrameGrabber();
                    _frameGrabber.Init(url);
                    break;
            }
            
            _outputUrl = outputUrl;
        }

        private Data GetNextFrame()
        {
            var image = _frameGrabber.GetNextFrame();
            if (image == null)
            {
                return null;
            }
            Data temp = _dataPool.GetData();
            temp.Frame.Image = image;
            temp.Frame.FrameID = _frameCount;
            _frameCount++;
            return temp;
        }

        private void ReturnAnalyzedFrame(Data data)
        {
            _dataPool.ReleaseData(data);

            if (_frameEncoder == null)
            {
                switch (_mediaType)
                {
                    case "video":
                        _frameEncoder =
                            new VideoFrameEncoder(@"C:\Users\hanne\RiderProjects\output.mp4", data.Frame.Image.Size);
                        break;
                    case "stream":
                        //TODO encoding stream start
                        break;
                    case "image":
                        //TODO encoding image start
                        break;
                    default:
                        _frameEncoder =
                            new VideoFrameEncoder(@"C:\Users\hanne\RiderProjects\output.mp4", data.Frame.Image.Size);
                        break;
                }
            }
            _frameEncoder.AddFrame(data);

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
                while (data!=null){
                    _pipeline.Source.Push(data);
                    data = GetNextFrame();
                }
                _pipeline.Source.Push(null);
            });

            Task.Factory.StartNew(() =>
            {
                var data = _pipeline.Drain.Pop();
                while (data != null)
                {
                    ReturnAnalyzedFrame(data);
                    data = _pipeline.Drain.Pop();
                }
                _analysisObserver.AnalysisFinished();
            });
        }
    }
}