using System;

namespace High5SDK
{
    public abstract class Tool
    {
        public static Buffer Buffer;
        public abstract Data Process(Data data);
        public abstract void Init();

        public abstract void Dispose();
    }
}