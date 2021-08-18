using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using FFMediaToolkit.Graphics;
using Org.OpenAPITools.Models;
using src.AnalysisTools.AnalysisThread;
using src.Subsystems.Analysis;

namespace src.AnalysisTools
{
    
    public class AnalyserImpl: IAnalyser
    {
        /*
         *      Description:
         * This class will be used to analyse images and videos. This simplifies the process of feeding
         * frames to our analysis tools. You simply call the start analysis function to start feeding
         * frames, then use the feed frame function to add frames to be analysed. After you are done feeding
         * frames you call the stop analysis function and then you can retrieve the analysed frames with the
         * get frames function
         *
         *      Attributes:
         * -> _outputQueues: different output queues for the different analysed frames.
         * -> _tools: tools in the pipline used for the analysis.
         * -> _toolRunners: used to run analysis for different tools.
         * -> _frameCount: counts how many frames have been fed to the feedFrame function.
         */
        
        private List<BlockingCollection<byte[]>> _outputQueues;
        private List<Tool> _tools;
        private List<ToolRunner> _toolRunners = new();
        private int _frameCount;

        public void StartAnalysis(Pipeline pipeline, IAnalysisModels analysisModels)
        {
            /*
             *      Description:
             * This function sets the class up to start accepting frames to be analysed.
             * All the threads are started here and the queues used by the threads to feed
             * each other data are initialised and passed to them. After this function is
             * called, we are ready to use the feedFrame function.
             *
             *      Parameters:
             * -> pipeline: passes the pipeline containing the names of all the tools that
             *              will be used to perform the analysis
             * -> analysisModels: a class that gives a singleton of each tool with an already
             *                    loaded model that can be used for inference.
             */
            
            _outputQueues = new List<BlockingCollection<byte[]>>();
            _tools = new List<Tool>();
            
            var count = 1;
            foreach (var toolName in pipeline.Tools)
            {
                _tools.Add(analysisModels.GetTool(toolName));
                if (analysisModels.GetTool(toolName).SeparateOutput) count++;
            }

            for (var i = 0; i < count; i++)
            {
                _outputQueues.Add(new BlockingCollection<byte[]>());
            }

            count = 1;
            var baseTools = new List<Tool>();
            foreach (var tool in _tools)
            {
                if (tool.SeparateOutput)
                {
                    var tempList = new List<Tool>();
                    tempList.Add(tool);
                    _toolRunners.Add(new ToolRunner(tempList, _outputQueues[count]));
                    count++;
                }
                else
                {
                    baseTools.Add(tool);
                }
            }
            _toolRunners.Insert(0, new ToolRunner(baseTools, _outputQueues[0]));

            _frameCount = 0;
        }

        public void FeedFrame(byte[] frame)
        {
            /*
             *      Description:
             * This function is called to add a frame to the analysis pipeline to be analysed
             * It goes through all the selected tool for analysis and is then added to an output
             * queue.
             *
             *      Parameters:
             * -> frame: this function recieves a byte array of a single frame that will be used
             *           for analysis
             */
            
            _frameCount++;
            foreach (var toolRunner in _toolRunners)
            {
                toolRunner.Enqueue(frame);
            }
        }

        public void FeedFrame(Bitmap frame)
        {
            /*
             *      Description:
             * This function is called to add a frame to the analysis pipeline to be analysed
             * It goes through all the selected tool for analysis and is then added to an output
             * queue.
             *
             *      Parameters:
             * -> frame: this function recieves a bitmap which will be converted to a byte array
             *           so that it can be analysed
             */
            
            byte[] inputFrame;
            
            var rect = new Rectangle(Point.Empty, frame.Size);
            var bitLock = frame.LockBits(rect, ImageLockMode.ReadWrite, frame.PixelFormat);
            
            using (var ms = new MemoryStream())
            {
                frame.Save(ms, ImageFormat.Png);
                inputFrame =  ms.ToArray();
            }
            
            frame.UnlockBits(bitLock);
            
            
            _frameCount++;
            foreach (var toolRunner in _toolRunners)
            {
                toolRunner.Enqueue(inputFrame);
            }
        }

        public List<List<byte[]>> GetFrames()
        {
            /*
             *      Description:
             * This function waits for the analysis to finish on all the frames fed by the feedFrame
             * function and then returns A list of lists of the frames. The first list is to separate
             * tools that output seperate frames and each of those lists have an analysed version of
             * all the fed frames.
             *
             */
            
            var output = new List<List<byte[]>>();
            
            for (var i = 0; i < _outputQueues.Count; i++)
            {
                output.Add(new List<byte[]>());
                if (_frameCount == 0) break;
                var count = 0;
                foreach (var frame in _outputQueues[i].GetConsumingEnumerable(CancellationToken.None))
                {
                    output[i].Add(frame);
                    count++;
                    if (count == _frameCount) break;
                }
            }

            return output;
        }

        public void EndAnalysis()
        {
            /*
             *      Description:
             * This function is called to end the analysis and allow the threads to stop working.
             * It feeds a byte array of size one to the end of the input queue, and the threads
             * will know that it is the end of the queue.
             * 
             */
            
            foreach (var toolRunner in _toolRunners)
            {
                toolRunner.Enqueue(new byte[] { 0 });
            }
        }
    }
}