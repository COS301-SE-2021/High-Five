using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using analysis_engine_v2.BrokerClient.Storage;
using analysis_engine.BrokerClient.CommandHandler.Models;
using analysis_engine.BrokerClient.CommandHandler.Models.commandbody;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace analysis_engine.BrokerClient.CommandHandler.CommandHandler
{
    public class CommandHandler : ICommandHandler
    {
        public void HandleCommand(AnalysisCommand command)
        {
            

            AnalysisStorageManager storageManager = new AnalysisStorageManager();

            string tmpFolder = Path.GetTempPath() + Path.PathSeparator;
            
            string url = "";
            string mediaType = "";
            string pipelineString = "";
            string outputUrl = "";
            if (command.CommandType.Contains("Analyze"))
            {
                StoredMediaCommandBody body =
                    JsonConvert.DeserializeObject<StoredMediaCommandBody>(JsonConvert.SerializeObject(command.Body));
                mediaType = command.CommandType.Contains("AnalyzeImage") ? "image" : "video";
                Debug.Assert(body != null, nameof(body) + " != null");
                pipelineString = storageManager.GetPipeline(body.PipelineId).Result;
                url = mediaType == "image" ? storageManager.GetImage(body.MediaId).Result : storageManager.GetVideo(body.MediaId).Result;
                outputUrl = tmpFolder + mediaType == "image" ? "tmp.jpg" : "tmp.mp4";
            }
            else
            {
                LiveAnalysisCommandBody body =
                    JsonConvert.DeserializeObject<LiveAnalysisCommandBody>(JsonConvert.SerializeObject(command.Body));
                url = ""; //SDK guys need to get this
                outputUrl = body.PublishLink;
                pipelineString = storageManager.GetLivePipeline().Result; 
                mediaType = "stream";
            }

            var runThread = new Thread(delegate()
            {
                RunAnalysis(url, mediaType, pipelineString, outputUrl);
            });
            
            runThread.Start();
            runThread.Join();
        }

        private void RunAnalysis(string url, string mediaType, string pipelineString, string outputUrl)
        {
            var clientId = Environment.GetEnvironmentVariable("ENGINE_CLIENT_ID");
            
            //Create a new Kafka producer
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
            };
            TopicPartition partition = new TopicPartition(clientId, 2);
            var producer = new ProducerBuilder<string, string>(config).Build();

            AnalysisObserver analysisObserver = new AnalysisObserver(url, mediaType, pipelineString, outputUrl);

            while (!analysisObserver.Done)
            {
                //Only send heartbeats if we're analysing stored media.
                if (mediaType != "stream")
                {
                    Thread.Sleep(1000);

                    //Send information to Broker
                    Message<string, string> msg = new Message<string, string>
                    {
                        Key = Guid.NewGuid().ToString(),
                        Value = "heartbeat"
                    };
                    producer.Produce(partition, msg);
                }
            }
        }
    }
}