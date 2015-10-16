using System;

namespace CliParse
{
    public class ParsableArgument : Attribute
    {
        public int ImpliedPosition { get; set; }
        public string Name { get; set; }
        public char ShortName { get; set; }
        public object DefaultValue { get; set; }
        public string Description { get; set; }
        public string Example { get; set; }
        public bool Required { get; set; }

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