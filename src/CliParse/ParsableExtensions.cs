using System.Reflection;

namespace CliParse
{
    public static class ParsableExtensions
    {
        /// <summary>
        /// Parses the provided args collection and uses its values to set 
        /// the appropriate properties on the parsable object.
        /// </summary>
        /// <param name="parsable"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static CliParseResult CliParse(this Parsable parsable, string[] args)
        {
            return Parser.Parse(parsable, args);
        }

        /// <summary>
        /// Builds a help information string which contains descriptions 
        /// and details of the properties on the parsable object using 
        /// values from its ParsableClass and ParsableArgument attributes.
        /// </summary>
        /// <param name="parsable"></param>
        /// <param name="template"></param>
        /// <param name="argumentTemplate"></param>
        /// <returns></returns>
        public static string GetHelpInfo(this Parsable parsable, string template = Parsable.DefaultTemplate, string argumentTemplate = ParsableArgumentAttribute.DefaultTemplate)
        {
            return InfoBuilder.GetHelpInfo(parsable, template, argumentTemplate);
        }

        /// <summary>
        /// Builds a help information string which contains descriptions 
        /// and details of the properties on the parsable object using 
        /// values from its ParsableArgument attributes and from the 
        /// provided assemblies attributes.
        /// </summary>
        /// <param name="parsable"></param>
        /// <param name="asm"></param>
        /// <param name="template">The template to use when building the help string.</param>
        /// <param name="argumentTemplate">The argument template to use when building the help string.</param>
        /// <returns></returns>
        public static string GetHelpInfoFromAssembly(this Parsable parsable, Assembly asm, string template = Parsable.DefaultTemplate, string argumentTemplate=ParsableArgumentAttribute.DefaultTemplate)
        {
            return InfoBuilder.GetHelpInfoFromAssembly(parsable, asm, template, argumentTemplate);
        }
    }
}