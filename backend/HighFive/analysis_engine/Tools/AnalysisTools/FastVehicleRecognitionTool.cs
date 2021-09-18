using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using High5SDK;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using NumSharp;

namespace analysis_engine
{
    public class FastVehicleRecognitionTool : AnalysisTool
    {
        private const string ModelPath = @"../../Models/ssd_mobilenet_v1_10.onnx";
        private InferenceSession _model;
        private string _modelInputLayerName;
        private const double MinScore=0.50;
        private const long MinClass = 1;
        private const long MaxClass = 9;

        private static string[] _classes ={
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

        public override void Init()
        {
            //
            _model = new InferenceSession(
                ModelPath,SessionOptions.MakeSessionOptionWithCudaProvider()
                );
            _modelInputLayerName = _model.InputMetadata.Keys.Single();
        }
        public override Data Process(Data data)
        {
            var image = data.Frame.Image;
            var input = np.array(image.Bytes);

            int[] dimensions = { 1, image.Height, image.Width, 3 };
            var inputTensor = new DenseTensor<byte>(input.reshape(image.Height*image.Width*3).ToArray<byte>(),dimensions);
            //var inputTensor = new DenseTensor<byte>(image.Bytes,dimensions);
            
            var modelInput = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor(_modelInputLayerName, inputTensor)
            };
            
            var predictions = _model.Run(modelInput);
            
            var boxes=((DenseTensor<float>) predictions.ElementAtOrDefault(0).Value).ToArray();//Convert to output type
            var labels=((DenseTensor<float>) predictions.ElementAtOrDefault(1).Value).ToArray();
            var scores=((DenseTensor<float>) predictions.ElementAtOrDefault(2).Value).ToArray();
            var numDetections=((DenseTensor<float>) predictions.ElementAtOrDefault(3).Value).ToArray();
            
            return PostProcessFrame(data, boxes, labels, scores, numDetections);

        }

        private Data PostProcessFrame(Data data, IReadOnlyList<float> boxes, IReadOnlyList<float> labels, IReadOnlyList<float> scores, IReadOnlyList<float> numDetections)
        {
            var output= new BoxCoordinateData();
            output.Classes = new List<string>();
            output.Boxes = new List<float>();
            output.Purpose = "Object";
            var width = data.Frame.Image.Width;
            var height = data.Frame.Image.Height;
            for (int i = 0; i < numDetections[0]; i++)
            {
                if (scores[i] > MinScore && labels[i]>=MinClass && labels[i]<=MaxClass)
                {
                    output.Classes.Add(_classes[Convert.ToInt32(labels[i]-1)]);
                    output.Boxes.Add(boxes[i * 4 + 1] * width);
                    output.Boxes.Add(boxes[i * 4] * height);
                    output.Boxes.Add(boxes[i * 4 + 3] * width - boxes[i * 4 + 1] * width);
                    output.Boxes.Add(boxes[i * 4 + 2] * height - boxes[i * 4] * height);
                }
            }
            data.Meta.Add(output);
            return data;//DrawBoxes(data, output);
        }
        
        private Data DrawBoxes(Data data, BoxCoordinateData output)
        {
            var image = data.Frame.Image;
            for (var i = 0; i < output.Classes.Count; i++)
            {
                var box = new Rectangle(Convert.ToInt32(output.Boxes[i * 4]),
                    Convert.ToInt32(output.Boxes[i * 4 + 1]), 
                    Convert.ToInt32(output.Boxes[i * 4 + 2]),
                    Convert.ToInt32(output.Boxes[i * 4 + 3]));
                var point = new Point(Convert.ToInt32(output.Boxes[i * 4]),
                    Convert.ToInt32(output.Boxes[i * 4 + 1] - 10.0 * image.Height / 2286.0));
                CvInvoke.Rectangle(image, box, new Bgr(Color.Red).MCvScalar, 5, LineType.Filled);
                CvInvoke.PutText(image, output.Classes[i].ToUpper(), point, FontFace.HersheyTriplex, 2.0, new Bgr(Color.Red).MCvScalar, 5);
            }
            var textPoint = new Point(image.Width / 445, 6*image.Height / 229);
            CvInvoke.PutText(image, "Vehicle Count: "+output.Classes.Count, textPoint, FontFace.HersheyTriplex, 2.0, new Bgr(Color.Red).MCvScalar, 5);

            data.Frame.Image = image;
            return data;
        }
    }
}