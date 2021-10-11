using System;
using System.Threading;
using analysis_engine.BrokerClient.CommandHandler.CommandHandler;
using analysis_engine.BrokerClient.CommandHandler.Models;
using Confluent.Kafka;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace analysis_engine.BrokerClient.CommandHandler
{
    public class Command : ICommand
    {
        public void Run()
        {
            var clientId = "analysisclient001";
            
            //Create a new Kafka consumer
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "0",
                AutoOffsetReset = AutoOffsetReset.Latest
            };
            TopicPartition partition = new TopicPartition(clientId, 1);
            var consumer = new ConsumerBuilder<String, string>(config).Build();
            consumer.Assign(partition);
            while (true)
            {
                Console.WriteLine("Time to start getting commands");
                var command = consumer.Consume();
                new CommandHandler.CommandHandler().HandleCommand(JsonConvert.DeserializeObject<AnalysisCommand>(command.Message.Value));
            }
        }
    }
}