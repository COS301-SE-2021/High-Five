﻿

using analysis_engine.Util;

namespace analysis_engine.Tools
{
    public interface Tool
    {
        public static Buffer Buffer;
        public Data Process(Data data);
        public void Init();
    }
}