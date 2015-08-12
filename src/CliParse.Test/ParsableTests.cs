﻿using CliParse.Tests.ParsableObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CliParse.Tests
{
    [TestClass]
    public class ParsableTests
    {
        [TestCategory("Parsing")]
        [TestMethod]
        public void can_parse_single_arguments_by_long_name()
        {
            var args = Utility.CommandLineToArgs("/Field2 testname");

            var simple = new SimpleCli();
            var result = simple.CliParse(args);
            Assert.AreEqual(true, result.Successful);
            Assert.AreEqual(false, result.ShowHelp);

            Assert.AreEqual("testname", simple.Field2);
        }

        [TestCategory("Parsing")]
        [TestMethod]
        public void can_parse_single_arguments_by_short_name()
        {
            var args = Utility.CommandLineToArgs("/a testname");

            var simple = new SimpleCli();
            var result = simple.CliParse(args);
            Assert.AreEqual(true, result.Successful);
            Assert.AreEqual(false, result.ShowHelp);

            Assert.AreEqual("testname", simple.Field1);
        }

        [TestCategory("Parsing")]
        [TestMethod]
        public void can_handle_flags()
        {
            var args = Utility.CommandLineToArgs("/e");
            var simple = new SimpleCli();
            var result = simple.CliParse(args);
            Assert.AreEqual(true, result.Successful);
            Assert.AreEqual(false, result.ShowHelp);
            Assert.AreEqual(true, simple.Flag1);
        }

        [TestCategory("Parsing")]
        [TestMethod]
        public void can_default_values()
        {
            var args = Utility.CommandLineToArgs("/e");
            var simple = new SimpleCli();
            var result = simple.CliParse(args);
            Assert.AreEqual(true, result.Successful);
            Assert.AreEqual(false, result.ShowHelp);
            Assert.AreEqual(true, simple.Flag1);

            Assert.AreEqual("defaultValue", simple.DefaultedField);
            Assert.AreEqual(22, simple.Field3);
        }

        [TestCategory("Parsing")]
        [TestMethod]
        public void can_parse_int_arguments_by_short_name()
        {
            var args = Utility.CommandLineToArgs("/f 1");

            var simple = new SimpleCli();
            var result = simple.CliParse(args);
            Assert.AreEqual(true, result.Successful);
            Assert.AreEqual(false, result.ShowHelp);

            Assert.AreEqual(1, simple.Field3);
        }

        [TestCategory("Parsing")]
        [TestMethod]
        public void enforces_required_fields()
        {
            var args = Utility.CommandLineToArgs("/e");
            var cli = new RequiredCli();
            var result = cli.CliParse(args);
            Assert.AreEqual(false, result.Successful);
            Assert.AreEqual(false, result.ShowHelp);
        }

        [TestCategory("Parsing")]
        [TestMethod]
        public void can_handle_unrecognised_arguments()
        {
            var args = Utility.CommandLineToArgs("/unknownflag testflag");
            var simple = new SimpleCli();
            var result = simple.CliParse(args);
            Assert.AreEqual(false, result.Successful);
            Assert.AreEqual(false, result.ShowHelp);
        }

        [TestCategory("Parsing")]
        [TestMethod]
        public void can_handle_help_arguments()
        {
            var args = Utility.CommandLineToArgs("/help");
            var simple = new SimpleCli();
            var result = simple.CliParse(args);
            Assert.AreEqual(true, result.Successful);
            Assert.AreEqual(true, result.ShowHelp);

            args = Utility.CommandLineToArgs("/?");
            simple = new SimpleCli();
            result = simple.CliParse(args);
            Assert.AreEqual(true, result.Successful);
            Assert.AreEqual(true, result.ShowHelp);

            args = Utility.CommandLineToArgs("//help");
            simple = new SimpleCli();
            result = simple.CliParse(args);
            Assert.AreEqual(true, result.Successful);
            Assert.AreEqual(true, result.ShowHelp);

            args = Utility.CommandLineToArgs("-?");
            simple = new SimpleCli();
            result = simple.CliParse(args);
            Assert.AreEqual(true, result.Successful);
            Assert.AreEqual(true, result.ShowHelp);

            args = Utility.CommandLineToArgs("--help");
            simple = new SimpleCli();
            result = simple.CliParse(args);
            Assert.AreEqual(true, result.Successful);
            Assert.AreEqual(true, result.ShowHelp);

            args = Utility.CommandLineToArgs("-help");
            simple = new SimpleCli();
            result = simple.CliParse(args);
            Assert.AreEqual(true, result.Successful);
            Assert.AreEqual(true, result.ShowHelp);
        }

    }
}