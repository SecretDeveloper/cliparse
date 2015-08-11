using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace CliParse
{
    public class CliParseResult
    {
        public bool Successful { get; protected set; }

        private List<string> messages;
        public IEnumerable<string> CliParseMessages { get; protected set; }

        public CliParseResult()
        {
            Successful = true;
            messages = new List<string>();
        }

        public void AddErrorMessage(string message)
        {
            Successful = false;
            messages.Add(message);
        }

        public void AddMessageFromException(CliParseException exception)
        {
            Successful = false;
            messages.Add(string.Format("Exception of type '{0}' thrown.  - {1}", exception.GetType(), exception.Message));
        }
    }
}