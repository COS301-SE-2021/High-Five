using System;  
using IronPython.Hosting;  
using Microsoft.Scripting.Hosting;  
using Microsoft.Scripting;  
using Microsoft.Scripting.Runtime;  
using System.IO; 

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace src.AnalysisTools.ConcreteTools
{
    public class CarRecognition: ITool
    {
        private const string modelPath = @"car_detection.onnx";
        private InferenceSession model;
        private string modelInputLayerName;
        public CarRecognition()
        {
            //Load object recognition model and get ready for analysis
            model = new InferenceSession(modelPath);
            modelInputLayerName = model.InputMetadata.Keys.Single();
        }
        public object AnalyseFrame(object frame)
        {
            return null;
        }
    }
}