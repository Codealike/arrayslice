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
    public class WeaverBase 
    {
        private static Random rnd = new Random();

        protected Assembly Assembly;
        protected ModuleWeaver Weaver;

        protected string NewAssemblyPath;
        protected string AssemblyPath;

        public WeaverBase()
        {
            var projectPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\AssemblyToProcess\AssemblyToProcess.csproj"));
            
            AssemblyPath = Path.Combine(Path.GetDirectoryName(projectPath), @"bin\Debug\AssemblyToProcess.dll");
#if (!DEBUG)
            AssemblyPath = AssemblyPath.Replace("Debug", "Release");
#endif

            NewAssemblyPath = AssemblyPath.Replace(".dll", rnd.Next() + ".dll");
            File.Copy(AssemblyPath, NewAssemblyPath, true);

            var moduleDefinition = ModuleDefinition.ReadModule(NewAssemblyPath);
            Weaver = new ModuleWeaver
            {
                ModuleDefinition = moduleDefinition,
                AssemblyResolver = new MockAssemblyResolver()
            };

            Weaver.Execute();
            moduleDefinition.Write(NewAssemblyPath);            

            Assembly = Assembly.LoadFile(NewAssemblyPath);            
        }

    }
}
