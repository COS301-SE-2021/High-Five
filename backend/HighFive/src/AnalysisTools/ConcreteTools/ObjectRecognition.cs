using System;  
using IronPython.Hosting;  
using Microsoft.Scripting.Hosting;  
using Microsoft.Scripting;  
using Microsoft.Scripting.Runtime;  
using System.IO; 

// using System;
// using System.Collections.Generic;
// using System.Drawing;
// using System.Linq;
// using Microsoft.ML.OnnxRuntime;
// using Microsoft.ML.OnnxRuntime.Tensors;

namespace src.AnalysisTools.ConcreteTools
{
    public class ObjectRecognition
    {
		public static void Start(string[] args)  
        {  
            // float[][] image = PreprocessTestImage(imagePath);
            //
            // const string modelPath = @"mnist-model.onnx";
            // float[] probabilities = Predict(modelPath, image);
        }
        
        private static float[][] PreprocessTestImage(string path)
        {
            // var img = new Bitmap(path);
            // var result = new float[img.Width][];
            //
            // for (int i = 0; i < img.Width; i++)
            // {
            //     result[i] = new float[img.Height];
            //     for (int j = 0; j < img.Height; j++)
            //     {
            //         var pixel = img.GetPixel(i, j);
            //         
            //         var gray = RgbToGray(pixel);
            //         
            //         // Normalize the Gray value to 0-1 range
            //         var normalized = gray / 255;
            //         
            //         result[i][j] = normalized;
            //     }
            // }
            // return result;
            return null;
        }
        
        private static float[] Predict(string modelPath, float[][] image)
        {
            // using var session = new InferenceSession(modelPath);
            // var modelInputLayerName = session.InputMetadata.Keys.Single();
            //
            // var imageFlattened = image.SelectMany(x => x).ToArray();
            // int[] dimensions = {1, 28, 28};
            // var inputTensor = new DenseTensor<float>(imageFlattened, dimensions);
            // var modelInput = new List<NamedOnnxValue>
            // {
            //     NamedOnnxValue.CreateFromTensor(modelInputLayerName, inputTensor)
            // };
            //
            // var result = session.Run(modelInput);
            // return ((DenseTensor<float>) result.Single().Value).ToArray();
        }
    }
}