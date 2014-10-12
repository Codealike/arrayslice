using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corvalius.ArraySlice.Fody
{
    public class WeavingException : Exception
    {
        public WeavingException(string message)
            : base(message)
        {

        }
    }
}
