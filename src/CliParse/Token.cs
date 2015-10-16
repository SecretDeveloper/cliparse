using System.Collections.Generic;

namespace CliParse
{
    public enum TokenType : byte
    {
        Field,
        Value
    }

    public class Token
    {
        public int Index { get; set; }
        public TokenType Type { get; set; }
        public object Value { get; set; }
    }

    public static class Tokenizer
    {
        public static IEnumerable<Token> Tokenize(IEnumerable<string> args)
        {
            int index = 0;
            foreach (var arg in args)
            {
                if (arg.StartsWith("/") || arg.StartsWith("-"))
                {
                    yield return new Token {Index = index++, Value=arg.TrimStart('-','/'), Type = TokenType.Field};
                }
                else
                {
                    yield return new Token { Index = index++, Value = arg, Type = TokenType.Value };
                }
            }
        }
    }
}
