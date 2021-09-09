using System;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using analysis_engine;
using analysis_engine.BrokerClient;
using broker_analysis_client.Client.Models;

namespace broker_analysis_client.Client
{
    public class AnalysisClientService: IAnalysisClientService
    {
        public AnalyzedImageMetaData AnalyzeImage(AnalyzeImageRequest request)
        {
            DynamicCompilation();//A test function
            return null;
            throw new System.NotImplementedException();
        }

        public AnalyzedVideoMetaData AnalyzeVideo(AnalyzeVideoRequest request)
        {
            throw new System.NotImplementedException();
        }
        
        //----------------------------TEST CODE----------------------------------//
        //TODO: REMOVE THIS LATER

        private void DynamicCompilation()
        {
            var sourceCode = File.ReadAllText("../../BrokerClient/Service/SampleCode.txt");
            var userToolFactory = new DynamicToolFactory();

            var dynamicTool = userToolFactory.CreateDynamicTool("MyCustomTool", sourceCode);
            dynamicTool.Init();
            dynamicTool.Process(null);
            
            userToolFactory.UnloadRestrictedDomain();
        }
        
    }
}