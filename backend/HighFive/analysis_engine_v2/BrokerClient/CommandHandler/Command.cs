using System;
using System.Runtime.ExceptionServices;
using analysis_engine.BrokerClient.CommandHandler.Models;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace analysis_engine.BrokerClient.CommandHandler
{
    public class Command : ICommand
    {
        [HandleProcessCorruptedStateExceptions]
        public void Run()
        {
            var clientId = System.IO.File.ReadAllText(@"clientid.txt").Replace("\r", "").Replace("\n", "");
            
            //Create a new Kafka consumer
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "0",
                AutoOffsetReset = AutoOffsetReset.Latest
            };
            TopicPartition partition = new TopicPartition(clientId, 1);
            var consumer = new ConsumerBuilder<String, string>(config).SetErrorHandler((prod, _) => {
                prod.Dispose();
                BrokerClient.isConnected = false;
            }).Build();
            try
            {
                consumer.Assign(partition);
                while (true)
                {
                    Console.WriteLine("Time to start getting commands");
                    var command = consumer.Consume();
                    new CommandHandler.CommandHandler().HandleCommand(
                        JsonConvert.DeserializeObject<AnalysisCommand>(command.Message.Value));
                }
            }
            catch (AccessViolationException ignored)
            {
            }
            catch
            {
                // ignored
            }
        }
    }
}