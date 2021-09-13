using System.Threading;

namespace analysis_engine.BrokerClient.CommandHandler
{
    public class Command : ICommand
    {
        public void Run()
        {
            while (true)
            {
                Thread.Sleep(1000);
            }
        }
    }
}