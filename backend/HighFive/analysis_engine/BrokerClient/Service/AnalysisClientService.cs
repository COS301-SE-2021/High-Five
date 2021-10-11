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
            throw new System.NotImplementedException();
        }

        public AnalyzedVideoMetaData AnalyzeVideo(AnalyzeVideoRequest request)
        {
            throw new System.NotImplementedException();
        }
        
        //----------------------------TEST CODE----------------------------------//
        //TODO: REMOVE THIS LATER
        /* This is an example code segment of how the DynamicToolFactory should be called.
         * This creates a user-written tool dynamically and loads it into a secure
         * AppDomain.
         */

        private void DynamicCompilation()
        {
            var sourceCode = File.ReadAllText("../../DynamicTools/SampleCode.txt");
            //var metadataCode = File.ReadAllText("../../DynamicTools/SampleMetadata.txt");
            var userToolFactory = new DynamicToolFactory();

            /*var dynamicTool = userToolFactory.CreateDynamicTool("MyCustomTool", sourceCode);
            dynamicTool.Init();
            dynamicTool.Process(null);*/
            
            userToolFactory.UnloadRestrictedDomain();
        }
        
    }
}