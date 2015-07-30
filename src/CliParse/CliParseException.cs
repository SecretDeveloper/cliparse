using System;
using System.IO;
using System.Runtime.Serialization;

namespace CliParse
{
    public class CliParseException : Exception
    {
        public CliParseException(string message) : base(message){}
        public CliParseException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public CliParseException(string message, Exception innerException) : base(message, innerException) { }
    }
}