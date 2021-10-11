using System;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;
using analysis_engine.BrokerClient.ResourceUsageCollector.Models;
using analysis_engine.BrokerClient.ResourceUsageCollector.ResourceCollector;
using Confluent.Kafka;

namespace analysis_engine.BrokerClient.ResourceUsageCollector
{
    public class ResourceUsageCollector : IResourceUsageCollector
    {
        [HandleProcessCorruptedStateExceptions]
        public void Run()
        {
            while (true)
            {
                //Get the external IP of the Analysis Server
                string externalIpString = new WebClient().DownloadString("http://icanhazip.com")
                    .Replace("\\r\\n", "").Replace("\\n", "").Trim();
                var externalIp = IPAddress.Parse(externalIpString);

                var clientId = System.IO.File.ReadAllText(@"clientid.txt").Replace("\r", "").Replace("\n", "");
                
                //Create a new ServerInformation object
                ServerInformation info = new ServerInformation
                {
                    ServerIp = externalIp.ToString(),
                    ServerId = clientId,
                    ServerPort = "6666",
                    Credentials = "test1234",
                    Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
                    Usage = GetUsage()
                };
                
                //Create a new Kafka producer
                var config = new ProducerConfig
                {
                    BootstrapServers = "localhost:9092",
                };
                TopicPartition partition = new TopicPartition(clientId, 0);
                var producer = new ProducerBuilder<Null, string>(config).SetErrorHandler((prod, _) =>
                {
                    prod.AbortTransaction();
                    prod.Dispose();
                    BrokerClient.isConnected = false;
                }).Build();
            
                //Send information to Broker
                Message<Null, string> msg = new Message<Null, string>();
                msg.Value = info.ToJson();
                try
                {
                    producer.Produce(partition, msg);
                }
                catch (AccessViolationException ignored)
                {
                    // ignored
                }
                catch
                {
                    // ignored
                }

                Thread.Sleep(10000);
            }
        }

        private PerformanceInfo GetUsage()
        {
            return new MockResourceCollector().GetPerformance();
        }
    }
}