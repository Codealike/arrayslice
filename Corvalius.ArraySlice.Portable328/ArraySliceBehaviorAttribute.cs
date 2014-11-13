using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public enum OptimizationMode
    {
        None,
        Safe,
    }

    [AttributeUsage( AttributeTargets.Method, Inherited=false, AllowMultiple=false )]
    public class ArraySliceBehaviorAttribute : Attribute
    {
        public OptimizationMode Mode { get; private set; }

        public ArraySliceBehaviorAttribute(OptimizationMode mode)
        {
            this.Mode = mode;
        }
    }
}
