using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixPack.Minifiers.BetterMinifyJS
{
    class MinificationException : Exception
    {
        public MinificationException()
        {
        }

        public MinificationException(string message)
            : base(message)
        {
        }

        public MinificationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
