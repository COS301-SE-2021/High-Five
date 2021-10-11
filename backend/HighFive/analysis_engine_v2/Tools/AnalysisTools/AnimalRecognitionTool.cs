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
    public class AnimalRecognitionTool : AnalysisTool
    {
        private const string ModelPath = @"Models/ssd-10.onnx";
        private InferenceSession _model;
        private string _modelInputLayerName;
        private const double MinScore=0.5;
        private const long MinClass = 15;
        private const long MaxClass = 24;
        
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

        public override void Init()
        {
            _model = new InferenceSession(
                ModelPath,
                SessionOptions.MakeSessionOptionWithCudaProvider());
            _modelInputLayerName = _model.InputMetadata.Keys.Single();
        }
        public override Data Process(Data data)
        {
            var image = Resize(data.Frame.Image);
            
            image = np.transpose(image, new[] { 2, 0, 1 });
            (image[0], image[2]) = (image[2], image[0]);
            image = np.expand_dims(image, 0);

            float[] mean = { 0.485f, 0.456f, 0.406f };
            float[] std = { 0.229f, 0.224f, 0.225f };
            var input = image/255.0f;
            input[0][0] = input[0][0] - mean[0];
            input[0][1] = input[0][1] - mean[1];
            input[0][2] = input[0][2] - mean[2];
            input[0][0] = input[0][0] / std[0];
            input[0][1] = input[0][1] / std[1];
            input[0][2] = input[0][2] / std[2];
            
            
            int[] dimensions = { 1, 3, 1200, 1200 };
            var inputTensor = new DenseTensor<float>(input.reshape(1*1200*1200*3).ToArray<float>(),dimensions);
            
            var modelInput = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor(_modelInputLayerName, inputTensor)
            };
            
            var predictions = _model.Run(modelInput);
            
            var boxes=((DenseTensor<float>) predictions.ElementAtOrDefault(0).Value).ToArray();
            var labels=((DenseTensor<long>) predictions.ElementAtOrDefault(1).Value).ToArray();
            var scores=((DenseTensor<float>) predictions.ElementAtOrDefault(2).Value).ToArray();
            predictions.Dispose();
            return PostProcessFrame(data, boxes, labels, scores);

        }

        private Data PostProcessFrame(Data data, IReadOnlyList<float> boxes, IReadOnlyList<long> labels, IReadOnlyList<float> scores)
        {
            var output= new BoxCoordinateData();
            output.Classes = new List<string>();
            output.Boxes = new List<float>();
            output.Purpose = "Animal";
            var width = data.Frame.Image.Width;
            var height = data.Frame.Image.Height;
            for (int i = 0; i < labels.Count; i++)
            {
                if (scores[i] > MinScore && labels[i]>=MinClass && labels[i]<=MaxClass)
                {
                    output.Classes.Add(_classes[labels[i]]);
                    output.Boxes.Add(boxes[i * 4] * width);
                    output.Boxes.Add(boxes[i * 4 + 1] * height);
                    output.Boxes.Add(boxes[i * 4 + 2] * width - boxes[i * 4] * width);
                    output.Boxes.Add(boxes[i * 4 + 3] * height - boxes[i * 4 + 1] * height);
                }
            }
            
            data.Meta.Add(output);
            
            return data;
        }
        
        private static NDArray Resize(Image<Rgb,byte> image)
        {
            var resized = image.Resize(1200, 1200, Inter.Area);
            

            var result = np.array(resized.Data).reshape(resized.Height, resized.Width, 3);
        
            return result;
        }
    }
}