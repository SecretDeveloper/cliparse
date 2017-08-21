using System;
using System.Collections.Generic;

namespace CliParse
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
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
        /// <summary>
        /// A list of the parameter values which will set ShowHelp to true if they are provided. Default is "help" and '?'.
        /// </summary>
        public IEnumerable<string> ShowHelpParameters { get; private set; }
        /// <summary>
        /// Determines whether ShowHelp is set to true when no arguments are provided.
        /// Default is true.
        /// </summary>
        public bool ShowHelpWhenNoArgumentsProvided { get; set; }

        /// <summary>
        /// Ingore unknown parameters
        /// </summary>
        /// <remarks>Default is false</remarks>
        public bool IgnoreUnknowns { get; set; }

        /// <summary>
        /// A parsable class attribute.
        /// </summary>
        /// <param name="title">The title used for help content</param>
        public ParsableClassAttribute(string title)
        {
            Reset();
            Title = title;
        }

        /// <summary>
        /// A parsable class attribute.
        /// </summary>
        /// <param name="title">The title used for help content</param>
        /// <param name="description">The description used for help content</param>
        public ParsableClassAttribute(string title, string description = "")
        {
            Reset();
            Title = title;
            Description = description;
        }

        private void Reset()
        {
            Description = "";
            AllowedPrefixes = new[] {'-', '/'};
            ShowHelpParameters = new List<string>() {"?", "help"};
            ShowHelpWhenNoArgumentsProvided = true;
        }
    }
}
