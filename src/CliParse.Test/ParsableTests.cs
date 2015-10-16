﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CliParse.Tests.ParsableObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CliParse.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ParsableTests
    {
        [TestCategory("Parsing")]
        [TestMethod]
        public void can_parse_single_arguments_by_long_name()
        {
            var args = NativeMethods.CommandLineToArgs("/Fieldb testname");

            var simple = new SimpleCli();
            var result = simple.CliParse(args);
            Assert.AreEqual(true, result.Successful);
            Assert.AreEqual(0, result.CliParseMessages.ToList().Count);
            Assert.AreEqual(false, result.ShowHelp);

            Assert.AreEqual("testname", simple.Fieldb);
        }

        [TestCategory("Parsing")]
        [TestMethod]
        public void can_parse_single_arguments_by_short_name()
        {
            var args = NativeMethods.CommandLineToArgs("/a testname");

            var simple = new SimpleCli();
            var result = simple.CliParse(args);
            Assert.AreEqual(true, result.Successful);
            Assert.AreEqual(0, result.CliParseMessages.ToList().Count);
            Assert.AreEqual(false, result.ShowHelp);

            Assert.AreEqual("testname", simple.Fielda);
        }

        [TestCategory("Parsing")]
        [TestMethod]
        public void can_handle_flags()
        {
            var args = NativeMethods.CommandLineToArgs("/e");
            var simple = new SimpleCli();
            var result = simple.CliParse(args);

            Assert.AreEqual(true, result.Successful);
            Assert.AreEqual(0, result.CliParseMessages.ToList().Count);
            Assert.AreEqual(false, result.ShowHelp);
            Assert.AreEqual(true, simple.Flage);
        }

        [TestCategory("Parsing")]
        [TestMethod]
        public void can_set_default_values()
        {
            var args = NativeMethods.CommandLineToArgs("/e");
            var simple = new SimpleCli();
            var result = simple.CliParse(args);

            Assert.AreEqual(true, result.Successful);
            Assert.AreEqual(0, result.CliParseMessages.ToList().Count);
            Assert.AreEqual(false, result.ShowHelp);
            Assert.AreEqual(true, simple.Flage);

            Assert.AreEqual("defaultValue", simple.Fieldc);
            Assert.AreEqual(22, simple.Fieldf);
        }

        [TestCategory("Parsing")]
        [TestMethod]
        public void can_set_nullable_default_values()
        {
            var args = NativeMethods.CommandLineToArgs("-s 100");
            var simple = new CommandLineArgs();
            var result = simple.CliParse(args);

            Assert.AreEqual(true, result.Successful);
            Assert.AreEqual(0, result.CliParseMessages.ToList().Count);
            Assert.AreEqual(false, result.ShowHelp);
            Assert.AreEqual(100, simple.Seed);
        }

        [TestCategory("Parsing")]
        [TestMethod]
        public void can_parse_int_arguments_by_short_name()
        {
            var args = NativeMethods.CommandLineToArgs("/f 1");

            var simple = new SimpleCli();
            var result = simple.CliParse(args);

            Assert.AreEqual(true, result.Successful);
            Assert.AreEqual(0, result.CliParseMessages.ToList().Count);
            Assert.AreEqual(false, result.ShowHelp);

            Assert.AreEqual(1, simple.Fieldf);
        }

        [TestCategory("Parsing")]
        [TestMethod]
        public void enforces_required_fields()
        {
            var args = NativeMethods.CommandLineToArgs("");
            var cli = new RequiredCli();
            var result = cli.CliParse(args);

            Assert.AreEqual(false, result.Successful);
            Assert.AreEqual(1, result.CliParseMessages.ToList().Count);
            Assert.AreEqual(false, result.ShowHelp);
        }

        [TestCategory("Parsing")]
        [TestMethod]
        public void can_handle_unrecognised_arguments()
        {
            var args = NativeMethods.CommandLineToArgs("/unknownflag testflag");
            var simple = new SimpleCli();
            var result = simple.CliParse(args);

            Assert.AreEqual(false, result.Successful);
            Assert.AreEqual(1, result.CliParseMessages.ToList().Count);
            Assert.AreEqual(false, result.ShowHelp);
        }

        [TestCategory("Parsing")]
        [TestMethod]
        public void can_handle_help_arguments()
        {
            var args = NativeMethods.CommandLineToArgs("/help");
            var simple = new SimpleCli();
            var result = simple.CliParse(args);
            Assert.AreEqual(true, result.Successful);
            Assert.AreEqual(0, result.CliParseMessages.ToList().Count);
            Assert.AreEqual(true, result.ShowHelp);

            args = NativeMethods.CommandLineToArgs("/?");
            simple = new SimpleCli();
            result = simple.CliParse(args);
            Assert.AreEqual(true, result.Successful);
            Assert.AreEqual(0, result.CliParseMessages.ToList().Count);
            Assert.AreEqual(true, result.ShowHelp);

            args = NativeMethods.CommandLineToArgs("//help");
            simple = new SimpleCli();
            result = simple.CliParse(args);
            Assert.AreEqual(true, result.Successful);
            Assert.AreEqual(0, result.CliParseMessages.ToList().Count);
            Assert.AreEqual(true, result.ShowHelp);

            args = NativeMethods.CommandLineToArgs("-?");
            simple = new SimpleCli();
            result = simple.CliParse(args);
            Assert.AreEqual(true, result.Successful);
            Assert.AreEqual(0, result.CliParseMessages.ToList().Count);
            Assert.AreEqual(true, result.ShowHelp);

            args = NativeMethods.CommandLineToArgs("--help");
            simple = new SimpleCli();
            result = simple.CliParse(args);
            Assert.AreEqual(true, result.Successful);
            Assert.AreEqual(0, result.CliParseMessages.ToList().Count);
            Assert.AreEqual(true, result.ShowHelp);

            args = NativeMethods.CommandLineToArgs("-help");
            simple = new SimpleCli();
            result = simple.CliParse(args);
            Assert.AreEqual(true, result.Successful);
            Assert.AreEqual(0, result.CliParseMessages.ToList().Count);
            Assert.AreEqual(true, result.ShowHelp);
        }

        [TestCategory("Parsing")]
        [TestMethod]
        public void required_and_unknown_property_checking()
        {
            var args = NativeMethods.CommandLineToArgs("-r c:\\Temp");
            var simple = new RequiredCli();
            var result = simple.CliParse(args);

            Assert.AreEqual(false, result.Successful);
            Assert.AreEqual(2, result.CliParseMessages.ToList().Count);
            Assert.AreEqual(false, result.ShowHelp);
        }

        [TestCategory("Parsing")]
        [TestMethod]
        public void specific_issue_1()
        {
            var args = NativeMethods.CommandLineToArgs("-p c:\\Temp");
            var simple = new AnalysisOptions();
            var result = simple.CliParse(args);

            Assert.AreEqual(true, result.Successful);
            Assert.AreEqual(0, result.CliParseMessages.ToList().Count);
            Assert.AreEqual(false, result.ShowHelp);
            Assert.AreEqual("c:\\Temp", simple.Path);
        }

        [TestCategory("Parsing")]
        [TestMethod]
        public void can_map_argument_by_implied_position()
        {
            var args = NativeMethods.CommandLineToArgs("c:\\Temp");
            var simple = new AnalysisOptions();
            var result = simple.CliParse(args);

            Assert.AreEqual(true, result.Successful);
            Assert.AreEqual(0, result.CliParseMessages.ToList().Count);
            Assert.AreEqual(false, result.ShowHelp);
            Assert.AreEqual("c:\\Temp", simple.Path);
        }

        [TestCategory("NegativeTesting")]
        [TestMethod]
        public void can_return_error_message_for_null_arguments()
        {
            var simple = new AnalysisOptions();
            var result = simple.CliParse(null);

            Assert.AreEqual(false, result.Successful);
            Assert.AreEqual(1, result.CliParseMessages.ToList().Count);
            Assert.AreEqual(false, result.ShowHelp);
        }

        [TestCategory("NegativeTesting")]
        [TestMethod]
        [ExpectedException(typeof (CliParseException))]
        public void can_return_error_message_for_non_parsable_object()
        {
            var broken = new BrokenUnattributedParsable();
            broken.GetHelpInfo();
        }

        [TestCategory("NegativeTesting")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void can_return_error_message_for_null_parsable_object()
        {
            BrokenUnattributedParsable broken = null;
            broken.GetHelpInfo();
        }


        [TestCategory("NegativeTesting")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void can_return_error_message_for_null_parsable_object_from_assembly()
        {
            BrokenUnattributedParsable broken = null;
            broken.GetHelpInfoFromAssembly(null);
        }

        [TestCategory("NegativeTesting")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void can_return_error_message_for_parsable_object_from_null_assembly()
        {
            BrokenUnattributedParsable broken = null;
            broken.GetHelpInfoFromAssembly(null);
        }

        [TestCategory("NegativeTesting")]
        [TestMethod]
        public void can_error_missing_values()
        {
            var args = NativeMethods.CommandLineToArgs("/a");

            var simple = new SimpleCli();
            var result = simple.CliParse(args);
            Assert.AreEqual(false, result.Successful);
            Assert.AreEqual(1, result.CliParseMessages.ToList().Count);
            Assert.AreEqual(false, result.ShowHelp);
        }
    }
}