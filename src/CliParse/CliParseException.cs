using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace CliParse
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class CliParseException : Exception
    {
        public CliParseException() : base() { }
        public CliParseException(string message) : base(message){}
        protected CliParseException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public CliParseException(string message, Exception innerException) : base(message, innerException) { }
    }
}