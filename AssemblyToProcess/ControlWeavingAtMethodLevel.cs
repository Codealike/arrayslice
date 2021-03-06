﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyToProcess
{
    public class ControlWeavingAtMethodLevel : ArraySliceContainerBase
    {
        [ArraySliceDoNotOptimize]
        public float DoNotOptimizeAccessGetter()
        {
            int offset = 10;
            var data = InitializeData();

            var segment = new ArraySlice<float>(data, offset, 5);
            float result = segment[4];

            return result;
        }

        [ArraySliceDoNotOptimize]
        public float[] DoNotOptimizeAccessSetter()
        {
            int offset = 10;
            var data = InitializeData();

            var segment = new ArraySlice<float>(data, offset, 5);
            segment[4] = 999;

            return segment.Array;
        }
    }
}
