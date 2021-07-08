using System;  
using IronPython.Hosting;  
using Microsoft.Scripting.Hosting;  
using Microsoft.Scripting;  
using Microsoft.Scripting.Runtime;  
using System.IO; 

namespace src.Subsystems.MediaStorage
{
    public class ObjectRecognition
    {
		public static void Start(string[] args)  
        {  
            //// Creates the python runtime   
            ScriptRuntime ipy = Python.CreateRuntime();  
            //// The dynamic keyword is needed   
            //// here because Python is an interpreted   
            //// scripting language where calls are bound   
            //// at run time, not at compile time.   
            //// There is no simple or practical way to   
            //// bind calls to Python at compile time   
            //// because Python is designed to be a   
            //// dynamic language which is resolved   
            //// at runtime.   
            dynamic test1 = ipy.UseFile("main.py");  
            // Call function   
            string result=test1.analyse();  
            Console.WriteLine(result);
        } 
		
        public byte[] ProcessFrame(byte[] frame)
        {
            // IntPtr image= CvInvoke.imread
            return new byte[0];
        }
    }
}