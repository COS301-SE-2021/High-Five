using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace analysis_engine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var originalImage1 = CvInvoke.Imread("C:\\Users\\Bieldt\\OneDrive\\Pictures\\cows.jpg", ImreadModes.Unchanged);
            
            var watch1 = new Stopwatch();
            
            var runner1 = new FastVehicleRecognitionTool();
            
            runner1.Init();
            
            var data1 = new Data();
            
            data1.Frame.Image = originalImage1.ToImage<Rgb, byte>();
            
            var result1 = data1;
            
            // Task.Run(() =>
            // {
            //     watch1.Reset();
            //     watch1.Start();
            //     for (int i = 0; i < 100; i++)
            //     {
            //         result1=runner1.Process(data1);
            //     }
            //     watch1.Stop();
            //     Console.WriteLine("1: Execution time: " + watch1.ElapsedMilliseconds/100.0 + "ms");
            // });


            while (true)
            {
                //Make thread sleep such as to not overuse resources
                Thread.Sleep(1000);
            }
            // CvInvoke.Imwrite("C:\\Users\\Bieldt\\OneDrive\\Pictures\\output.jpg", result.Frame.Image);
        }

        
    }
}