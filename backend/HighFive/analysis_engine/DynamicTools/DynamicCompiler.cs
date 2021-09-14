using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace analysis_engine.BrokerClient
{
    public class DynamicCompiler: MarshalByRefObject
    {
        /*
         * This class dynamically compiles the source code for an analysis- or drawing
         * tool. It must receive the source code as an input string, and will return
         * a compiled assembly if the source code is error-free. Otherwise the errors
         * will be logged in the console and null will be returned.
         */
        public static readonly List<MetadataReference> AssemblyReferences;
        
        static DynamicCompiler()
        {
            AssemblyReferences = new List<MetadataReference>
            {
                /*
                 * TODO: in the references variable, all data types that are not in system must be
                 * explicitly referenced.
                 */
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(File).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(FileStream).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(AnalysisTool).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(DrawingTool).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Data).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(FileMode).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(BoxCoordinateData).Assembly.Location)
            };
            Assembly.GetEntryAssembly()
                ?.GetReferencedAssemblies()
                .ToList()
                .ForEach(a => AssemblyReferences.Add(MetadataReference.CreateFromFile(Assembly.Load(a).Location)));
        }
        
        public static byte[] Compile(string sourceCode)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
            var assemblyName = Path.GetRandomFileName();

            var compilation = CSharpCompilation.Create(
                assemblyName,
                new []{syntaxTree},
                AssemblyReferences,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            
            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);
            if (!result.Success)
            {
                //Errors in the source code/compilation phase will be logged here
                var failures = result.Diagnostics.Where(diagnostic => 
                    diagnostic.IsWarningAsError || 
                    diagnostic.Severity == DiagnosticSeverity.Error);

                foreach (var diagnostic in failures)
                {
                    Console.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                }

                return null;
            }
            ms.Seek(0, SeekOrigin.Begin);
            return ms.ToArray();
        }

    }
}