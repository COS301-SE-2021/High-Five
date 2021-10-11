using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using analysis_engine.Video;
using analysis_engine.Video.ConcreteFrameEncoder;
using Confluent.Kafka;
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
        private bool _analysisDone = false;
        private BlockingCollection<long> _timeQueue;
        private long _latency;

        public Manager(AnalysisObserver analysisObserver)
        {
            _latency = 0;
            _frameCount = 0;
            _analysisObserver=analysisObserver;
            _timeQueue = new BlockingCollection<long>(new ConcurrentQueue<long>());
        }

        public void CreatePipeline(string type, string pipelineString, string mediaType, string outputUrl="")
        {
            _outputUrl = outputUrl;
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
            

            if (_frameEncoder == null)
            {
                switch (_mediaType)
                {
                    case "video":
                        _frameEncoder =
                            new VideoFrameEncoder(_outputUrl, data.Frame.Image.Size);
                        break;
                    case "stream":
                        _frameEncoder = new VideoFrameEncoder(_outputUrl, data.Frame.Image.Size);
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
            if (_mediaType != "stream")
            {
                Task.Factory.StartNew(() =>
                {
                    var data = GetNextFrame();
                    while (data != null)
                    {
                        _timeQueue.Add(DateTimeOffset.Now.ToUnixTimeMilliseconds());
                        _pipeline.Source.Push(data);
                        data = GetNextFrame();
                    }

                    _pipeline.Source.Push(null);
                });
            }
            else
            {
                _analysisDone = false;
                _streamFrameCapture.ImageGrabbed += LiveFrameProcess;
                _tempFrame = new Mat();
                _streamFrameCapture.Start();
                while (!_analysisDone)
                {
                    _analysisDone = true;
                    Thread.Sleep(2000);
                }
                _streamFrameCapture.Stop();
                _pipeline.Source.Push(null);
            }

            Task.Factory.StartNew(() =>
            {
                var data = _pipeline.Drain.Pop();
                while (data != null)
                {
                    ReturnAnalyzedFrame(data);
                    data = _pipeline.Drain.Pop();
                    if (data != null)
                    {
                        //GC.Collect();
                        _latency += (DateTimeOffset.Now.ToUnixTimeMilliseconds() - _timeQueue.Take());
                    }
                }
                
                _frameEncoder.Dispose();
                Console.WriteLine("Average Pipeline Latency: "+ _latency/Convert.ToDouble(_frameCount)+ "ms");
                _analysisObserver.AnalysisFinished(_frameCount);
            });
        }

        private void LiveFrameProcess(object sender, EventArgs e)
        {
            _analysisDone = false;
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
            _timeQueue.Add(DateTimeOffset.Now.ToUnixTimeMilliseconds());
            _pipeline.Source.Push(temp);
            // if (_frameCount > 3600)//This is for stopping the stream after a certain amount of time
            // {
            //     _streamFrameCapture.Stop();
            //     _pipeline.Source.Push(null);
            // }
        }

        private void StopAnalysis()
        {
            _pipeline.Source.Push(null);
            _pipeline = null;
            _dataPool = null;
        }
    }
}