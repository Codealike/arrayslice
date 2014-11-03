using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyToProcess
{
    public class ArraySliceContainerBase 
    {
        protected static float[] InitializeData()
        {
            float[] data = new float[100];
            for (int i = 0; i < data.Length; i++)
                data[i] = i;

            return data;
        }

        protected static T[] InitializeData<T>()
        {
            T[] data = new T[100];
            for (int i = 0; i < data.Length; i++)
                data[i] = default(T);

            return data;
        }
    }
}
