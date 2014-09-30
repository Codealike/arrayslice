using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace System
{
    public sealed class ArraySlice<T> 
    {
        public readonly int Offset;
        public readonly int Count;
        public readonly T[] Array;

        public ArraySlice(T[] array)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            Array = array;
            Offset = 0;
            Count = array.Length;
        }

        public ArraySlice(T[] array, int offset, int count)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset", "The argument cannot be negative."); 
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "The argument cannot be negative."); 
            if (array.Length - offset < count)
                throw new ArgumentException("The length of the slice cannot be less than 0.");

            Array = array;
            Offset = offset;
            Count = count;
        }

        public override int GetHashCode()
        {
            return null == Array
                        ? 0
                        : Array.GetHashCode() ^ Offset ^ Count;
        }

        public override bool Equals(Object obj)
        {
            if (obj is ArraySlice<T>)
                return Equals((ArraySlice<T>)obj);
            else
                return false;
        }

        public bool Equals(ArraySlice<T> obj)
        {
            return obj.Array == Array && obj.Offset == Offset && obj.Count == Count;
        }

        public static bool operator ==(ArraySlice<T> a, ArraySlice<T> b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ArraySlice<T> a, ArraySlice<T> b)
        {
            return !(a == b);
        }

        public T this[int index]
        {
            get
            {                
                return Array[Offset + index];
            }
            set
            {
                Array[Offset + index] = value;
            }
        }

        public int GetOffset()
        {
            return this.Offset;
        }

        public T[] GetArray()
        {
            return this.Array;
        }
    }
}
