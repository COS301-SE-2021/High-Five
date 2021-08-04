using System;  
using IronPython.Hosting;  
using Microsoft.Scripting.Hosting;  
using Microsoft.Scripting;  
using Microsoft.Scripting.Runtime;  
using System.IO; 

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace src.AnalysisTools.ConcreteTools
{
    public class ObjectRecognition
    {
        private BlockingCollection<object> _frames = new BlockingCollection<object>();
 
        public ObjectRecognition()
        {
            var thread = new Thread(new ThreadStart(OnStart));
            thread.IsBackground = true;
            thread.Start();
        }
 
        public void Enqueue(object job)
        {
            _frames.Add(job);
        }
 
        private void OnStart()
        {
            foreach (var frame in _frames.GetConsumingEnumerable(CancellationToken.None))
            {
                Console.WriteLine(frame);
            }
        }
    }
}