using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            var originalImage = CvInvoke.Imread(@"C:\Users\hanne\RiderProjects\ConsoleApp1\ConsoleApp1\Tensorflow\data\cows.jpg", ImreadModes.Unchanged);
            var watch = new Stopwatch();
            var runner = new SelfDrawingAnimalRecognitionTool();
            runner.Init();
            var data = new Data();
            data.Frame.Image = originalImage.ToImage<Rgb, byte>();
            var result=runner.Process(data);
            watch.Reset();
            watch.Start();
            for (int i = 0; i < 1; i++)
            {
                result=runner.Process(data);
            }
            
            watch.Stop();
            Console.WriteLine("Execution time: " + watch.ElapsedMilliseconds + "ms");
            CvInvoke.Imwrite(@"C:\Users\hanne\RiderProjects\ConsoleApp1\ConsoleApp1\Tensorflow\data\output.jpg", result.Frame.Image);
            
        }

        
    }
}