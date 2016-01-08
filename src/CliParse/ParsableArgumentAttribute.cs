using System;

namespace CliParse
{
    /// <summary>
    /// Properties that should be populated during the CliParse() process should have this attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property|AttributeTargets.Field, AllowMultiple = false, Inherited=false)]
    public sealed class ParsableArgumentAttribute : Attribute
    {
        /// <summary>
        /// Argument values supplied without a name can be determined by their position
        /// An ImpliedPosition of 1 means the value can be supplied as the first parameter.
        /// An ImpliedPosition of -1 means the value can be supplied as the last parameter.
        /// The default value is 0 which means ImpliedPosition is not used.
        /// </summary>
        /// <example>
        /// An argument named 'param1' with ImpliedPosition 1 can be provided as
        /// "--param1 value" or "value"
        /// </example>
        /// <example>
        /// An argument named 'param1' with ImpliedPosition -1 can be provided as
        /// "otherParam 'otherParamValue' --param1 value" or "otherParam 'otherParamValue' value"
        /// </example>
        public int ImpliedPosition { get; set; }
        
        /// <summary>
        /// The longer name of the argument, supplied in the commandline using double prefix characters e.g. --param1 value.
        /// </summary>
        public string Name { get; private set; }

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
        
        /// <summary>
        /// The default template used when building help screens.
        /// </summary>
        public const string DefaultTemplate = @"    {name}, {shortname}    
        {description}
        {required}, Default:'{defaultvalue}'
        {example}";

        /// <summary>
        /// ParsableArgumentAttribute
        /// </summary>
        /// <param name="name"></param>
       public ParsableArgumentAttribute(string name)
       {
           ImpliedPosition = 0; 
           Name = name;
        }

        /// <summary>
        /// Returns a string containing syntax help information for this property.
        /// </summary>
        /// <param name="template"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public string GetSyntax(string template, string prefix)
        {
            if (string.IsNullOrEmpty(template)) return "";

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