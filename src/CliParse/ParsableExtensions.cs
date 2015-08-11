using System.Reflection;

namespace CliParse
{
    public static class ParsableExtensions
    {
        public static CliParseResult CliParse(this Parsable parsable, string[] args)
        {
            return Parser.Parse(parsable, args);
        }

        public static string GetHelpInfoFromAssembly(this Parsable parsable, Assembly asm, string template = Parsable.DefaultTemplate, string argumentTemplate=Argument.DefaultTemplate, string argumentPrefix=Argument.DefaultPrefix)
        {
            return InfoBuilder.GetHelpInfoFromAssemblyInfo(parsable, asm, template, argumentTemplate, argumentPrefix);
        }
    }
}