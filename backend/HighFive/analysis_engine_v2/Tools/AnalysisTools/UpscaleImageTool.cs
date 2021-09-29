using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class UpscaleImageTool : AnalysisTool
    {
        private const string ModelPath = @"../../Models/super-resolution-10.onnx";
        private InferenceSession _model;
        private string _modelInputLayerName;
        
        public override void Init()
        {
            var sessionOptions = SessionOptions.MakeSessionOptionWithCudaProvider();
            sessionOptions.InterOpNumThreads = 4;
            sessionOptions.IntraOpNumThreads = 4;
            _model = new InferenceSession(
                ModelPath, sessionOptions
            );
            _modelInputLayerName = _model.InputMetadata.Keys.Single();
        }
        
        public override Data Process(Data data)
        {
            var image = data.Frame.Image;
            // var watch = new Stopwatch();
            // watch.Reset();
            // watch.Start();
            var (inputs,rest) = Preprocess(image);
            // watch.Stop();
            // Console.WriteLine(watch.ElapsedMilliseconds+"ms");

            int[] dimensions = { 1, 1, 224, 224 };
            float[][][] outputs = new float[inputs.Length][][];
            
            var watch = new Stopwatch();
            watch.Reset();
            watch.Start();
            
            for (int i = 0; i < inputs.Length; i++)
            {
                outputs[i] = new float[inputs[i].Length][];
                for (int j = 0; j < inputs[i].Length; j++)
                {
                    inputs[i][j] = inputs[i][j] / 255.0f;
                    var inputTensor = new DenseTensor<float>(inputs[i][j].reshape(1 * 224 * 224 * 1).ToArray<float>(),
                        dimensions);

                    var modelInput = new List<NamedOnnxValue>
                    {
                        NamedOnnxValue.CreateFromTensor(_modelInputLayerName, inputTensor)
                    };

                    var predictions = _model.Run(modelInput);
                    outputs[i][j] = ((DenseTensor<float>)predictions.ElementAtOrDefault(0).Value).ToArray();
                }
            }
            
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds+"ms");




            return PostProcessFrame(data, outputs, rest);
        }

        private (NDArray[][], Image<Gray, byte>[][][]) Preprocess(Image<Rgb, byte> inputImage)
        {
            
            Image<Ycc, byte> image = new Image<Ycc, byte>(224 * ((inputImage.Width / 224) + 1),224 * ((inputImage.Height / 224) + 1));
            var temp = inputImage.Convert<Ycc, byte>();
            CvInvoke.CopyMakeBorder(temp, image, 0,
                224 * ((inputImage.Height / 224) + 1) - inputImage.Height, 0,
                224 * ((inputImage.Width / 224) + 1) - inputImage.Width, BorderType.Constant, new MCvScalar(0,0,0));
            NDArray[][] regions = new NDArray[image.Height / 224][];
            Image<Gray, byte>[][][] rest = new Image<Gray, byte>[image.Height / 224][][];
            for (int i = 0; i < image.Height / 224; i++)
            {
                regions[i] = new NDArray[image.Width / 224];
                rest[i] = new Image<Gray, byte>[image.Width / 224][];
                for (int j = 0; j < image.Width / 224; j++)
                {
                    image.ROI = new Rectangle(j * 224, i * 224, 224, 224);
                    var separatedImage = image.Copy().Split();
                    regions[i][j] = np.array(separatedImage[0].Bytes);
                    rest[i][j] = new [] { separatedImage[1].Resize(672,672, Inter.Cubic), separatedImage[2].Resize(672,672, Inter.Cubic) };
                    image.ROI=Rectangle.Empty;
                }
            }
            
            return (regions,rest);
        }

        private Data PostProcessFrame(Data data, float[][][] outputs, Image<Gray, byte>[][][] rest)
        {
            Image<Rgb, byte> image = new Image<Rgb, byte>(outputs[0].Length * 672, outputs.Length * 672);
            float max = 0;
            float min = 1;
            for (int i = 0; i < outputs.Length; i++)
            {
                for (int j = 0; j < outputs[i].Length; j++)
                {
                    for (int k = 0; k < outputs[i][j].Length; k++)
                    {
                        if (outputs[i][j][k] > max) max = outputs[i][j][k];
                        if (outputs[i][j][k] < min) min = outputs[i][j][k];
                    }
                }
            }

            for (int i = 0; i < outputs.Length; i++)
            {
                for (int j = 0; j < outputs[i].Length; j++)
                {

                    byte[] arr = new byte[outputs[i][j].Length];
                    for (int k = 0; k < arr.Length; k++)
                    {
                        var result = Math.Max(Math.Min(((outputs[i][j][k] - min) / (max - min))*255, 255), 0);
                        arr[k] = Convert.ToByte(result);
                    }
                    var components = new MatND<byte>(new []{3,672,672});
                    var img = new Image<Gray, byte>(672, 672);
                    img.Bytes = arr;
                    components.Bytes = img.Bytes.Concat(rest[i][j][0].Bytes).Concat(rest[i][j][1].Bytes).ToArray();
                    var unfinishedSegment = new Image<Ycc, byte>(672,672);
                    CvInvoke.Merge(components,unfinishedSegment);
                    var outputSegment = unfinishedSegment.Convert<Rgb, byte>();
                    image.ROI = new Rectangle(j * 672, i * 672, 672, 672);
                    outputSegment.CopyTo(image);
                    image.ROI=Rectangle.Empty;
                }
            }

            image.ROI = new Rectangle(0, 0, 3 * data.Frame.Image.Width,
                3 * data.Frame.Image.Height);
            data.Frame.Image = image.Copy();
            return data;
        }
    }
}