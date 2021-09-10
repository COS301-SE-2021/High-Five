using System;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using Microsoft.CodeAnalysis;

namespace analysis_engine.BrokerClient
{
    public class DynamicToolFactory
    {
        private static AppDomain _restrictedDomain;
        private readonly Type _dynamicToolType = typeof(DynamicTool);

        static DynamicToolFactory()
        {
            var permissions = new PermissionSet(PermissionState.None);
            permissions.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
            permissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.Read, ConfigStrings.ModelDirectory));
            var setup = new AppDomainSetup();
            setup.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            
            _restrictedDomain = AppDomain.CreateDomain("restrictedDomain",
                null,
                setup,
                permissions);
            
        }

        public Tool CreateDynamicTool(string toolName, string toolCode)
        {
            /*
             * This function might throw an exception. If it does, it means there is a compilation
             * error within the user's uploaded code.
             */

            var assemblyBytes = DynamicCompiler.Compile(toolCode);
            var dynamicTool = (DynamicTool) _restrictedDomain.CreateInstanceAndUnwrap(
                _dynamicToolType.Assembly.FullName, _dynamicToolType.FullName,
                false, BindingFlags.Default, null, new object[] {toolName}, null, null);
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