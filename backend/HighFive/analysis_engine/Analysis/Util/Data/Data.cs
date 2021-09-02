﻿using System.Collections.Generic;
using analysis_engine.Analysis.Util.Data.ConcreteData;
using analysis_engine.Util;

namespace analysis_engine.Analysis.Util.Data
{
    public abstract class Data
    {
        public Frame Frame { get; set; }
        public bool HasFrame { get; set; }
        
        public List<MetaData> Meta { get; set; }
        protected Data()
        {
        }

        protected Data(Frame frame, bool hasFrame)
        {
            Frame = frame;
            HasFrame = hasFrame;
        }
    }
}