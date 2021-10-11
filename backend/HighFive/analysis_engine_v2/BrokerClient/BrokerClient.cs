using System;
using System.Net;
using System.Net.Sockets;
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
        private long brokerTime;

        public static volatile bool isConnected = true;

        /// <summary>
        /// Run the BrokerClient. This function registers the AnalysisEngine to the Broker
        /// and creates and runs the ResourceCollector and CommandHandler classes.
        /// </summary>
        /// 
        public void Run()
        {
            while (true)
            {
                Console.WriteLine("Broker client started");
                //Open a SSH tunnel to the remote server. This authenticates the BrokerClient.
                PrivateKeyFile file = new PrivateKeyFile(@"id_rsa");
                using (var client = new SshClient("newideassolutions.com", "root", file))
                {
                    isConnected = true;
                    client.Connect();
                    var port = new ForwardedPortLocal("127.0.0.1", 9092, "localhost", 9092);
                    client.AddForwardedPort(port);
                    port.Start();

                    var clientId = System.IO.File.ReadAllText(@"clientid.txt").Replace("\r", "").Replace("\n", "");
                    var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

                    var registrationString = $"{{\"ServerId\":{clientId},\"Timestamp\":{timestamp}}}";

                    //Create a new Kafka producer
                    var config = new ProducerConfig
                    {
                        BootstrapServers = "localhost:9092",
                        RequestTimeoutMs = 10000
                    };

                    TopicPartition partition = new TopicPartition("SERVER_REGISTRATION", 0);
                    var producer = new ProducerBuilder<Null, string>(config)
                        .SetErrorHandler((prod, _) => {
                            prod.AbortTransaction();
                            prod.Dispose();
                            isConnected = false;
                        }).Build();

                    //Send information to Broker
                    Message<Null, string> msg = new Message<Null, string>();
                    msg.Value = registrationString;
                    try
                    {
                        producer.Produce(partition, msg);
                    }
                    catch (AccessViolationException ignored)
                    {
                    }
                    catch (Exception ignored)
                    {
                    }

                    //Give Broker enough time to register the AnalysisEngine
                    Thread.Sleep(15000);

                    _commander = new Command();
                    _usageCollector = new ResourceUsageCollector.ResourceUsageCollector();

                    Thread thread1 = new Thread(_commander.Run);
                    thread1.Start();
                    Thread thread2 = new Thread(_usageCollector.Run);
                    thread2.Start();

                    while (true)
                    {
                        if (!isConnected)
                        {
                            thread1.Abort();
                            thread2.Abort();
                            break;
                        }

                        Thread.Sleep(1000);
                    }

                    client.Disconnect();

                    thread1.Join();
                    thread2.Join();
                }

                Console.WriteLine("Connection lost. Restarting client");
                Thread.Sleep(60000);
            }
        }
    }
}