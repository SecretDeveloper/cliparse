using System;

namespace CliParse
{
    public class ParsableArgument : Attribute
    {
        /// <summary>
        /// Argument values supplied without a name can be determined by their position
        /// </summary>
        /// <example>
        /// An argument named 'param1' with ImpliedPosition 0 can be provided as
        /// "--param1 value" or "value"
        /// </example>
        public int ImpliedPosition { get; set; }

        /// <summary>
        /// The longer name of the argument, supplied in the commandline using double prefix characters e.g. --param1 value.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The single character name of the argument, supplied in the commandline using a single prefix character e.g. -p value.
        /// </summary>
        public char ShortName { get; set; }
        /// <summary>
        /// The default value to use when the argument is not supplied. Cannot be used when 'Required' is true.
        /// </summary>
        public object DefaultValue { get; set; }
        /// <summary>
        /// Represents whether the argument must be supplied and returns a failure parse result if it was not found.
        /// </summary>
        public bool Required { get; set; }
        /// <summary>
        /// The description of what the argument represents.  This is used when building the argument help content.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The example instructions of how an argument can be supplied.  This is used when building the argument help content.
        /// </summary>
        public string Example { get; set; }
        
        public const string DefaultPrefix = "-";
        public const string DefaultTemplate = @"    {name}, {shortname}    
        {description}
        {required}, Default:'{defaultvalue}'
        {example}";

       public ParsableArgument(string name)
       {
           ImpliedPosition = -1;
           Name = name;
        }

        public string GetSyntax(string template, string prefix)
        {
            var syntax = template;
            syntax = syntax.Replace("{name}", String.IsNullOrEmpty(Name) ? "" : prefix + prefix + Name);
            syntax = syntax.Replace("{shortname}", ShortName == '\0' ? "" : prefix + ShortName);
            syntax = syntax.Replace("{description}", String.IsNullOrEmpty(Description) ? "" : Description);
            syntax = syntax.Replace("{example}", String.IsNullOrEmpty(Example) ? "" : Example);
            syntax = syntax.Replace("{required}", Required ? "Required" : "[Optional]");
            syntax = syntax.Replace("{defaultvalue}", DefaultValue == null ? "" : DefaultValue.ToString());

            return syntax;
        }
    }
}