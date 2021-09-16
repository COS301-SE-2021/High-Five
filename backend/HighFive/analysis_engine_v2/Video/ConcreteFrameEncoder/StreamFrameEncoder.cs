using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Confluent.Kafka;
using Emgu.CV;
using FFMpegCore;
using FFMpegCore.Pipes;
using High5SDK;
using NReco.VideoConverter;
using DotNetPusher.Encoders;
using DotNetPusher.Pushers;
using Emgu.CV.Structure;
using Encoder = DotNetPusher.Encoders.Encoder;

namespace analysis_engine.Video.ConcreteFrameEncoder
{
    public class StreamFrameEncoder : FrameEncoder
    {
        private readonly VideoWriter _videoWriter;
        private readonly FFMpegConverter _streamWriter;
        private Stream _inputStream;
        private readonly string _url;
        private Socket _clientSocket;
        // private MediaOutput _file;
        // private VideoEncoderSettings _settings;
        private Encoder _encoder;

        public StreamFrameEncoder(string url, Size size)
        {
            // _url = url;
            // _videoWriter = new VideoWriter(_url, VideoWriter.Fourcc('m', 'p', '4', 'v'), 30,
            //     size, true);
            
            // _streamWriter = new FFMpegConverter();
            // var stream = new MemoryStream();
            // var convertSettings = new ConvertSettings(){VideoCodec="h264"};
            // convertSettings.VideoFrameRate = 30;
            // convertSettings.SetVideoFrameSize(size.Width, size.Height);
            // var task=_streamWriter.ConvertLiveMedia(_stream,Format.mp4, url, Format.mp4, convertSettings);
            // task.Start();
            try
            {
                // _inputStream =
                //     new FileStream(
                //         @"D:\Users\hanne\Documents\Hannes\Movies and Series\The.Lord.of.the.Rings.The.Fellowship.of.the.Ring.2001.EXTENDED.1080p.BluRay.10bit.HEVC.6CH.MkvCage.ws.mkv",
                //         FileMode.Open);


                // Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //
                // IPAddress IP = IPAddress.Parse("127.0.0.1");
                // IPEndPoint IPE = new IPEndPoint(IP, 55556);
                //
                // // sender.Bind(IPE);
                // // sender.Listen(2);
                // sender.Connect(IPE);
                // _clientSocket = sender;

                
                // _inputStream = new MemoryStream();
                // FFMpegArguments
                //     .FromFileInput(@"D:\Users\hanne\Documents\Hannes\Movies and Series\The.Lord.of.the.Rings.The.Fellowship.of.the.Ring.2001.EXTENDED.1080p.BluRay.10bit.HEVC.6CH.MkvCage.ws.mkv")
                //     .OutputToFile(@"rtmp://192.168.11.153/55799ed725ac42bcbb1925c715380541/065753898677033738659196", true, options => options
                //         .WithVideoCodec("h264").ForceFormat("mpeg-4 avc").WithFramerate(30))
                //     .ProcessAsynchronously();
                // Console.WriteLine("StartedFFmpeg conversion");



                // var inputArgs = "-framerate 30 -f rawvideo -pix_fmt rgb24 -video_size 1280x720 -i -";
                // var outputArgs = "-f mpegts tcp://localhost:55555?listen";
                //
                // var process = new Process
                // {
                //     StartInfo =
                //     {
                //         FileName = "ffmpeg.exe",
                //         Arguments = $"{inputArgs} {outputArgs}",
                //         UseShellExecute = false,
                //         CreateNoWindow = true,
                //         RedirectStandardInput = true
                //     }
                // };
                //
                // process.Start();
                //
                // var ffmpegIn = process.StandardInput.BaseStream;
                //
                // byte[] arr = new byte[100000000];
                // _inputStream.Read(arr, 0, arr.Length);
                // ffmpegIn.Write(arr, 0, arr.Length);
                // Console.WriteLine("RUN!!!!");
                // ffmpegIn.Flush();
                // // ffmpegIn.Close();
                // // process.WaitForExit();
                
                // FFmpegLoader.FFmpegPath =
                //     @"C:\ffmpeg-4.4-full_build-shared\bin";
                //
                // _settings = new VideoEncoderSettings(width: size.Width, height: size.Height, framerate: 30, codec: VideoCodec.VP8);
                // _settings.EncoderPreset = EncoderPreset.Fast;
                // _settings.CRF = 17;
                // _file = MediaBuilder.CreateContainer(@"C:\Users\hanne\RiderProjects\output.webm").WithVideo(_settings).Create();
                // var files = Directory.GetFiles(@"C:\Input\");

                //file.Dispose();
                
                var pusher = new Pusher();
                pusher.StartPush("rtmp://192.168.11.153/55799ed725ac42bcbb1925c715380541/065753898677033738659196", size.Width, size.Height, 30);
                
                _encoder = new Encoder(size.Width, size.Height, 30, 1024*800);
                _encoder.FrameEncoded += (sender, e) =>
                {
                    //A frame encoded.
                    var packet = e.Packet;
                    pusher.PushPacket(packet);
                    // Console.WriteLine($"Packet pushed, size:{packet.Size}.");
                };
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            

        }

        public override void AddFrame(Data data)
        {
            _encoder.AddImage(data.Frame.Image.ToBitmap().ToImage<Bgr,byte>().ToBitmap());
            
            
            
            // CvInvoke.Imwrite(@"C:\Users\hanne\RiderProjects\output.jpg", data.Frame.Image);
            //_inputStream.Write(data.Frame.Image.Bytes, 0,data.Frame.Image.Bytes.Length);
             // _videoWriter.Write(data.Frame.Image);
            // _stream.Write(data.Frame.Image.Bytes,0,data.Frame.Image.Bytes.Length);
            // try
            // {
            //     _clientSocket.Send(data.Frame.Image.Bytes);
            // }
            // catch (Exception e)
            // {
            //     Console.WriteLine("Ai tog!");
            //     Console.WriteLine(e.Message);
            // }
            // _inputStream.Write(data.Frame.Image.Bytes, 0 , data.Frame.Image.Bytes.Length);
            // try
            // {
            //     var bitmap = new Bitmap(data.Frame.Image.ToBitmap());
            //     var rect = new Rectangle(Point.Empty, bitmap.Size);
            //     var bitLock = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            //     var bitmapData = ImageData.FromPointer(bitLock.Scan0, ImagePixelFormat.Bgr24, bitmap.Size);
            //     _file.Video.AddFrame(bitmapData); // Encode the frame
            //     bitmap.UnlockBits(bitLock);
            //     _file.Dispose();
            //     _file = MediaBuilder.CreateContainer(@"C:\Users\hanne\RiderProjects\output.webm").WithVideo(_settings).Create();
            // }
            // catch (Exception e)
            // {
            //     Console.WriteLine(e.Message);
            // }
        }

        public override string GetVideo()
        {
            return _url;
        }

        public override void Dispose()
        {
            _videoWriter.Dispose();
            _encoder.Dispose();
        }
    }
}