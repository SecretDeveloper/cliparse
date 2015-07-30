using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace CliParse
{
    public abstract class Parsable
    {
        public string GetHelpScreen()
        {
            throw new NotImplementedException();
        }
    }

    public static class ParsableExtensions
    {
        public static CliParseResult CliParse(this Parsable parsable, string[] args)
        {
            return Parser.Parse(parsable, args);
        }
    }


    public static class Parser
    {
        public static CliParseResult Parse(Parsable parsable, string[] args)
        {
            var result = new CliParseResult();
            try
            {
                if (args == null) throw new CliParseException("Parameter 'args' cannot be null.");
                if (parsable == null) throw new CliParseException("Parameter 'parsable' cannot be null.");

                var dict = ParseArguments(args);

                MapArguments(parsable, dict);
            }
            catch (CliParseException ex) 
            {
                result.AddMessageFromException(ex);
            }
            return result;
        }
        
        private static Dictionary<string, object> ParseArguments(string[] args)
        {
            var dict = new Dictionary<string, object>();
            for (int i = 0; i < args.Length; i++)
            {
                var keyname = cleanName(args[i]);
                dict[keyname] = args[++i];
            }
            return dict;
        }

        private static void MapArguments(Parsable parsable, Dictionary<string, object> dict)
        {
            var parsableType = parsable.GetType();
            foreach (var prop in parsableType.GetProperties())
            {
                foreach (var attribute in prop.GetCustomAttributes(true))
                {
                    var argument = attribute as Argument;
                    if (argument != null)
                    {
                        SetPropertyValue(parsable, dict, argument, prop);
                    }
                }
            }
        }

        private static void SetPropertyValue(Parsable parsable, Dictionary<string, object> dict, Argument argument, PropertyInfo prop)
        {
            var argumentName = cleanName(argument.ArgumentName);
            var shortName = cleanName(argument.ArgumentShortName);
            if (dict.ContainsKey(argumentName))
            {
                prop.SetValue(parsable, dict[argumentName]);
            }
            else if (dict.ContainsKey(shortName))
            {
                prop.SetValue(parsable, dict[shortName]);
            }
        }

        private static string cleanName(string argumentName)
        {
            return argumentName.Replace("-", "").ToLower(CultureInfo.InvariantCulture);
        }

    }
}
