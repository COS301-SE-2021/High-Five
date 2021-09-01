namespace analysis_engine.Util
{
    public class InputMerger : Pipe
    {
        private Pipe output;
        public InputMerger(Pipe output)
        {
            this.output = output;
        }

        public void push(Data data)
        {
            throw new System.NotImplementedException();
        }

        public Data pop()
        {
            throw new System.NotImplementedException();
        }
    }
}