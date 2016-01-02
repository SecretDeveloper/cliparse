using System.Collections.Generic;

namespace CliParse
{
    public abstract class Parsable
    {
        public const string DefaultTemplate = @"{title} {version}
{copyright}
Description:
    {description}    

Syntax:
{syntax}
{footer}";

        /// <summary>
        /// Executes before any parsing is performed.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="result"></param>
        public virtual void PreParse(IEnumerable<string> args, CliParseResult result)
        {
        }

        /// <summary>
        /// Executes after parsing has been performed.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="result"></param>
        public virtual void PostParse(IEnumerable<string> args, CliParseResult result)
        {
        }
    }

}
