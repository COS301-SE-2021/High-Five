using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using analysis_engine.Video;
using analysis_engine.Video.ConcreteFrameEncoder;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using High5SDK;

//using Emgu.CV;
//using Emgu.CV.CvEnum;
//using Emgu.CV.Structure;

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
        private VideoCapture _streamFrameCapture;
        private Mat _tempFrame;

        public Manager(AnalysisObserver analysisObserver)
        {
            _frameCount = 0;
            _analysisObserver=analysisObserver;
        }

        public void CreatePipeline(string type, string pipelineString, string mediaType, string outputUrl="")
        {
            _outputUrl = outputUrl;
            if (mediaType == "stream")
            {
                _frameEncoder = new StreamFrameEncoder(_outputUrl, new Size(1280, 720));
            }
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

        public void GiveLinkToFootage(string mediaType, string url, 
            Stream input=null)
        {
            Console.WriteLine("Giving link to footage.");
            _mediaType = mediaType;
            switch (mediaType)
            {
                case "video":
                    _frameGrabber = new VideoFrameGrabber();
                    _frameGrabber.Init(url);
                    break;
                case "stream":
                    _streamFrameCapture = new VideoCapture(url);
                    break;
                case "image":
                    _frameGrabber = new ImageFrameGrabber();
                    _frameGrabber.Init(input);
                    break;
                default:
                    _frameGrabber = new VideoFrameGrabber();
                    _frameGrabber.Init(url);
                    break;
            }
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
                            new VideoFrameEncoder(_outputUrl, data.Frame.Image.Size);
                        break;
                    case "stream":
                        //Moved to create pipeline function
                        break;
                    case "image":
                        _frameEncoder = new ImageFrameEncoder(_outputUrl);
                        break;
                    default:
                        _frameEncoder =
                            new VideoFrameEncoder(_outputUrl, data.Frame.Image.Size);
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
            if (_mediaType != "stream")
            {
                Task.Factory.StartNew(() =>
                {
                    var data = GetNextFrame();
                    while (data != null)
                    {
                        _pipeline.Source.Push(data);
                        data = GetNextFrame();
                    }

                    _pipeline.Source.Push(null);
                });
            }
            else
            {
                _streamFrameCapture.ImageGrabbed += LiveFrameProcess;
                _tempFrame = new Mat();
                _streamFrameCapture.Start();
            }

            Task.Factory.StartNew(() =>
            {
                var data = _pipeline.Drain.Pop();
                while (data != null)
                {
                    ReturnAnalyzedFrame(data);
                    data = _pipeline.Drain.Pop();
                }
                
                _frameEncoder.Dispose();
                _analysisObserver.AnalysisFinished();
            });
        }

        private void LiveFrameProcess(object sender, EventArgs e)
        {
            _streamFrameCapture.Retrieve(_tempFrame);
            Data temp = _dataPool.GetData();
            var image=_tempFrame.ToImage<Rgb, byte>();
            if (image.Width % 4 != 0)
            {
                image=image.Resize(image.Width+(4-image.Width%4), image.Height, Inter.Area);
            }
            temp.Frame.Image = image;
            temp.Frame.FrameID = _frameCount;
            _frameCount++;
            _pipeline.Source.Push(temp);
            if (_frameCount > 3600)//This is for stopping the stream after a certain amount of time
            {
                _streamFrameCapture.Stop();
                _pipeline.Source.Push(null);
            }
        }

        private void StopAnalysis()
        {
            _pipeline.Source.Push(null);
        }
    }
}