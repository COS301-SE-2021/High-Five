using System;
using System.Diagnostics;
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
        private long _timer1;
        private long _timer2;

        public StreamFrameEncoder(string url, Size size)
        {
            _timer1 = 0;
            _timer2 = 0;
            try{
                var pusher = new Pusher();
                pusher.StartPush(url, size.Width, size.Height, 30);
                
                _encoder = new Encoder(size.Width, size.Height, 30, 1024*800);
                _encoder.FrameEncoded += (sender, e) =>
                {
                    //A frame encoded.
                    var packet = e.Packet;
                    pusher.PushPacket(packet);
                };
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public override void AddFrame(Data data)
        {
            if (data != null)
            {
                var watch = new Stopwatch();
                watch.Reset();
                watch.Start();
                var bitmap = ProcessUsingLockbitsAndUnsafeAndParallel(data.Frame.Image.ToBitmap());
                watch.Stop();
                _timer1 += watch.ElapsedMilliseconds;
                watch.Reset();
                watch.Start();
                _encoder.AddImage(bitmap);
                watch.Stop();
                _timer2 += watch.ElapsedMilliseconds;
                if (data.Frame.FrameID % 100 == 99)
                {
                    Console.WriteLine(_timer1 / 100.0 + "xxx" + _timer2 / 100.0);
                    _timer1=0;
                    _timer2=0;
                }
                // _encoder.Flush();
            }
            else
            {
                _encoder.Flush();
                _encoder.Dispose();
            }
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
        
        public static void RGBtoBGR(Bitmap bmp)
        {
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadWrite, bmp.PixelFormat);

            int length = Math.Abs(data.Stride) * bmp.Height;

            unsafe
            {
                byte* rgbValues = (byte*)data.Scan0.ToPointer();

                for (int i = 0; i < length; i += 3)
                {
                    byte dummy = rgbValues[i];
                    rgbValues[i] = rgbValues[i + 2];
                    rgbValues[i + 2] = dummy;
                }
            }

            bmp.UnlockBits(data);
        }
        
        private static Bitmap ProcessUsingLockbitsAndUnsafeAndParallel(Bitmap img)
        {
            unsafe
            {
                var processedBitmap = new Bitmap(img.Width, img.Height, PixelFormat.Format24bppRgb);
                using (var gr = Graphics.FromImage(processedBitmap))
                    gr.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height));
                BitmapData bitmapData = processedBitmap.LockBits(new Rectangle(0, 0, processedBitmap.Width, processedBitmap.Height), ImageLockMode.ReadWrite, processedBitmap.PixelFormat);
 
                var bytesPerPixel = Bitmap.GetPixelFormatSize(processedBitmap.PixelFormat) / 8;
                var heightInPixels = bitmapData.Height;
                var widthInBytes = bitmapData.Width * bytesPerPixel;
                var PtrFirstPixel = (byte*)bitmapData.Scan0;
                byte[] output = new byte[processedBitmap.Height*processedBitmap.Width*3];
                fixed (byte* p = &output[0])
                {
                    // var PtrOut = (byte*)new byte[heightInPixels * widthInBytes];

                    // var output = new byte[processedBitmap.Height][][];
                    byte* ptr = p;

                    Parallel.For(0, heightInPixels, y =>
                    {

                        var currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                        // output[y] = new byte[processedBitmap.Width][];

                        for (var x = 0; x < widthInBytes; x += bytesPerPixel)
                        {
                            // output[y][x] = new byte[3];

                            int oldBlue = currentLine[x];
                            int oldGreen = currentLine[x + 1];
                            int oldRed = currentLine[x + 2];

                            currentLine[x] = (byte)oldRed;
                            currentLine[x + 1] = (byte)oldGreen;
                            currentLine[x + 2] = (byte)oldBlue;

                            ptr[y * widthInBytes + x] = (byte)oldRed;
                            ptr[y * widthInBytes + x + 1] = (byte)oldGreen;
                            ptr[y * widthInBytes + x + 2] = (byte)oldBlue;

                            // output[y][x / bytesPerPixel][0] = (byte)oldRed;
                            // output[y][x / bytesPerPixel][1] = (byte)oldGreen;
                            // output[y][x / bytesPerPixel][2] = (byte)oldBlue;
                        }
                    });
                }

                processedBitmap.UnlockBits(bitmapData);
                
                // Marshal.Copy((IntPtr)PtrFirstPixel, output, 0, processedBitmap.Height*processedBitmap.Width*3);
                return processedBitmap;
            }
        }
    }
}