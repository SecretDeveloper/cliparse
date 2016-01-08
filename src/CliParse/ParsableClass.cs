using System;

namespace CliParse
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ParsableClassAttribute : Attribute
    {
        /// <summary>
        /// The Title that will be displayed on help screens.
        /// </summary>
        public string Title { get; private set; }
        /// <summary>
        /// A description that will be displayed on help screens.
        /// </summary>
        public string Description { get; private set; }
        /// <summary>
        /// The applications current version.
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// The applications copyright statement.
        /// </summary>
        public string Copyright { get; set; }
        /// <summary>
        /// Example content that will be included on help screens.
        /// </summary>
        public string ExampleText { get; set; }
        /// <summary>
        /// Footer content that will be included on help screens.
        /// </summary>
        public string FooterText { get; set; }
        /// <summary>
        /// The allowed parameter prefix characters. Default is '-' and '/'.
        /// </summary>
        public char[] AllowedPrefixes { get; set; }

        public ParsableClassAttribute(string title)
        {
            reset();
            Title = title;
        }

        public ParsableClassAttribute(string title, string description = "")
        {
            reset();
            Title = title;
            Description = description;
        }

        private void reset()
        {
            Description = "";
            AllowedPrefixes = new char[] {'-', '/'};
        }
    }
}
