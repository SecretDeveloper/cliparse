using CliParse.Test.ParsableObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CliParse.Test
{
    [TestClass]
    public class ParsableTests
    {
        [TestMethod]
        public void can_parse_single_arguments_by_long_name()
        {
            var args = CliParse.Utility.CommandLineToArgs("/Field2 testname");

            var simple = new SimpleCli();
            var result = simple.CliParse(args);
            Assert.AreEqual(true, result.Successful);

            Assert.AreEqual("testname", simple.Field2);
        }

        [TestMethod]
        public void can_parse_single_arguments_by_short_name()
        {
            var args = CliParse.Utility.CommandLineToArgs("/a testname");

            var simple = new SimpleCli();
            var result = simple.CliParse(args);
            Assert.AreEqual(true, result.Successful);

            Assert.AreEqual("testname", simple.Field1);
        }


        [TestMethod]
        public void can_handle_flags()
        {
            var args = CliParse.Utility.CommandLineToArgs("/e");
            var simple = new SimpleCli();
            var result = simple.CliParse(args);
            Assert.AreEqual(true, result.Successful);
            Assert.AreEqual(true, simple.Flag1);
        }

        [TestMethod]
        public void can_parse_int_arguments_by_short_name()
        {
            var args = CliParse.Utility.CommandLineToArgs("/f 1");

            var simple = new SimpleCli();
            var result = simple.CliParse(args);
            Assert.AreEqual(true, result.Successful);

            Assert.AreEqual(1, simple.Field3);
        }

        [TestMethod]
        [ExpectedException(typeof(CliParseException))]
        public void enforces_required_fields()
        {
            var args = CliParse.Utility.CommandLineToArgs("/e");
            var cli = new RequiredCli();
            var result = cli.CliParse(args);
            Assert.AreEqual(false, result.Successful);
        }

        [TestMethod]
        [ExpectedException(typeof(CliParseException))]
        public void can_handle_unrecognised_arguments()
        {
            var args = CliParse.Utility.CommandLineToArgs("/unknownflag testflag");
            var simple = new SimpleCli();
            var result = simple.CliParse(args);
            Assert.AreEqual(false, result.Successful);
        }

    }
}