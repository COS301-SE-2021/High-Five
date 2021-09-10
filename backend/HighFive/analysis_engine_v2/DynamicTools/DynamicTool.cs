using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using analysis_engine_v2.BrokerClient.Service.Models;
using Emgu.CV.DepthAI;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace analysis_engine.BrokerClient
{
    public class DynamicTool: Tool
    {
        /*
         * This is the object adapter class for dynamically compiled tools. It can be both a
         * drawing tool or analysis tool. It must receive a name as input. The LoadCompiledBytes
         * function must be called before Init or process can be called, this loads the dynamic
         * code into the tool.
         */
        
        private Type _dynamicType;
        private object _dynamicObject;
        public string Name { get; }
        
        public DynamicTool(string name)
        {
            Name = name;
        }

        public void LoadCompiledBytes(byte[] assemblyBytes)
        {
            var assembly = Assembly.Load(assemblyBytes);
            _dynamicType = assembly.GetType("High5.CustomTool");
            //constructors can be called by passing parameters to Activator.CreateInstance
            _dynamicObject = Activator.CreateInstance(_dynamicType);
        }

        public override Data Process(Data data)
        {
            return (Data) _dynamicType.InvokeMember("Process",
                BindingFlags.Default | BindingFlags.InvokeMethod,
                null,
                _dynamicObject,
                new[] {data}
            );
        }

        public override void Init()
        {
            _dynamicType.InvokeMember("Init",
                BindingFlags.Default | BindingFlags.InvokeMethod,
                null,
                _dynamicObject,
                null
            );
        }
    }
}