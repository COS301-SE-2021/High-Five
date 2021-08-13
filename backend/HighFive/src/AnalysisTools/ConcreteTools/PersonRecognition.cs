using System;
using System.IO; 

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using Accord.Imaging.Converters;
using Accord.Math;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using src.AnalysisTools.AnalysisThread;
using Image = System.Drawing.Image;

namespace src.AnalysisTools.ConcreteTools
{
    public class PersonRecognition : ITool
    {
        private const string ModelPath = @"";//TODO add path to model
        private readonly InferenceSession _model;
        private readonly string _modelInputLayerName;
        private const double MinScore=0.70;
        private const long MinClass = 1;
        private const long MaxClass = 1;
        public const string ToolPurpose = "Person";
        
        private readonly string[] _classes ={
            "__background", "person", "bicycle", "car", "motorcycle", "airplane", "bus", "train", "truck", "boat",
            "traffic light", "fire hydrant", "stop sign", "parking meter", "bench", "bird", "cat", "dog", "horse",
            "sheep", "cow", "elephant", "bear", "zebra", "giraffe", "backpack", "umbrella", "handbag", "tie",
            "suitcase", "frisbee", "skis", "snowboard", "sports ball", "kite", "baseball bat", "baseball glove",
            "skateboard", "surfboard", "tennis racket", "bottle", "wine glass", "cup", "fork", "knife", "spoon", "bowl",
            "banana", "apple", "sandwich", "orange", "broccoli", "carrot", "hot dog", "pizza", "donut", "cake", "chair",
            "couch", "potted plant", "bed", "dining table", "toilet", "tv", "laptop", "mouse", "remote", "keyboard",
            "cell phone", "microwave", "oven", "toaster", "sink", "refrigerator", "book", "clock", "vase", "scissors",
            "teddy bear", "hair drier", "toothbrush",
        };
        public PersonRecognition()
        {
            //Load object recognition model and get ready for analysis
            _model = new InferenceSession(ModelPath);
            _modelInputLayerName = _model.InputMetadata.Keys.Single();
        }
        public AnalysisOutput AnalyseFrame(byte[] frame)
        {
            //Convert from input type frame to 3D array
            Image originalImage;
            using (var ms = new MemoryStream(frame))
            {
                originalImage = Image.FromStream(ms);
            }
            
            var image = PreprocessFrame(originalImage);
            
            int[] dimensions = {3, image[0].Length, image[0][0].Length};
            var inputTensor = new DenseTensor<float>(image.Flatten().Flatten(),dimensions); //image.ToTensor();
            
            var modelInput = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor(_modelInputLayerName, inputTensor)
            };
            using var session = _model;
            
            var result = session.Run(modelInput);
            
            
            var boxes=((DenseTensor<float>) result.ElementAtOrDefault(0).Value).ToArray();//Convert to output type
            var labels=((DenseTensor<long>) result.ElementAtOrDefault(1).Value).ToArray();
            var scores=((DenseTensor<float>) result.ElementAtOrDefault(2).Value).ToArray();
            
            
            return PostProcessFrame(originalImage, boxes, labels, scores);
        }

        private static float[][][] PreprocessFrame(Image image)
        {


            var oldWidth = image.Width;
            var oldHeight = image.Height;
            var ratio = 800.0 / Math.Min(oldWidth, oldHeight);
            var width = Convert.ToInt32(ratio * oldWidth);
            var height = Convert.ToInt32(ratio * oldHeight);
            width = width + 32 - (width % 32);
            height = height + 32 - (height % 32);

            Bitmap bImage = ResizeImage(image, width, height);


            var processedFrame = new float[3][][];

            float[] colourMeans =
                { Convert.ToSingle(102.9801), Convert.ToSingle(115.9465), Convert.ToSingle(122.7717) };

            processedFrame[0] = new float[bImage.Height][];
            processedFrame[1] = new float[bImage.Height][];
            processedFrame[2] = new float[bImage.Height][];
            
            for (var i = 0; i < processedFrame[0].Length; i++) //Might replace with parallel for loop
            {
                processedFrame[0][i] = new float[bImage.Width];
                processedFrame[1][i] = new float[bImage.Width];
                processedFrame[2][i] = new float[bImage.Width];
                
                for (var j = 0; j < processedFrame[0][0].Length; j++)
                {
                    processedFrame[0][i][j] = bImage.GetPixel(j,i).B - colourMeans[0];
                    processedFrame[1][i][j] = bImage.GetPixel(j,i).G - colourMeans[1];
                    processedFrame[2][i][j] = bImage.GetPixel(j,i).R - colourMeans[2];
                }
            }

            return processedFrame;
        }
        
        private AnalysisOutput PostProcessFrame(Image image, IReadOnlyList<float> boxes, IReadOnlyList<long> labels, IReadOnlyList<float> scores)
        {
            //get scale
            var oldWidth = image.Width;
            var oldHeight = image.Height;
            var ratio = Convert.ToSingle(800.0 / Math.Min(oldWidth, oldHeight));
            
            var output = new AnalysisOutput();
            output.Purpose = ToolPurpose;
            output.Boxes = new List<float>();
            output.Classes = new List<string>();
            for (int i = 0; i < labels.Count; i++)
            {
                if (labels[i] >= MinClass && labels[i] <= MaxClass && scores[i] > MinScore)
                {
                    output.Boxes.Add(boxes[i*4] / ratio);
                    output.Boxes.Add(boxes[i*4+1] / ratio);
                    output.Boxes.Add(boxes[i*4+2] / ratio);
                    output.Boxes.Add(boxes[i*4+3] / ratio);
                    
                    output.Classes.Add(_classes[labels[i]]);
                }
            }

            return output;
        }

        private static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using var graphics = Graphics.FromImage(destImage);
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using var wrapMode = new ImageAttributes();
            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
            graphics.DrawImage(image, destRect, 0, 0, image.Width,image.Height, GraphicsUnit.Pixel, wrapMode);

            return destImage;
        }
    }
}