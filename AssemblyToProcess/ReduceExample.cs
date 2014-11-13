using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AssemblyToProcess
{
    public static class ReduceExample
    {
        private static void ReduceStepWithArrays(double[] array, int lenght)
        {
            int size = lenght / 2;
            for (int i = 0; i < size; i++)
                array[i] = array[i] + array[size + i];
        }

        public static double WithArrays(double[] array)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            int lenght = array.Length;
            do
            {
                ReduceStepWithArrays(array, lenght);
                lenght /= 2;
            }
            while (lenght > 1);

            watch.Stop();
            Console.WriteLine(string.Format("Reduced with Arrays: '{0}' elements in {1}ms.", array.Length, watch.ElapsedMilliseconds));

            return array[0];
        }

        private static void ReduceStepWithSlices(ArraySlice<double> first, ArraySlice<double> second)
        {
            int length = first.Length;
            for (int i = 0; i < length; i++)
                first[i] = first[i] + second[i];
        }

        public static double WithSlices(ArraySlice<double> array)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            int lenght = array.Length;
            do
            {
                var size = lenght / 2;
                ReduceStepWithSlices(new ArraySlice<double>(array.Array, 0, size), new ArraySlice<double>(array.Array, size, size));
                lenght /= 2;
            }
            while (lenght > 1);

            watch.Stop();
            Console.WriteLine(string.Format("Reduced with ArraySlice: '{0}' elements in {1}ms.", array.Length, watch.ElapsedMilliseconds));

            return array[0];
        }

        private static void ReduceRecursiveStepWithSlices(ArraySlice<double> first, ArraySlice<double> second)
        {
            int length = first.Length;
            if (length > 1)
            {
                for (int i = 0; i < length; i++)
                    first[i] = first[i] + second[i];

                var size = length / 2;
                ReduceRecursiveStepWithSlices(new ArraySlice<double>(first, 0, size), new ArraySlice<double>(first, size, size));
            }
            else
            {
                first[0] = first[0] + second[0];
            }
        }

        public static double RecursiveWithSlices(ArraySlice<double> array)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            var size = array.Length / 2;
            ReduceRecursiveStepWithSlices(new ArraySlice<double>(array, 0, size), new ArraySlice<double>(array, size, size));

            watch.Stop();

            Console.WriteLine(string.Format("Reduced with ArraySlice-Recursive: '{0}' elements in {1}ms.", array.Length, watch.ElapsedMilliseconds));
            return array[0];
        }

        [ArraySliceBehavior(OptimizationMode.None)]
        private static void ReduceStepWithSlicesNoOptimization(ArraySlice<double> first, ArraySlice<double> second)
        {
            int length = first.Length;
            for (int i = 0; i < length; i++)
                first[i] = first[i] + second[i];
        }

        [ArraySliceBehavior(OptimizationMode.None)]
        public static double WithSlicesNoOptimization(ArraySlice<double> array)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            int lenght = array.Length;
            do
            {
                var size = lenght / 2;
                ReduceStepWithSlicesNoOptimization(new ArraySlice<double>(array.Array, 0, size), new ArraySlice<double>(array.Array, size, size));
                lenght /= 2;
            }
            while (lenght > 1);

            watch.Stop();
            Console.WriteLine(string.Format("Reduced with unoptimized ArraySlice: '{0}' elements in {1}ms.", array.Length, watch.ElapsedMilliseconds));

            return array[0];
        }
    }
}
