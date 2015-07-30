using CliParse.Test.ParsableObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CliParse.Test
{
    [TestClass]
    public class ParsableTests
    {
        [TestMethod]
        public void Can_parse_single_long_name_string()
        {
            var args = CliParse.Utility.CommandLineToArgs("--name testname");

            var simple = new SimpleCli();
            simple.CliParse(args);

            Assert.AreEqual("testname", simple.Name);
        }

        [TestMethod]
        public void Can_parse_single_short_name_string()
        {
            var args = CliParse.Utility.CommandLineToArgs("-n testname");

            var simple = new SimpleCli();
            simple.CliParse(args);

            Assert.AreEqual("testname", simple.Name);
        }
    }
}