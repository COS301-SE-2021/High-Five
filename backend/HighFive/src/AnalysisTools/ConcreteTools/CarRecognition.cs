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
    }
}