namespace analysis_engine.BrokerClient
{
    public class DynamicToolBuilder : ToolBuilder
    {
        private DynamicToolFactory _factory;
        public override void BuildTool(string name)
        {
            _factory = new DynamicToolFactory();
            Tool = _factory.CreateDynamicTool(name);
        }

        public override Tool GetTool()
        {
            return Tool;
        }

        /*~DynamicToolBuilder()
        {
            _factory.UnloadRestrictedDomain();
        }*/
    }
}