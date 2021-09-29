using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using High5SDK;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using NumSharp;

namespace analysis_engine
{
    public class GenderDetectionTool : AnalysisTool
    {
        private const string ModelPath = @"../../Models/vgg_ilsvrc_16_gender_imdb_wiki.onnx";
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
            var image = Resize(data.Frame.Image);
            
            image = np.transpose(image, new[] { 2, 0, 1 });
            (image[0], image[2]) = (image[2], image[0]);
            image = np.expand_dims(image, 0);
            var input = image/1.0f;

            int[] dimensions = { 1, 3, 224, 224 };
            var inputTensor = new DenseTensor<float>(input.reshape(1*224*224*3).ToArray<float>(),dimensions);
            
            var modelInput = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor(_modelInputLayerName, inputTensor)
            };
            
            var predictions = _model.Run(modelInput);
            
            var gender=((DenseTensor<float>) predictions.ElementAtOrDefault(0).Value).ToArray();
            
            return PostProcessFrame(data, gender);
        }
        
        private Data PostProcessFrame(Data data, IReadOnlyList<float> gender)
        {
            var output = new BoxCoordinateData();
            if (gender[0] > gender[1])
            {
                output.Purpose = "Female";
            }
            else
            {
                output.Purpose = "Male";
            }
            
            data.Meta.Add(output);
            return data;
        }
        
        private static NDArray Resize(Image<Rgb,byte> image)
        {
            var resized = ProcessUsingLockbitsAndUnsafeAndParallel(image.Resize(224, 224, Inter.Area).ToBitmap())
                .ToImage<Bgr, byte>();
            
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