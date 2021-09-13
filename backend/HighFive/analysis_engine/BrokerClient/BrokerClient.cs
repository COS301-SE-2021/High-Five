using System;
using System.Threading;
using analysis_engine.BrokerClient.CommandHandler;
using analysis_engine.BrokerClient.ResourceUsageCollector;
using Confluent.Kafka;
using Renci.SshNet;

namespace analysis_engine.BrokerClient
{
    public class BrokerClient : IBrokerClient
    {
        private ICommand _commander;
        private IResourceUsageCollector _usageCollector;
        public void Run()
        {
            PrivateKeyFile file = new PrivateKeyFile(@"/home/kyle-pc/.ssh/id_rsa");
            using (var client = new SshClient("newideassolutions.com", "root", file))
            {

                client.Connect();
                var port = new ForwardedPortLocal("127.0.0.1", 9092, "localhost", 9092);
                client.AddForwardedPort(port);
                port.Start();
                
                var clientId = Environment.GetEnvironmentVariable("ENGINE_CLIENT_ID");
                var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

                var registrationString = $"{{\"ServerId\":{clientId},\"Timestamp\":{timestamp}}}";
                
                //Create a new Kafka producer
                var config = new ProducerConfig
                {
                    BootstrapServers = "localhost:9092",
                };
                TopicPartition partition = new TopicPartition("SERVER_REGISTRATION", 0);
                var producer = new ProducerBuilder<Null, string>(config).Build();
            
                //Send information to Broker
                Message<Null, string> msg = new Message<Null, string>();
                msg.Value = registrationString;
                producer.Produce(partition, msg);
                
                Thread.Sleep(30000);

                _commander = new Command();
                _usageCollector = new ResourceUsageCollector.ResourceUsageCollector();

                Thread thread1 = new Thread(_commander.Run);
                thread1.Start();
                Thread thread2 = new Thread(_usageCollector.Run);
                thread2.Start();
                thread1.Join();
                thread2.Join();
                
                
                client.Disconnect();
            }
        }
    }
}