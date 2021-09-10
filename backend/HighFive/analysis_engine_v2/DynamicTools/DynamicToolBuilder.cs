namespace analysis_engine.BrokerClient
{
    public class DynamicToolBuilder : ToolBuilder
    {
        public override void BuildTool(string name)
        {
            var toolFactory = new DynamicToolFactory();
            Tool = toolFactory.CreateDynamicTool(name);
        }

        public override Tool GetTool()
        {
            return Tool;
        }
    }
}