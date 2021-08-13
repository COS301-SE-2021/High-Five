using System;
using System.IO; 

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading;
using Accord.Imaging;
using Accord.Imaging.Converters;
using Accord.Math;
using Accord.Statistics.Kernels;
using Accord.Statistics.Models.Fields.Features;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Image = System.Drawing.Image;
using Matrix = Accord.Math.Matrix;

namespace src.AnalysisTools.ConcreteTools
{
    public class CarRecognition: ITool
    {
        private const string ModelPath = @"";//TODO add path to model
        private readonly InferenceSession _model;
        private readonly string _modelInputLayerName;
        private const double MinScore=0.70;
        private const long MinClass = 2;
        private const long MaxClass = 9;
        private readonly ImageFormat _frameFormat = ImageFormat.Jpeg;
        private const string ToolPurpose = "Vehicle";
        
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
        public CarRecognition()
        {
            //Load object recognition model and get ready for analysis
            _model = new InferenceSession(ModelPath);
            _modelInputLayerName = _model.InputMetadata.Keys.Single();
        }
        public object AnalyseFrame(object frame)
        {
            return null;
        }
        
        private float[][][] PreprocessFrame(Image image)
        {
            
            
            var oldWidth = image.Width;
            var oldHeight = image.Height;
            var ratio = 800.0 / Math.Min(oldWidth, oldHeight);
            var width = Convert.ToInt32(ratio * oldWidth);
            var height = Convert.ToInt32(ratio * oldHeight);
            width = width + 32 - (width % 32);
            height = height + 32 - (height % 32);
            
            var bImage = ResizeImage(image, width, height);
            
            
            var processedFrame=new float[3][][];
            var converter = new ImageToMatrix();
            
            converter.Channel = 0;
            converter.Convert(bImage,out processedFrame[2]);
            converter.Channel = 1;
            converter.Convert(bImage,out processedFrame[1]);
            converter.Channel = 2;
            converter.Convert(bImage,out processedFrame[0]);
            
            float[] colourMeans ={ Convert.ToSingle(102.9801), Convert.ToSingle(115.9465), Convert.ToSingle(122.7717) };
            
            for (var i = 0; i < processedFrame[0].Length; i++)//Might replace with parallel for loop
            {
                for (var j = 0; j < processedFrame[0][0].Length; j++)
                {
                    processedFrame[0][i][j] = processedFrame[0][i][j] * 255 - colourMeans[0];
                    processedFrame[1][i][j] = processedFrame[1][i][j] * 255 - colourMeans[1];
                    processedFrame[2][i][j] = processedFrame[2][i][j] * 255 - colourMeans[2];
                }
            }
            return processedFrame;
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