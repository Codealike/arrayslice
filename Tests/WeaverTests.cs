using System;
using System.IO;
using System.Reflection;
using System.Linq;
using Mono.Cecil;
using NUnit.Framework;
using Corvalius.ArraySlice.Fody;
using Tests;

namespace Corvalius.ArraySlice.Tests
{
    [TestFixture]
    public class WeaverTests
    {
        Assembly assembly;
        string newAssemblyPath;
        string assemblyPath;

        ModuleWeaver weavingTask;

        [TestFixtureSetUp]
        public void Setup()
        {
            var projectPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\AssemblyToProcess\AssemblyToProcess.csproj"));
            assemblyPath = Path.Combine(Path.GetDirectoryName(projectPath), @"bin\Debug\AssemblyToProcess.dll");
#if (!DEBUG)
            assemblyPath = assemblyPath.Replace("Debug", "Release");
#endif

            newAssemblyPath = assemblyPath.Replace(".dll", "2.dll");
            File.Copy(assemblyPath, newAssemblyPath, true);

            var moduleDefinition = ModuleDefinition.ReadModule(newAssemblyPath);
            weavingTask = new ModuleWeaver
            {
                ModuleDefinition = moduleDefinition,
                AssemblyResolver = new MockAssemblyResolver()
            };

            weavingTask.Execute();
            moduleDefinition.Write(newAssemblyPath);

            assembly = Assembly.LoadFile(newAssemblyPath);
        }

        [Test]
        public void ValidateFindMethodsThatUsesArraySlicesInBody()
        {
            var typeToFind = DefinitionFinder.FindType(typeof(ArraySlice<>));
            var methods = weavingTask.FindMethodsUsingArraySlices(typeToFind)
                                     .Where(x => !x.Name.StartsWith("DoNot"));

            Assert.AreEqual(0, methods.Count());
        }

        [Test]
        public void ValidateFindMethodsThatDoNotUsesArraySlicesInBody()
        {
            var typeToFind = DefinitionFinder.FindType(typeof(ArraySlice<>));
            var methods = weavingTask.FindMethodsUsingArraySlices(typeToFind)
                                     .Where(x => x.Name.StartsWith("DoNot"));

            Assert.AreEqual(2, methods.Count());
        }

#if(DEBUG)
    [Test]
    public void PeVerify()
    {
        Verifier.Verify(assemblyPath,newAssemblyPath);
    }
#endif
    }

}
