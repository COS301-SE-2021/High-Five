using System;

namespace analysis_engine
{
    public abstract class Tool: MarshalByRefObject
    {
        public static Buffer Buffer;
        public abstract Data Process(Data data);
        public abstract void Init();
    }
}