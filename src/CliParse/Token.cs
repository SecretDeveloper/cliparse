using System.Collections.Generic;
using System.Linq;

namespace CliParse
{
    /// <summary>
    /// The types of tokens that can be provided.
    /// </summary>
    public enum TokenType : int
    {
        /// <summary>
        /// Named field arguments
        /// </summary>
        Field,
        /// <summary>
        /// Value arguments
        /// </summary>
        Value
    }

    /// <summary>
    /// Represents a single argument item.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// The original index of the argument.
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// Token type.
        /// </summary>
        public TokenType Type { get; set; }
        /// <summary>
        /// The arguments value, if provided.
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// Whether this token has been accounted for during the CliParse() process.
        /// </summary>
        public bool Taken { get; set; }

        /// <summary>
        /// Returns a new Token object.
        /// </summary>
        public Token()
        {
            Taken = false;
        }
    }

    /// <summary>
    /// Simple argument tokenizer class that can seperate and define an argument string into a collection of Token objects.
    /// </summary>
    public static class Tokenizer
    {
        private static readonly char[] _defaultPrefixes = new[] {'-', '/'};

        /// <summary>
        /// Converts a collection of arguments into a collection of defined Tokens.
        /// </summary>
        /// <param name="args">The arguments received from the command line.</param>
        /// <param name="allowedPrefixes">The prefixes used to identify named arguments.</param>
        /// <returns></returns>
        public static IEnumerable<Token> Tokenize(IEnumerable<string> args, ICollection<char> allowedPrefixes = null)
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
                    foreach (char t in arg.TrimStart(allowedPrefixes.ToArray()))
                        yield return new Token { Index = ++index, Value = t.ToString(), Type = TokenType.Field };
                    
                }
                else if (prefixCount == 2) // longnames
                {
                    yield return new Token { Index = ++index, Value = arg.TrimStart(allowedPrefixes.ToArray()), Type = TokenType.Field };
                }
                else // values
                {
                    yield return new Token { Index = ++index, Value = arg, Type = TokenType.Value };
                }
            }
        }

        private static int CountStringStartsWith(string value, ICollection<char> allowedPrefixs)
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
