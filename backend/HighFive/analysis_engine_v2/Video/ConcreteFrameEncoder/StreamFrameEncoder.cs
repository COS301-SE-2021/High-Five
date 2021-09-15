using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Emgu.CV;
using FFMpegCore;
using FFMpegCore.Pipes;
using High5SDK;
using NReco.VideoConverter;

namespace analysis_engine.Video.ConcreteFrameEncoder
{
    public class StreamFrameEncoder : FrameEncoder
    {
        private readonly VideoWriter _videoWriter;
        private readonly FFMpegConverter _streamWriter;
        private Stream _inputStream;
        private readonly string _url;

        public StreamFrameEncoder(string url, Size size)
        {
            _url = url;
            _videoWriter = new VideoWriter(_url, VideoWriter.Fourcc('m', 'p', '4', 'v'), 30,
                size, true);
            // _streamWriter = new FFMpegConverter();
            // var stream = new MemoryStream();
            // var convertSettings = new ConvertSettings(){VideoCodec="h264"};
            // convertSettings.VideoFrameRate = 30;
            // convertSettings.SetVideoFrameSize(size.Width, size.Height);
            // var task=_streamWriter.ConvertLiveMedia(_stream,Format.mp4, url, Format.mp4, convertSettings);
            // task.Start();
            
            // _inputStream = new MemoryStream();
            // var outputStream = new FileStream(@"C:\Users\hanne\RiderProjects\output.mp4", FileMode.Create);
            // Task.Factory.StartNew(() =>
            // {
            //     
            //     FFMpegArguments
            //         .FromPipeInput(new StreamPipeSource(_inputStream))
            //         .OutputToFile(@"C:\Users\hanne\RiderProjects\output.mp4", true, options => options
            //             .WithVideoCodec("mp4"))
            //         .ProcessAsynchronously();
            // });
        }

        public override void AddFrame(Data data)
        {
            // CvInvoke.Imwrite(@"C:\Users\hanne\RiderProjects\output.jpg", data.Frame.Image);
            //_inputStream.Write(data.Frame.Image.Bytes, 0,data.Frame.Image.Bytes.Length);
             _videoWriter.Write(data.Frame.Image);
            // _stream.Write(data.Frame.Image.Bytes,0,data.Frame.Image.Bytes.Length);
        }

        public override string GetVideo()
        {
            return _url;
        }

        public override void Dispose()
        {
            _videoWriter.Dispose();
        }
    }
}