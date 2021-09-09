using System;
using System.Reflection;
using System.Security;
using System.Security.Permissions;

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

        public DynamicTool CreateDynamicTool(string toolName, string toolCode, string metadataCode = null)
        {
            /*
             * This function might throw an exception. If it does, it means there is a compilation
             * error within the user's uploaded code.
             */
            
            DynamicTool dynamicTool;
            var assemblyBytes = DynamicCompiler.Compile(toolCode);
            dynamicTool = (DynamicTool) _restrictedDomain.CreateInstanceAndUnwrap(
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