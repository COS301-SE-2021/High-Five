using System;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using analysis_engine_v2.BrokerClient.Storage;
using broker_analysis_client.Client.Models;
using Microsoft.CodeAnalysis;

namespace analysis_engine.BrokerClient
{
    public class DynamicToolFactory
    {
        private static AppDomain _restrictedDomain;
        private readonly Type _dynamicToolType = typeof(DynamicTool);
        private static readonly IAnalysisStorageManager _analysisStorageManager;

        static DynamicToolFactory()
        {
            var permissions = new PermissionSet(PermissionState.None);
            permissions.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
            permissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery, ConfigStrings.ModelDirectory));
            permissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery, Environment.CurrentDirectory));
            var setup = new AppDomainSetup();
            setup.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            
            _restrictedDomain = AppDomain.CreateDomain("restrictedDomain",
                null,
                setup,
                permissions);

            _analysisStorageManager = new AnalysisStorageManager();
        }

        public Tool CreateDynamicTool(string toolId)
        {
            /*
             * This function might throw an exception. If it does, it means there is a compilation
             * error within the user's uploaded code.
             */

            var toolFiles = _analysisStorageManager.GetAnalysisTool(toolId) ?? new AnalysisToolComposite
            {
                ByteData = _analysisStorageManager.GetDrawingTool(toolId)
            };

            var assemblyBytes = toolFiles.ByteData;
            /*var assemblyBytes =
                File.ReadAllBytes(
                    @"D:\Tuks\2021\COS301\CapstoneProject\Code\DLLTest\MyCustomTool\MyCustomTool\bin\Debug\MyCustomTool.dll");*/
            var dynamicTool = (DynamicTool) _restrictedDomain.CreateInstanceAndUnwrap(
                _dynamicToolType.Assembly.FullName, _dynamicToolType.FullName,
                false, BindingFlags.Default, null, new object[] {toolId}, null, null);
            dynamicTool.LoadCompiledBytes(assemblyBytes);
            return dynamicTool;
        }
        
        public void UnloadRestrictedDomain()
        {
            if (_restrictedDomain != null)
            {
                AppDomain.Unload(_restrictedDomain);
            }
            _restrictedDomain = null;
        }
    }
}