using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
            Console.WriteLine("Starting Analysis...");
            var analysis=new AnalysisObserver(@"https://high5storage.blob.core.windows.net/31eb910a-c3f4-412c-b641-26ca8c7c38e3/video/9269F3E498E14EC2F1871D8221727A4D.mp4?sp=r&st=2021-09-13T13:55:51Z&se=2021-09-13T21:55:51Z&spr=https&sv=2020-08-04&sr=b&sig=3ysadlbbPC84oTqThgrLARR84eqDrbECgUTt0GPFW1E%3D");
            while (!analysis.Done) System.Threading.Thread.Sleep(1000);
            Console.WriteLine("Analysis Done!");
        }

        
    }
}