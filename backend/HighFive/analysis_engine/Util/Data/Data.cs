﻿namespace analysis_engine.Util
{
    public abstract class Data
    {
        public Frame Frame { get; set; }
        public bool HasFrame { get; set; }

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