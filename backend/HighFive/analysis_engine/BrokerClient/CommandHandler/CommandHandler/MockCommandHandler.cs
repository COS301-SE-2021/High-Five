using System;
using System.Threading;
using analysis_engine.BrokerClient.CommandHandler.Models;
using broker_analysis_client.Client.Models;
using Confluent.Kafka;

namespace analysis_engine.BrokerClient.CommandHandler.CommandHandler
{
    public class MockCommand : ICommandHandler
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
            var producer = new ProducerBuilder<Null, string>(config).Build();
            for (var i = 0; i < 3; i++)
            {
                Thread.Sleep(1000);

                //Send information to Broker
                Message<Null, string> msg = new Message<Null, string>();
                msg.Value = "heartbeat";
                producer.Produce(partition, msg);
            }

            var metaData = new AnalyzedVideoMetaData
            {
                DateAnalyzed = new DateTime(2021, 01, 01),
                Id = "12345678",
                PipelineId = "87654321",
                Thumbnail = "Thumbnail",
                Url = "http://localhost",
                VideoId = "vidid"
            };
            Message<Null, string> msg2 = new Message<Null, string>();
            msg2.Value = metaData.ToJson();
            producer.Produce(partition, msg2);
        }
    }
}