using System.Net;

namespace analysis_engine.Util
{
    public interface Pipe
    {
        
        /*
         *      Description:
         * This is an abstract function which should be implemented to add a Data object to the Pipe.
         *      Parameters:
         * -> data: The Data to be added to the Pipe.
         */
        public abstract void push(Data data);
        
        /*
         *      Description:
         * This is an abstract function which should be implemented to add a Data object to the Pipe.
         *      Parameters:
         * -> data: The Data to be added to the Pipe.
         */
        public abstract Data pop();
    }
}