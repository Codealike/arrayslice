using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xunit;
using AssemblyToProcess;

namespace Tests
{
    public class ExecutionTests : WeaverBase
    {
        [Fact]
        public void Reduce()
        {
            double[] data = new double[1024 * 4096];

            for (int i = 0; i < data.Length; i++)
                data[i] = 1;
            var withArrays = ReduceExample.WithArrays(data);

            for (int i = 0; i < data.Length; i++)
                data[i] = 1;
            double withSlices = ReduceExample.WithSlices(data);

            for (int i = 0; i < data.Length; i++)
                data[i] = 1;
            double withRecursiveSlices = ReduceExample.RecursiveWithSlices(data);

            for (int i = 0; i < data.Length; i++)
                data[i] = 1;
            double withSlicesWithoutOptimization = ReduceExample.WithSlicesNoOptimization(data);

            Assert.Equal(withArrays, withSlices);
            Assert.Equal(withSlices, withRecursiveSlices);
            Assert.Equal(withSlices, withSlicesWithoutOptimization);

            Console.WriteLine("Timings are not representative when run in debug mode or under tests.");
        } 
    }
}
