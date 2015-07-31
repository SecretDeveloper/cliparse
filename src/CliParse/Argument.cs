using System;

namespace CliParse
{
    public class Argument : Attribute
    {
        public string Name { get; private set; }
        public char ShortName { get; private set; }
        public object DefaultValue { get; set; }
        public string Description { get; set; }
        public string Example { get; set; }
        public bool Required { get; set; }

        public Argument(char shortName)
        {
            ShortName = shortName;
        }

        public Argument(char shortName, string name)
        {
            Name = name;
            ShortName = shortName;
        }
    }
}