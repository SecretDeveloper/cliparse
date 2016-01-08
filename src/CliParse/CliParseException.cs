using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace CliParse
{
    /// <summary>
    /// Exception which can be thrown during the CliParse() process.
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class CliParseException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public CliParseException() : base() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public CliParseException(string message) : base(message){}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected CliParseException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public CliParseException(string message, Exception innerException) : base(message, innerException) { }
    }
}