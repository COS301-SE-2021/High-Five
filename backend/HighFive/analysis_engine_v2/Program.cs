using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using analysis_engine.BrokerClient;
using analysis_engine.Video;
using analysis_engine.Video.ConcreteFrameEncoder;
using analysis_engine_v2.BrokerClient.Storage;
using broker_analysis_client.Client;
using broker_analysis_client.Storage;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using High5SDK;

namespace analysis_engine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            TestStreamAnalysis();
            // var client = new BrokerClient.BrokerClient();
            // client.Run();
        }

        private static void TestVideoAnalysis()
        {
            Console.WriteLine("Starting Analysis...");
            var url =
                @"https://high5storage.blob.core.windows.net/31eb910a-c3f4-412c-b641-26ca8c7c38e3/video/D533488463D0867B3CF57173FA6ABA98.mp4?sp=r&st=2021-09-14T08:49:18Z&se=2021-09-14T16:49:18Z&spr=https&sv=2020-08-04&sr=b&sig=s9cFdgGCI%2FbpkJnJjgmn1cR38g55F33%2B3tjdaUqZG1s%3D";
            var analysis=new AnalysisObserver(url, "video", "analysis:fastvehicles,drawing:boxes", @"C:\Users\hanne\RiderProjects\output.mp4");
            while (!analysis.Done) System.Threading.Thread.Sleep(1000);
            Console.WriteLine("Analysis Done!");
        }

        private static void TestStreamAnalysis()
        {
            Console.WriteLine("Starting Analysis...");
            var url =
                @"http://192.168.11.153:5080/55799ed725ac42bcbb1925c715380541/streams/114725691588075395384115.m3u8";
            var analysis=new AnalysisObserver(url, "stream", "analysis:fastvehicles,drawing:boxes", @"C:\Users\hanne\RiderProjects\output.mp4");
            while (!analysis.Done) System.Threading.Thread.Sleep(1000);
            Console.WriteLine("Analysis Done!");
        }

        private static void TestVideo()
        {
            var count = 0;
            var frameGrabber = new VideoFrameGrabber();
            frameGrabber.Init(@"https://high5storage.blob.core.windows.net/31eb910a-c3f4-412c-b641-26ca8c7c38e3/image/1018157F212558009EE97507E4972AF0.img?sp=r&st=2021-09-14T08:49:40Z&se=2021-09-14T16:49:40Z&spr=https&sv=2020-08-04&sr=b&sig=F8p%2FujTW61op3eKZqC4NagFUfMXrfp1lbjrEDhwusMA%3D");
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

        private static void TestImageAnalysis()
        {
            Console.WriteLine("Starting Analysis...");
            var url =
                @"C:\Users\hanne\RiderProjects\1018157F212558009EE97507E4972AF0.jpg";
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(url);
            var analysis=new AnalysisObserver(stream, "image","analysis:vehicles,drawing:boxes", @"C:\Users\hanne\RiderProjects\output.jpg");
            while (!analysis.Done) System.Threading.Thread.Sleep(1000);
            Console.WriteLine("Analysis Done!");
        }

        
    }
}