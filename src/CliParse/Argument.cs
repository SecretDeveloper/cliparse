using System;

namespace CliParse
{
    public class Argument : Attribute
    {
        public string ArgumentName { get; set; }
        public string ArgumentShortName { get; set; }
        public string DefaultValue { get; set; }
        public string Description { get; set; }
        public string Example { get; set; }
        public bool Required { get; set; }

        public Argument(string argumentName)
        {
            ArgumentName = argumentName;
        }

        public Argument(string argumentName, string shortName)
        {
            ArgumentName = argumentName;
            ArgumentShortName = shortName;
        }
    }
}