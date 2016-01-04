using System;
using System.Collections.Generic;
using System.Globalization;

namespace CliParse
{
    public class CliParseResult
    {
        public bool Successful { get; private set; }
        public bool ShowHelp { get; internal set; }
        
        private readonly List<string> _messages;

        public IEnumerable<string> CliParseMessages
        {
            get { return _messages; }
        }

        public CliParseResult()
        {
            Successful = true;
            _messages = new List<string>();
        }

        public void AddErrorMessage(string message)
        {
            Successful = false;
            if(string.IsNullOrEmpty(message) == false) _messages.Add(message);
        }

        public void AddMessageFromException(Exception exception)
        {
            Successful = false;

            if (exception == null)
            {
                _messages.Add("An unknown error has occured");
                return;
            }

            _messages.Add(string.Format(CultureInfo.CurrentCulture, "Exception of type '{0}' thrown.  - {1}", exception.GetType(), exception.Message));
        }
    }
}