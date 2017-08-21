using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace CliParse.Tests
{
    [ExcludeFromCodeCoverage]
    public class TokenizerTests
    {
        [Fact]
        public void Can_Parse_Flags_Slash()
        {
            var args = NativeMethods.CommandLineToArgs("/a");
            var tokens = Tokenizer.Tokenize(args).ToList<Token>();

            var token = tokens.FirstOrDefault();
            Assert.Single(tokens);
            Assert.Equal(1, token.Index);
            Assert.Equal("a", token.Value);
            Assert.Equal(TokenType.Field, token.Type);
        }


        [Fact]
        public void Can_Parse_Flags_hyphen()
        { 
            var args = NativeMethods.CommandLineToArgs("-a");
            var tokens = Tokenizer.Tokenize(args).ToList();
            Assert.NotNull(tokens);
            Assert.Single(tokens);

            var token = tokens.FirstOrDefault();
            Assert.NotNull(token);
            Assert.Equal(1, token.Index);

            Assert.Equal("a", token.Value);
            Assert.Equal(TokenType.Field, token.Type);
        }

        [Fact]
        public void Can_Parse_FieldValues()
        {
            var args = NativeMethods.CommandLineToArgs("/a myvalue");
            var tokens = Tokenizer.Tokenize(args).ToList();

            Assert.Equal(2, tokens.Count());

            Assert.Equal(1, tokens[0].Index);
            Assert.Equal("a", tokens[0].Value);
            Assert.Equal(TokenType.Field, tokens[0].Type);
            
            Assert.Equal(2, tokens[1].Index);
            Assert.Equal("myvalue", tokens[1].Value);
            Assert.Equal(TokenType.Value, tokens[1].Type);
        }

        [Fact]
        public void Can_Handle_Quoted_Values()
        {
            var args = NativeMethods.CommandLineToArgs("/a \"this is a quoted value\"");
            var tokens = Tokenizer.Tokenize(args).ToList();

            Assert.Equal(2, tokens.Count());

            Assert.Equal(1, tokens[0].Index);
            Assert.Equal("a", tokens[0].Value);
            Assert.Equal(TokenType.Field, tokens[0].Type);

            Assert.Equal(2, tokens[1].Index);
            Assert.Equal("this is a quoted value", tokens[1].Value);
            Assert.Equal(TokenType.Value, tokens[1].Type);
        }

        [Fact]
        public void Can_Handle_URLs()
        {
            var args = NativeMethods.CommandLineToArgs("/a http://google.com?q=asdf-gg");
            var tokens = Tokenizer.Tokenize(args).ToList();

            Assert.Equal(2, tokens.Count());

            Assert.Equal(1, tokens[0].Index);
            Assert.Equal("a", tokens[0].Value);
            Assert.Equal(TokenType.Field, tokens[0].Type);

            Assert.Equal(2, tokens[1].Index);
            Assert.Equal("http://google.com?q=asdf-gg", tokens[1].Value);
            Assert.Equal(TokenType.Value, tokens[1].Type);

            
        }
    }
}
