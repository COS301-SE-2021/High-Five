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
        private volatile object _url;
        private volatile object _request = null;
        private volatile string _mediaType = "";
        private volatile string _pipelineString = "";
        private volatile string _outputUrl = "";
        private volatile TopicPartition _partition;
        private volatile IProducer<string, string> _producer;

        public CommandHandler()
        {
            var clientId = "analysisclient001";
            
            //Create a new Kafka producer
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
            };
            _partition = new TopicPartition(clientId, 2);
            _producer = new ProducerBuilder<string, string>(config).Build();
        }
        
        public void HandleCommand(AnalysisCommand command)
        {
            _isDone = false;

            StorageManagerContainer.StorageManager = new StorageManager(command.UserId);
            _storageManager = new AnalysisStorageManager();

            string tmpFolder = Path.GetTempPath();
            
            
            var initThread = new Thread(delegate()
            {
                if (command.CommandType.Contains("Analyze"))
                {
                    StoredMediaCommandBody body =
                        JsonConvert.DeserializeObject<StoredMediaCommandBody>(JsonConvert.SerializeObject(command.Body));
                
                    _mediaType = command.CommandType.Contains("AnalyzeImage") ? "image" : "video";
                    Debug.Assert(body != null, nameof(body) + " != null");
                    _pipelineString = _storageManager.GetPipeline(body.PipelineId).Result;
                    Console.WriteLine(_pipelineString);
                    _url =_mediaType == "image" ? _storageManager.GetImage(body.MediaId) : _storageManager.GetVideo(body.MediaId);
                    _outputUrl = "tmp" +  Guid.NewGuid().ToString().Replace("-", "") +  (_mediaType == "image" ? ".jpg" : ".mp4");
                    _outputUrl = tmpFolder + _outputUrl;
                    if (_mediaType == "image")
                    {
                        _request = new AnalyzeImageRequest
                        {
                            ImageId = body.MediaId,
                            PipelineId = body.PipelineId
                        };
                    }
                    else
                    {
                        _request = new AnalyzeVideoRequest()
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
                    Debug.Assert(body != null, nameof(body) + " != null");
                    _url = body.PlayLink; //SDK guys need to get this
                    _outputUrl = _outputUrl = "tmp" +  Guid.NewGuid().ToString().Replace("-", "") +  ".mp4";
                    _pipelineString = _storageManager.GetLivePipeline().Result; //Backend guy has delivered.
                    _mediaType = "stream";
                }
                
                _isDone = true;
            });
            
            initThread.Start();
            while (!_isDone)
            {
                //Only send heartbeats if we're analysing stored media.
                if (_mediaType != "stream")
                {
                    SendHeartbeat();
                }
            }

            initThread.Join();
            
            var runThread = new Thread(RunAnalysis);
            
            runThread.Start();
            runThread.Join();
            
            _request = null;
            _mediaType = "";
            _pipelineString = "";
            _outputUrl = "";
        }

        private void RunAnalysis()
        {
            _isDone = false;

            AnalysisObserver analysisObserver = null;

            var createObserverThread = new Thread(delegate()
            {
                if (_mediaType == "image")
                {
                    analysisObserver = new AnalysisObserver((Stream) _url, _mediaType, _pipelineString, _outputUrl);
                }
                else
                {
                    analysisObserver = new AnalysisObserver((string) _url, _mediaType, _pipelineString, _outputUrl);
                }

                _isDone = true;
            });
            
            createObserverThread.Start();

            while (!_isDone)
            {
                //Only send heartbeats if we're analysing stored media.
                if (_mediaType != "stream")
                {
                    SendHeartbeat();
                }
            }

            createObserverThread.Join();
            
            Console.WriteLine("Analysis Started");

            Debug.Assert(analysisObserver != null, nameof(analysisObserver) + " != null");
            while (!analysisObserver.Done)
            {
                //Only send heartbeats if we're analysing stored media.
                if (_mediaType != "stream")
                {
                    SendHeartbeat();
                }
            }
            
            Console.WriteLine("Analysis Done!");

            if (_request != null)
            {
                _isDone = false;
                var mediaUploader = new Thread(delegate()
                {
                    if (_mediaType == "image")
                    {
                        var analyzedImageMetaData =
                            _storageManager.StoreImage(_outputUrl, (AnalyzeImageRequest) _request).Result;
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
                            _storageManager.StoreVideo(_outputUrl, (AnalyzeVideoRequest) _request).Result;
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
                    if (_mediaType != "stream")
                    {
                        SendHeartbeat();
                    }
                }
                var returnMessage = new Message<string, string>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = _retString.Replace("\r", "").Replace("\n", "")
                };
                _producer.Produce(_partition, returnMessage);
                _isDone = false;
                _retString = "";
                mediaUploader.Join();
            }
        }

        private void SendHeartbeat()
        {
            Thread.Sleep(1000);

            //Send information to Broker
            Message<string, string> msg = new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = "heartbeat"
            };
            _producer.Produce(_partition, msg);
        }
    }
}