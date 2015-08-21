using System;

namespace CliParse
{
    public class ParsableArgument : Attribute
    {
        public string Name { get; private set; }
        public char ShortName { get; private set; }
        public object DefaultValue { get; set; }
        public string Description { get; set; }
        public string Example { get; set; }
        public bool Required { get; set; }

        public const string DefaultPrefix = "-";
        public const string DefaultTemplate = @"    {shortname} {name}    
        {description}
        {required}, Default:'{defaultvalue}'
        {example}";

        public ParsableArgument(char shortName)
        {
            ShortName = shortName;
        }

        public ParsableArgument(char shortName, string name)
        {
            Name = name;
            ShortName = shortName;
        }

        public string GetSyntax(string template, string prefix)
        {
            var syntax = template.Replace("{name}", String.IsNullOrEmpty(Name)?"":prefix+prefix+Name);
            syntax = syntax.Replace("{shortname}", String.IsNullOrEmpty(ShortName.ToString())?"":prefix+ShortName);
            syntax = syntax.Replace("{description}", String.IsNullOrEmpty(Description) ? "" : Description);
            syntax = syntax.Replace("{example}", String.IsNullOrEmpty(Example) ? "" : Example);
            syntax = syntax.Replace("{required}", Required ? "Required" : "[Optional]");
            syntax = syntax.Replace("{defaultvalue}", DefaultValue == null ? "" : DefaultValue.ToString());

            return syntax;
        }
    }
}