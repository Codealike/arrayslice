using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class ArraySliceDoNotOptimize : Attribute
    { }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class ArraySliceUnsafeOptimize : Attribute
    { }
}
