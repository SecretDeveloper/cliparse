using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace CliParse
{
    public static class Parser
    {
        /// <summary>
        /// Parses the provided args collection and uses its values to set the appropriate properties on the parsable object.
        /// </summary>
        /// <param name="parsable"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static CliParseResult Parse(Parsable parsable, IEnumerable<string> args)
        {
            if (args == null) throw new CliParseException("Parameter 'args' cannot be null.");
            if (parsable == null) throw new CliParseException("Parameter 'parsable' cannot be null.");

            var result = new CliParseResult();
            try
            {
                // single enumeration.
                var argumentList = args as IList<string> ?? args.ToList();
                parsable.PreParse(argumentList, result);
                if (result.Successful == false || result.ShowHelp) return result;
                result = MapArguments(parsable, argumentList);
                parsable.PostParse(argumentList, result);
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

            var parsableClass = Helper.GetObjectAttribute(parsable, typeof(ParsableClassAttribute)) as ParsableClassAttribute;
            var allowedPrefixes = GetParsableClassAllowedPrefixs(parsableClass);

            var tokens = Tokenizer.Tokenize(args, allowedPrefixes).ToList();
            result.ShowHelp = tokens.Any(token=>IsHelpToken(token, parsable));
            if (tokens.Count == 0)
            {
                if (parsableClass == null || parsableClass.ShowHelpWhenNoArgumentsProvided) result.ShowHelp = true;
            }

            var discoveredArguments = new List<ParsableArgumentAttribute>();

            var parsableType = parsable.GetType();
            var properties = parsableType.GetProperties();
            List<PropertyInfo> propertiesToFindByPosition = new List<PropertyInfo>();
            List<PropertyInfo> requiredProperties = new List<PropertyInfo>();
            
            // find by names
            foreach (var prop in properties)
            {
                foreach (var argument in prop.GetCustomAttributes(true).OfType<ParsableArgumentAttribute>())
                {
                    discoveredArguments.Add(argument);
                    var token = GetTokenForArgumentByName(tokens, argument);
                    var propertySet = SetPropertyValue(parsable, token, tokens, argument, prop);

                    if (!propertySet && token == null)
                    {
                        propertiesToFindByPosition.Add(prop);
                    }
                }
            }

            // find by positions
            foreach (var prop in propertiesToFindByPosition)
            {
                foreach (var argument in prop.GetCustomAttributes(true).OfType<ParsableArgumentAttribute>())
                {
                    var token = GetTokenForArgumentByPosition(tokens, argument);
                    var propertySet = SetPropertyValue(parsable, token, tokens, argument, prop);
                    
                    if (!propertySet && argument.Required)
                        requiredProperties.Add(prop);

                    if (propertySet)
                        requiredProperties.Remove(prop);
                }
            }

            foreach (var requiredProperty in requiredProperties)
            {
                foreach (var argument in requiredProperty.GetCustomAttributes(true).OfType<ParsableArgumentAttribute>())
                {
                    result.AddErrorMessage(string.Format(CultureInfo.CurrentCulture,"Required argument '{0}' was not supplied.", argument.Name));
                }
            }

            // unknown/unused aruments
            if (!result.ShowHelp)
            {
                tokens.Where(x => !x.Taken)
                    .ToList()
                    .ForEach(
                        x =>
                            result.AddErrorMessage(string.Format(CultureInfo.CurrentCulture,
                                "Unknown argument '{0}' was supplied.", x.Value.ToString())));
            }

            //if (result.ShowHelp == false)
            //{
            //    foreach (var token in tokens.Where(x => x.Type == TokenType.Field))
            //    {
            //        foreach (var discoveredArg in discoveredArguments)
            //        {
            //            if (discoveredArg.Name.Equals(token.Value.ToString(), StringComparison.InvariantCultureIgnoreCase) 
            //                || discoveredArg.ShortName.ToString().Equals(token.Value.ToString(), StringComparison.InvariantCulture))
            //            {
            //                result.AddErrorMessage(string.Format(CultureInfo.CurrentCulture,
            //                    "Unknown argument '{0}' was supplied.", token.Value));
            //            }
            //        }
            //    }
            //}

            return result;
        }

        private static ICollection<char> GetParsableClassAllowedPrefixs(ParsableClassAttribute parsableClass)
        {
            if (parsableClass == null) return null;
            return parsableClass.AllowedPrefixes;
        }

        private static bool IsHelpToken(Token token, Parsable parsable)
        {
            var parsableClass = Helper.GetObjectAttribute(parsable, typeof(ParsableClassAttribute)) as ParsableClassAttribute ?? new ParsableClassAttribute("");
            var defaultHelpArgs = parsableClass.ShowHelpParameters;
            
            return token.Type == TokenType.Field && defaultHelpArgs.Contains(token.Value.ToString());
        }

        private static bool SetPropertyValue(Parsable parsable, Token token, IEnumerable<Token> tokens, ParsableArgumentAttribute parsableArgument, PropertyInfo prop)
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

            Token tokenValue = null;
            if(token.Type == TokenType.Field)
            {
                if (prop.PropertyType == typeof (bool))
                {
                    // check if we have been provided a "true" or "false" value.
                    tokenValue =
                        tokens.FirstOrDefault(x => !x.Taken && x.Index == token.Index + 1 && x.Type == TokenType.Value);

                    var optionValue = true;
                    if (tokenValue != null && !Boolean.TryParse(tokenValue.Value.ToString(), out optionValue))
                    {
                        // tokenValue did not contain a valid bool so do not use its value, set it back to null so it will not be flagged as Taken.
                        tokenValue = null;
                        optionValue = true;
                    }
                    prop.SetValue(parsable, optionValue); // set property to true if flag was provided.
                }
                else
                {
                    tokenValue =
                        tokens.FirstOrDefault(x => !x.Taken && x.Index == token.Index + 1 && x.Type == TokenType.Value);
                    if (tokenValue == null)
                        throw new CliParseException(string.Format(CultureInfo.CurrentCulture, "Missing value for ParsableArgument {0}", token.Value));

                    PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(parsable)[prop.Name];
                    prop.SetValue(parsable,
                        propertyDescriptor.Converter != null
                            ? propertyDescriptor.Converter.ConvertFrom(tokenValue.Value)
                            : tokenValue);
                }
            }
            
            if (token.Type == TokenType.Value)
            {
                PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(parsable)[prop.Name];
                prop.SetValue(parsable,
                    propertyDescriptor.Converter != null
                        ? propertyDescriptor.Converter.ConvertFrom(token.Value)
                        : token.Value);
            }

            token.Taken = true;
            if (tokenValue != null) tokenValue.Taken = true;

            return true;
        }

        private static Token GetTokenForArgumentByName(ICollection<Token> tokens, ParsableArgumentAttribute parsableArgument)
        {
            var shortName = parsableArgument.ShortName.ToString(CultureInfo.InvariantCulture);
            var longName = parsableArgument.Name;

            var tokensArray = tokens as Token[] ?? tokens.ToArray();

            // Match by name. 
            // shortnames are case sensitive
            // long names are not
            var token =
                tokensArray.FirstOrDefault( x =>
                        !x.Taken && x.Type == TokenType.Field && (x.Value.ToString().Equals(shortName, StringComparison.Ordinal) 
                        || x.Value.ToString().Equals(longName, StringComparison.OrdinalIgnoreCase)));

            return token;
        }

        private static Token GetTokenForArgumentByPosition(ICollection<Token> tokens, ParsableArgumentAttribute parsableArgument)
        {
            var impliedPosition = parsableArgument.ImpliedPosition;

            var tokensArray = tokens as Token[] ?? tokens.ToArray();
            Token token = null;

            // match by position - postives
            if (impliedPosition > 0)
            {
                token = tokensArray.FirstOrDefault(x =>
                    !x.Taken && x.Type == TokenType.Value && x.Index == impliedPosition);
            }
            else
            {
                // match by position - negatives
                token = tokensArray.FirstOrDefault(x =>
                    !x.Taken && x.Type == TokenType.Value && (x.Index - tokens.Count - 1) == impliedPosition);
            }


            return token;
        }
    }
}