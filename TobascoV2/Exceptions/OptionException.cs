using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TobascoV2.Exceptions
{
    public class OptionException : Exception
    {
        public OptionException()
        {
        }

        public OptionException(string message) : base(message)
        {
        }

        public OptionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OptionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
