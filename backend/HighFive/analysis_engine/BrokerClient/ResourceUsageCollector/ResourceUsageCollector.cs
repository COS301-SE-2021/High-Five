using System;
using Confluent.Kafka;

namespace analysis_engine.BrokerClient.ResourceUsageCollector
{
    public class ResourceUsageCollector
    {
        public void Run()
        {
            GetUsage();

            // var config = new ProducerConfig
            // {
            //     BootstrapServers = "localhost:9092",
            //     ClientId = "0",
            // };
            //
            // var clientId = System.Environment.GetEnvironmentVariable("ENGINE_CLIENT_ID");
            //
            // TopicPartition partition = new TopicPartition(clientId, 0);
            //
            // var producer = new ProducerBuilder<Null, string>(config).Build();
            //
            // while (true)
            // {
            //     Message<Null, string> msg = new Message<Null, string>();
            //     producer.Produce(partition, msg);
            // }
        }

        private string GetUsage()
        {
            return null;
        }
    }
}