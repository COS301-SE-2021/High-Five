﻿namespace analysis_engine.Util
{
    public class InputSplitter : Pipe
    {
        private Pipe[] inputs;
        public InputSplitter(Pipe[] inputs)
        {
            this.inputs = inputs;
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