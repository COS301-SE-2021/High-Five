using System;
using System.Threading;
using analysis_engine.BrokerClient.CommandHandler.Models;
using broker_analysis_client.Client.Models;
using Confluent.Kafka;

namespace analysis_engine.BrokerClient.CommandHandler.CommandHandler
{
    public class MockCommandHandler : ICommandHandler
    {
        /// <summary>
        /// Mocks the command handler interface by waiting 30 seconds and returning a fake result
        /// </summary>
        /// <param name="command"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void HandleCommand(AnalysisCommand command)
        {
            var clientId = Environment.GetEnvironmentVariable("ENGINE_CLIENT_ID");
            
            //Create a new Kafka producer
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
            };
            TopicPartition partition = new TopicPartition(clientId, 2);
            var producer = new ProducerBuilder<string, string>(config).Build();
            for (var i = 0; i < 3; i++)
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

            Message<string, string> msg2;

            if (command.CommandType == "AnalyzeVideo")
            {
                var metaData = new AnalyzedVideoMetaData
                {
                    DateAnalyzed = new DateTime(2021, 01, 01, 12, 59, 05),
                    Id = "12345678",
                    PipelineId = "87654321",
                    Thumbnail = "Thumbnail",
                    Url = "http://localhost",
                    VideoId = "vidid"
                };
                 msg2 = new Message<string, string>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = metaData.ToJson().Replace("\r", "").Replace("\n", "")
                };
                 Console.WriteLine(msg2.Value);
                 producer.Produce(partition, msg2);
            }
            else if (command.CommandType == "AnalyzeImage")
            {
                var metaData = new AnalyzedImageMetaData
                {
                    DateAnalyzed = new DateTime(2021, 01, 01, 12, 59, 05),
                    Id = "12345678",
                    PipelineId = "87654321",
                    Url = "http://localhost",
                    ImageId = "imgid"
                };
                msg2 = new Message<string, string>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = metaData.ToJson().Replace("\r", "").Replace("\n", "")
                };
                Console.WriteLine(msg2.Value);
                producer.Produce(partition, msg2);
            }
        }
    }
}