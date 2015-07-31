using System;
using System.Linq;
using CliParse.Tokenize;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace cliparse.test
{
    [TestClass]
    public class TokenizerTests
    {
        [TestMethod]
        public void Can_Parse_Flags()
        {
            var args = CliParse.Utility.CommandLineToArgs("/a");
            var tokens = Tokenizer.Tokenize(args);

            Assert.AreEqual(1, tokens.Count());
            Assert.AreEqual(tokens.FirstOrDefault().Index, 0);
            Assert.AreEqual(tokens.FirstOrDefault().Value, "a");
            Assert.AreEqual(tokens.FirstOrDefault().Type, TokenType.Field);

            args = CliParse.Utility.CommandLineToArgs("-a");
            tokens = Tokenizer.Tokenize(args);

            Assert.AreEqual(1, tokens.Count());
            Assert.AreEqual(tokens.FirstOrDefault().Index, 0);
            Assert.AreEqual(tokens.FirstOrDefault().Value, "a");
            Assert.AreEqual(tokens.FirstOrDefault().Type, TokenType.Field);
        }

        [TestMethod]
        public void Can_Parse_FieldValues()
        {
            var args = CliParse.Utility.CommandLineToArgs("/a myvalue");
            var tokens = Tokenizer.Tokenize(args).ToList();

            Assert.AreEqual(2, tokens.Count());

            Assert.AreEqual(0, tokens[0].Index);
            Assert.AreEqual("a", tokens[0].Value);
            Assert.AreEqual(TokenType.Field, tokens[0].Type);
            
            Assert.AreEqual(1, tokens[1].Index);
            Assert.AreEqual("myvalue", tokens[1].Value);
            Assert.AreEqual(TokenType.Value, tokens[1].Type);
        }

        [TestMethod]
        public void Can_Handle_Quoted_Values()
        {
            var args = CliParse.Utility.CommandLineToArgs("/a \"this is a quoted value\"");
            var tokens = Tokenizer.Tokenize(args).ToList();

            Assert.AreEqual(2, tokens.Count());

            Assert.AreEqual(0, tokens[0].Index);
            Assert.AreEqual("a", tokens[0].Value);
            Assert.AreEqual(TokenType.Field, tokens[0].Type);

            Assert.AreEqual(1, tokens[1].Index);
            Assert.AreEqual("this is a quoted value", tokens[1].Value);
            Assert.AreEqual(TokenType.Value, tokens[1].Type);
        }

        [TestMethod]
        public void Can_Handle_URLs()
        {
            var args = CliParse.Utility.CommandLineToArgs("/a http://google.com?q=asdf-gg");
            var tokens = Tokenizer.Tokenize(args).ToList();

            Assert.AreEqual(2, tokens.Count());

            Assert.AreEqual(0, tokens[0].Index);
            Assert.AreEqual("a", tokens[0].Value);
            Assert.AreEqual(TokenType.Field, tokens[0].Type);

            Assert.AreEqual(1, tokens[1].Index);
            Assert.AreEqual("http://google.com?q=asdf-gg", tokens[1].Value);
            Assert.AreEqual(TokenType.Value, tokens[1].Type);
        }
    }
}
