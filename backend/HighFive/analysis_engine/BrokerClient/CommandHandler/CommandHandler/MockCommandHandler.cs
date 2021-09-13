using analysis_engine.BrokerClient.CommandHandler.Models;

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
            throw new System.NotImplementedException();
        }
    }
}