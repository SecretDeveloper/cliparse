using System.Collections.Generic;

namespace CliParse
{
    public class CliParseResult
    {
        public bool Successful { get; protected set; }
        public ICollection<string> CliParseMessages { get; protected set; }

        public CliParseResult()
        {
            Successful = true;
            CliParseMessages = new List<string>();
        }

        public void AddMessageFromException(CliParseException exception)
        {
            Successful = false;
            CliParseMessages.Add(string.Format("Exception of type '{0}' thrown.  - {1}",exception.GetType().ToString(), exception.Message));
        }
    }
}