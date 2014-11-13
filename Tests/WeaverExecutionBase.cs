using Corvalius.ArraySlice.Fody;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Tests
{
    public class WeaverExecutionBase
    {        
        static WeaverExecutionBase()
        {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
        }

        ~WeaverExecutionBase()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= ResolveAssembly;
        }

        private static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            if ( args.Name.StartsWith( "Corvalius.ArraySlice.Portable" ))
            {
                var projectPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\AssemblyToProcess\AssemblyToProcess.csproj"));
                var assemblyPath = Path.Combine(Path.GetDirectoryName(projectPath), @"bin\Debug\AssemblyToProcess.dll");
#if (!DEBUG)
                assemblyPath = assemblyPath.Replace("Debug", "Release");
#endif

                var newAssemblyPath = assemblyPath.Replace(".dll", "2.dll");
                File.Copy(assemblyPath, newAssemblyPath, true);

                var moduleDefinition = ModuleDefinition.ReadModule(newAssemblyPath);
                var weaver = new ModuleWeaver
                {
                    ModuleDefinition = moduleDefinition,
                    AssemblyResolver = new MockAssemblyResolver()
                };

                weaver.Execute();
                moduleDefinition.Write(newAssemblyPath);

                return Assembly.LoadFile(newAssemblyPath);
            }

            return null;
        }
    }
}
