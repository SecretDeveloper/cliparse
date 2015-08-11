using System;

namespace CliParse
{
    public class ParsableClass : Attribute
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Version { get; private set; }
        public string Copyright { get; private set; }
        public string ExampleText { get; private set; }
        public string FooterText { get; private set; }

        public ParsableClass(string title, string description = "")
        {
            Title = title;
            Description = description;
        }

    }
}
