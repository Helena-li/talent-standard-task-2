using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Common.Auth
{
    class InvalidHashException : Exception
    {
        public InvalidHashException() { }
        public InvalidHashException(string message)
            : base(message) { }
        public InvalidHashException(string message, Exception inner)
            : base(message, inner) { }
    }
}
