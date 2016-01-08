using System.Collections.Generic;
using System.Linq;

namespace CliParse
{
    public enum TokenType : int
    {
        Field,
        Value
    }

    public class Token
    {
        public int Index { get; set; }
        public TokenType Type { get; set; }
        public object Value { get; set; }
        public bool Taken { get; set; }

        public Token()
        {
            Taken = false;
        }
    }

    public static class Tokenizer
    {
        private static readonly char[] _defaultPrefixes = new[] {'-', '/'};

        public static IEnumerable<Token> Tokenize(IEnumerable<string> args, char[] allowedPrefixes = null)
        {
            int index = 0;
            allowedPrefixes = allowedPrefixes ?? _defaultPrefixes;

            foreach (var arg in args)
            {
                var prefixCount = CountStringStartsWith(arg, allowedPrefixes);
                // shortnamed
                if (prefixCount==1)
                {
                    // split multi shortname arguments into seperate fields
                    // '-am' becomes ['a','m']
                    foreach (char t in arg.TrimStart(allowedPrefixes))
                        yield return new Token { Index = ++index, Value = t.ToString(), Type = TokenType.Field };
                    
                }
                else if (prefixCount == 2) // longnames
                {
                    yield return new Token { Index = ++index, Value = arg.TrimStart(allowedPrefixes), Type = TokenType.Field };
                }
                else // values
                {
                    yield return new Token { Index = ++index, Value = arg, Type = TokenType.Value };
                }
            }
        }

        private static int CountStringStartsWith(string value, char[] allowedPrefixs)
        {
            // confirm parameter begins with a prefix and also contains non prefix characters
            var i = 0;
            foreach (char c in value)
            {
                if (!allowedPrefixs.Contains(c)) break;
                i++;
            }
            // if '--' is provided then it should be treated as a value rather than the start of a field.
            if (i == value.Length) i = 0;
            return i;
        }
    }
}
