using System;

namespace CliParse
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ParsableClassAttribute : Attribute
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Version { get; set; }
        public string Copyright { get; set; }
        public string ExampleText { get; set; }
        public string FooterText { get; set; }

        public ParsableClassAttribute(string title)
        {
            Title = title;
            Description = string.Empty;
        }

        public ParsableClassAttribute(string title, string description = "")
        {
            Title = title;
            Description = description;
        }

    }
}
