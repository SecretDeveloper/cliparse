using System;

namespace CliParse
{
    public class ParsableClass : Attribute
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string Copyright { get; set; }
        public string ExampleText { get; set; }
        public string FooterText { get; set; }

        public ParsableClass(string title, string description = "")
        {
            Title = title;
            Description = description;
        }

    }
}
