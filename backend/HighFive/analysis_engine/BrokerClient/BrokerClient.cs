using analysis_engine.BrokerClient.CommandHandler;
using analysis_engine.BrokerClient.ResourceUsageCollector;

namespace analysis_engine.BrokerClient
{
    public class BrokerClient : IBrokerClient
    {
        private ICommandHandler _commandHandler;
        private IResourceUsageCollector _usageCollector;
        public void Run()
        {
            throw new System.NotImplementedException();
        }
    }
}