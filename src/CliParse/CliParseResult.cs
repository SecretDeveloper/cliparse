using System.Collections.Generic;

namespace CliParse
{
    public class CliParseResult
    {
        public bool Successful { get; private set; }
        public bool ShowHelp { get; internal set; }
        
        private readonly List<string> _messages;
        public IEnumerable<string> CliParseMessages { get; internal set; }

        public CliParseResult()
        {
            Successful = true;
            _messages = new List<string>();
        }

        public void AddErrorMessage(string message)
        {
            Successful = false;
            _messages.Add(message);
        }

        public void AddMessageFromException(CliParseException exception)
        {
            Successful = false;
            _messages.Add(string.Format("Exception of type '{0}' thrown.  - {1}", exception.GetType(), exception.Message));
        }
    }
}