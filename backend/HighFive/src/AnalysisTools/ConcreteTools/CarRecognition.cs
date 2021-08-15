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
    public class CarRecognition: ITool
    {
        private const string ModelName = "ssd_mobilenet_v1_10.onnx";
        private static readonly string ModelPath = Directory.GetCurrentDirectory() + "\\Models\\" + ModelName;
        private readonly InferenceSession _model;
        private readonly string _modelInputLayerName;
        private const double MinScore=0.50;
        private const long MinClass = 2;
        private const long MaxClass = 9;
        public const string ToolPurpose = "Vehicle";

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
        public CarRecognition()
        {
            //Load object recognition model and get ready for analysis
            _model = new InferenceSession(ModelPath);
            _modelInputLayerName = _model.InputMetadata.Keys.Single();
        }
        public AnalysisOutput AnalyseFrame(byte[] frame)
        {
            //Convert from input type frame to 3D array
            Bitmap originalImage;
            using (var ms = new MemoryStream(frame))
            {
                originalImage = new Bitmap(Image.FromStream(ms));
            }
            var image = PreprocessFrame(originalImage);
            
            int[] dimensions = { 1, originalImage.Height, originalImage.Width, 3 };
            var inputTensor = new DenseTensor<byte>(image, dimensions);
            
            var modelInput = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor(_modelInputLayerName, inputTensor)
            };
            
            using var session = _model;
            
            
            var result = session.Run(modelInput);
            
            
            var boxes=((DenseTensor<float>) result.ElementAtOrDefault(0).Value).ToArray();//Convert to output type
            var labels=((DenseTensor<float>) result.ElementAtOrDefault(1).Value).ToArray();
            var scores=((DenseTensor<float>) result.ElementAtOrDefault(2).Value).ToArray();
            var numDetections=((DenseTensor<float>) result.ElementAtOrDefault(3).Value).ToArray();
            
            
            var output = PostProcessFrame(originalImage, boxes, labels, scores, numDetections);
            
            return output;
        }

        private byte[] PreprocessFrame(Image image)
        {


            var bImage = new Bitmap(image);


            var bytes = ProcessUsingLockbitsAndUnsafeAndParallel(bImage);
            

            return bytes;
        }

        private AnalysisOutput PostProcessFrame(Image image, IReadOnlyList<float> boxes, IReadOnlyList<float> labels, IReadOnlyList<float> scores, IReadOnlyList<float> numDetections)
        {
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

        private byte[] ProcessUsingLockbitsAndUnsafeAndParallel(Bitmap img)
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
