using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using FFMpegCore;
using High5SDK;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using NumSharp;

namespace analysis_engine
{
    public class ImageSegmentationTool : AnalysisTool
    {
        private const string ModelPath = @"../../Models/ResNet101_DUC_HDC.onnx";
        private InferenceSession _model;
        private string _modelInputLayerName;
        public override void Init()
        {
            var sessionOptions = SessionOptions.MakeSessionOptionWithCudaProvider();
            // sessionOptions.InterOpNumThreads = 4;
            // sessionOptions.IntraOpNumThreads = 4;
            _model = new InferenceSession(
                ModelPath, sessionOptions
            );
            _modelInputLayerName = _model.InputMetadata.Keys.Single();
        }
        
        public override Data Process(Data data)
        {
            try
            {
                var image = Resize(data.Frame.Image);

                image = np.transpose(image, new[] { 2, 0, 1 });
                (image[0], image[2]) = (image[2], image[0]);
                image = np.expand_dims(image, 0);

                float[] mean = { 0.485f, 0.456f, 0.406f };
                float[] std = { 0.229f, 0.224f, 0.225f };
                var input = image / 1.0f; ///255.0f;
                // input[0][0] = input[0][0] - mean[0];
                // input[0][1] = input[0][1] - mean[1];
                // input[0][2] = input[0][2] - mean[2];
                // input[0][0] = input[0][0] / std[0];
                // input[0][1] = input[0][1] / std[1];
                // input[0][2] = input[0][2] / std[2];

                int[] dimensions = { 1, 3, 800, 800 };
                var inputTensor = new DenseTensor<float>(input.reshape(1 * 800 * 800 * 3).ToArray<float>(), dimensions);

                var modelInput = new List<NamedOnnxValue>
                {
                    NamedOnnxValue.CreateFromTensor(_modelInputLayerName, inputTensor)
                };

                var predictions = _model.Run(modelInput);

                var output = ((DenseTensor<float>)predictions.ElementAtOrDefault(0).Value).ToArray();

                return PostProcessFrame(data, output);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;

        }
        
        private Data PostProcessFrame(Data data, IReadOnlyList<float> result)
        {
            var info = np.array(result).reshape(19, 16, 100, 100);
            var output = new BoxCoordinateData();
            var image = new Image<Rgb, byte>(400, 400).ToBitmap();
            Color[] colours =
            {
                Color.Red, Color.Blue, Color.Green, Color.Aqua, Color.Fuchsia, Color.Lime, Color.Orange,
                Color.BlueViolet, Color.White, Color.Black, Color.Brown, Color.Gray, Color.Yellow, Color.LightSkyBlue,
                Color.Navy, Color.Aquamarine, Color.Maroon, Color.Teal, Color.Bisque
            };
            for (int i = 0; i < 100; i++)
            {
                for (int x = 0; x < 4; x++)
                {

                    for (int j = 0; j < 100; j++)
                    {
                        for (int y = 0; y < 4; y++)
                        {

                            var weight = info[0][x*y][j][i];
                            var colour = colours[0];
                            for (int k = 1; k < 19; k++)
                            {
                                if (weight < info[k][x*y][j][i])
                                {
                                    weight = info[k][x*y][j][i];
                                    colour = colours[k];
                                }
                            }

                            image.SetPixel((i*4+x), j*4+y, colour);
                        }
                    }
                }
            }

            var t = data.Frame.Image.Convert<Rgba, byte>().Mat;
            var bmp = t.ToBitmap();

            var overlay = image.ToImage<Rgba, byte>()
                .Resize(data.Frame.Image.Size.Width, data.Frame.Image.Height, Inter.Area).Data;//.ToBitmap();
            for (int i = 0; i < t.Rows; i++)
            {
                for (int j = 0; j < t.Cols; j++)
                {
                    overlay[i,j,3] = 100;
                    // overlay.SetPixel(i,j,Color.FromArgb(30,overlay.GetPixel(i,j)));
                }
            }
            var gra=Graphics.FromImage(bmp);
            gra.CompositingMode = CompositingMode.SourceOver;
            gra.DrawImage(new Image<Rgba,byte>(overlay).Mat.ToBitmap(),new Point(0,0));
            data.Frame.Image = bmp.ToImage<Rgb, byte>();
            data.Meta.Add(output);
            return data;
        }
        
        private static NDArray Resize(Image<Rgb,byte> image)
        {
            var resized = ProcessUsingLockbitsAndUnsafeAndParallel(image.Resize(800, 800, Inter.Area).ToBitmap()).ToImage<Bgr, byte>();
            
            var result = np.array(resized.Data).reshape(resized.Height, resized.Width, 3);
        
            return result;
        }
        
        private static Bitmap ProcessUsingLockbitsAndUnsafeAndParallel(Bitmap img)
        {
            unsafe
            {
                var processedBitmap = new Bitmap(img.Width, img.Height, PixelFormat.Format24bppRgb);
                using (var gr = Graphics.FromImage(processedBitmap))
                    gr.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height));
                BitmapData bitmapData = processedBitmap.LockBits(
                    new Rectangle(0, 0, processedBitmap.Width, processedBitmap.Height), ImageLockMode.ReadWrite,
                    processedBitmap.PixelFormat);

                var bytesPerPixel = Bitmap.GetPixelFormatSize(processedBitmap.PixelFormat) / 8;
                var heightInPixels = bitmapData.Height;
                var widthInBytes = bitmapData.Width * bytesPerPixel;
                var PtrFirstPixel = (byte*)bitmapData.Scan0;
                byte[] output = new byte[processedBitmap.Height * processedBitmap.Width * 3];
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