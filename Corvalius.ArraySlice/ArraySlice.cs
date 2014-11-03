using Corvalius.ArraySlice;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace System
{
    public sealed class ArraySlice<T> : IHideObjectMembers
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public readonly int Offset;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public readonly T[] Array;

        public readonly int Count;

        public ArraySlice(T[] array)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            Contract.EndContractBlock();

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
            Contract.EndContractBlock();

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
                Contract.Requires(index >= 0);
                Contract.Requires(index < this.Count);
                Contract.Requires(this.Offset + index < this.Array.Length);
                Contract.EndContractBlock();

                return Array[Offset + index];
            }
            set
            {
                Contract.Requires(index >= 0);
                Contract.Requires(index < this.Count);
                Contract.Requires(this.Offset + index < this.Array.Length);
                Contract.EndContractBlock();

                Array[Offset + index] = value;
            }
        }

        public static implicit operator ArraySlice<T> ( T[] src )
        {
            return new ArraySlice<T>(src);
        }

    }
}
