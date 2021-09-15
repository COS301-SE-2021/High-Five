namespace analysis_engine
{
    public interface Pipe
    {
        
        /*
         *      Description:
         * This is an abstract function which should be implemented to add a Data object to the Pipe.
         *      Parameters:
         * -> data: The Data to be added to the Pipe.
         */
        public abstract void Push(Data data);
        
        /*
         *      Description:
         * This is an abstract function which should be implemented to add a Data object to the Pipe.
         *      Parameters:
         * -> data: The Data to be added to the Pipe.
         */
        public abstract Data Pop();
    }
}