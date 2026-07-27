using System;
using System.Collections.Generic;
using System.Text;

namespace SpellChecker.Common
{
    public class InvalidInputFormatException : Exception
    {
        public InvalidInputFormatException(string message)
            : base(message)
        {
        }
    }
}
