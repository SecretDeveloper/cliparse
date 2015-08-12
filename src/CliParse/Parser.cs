using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using CliParse.Tokenize;

namespace CliParse
{
    public static class Parser
    {
        public static CliParseResult Parse(Parsable parsable, string[] args)
        {
            var result = new CliParseResult();
            try
            {
                if (args == null) throw new CliParseException("Parameter 'args' cannot be null.");
                if (parsable == null) throw new CliParseException("Parameter 'parsable' cannot be null.");
                
                result = MapArguments(parsable, args);
            }
            catch (CliParseException ex) 
            {
                result.AddMessageFromException(ex);
            }
            return result;
        }

        private static CliParseResult MapArguments(Parsable parsable, string[] args)
        {
            var result = new CliParseResult();

            var tokens = Tokenizer.Tokenize(args).ToList();
            result.ShowHelp = tokens.Any(IsHelpToken);

            var discoveredArguments = new List<ParsableArgument>();

            var parsableType = parsable.GetType();
            var properties = parsableType.GetProperties();
            
            foreach (var prop in properties)
            {
                foreach (var argument in prop.GetCustomAttributes(true).OfType<ParsableArgument>())
                {
                    discoveredArguments.Add(argument);
                    SetPropertyValue(parsable, tokens, argument, prop);
                }
            }

            // missing required fields
            if (result.ShowHelp == false)
            {
                foreach (
                    var argument in
                        discoveredArguments.Where(argument => argument.Required)
                            .Where(
                                argument =>
                                    tokens.FirstOrDefault(
                                        x =>
                                            x.Type == TokenType.Field &&
                                            (x.Value.Equals(argument.Name) || x.Value.Equals(argument.Name))) == null))
                {
                    result.AddErrorMessage(string.Format("Required ParsableArgument '{0}' was not supplied.",
                        argument.ShortName));
                }
            }

            // unknown aruments
            if (result.ShowHelp == false)
            {
                foreach (
                    var token in
                        tokens.Where(token => token.Type == TokenType.Field)
                            .Where(
                                token =>
                                    !discoveredArguments.Any(
                                        x =>
                                            ((x.Name != null && x.Name.Equals(token.Value)) ||
                                             (x.ShortName.ToString().Equals(token.Value))))))
                {
                    result.AddErrorMessage(string.Format("Unknown argument '{0}' was supplied.", token.Value));
                }
            }

            return result;
        }

        private static bool IsHelpToken(Token token)
        {
            return token.Type == TokenType.Field && (
                token.Value.ToString().IndexOf("help", System.StringComparison.OrdinalIgnoreCase) == 0
                || token.Value.ToString().IndexOf("?", System.StringComparison.OrdinalIgnoreCase) == 0);
        }

        private static void SetPropertyValue(Parsable parsable, IEnumerable<Token> tokens, ParsableArgument parsableArgument, PropertyInfo prop)
        {
            // shortnames are unique and required so use as key in dict.
            var shortName = parsableArgument.ShortName.ToString();
            var longName = parsableArgument.Name;
            
            var token = tokens.FirstOrDefault(x => x.Value.Equals(shortName) || x.Value.Equals(longName));
            if (token != null && token.Type == TokenType.Field)
            {
                if (prop.PropertyType == typeof (bool))
                {
                    prop.SetValue(parsable, true); // set property to true if flag was provided.
                }
                else
                {
                    var tokenValue = tokens.FirstOrDefault(x => x.Index == token.Index + 1 && x.Type == TokenType.Value);
                    if(tokenValue == null) throw new CliParseException(string.Format("Missing value for ParsableArgument {0}", token.Value));

                    prop.SetValue(parsable, Convert.ChangeType(tokenValue.Value, prop.PropertyType));
                    
                }
            }
            else if (parsableArgument.DefaultValue != null)
            {
                prop.SetValue(parsable, parsableArgument.DefaultValue);
            }
        }

        private static string CleanName(string argumentName)
        {
            return argumentName.Replace("-", "").Replace("/","").ToLower(CultureInfo.InvariantCulture);
        }

    }
}