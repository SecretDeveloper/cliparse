using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using CliParse;

namespace CliParse
{
    public static class Parser
    {
        public static CliParseResult Parse(Parsable parsable, IEnumerable<string> args)
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

        private static CliParseResult MapArguments(Parsable parsable, IEnumerable<string> args)
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
                    var token = GetTokenForArgument(tokens, argument);
                    var propertySet = SetPropertyValue(parsable, token, tokens, argument, prop);
                    if(argument.Required && !propertySet)
                        result.AddErrorMessage(string.Format("Required argument '{0}' was supplied.", argument.Name));
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
                                             (x.ShortName.ToString(CultureInfo.InvariantCulture).Equals(token.Value))))))
                {
                    result.AddErrorMessage(string.Format("Unknown argument '{0}' was supplied.", token.Value));
                }
            }

            return result;
        }

        private static bool IsHelpToken(Token token)
        {
            return token.Type == TokenType.Field && (
                token.Value.ToString().IndexOf("help", StringComparison.OrdinalIgnoreCase) == 0
                || token.Value.ToString().IndexOf("?", StringComparison.OrdinalIgnoreCase) == 0);
        }

        private static bool SetPropertyValue(Parsable parsable, Token token, IEnumerable<Token> tokens, ParsableArgument parsableArgument, PropertyInfo prop)
        {
            if (token == null)
            {
                if (parsableArgument.DefaultValue != null)
                {
                    prop.SetValue(parsable, parsableArgument.DefaultValue);
                    return true;
                }
                return false;    // couldnt find matching token, return false.
            }

            if(token.Type == TokenType.Field)
            {
                if (prop.PropertyType == typeof (bool))
                {
                    prop.SetValue(parsable, true); // set property to true if flag was provided.
                }
                else
                {
                    var tokenValue =
                        tokens.FirstOrDefault(x => x.Index == token.Index + 1 && x.Type == TokenType.Value);
                    if (tokenValue == null)
                        throw new CliParseException(string.Format("Missing value for ParsableArgument {0}", token.Value));

                    PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(parsable)[prop.Name];
                    prop.SetValue(parsable,
                        propertyDescriptor.Converter != null
                            ? propertyDescriptor.Converter.ConvertFrom(tokenValue.Value)
                            : tokenValue);
                }
            }
            
            if (token.Type == TokenType.Value)
            {
                if (token.Value == null)
                    throw new CliParseException(string.Format("Missing value for ParsableArgument {0}", token.Value));

                PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(parsable)[prop.Name];
                prop.SetValue(parsable,
                    propertyDescriptor.Converter != null
                        ? propertyDescriptor.Converter.ConvertFrom(token.Value)
                        : token.Value);
            }

            return true;
        }

        private static Token GetTokenForArgument(IEnumerable<Token> tokens, ParsableArgument parsableArgument)
        {
            var shortName = parsableArgument.ShortName.ToString(CultureInfo.InvariantCulture);
            var longName = parsableArgument.Name;
            var impliedPosition = parsableArgument.ImpliedPosition;

            var tokentsArray = tokens as Token[] ?? tokens.ToArray();
            var token =
                tokentsArray.FirstOrDefault(
                    x =>
                        x.Value.Equals(shortName) || x.Value.Equals(longName) ||
                        impliedPosition > -1 && x.Type == TokenType.Value && x.Index == impliedPosition);

            // find by position instead.
            if (token == null)
                token = tokentsArray.FirstOrDefault(x => x.Type == TokenType.Value && x.Index == impliedPosition);

            return token;
        }
    }
}