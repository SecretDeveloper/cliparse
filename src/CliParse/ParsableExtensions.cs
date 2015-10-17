using System.Reflection;

namespace CliParse
{
    public static class ParsableExtensions
    {
        public static CliParseResult CliParse(this Parsable parsable, string[] args)
        {
            return Parser.Parse(parsable, args);
        }

        public static string GetHelpInfo(this Parsable parsable, string template = Parsable.DefaultTemplate, string argumentTemplate = ParsableArgumentAttribute.DefaultTemplate, string argumentPrefix = ParsableArgumentAttribute.DefaultPrefix)
        {
            return InfoBuilder.GetHelpInfo(parsable, template, argumentTemplate, argumentPrefix);
        }

        public static string GetHelpInfoFromAssembly(this Parsable parsable, Assembly asm, string template = Parsable.DefaultTemplate, string argumentTemplate=ParsableArgumentAttribute.DefaultTemplate, string argumentPrefix=ParsableArgumentAttribute.DefaultPrefix)
        {
            return InfoBuilder.GetHelpInfoFromAssembly(parsable, asm, template, argumentTemplate, argumentPrefix);
        }
    }
}