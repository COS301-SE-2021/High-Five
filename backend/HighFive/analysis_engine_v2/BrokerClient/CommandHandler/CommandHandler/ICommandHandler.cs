using analysis_engine.BrokerClient.CommandHandler.Models;

namespace analysis_engine.BrokerClient.CommandHandler.CommandHandler
{
    public interface ICommandHandler
    {
        /// <summary>
        /// This function executes the instructions passed to it and returns the
        /// result to the Broker.
        /// </summary>
        /// <param name="command">Command to execute</param>
        public void HandleCommand(AnalysisCommand command);
    }
}