using System;
using System.Collections.Generic;
using System.Globalization;

namespace CliParse
{
    /// <summary>
    /// Result object returned from the CliParse() process which includes results and any error messages.
    /// </summary>
    public class CliParseResult
    {
        /// <summary>
        /// Represents whether the CliParse() process was successful.
        /// </summary>
        public bool Successful { get; private set; }

        /// <summary>
        /// Represents whether the provided arguments indicate that the Help content should be shown.
        /// </summary>
        public bool ShowHelp { get; internal set; }
        
        private readonly List<string> _messages;
        /// <summary>
        /// Error messages
        /// </summary>
        public IEnumerable<string> CliParseMessages
        {
            get { return _messages; }
        }

        /// <summary>
        /// Creates a new CliParseResult object.
        /// </summary>
        public CliParseResult()
        {
            Successful = true;
            _messages = new List<string>();
        }

        /// <summary>
        /// Adds an error message to the CliParseMessages list and sets the Successful flag to false.
        /// </summary>
        /// <param name="message"></param>
        public void AddErrorMessage(string message)
        {
            Successful = false;
            if(string.IsNullOrEmpty(message) == false) _messages.Add(message);
        }

        /// <summary>
        /// Adds an error message containing the exception details to the CliParseMessages list and sets the Successful flag to false.
        /// </summary>
        /// <param name="exception"></param>
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