using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using analysis_engine_v2.BrokerClient.Storage;
using analysis_engine.BrokerClient.CommandHandler.Models;
using analysis_engine.BrokerClient.CommandHandler.Models.commandbody;
using broker_analysis_client.Client.Models;
using broker_analysis_client.Storage;
using Confluent.Kafka;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace analysis_engine.BrokerClient.CommandHandler.CommandHandler
{
    public class CommandHandler : ICommandHandler
    {
        private volatile bool _isDone = false;
        private volatile string _retString = "";
        private AnalysisStorageManager _storageManager;
        
        public void HandleCommand(AnalysisCommand command)
        {

            StorageManagerContainer.StorageManager = new StorageManager(command.UserId);
            _storageManager = new AnalysisStorageManager();

            string tmpFolder = Path.GetTempPath();
            
            object url;
            object request = null;
            string mediaType = "";
            string pipelineString = "";
            string outputUrl = "";
            if (command.CommandType.Contains("Analyze"))
            {
                StoredMediaCommandBody body =
                    JsonConvert.DeserializeObject<StoredMediaCommandBody>(JsonConvert.SerializeObject(command.Body));
                
                mediaType = command.CommandType.Contains("AnalyzeImage") ? "image" : "video";
                Debug.Assert(body != null, nameof(body) + " != null");
                pipelineString = _storageManager.GetPipeline(body.PipelineId).Result;
                url = mediaType == "image" ? _storageManager.GetImage(body.MediaId) : _storageManager.GetVideo(body.MediaId);
                /*
                url = @"C:\Users\Marco\Downloads\cars.jpg";*/
                outputUrl = "tmp" +  Guid.NewGuid().ToString().Replace("-", "") +  (mediaType == "image" ? ".jpg" : ".mp4");
                outputUrl = tmpFolder + outputUrl;
                if (mediaType == "image")
                {
                    request = new AnalyzeImageRequest
                    {
                        ImageId = body.MediaId,
                        PipelineId = body.PipelineId
                    };
                }
                else
                {
                    request = new AnalyzeVideoRequest()
                    {
                        VideoId = body.MediaId,
                        PipelineId = body.PipelineId
                    };
                }
            }
            else
            {
                LiveAnalysisCommandBody body =
                    JsonConvert.DeserializeObject<LiveAnalysisCommandBody>(JsonConvert.SerializeObject(command.Body));
                url = ""; //SDK guys need to get this
                outputUrl = body.PublishLink;
                pipelineString = ""; //Backend guy needs to write a function to get this.
                mediaType = "stream";
            }

            var runThread = new Thread(delegate()
            {
                RunAnalysis(request, url, mediaType, pipelineString, outputUrl);
            });
            
            runThread.Start();
            runThread.Join();
        }

        private void RunAnalysis(object request, object url, string mediaType, string pipelineString, string outputUrl)
        {
            Console.WriteLine(outputUrl);
            var clientId = "analysisclient001";
            
            //Create a new Kafka producer
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
            };
            TopicPartition partition = new TopicPartition(clientId, 2);
            var producer = new ProducerBuilder<string, string>(config).Build();

            AnalysisObserver analysisObserver;

            if (mediaType == "image")
            {
                analysisObserver = new AnalysisObserver((Stream) url, mediaType, pipelineString, outputUrl);
            }
            else
            {
                analysisObserver = new AnalysisObserver((string)url, mediaType, pipelineString, outputUrl);
            }

            while (!analysisObserver.Done)
            {
                //Only send heartbeats if we're analysing stored media.
                if (mediaType != "stream")
                {
                    sendHeartbeat(producer, partition);
                }
            }

            if (request != null)
            {
                var mediaUploader = new Thread(delegate()
                {
                    Thread.Sleep(5000);
                    if (mediaType == "image")
                    {
                        var analyzedImageMetaData =
                            _storageManager.StoreImage(outputUrl, (AnalyzeImageRequest) request).Result;
                        JObject returnData = JsonConvert.DeserializeObject<JObject>(analyzedImageMetaData.ToJson());
                        if (returnData != null)
                        {
                            returnData["status"] = "success";
                            _retString = JsonConvert.SerializeObject(returnData);
                        }
                        else
                        {
                            _retString = analyzedImageMetaData.ToJson();
                        }
                    }
                    else
                    {
                        var analyzedVideoMetaData =
                            _storageManager.StoreVideo(outputUrl, (AnalyzeVideoRequest) request).Result;
                        JObject returnData = JsonConvert.DeserializeObject<JObject>(analyzedVideoMetaData.ToJson());
                        if (returnData != null)
                        {
                            returnData["status"] = "success";
                            _retString = JsonConvert.SerializeObject(returnData);
                        }
                        else
                        {
                            _retString = analyzedVideoMetaData.ToJson();
                        }
                    }

                    _isDone = true;
                });
            
                mediaUploader.Start();
                while (!_isDone)
                {
                    //Only send heartbeats if we're analysing stored media.
                    if (mediaType != "stream")
                    {
                        sendHeartbeat(producer, partition);
                    }
                }
                var returnMessage = new Message<string, string>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = _retString.Replace("\r", "").Replace("\n", "")
                };
                producer.Produce(partition, returnMessage);
                _isDone = false;
                _retString = "";
                mediaUploader.Join();
            }
        }

        private void sendHeartbeat(IProducer<string, string> producer, TopicPartition partition)
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