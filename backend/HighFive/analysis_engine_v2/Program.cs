using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using analysis_engine.Video;
using analysis_engine.Video.ConcreteFrameEncoder;
using broker_analysis_client.Client;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace analysis_engine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            TestAnalysis();
        }

        private static void TestAnalysis()
        {
            Console.WriteLine("Starting Analysis...");
            var url =
                @"https://high5storage.blob.core.windows.net/31eb910a-c3f4-412c-b641-26ca8c7c38e3/video/313E9C0D8AE9CEC3FC6327F00C481D70.mp4?sv=2015-12-11&sr=b&sig=teGCzQ4bfI6iK6rvDPam%2FrHDrZgvIh8KX6TwXReMZYU%3D&st=2021-09-13T15%3A41%3A27Z&se=2021-09-13T18%3A41%3A27Z&sp=r";
            var analysis=new AnalysisObserver(url);
            while (!analysis.Done) System.Threading.Thread.Sleep(1000);
            Console.WriteLine("Analysis Done!");
        }

        private static void TestVideo()
        {
            var count = 0;
            var frameGrabber = new VideoFrameGrabber();
            frameGrabber.Init(@"https://high5storage.blob.core.windows.net/31eb910a-c3f4-412c-b641-26ca8c7c38e3/video/9269F3E498E14EC2F1871D8221727A4D.mp4?sp=r&st=2021-09-13T13:55:51Z&se=2021-09-13T21:55:51Z&spr=https&sv=2020-08-04&sr=b&sig=3ysadlbbPC84oTqThgrLARR84eqDrbECgUTt0GPFW1E%3D");
            var data=new Data(new Frame(frameGrabber.GetNextFrame(), count));
            var frameEncoder = new VideoFrameEncoder(@"C:\Users\hanne\RiderProjects\output.mp4", data.Frame.Image.Size);

            while (data.Frame.Image != null)
            {
                count++;
                frameEncoder.AddFrame(data);
                data.Frame.Image = frameGrabber.GetNextFrame();
                data.Frame.FrameID = count;
            }
        }

        
    }
}