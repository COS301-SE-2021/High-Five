using System;
using System.IO;

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using src.AnalysisTools.AnalysisThread;
using Image = System.Drawing.Image;

namespace src.AnalysisTools.ConcreteTools
{
    public class PersonRecognition: Tool
    {
        private const string ModelName = "ssd_mobilenet_v1_10.onnx";
        private static readonly string ModelPath = Directory.GetCurrentDirectory() + "\\Models\\" + ModelName;
        private readonly InferenceSession _model;
        private readonly string _modelInputLayerName;
        private const double MinScore=0.50;
        private const long MinClass = 1;
        private const long MaxClass = 1;
        public const string ToolPurpose = "Person";

        private readonly string[] _classes ={
            "person",
            "bicycle",
            "car",
            "motorcycle",
            "airplane",
            "bus",
            "train",
            "truck",
            "boat",
            "traffic light",
        };
        public PersonRecognition()
        {
            //Load object recognition model and get ready for analysis
            _model = new InferenceSession(ModelPath);
            _modelInputLayerName = _model.InputMetadata.Keys.Single();
            SeparateOutput = false;
        }
        public override AnalysisOutput AnalyseFrame(byte[] frame)
        {
            //Convert from input type frame to 3D array
            
            var modelInput = PreprocessFrame(frame);
            
            var result = ProcessFrame(modelInput);
            
            var output = PostprocessFrame(result);
            
            return output;
        }

        public override IDisposableReadOnlyCollection<DisposableNamedOnnxValue> ProcessFrame(List<NamedOnnxValue> modelInput)
        {
            return _model.Run(modelInput);
        }

        public override List<NamedOnnxValue> PreprocessFrame(byte[] frame)
        {
            
            Image originalImage;
            Bitmap bImage;
            using (var ms = new MemoryStream(frame))
            {
                originalImage = Image.FromStream(ms);
            }

            bImage = new Bitmap(originalImage);


            var bytes = ProcessUsingLockbitsAndUnsafeAndParallel(bImage);
            
            int[] dimensions = { 1, bImage.Height, bImage.Width, 3 };
            var inputTensor = new DenseTensor<byte>(bytes, dimensions);
            
            var modelInput = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor(_modelInputLayerName, inputTensor)
            };

            return modelInput;
        }

        public override AnalysisOutput PostprocessFrame(IDisposableReadOnlyCollection<DisposableNamedOnnxValue> result)
        {
            var boxes=((DenseTensor<float>) result.ElementAtOrDefault(0).Value).ToArray();//Convert to output type
            var labels=((DenseTensor<float>) result.ElementAtOrDefault(1).Value).ToArray();
            var scores=((DenseTensor<float>) result.ElementAtOrDefault(2).Value).ToArray();
            var numDetections=((DenseTensor<float>) result.ElementAtOrDefault(3).Value).ToArray();
            
            var output = new AnalysisOutput();
            
            output.Purpose = ToolPurpose;
            output.Boxes = new List<float>();
            output.Classes = new List<string>();
            
            for (var i = 0; i < numDetections[0]; i++)
            {
                if (labels[i] >= MinClass && labels[i] <= MaxClass && scores[i] > MinScore)
                {
                    output.Boxes.Add(boxes[i*4+1]);
                    output.Boxes.Add(boxes[i*4]);
                    output.Boxes.Add((boxes[i * 4 + 3] - boxes[i * 4 + 1]));
                    output.Boxes.Add((boxes[i * 4 + 2] - boxes[i * 4]));

                    output.Classes.Add(_classes[Convert.ToInt32(labels[i]-1)]);
                }
            }

            return output;
        }

        private static byte[] ProcessUsingLockbitsAndUnsafeAndParallel(Bitmap img)
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
                return output;
            }
        }
    }
}
