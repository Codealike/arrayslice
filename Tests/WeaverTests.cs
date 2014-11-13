using System;
using System.IO;
using System.Reflection;
using System.Linq;
using Mono.Cecil;
using Corvalius.ArraySlice.Fody;
using Tests;
using Xunit;

namespace Corvalius.ArraySlice.Tests
{

    public class WeaverTests : WeaverBase
    {
        [Fact]
        public void ValidateFindMethodsThatUsesArraySlicesInBody()
        {
            var typeToFind = DefinitionFinder.FindType(typeof(ArraySlice<>));
            var methods = Weaver.FindMethodsUsingArraySlices(typeToFind)
                                .Where(x => !x.Name.StartsWith("DoNot") && x.DeclaringType.Name == "ControlWeavingAtMethodLevel");

            Assert.Equal(0, methods.Count());
        }

         [Fact]
        public void ValidateFindMethodsThatDoNotUsesArraySlicesInBody()
        {
            var typeToFind = DefinitionFinder.FindType(typeof(ArraySlice<>));
            var methods = Weaver.FindMethodsUsingArraySlices(typeToFind)
                                .Where(x => x.Name.StartsWith("DoNot") && x.DeclaringType.Name == "ControlWeavingAtMethodLevel");

            Assert.Equal(2, methods.Count());
        }


#if(DEBUG)
        [Fact]
        public void PeVerify()
        {
            Verifier.Verify(AssemblyPath,NewAssemblyPath);
        }
#endif

    }

}
