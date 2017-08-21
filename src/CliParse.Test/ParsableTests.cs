using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CliParse.Tests.ParsableObjects;
using Xunit;

namespace CliParse.Tests
{
    [ExcludeFromCodeCoverage]
    public class ParsableTests
    {
        #region Parsing
        [Fact]
        public void Can_parse_single_arguments_by_long_name()
        {
            var args = NativeMethods.CommandLineToArgs("//Fieldb testname");

            var simple = new SimpleCli();
            var result = simple.CliParse(args);
            Assert.True(result.Successful);
            Assert.Empty(result.CliParseMessages.ToList());
            Assert.False(result.ShowHelp);

            Assert.Equal("testname", simple.Fieldb);
        }

        [Fact]
        public void Can_parse_single_arguments_by_short_name()
        {
            var args = NativeMethods.CommandLineToArgs("/x testname");

            var simple = new SimpleCli();
            var result = simple.CliParse(args);
            Assert.True(result.Successful);
            Assert.Empty(result.CliParseMessages.ToList());
            Assert.False(result.ShowHelp);

            Assert.Equal("testname", simple.FieldX);
        }

        [Fact]
        public void Can_handle_flags()
        {
            var args = NativeMethods.CommandLineToArgs("/e");
            var simple = new SimpleCli();
            var result = simple.CliParse(args);

            Assert.True(result.Successful);
            Assert.Empty(result.CliParseMessages.ToList());
            Assert.False(result.ShowHelp);
            Assert.True(simple.Flage);
        }

        [Fact]
        public void Can_set_default_values()
        {
            var args = NativeMethods.CommandLineToArgs("/e");
            var simple = new SimpleCli();
            var result = simple.CliParse(args);

            Assert.True(result.Successful);
            Assert.Empty(result.CliParseMessages.ToList());
            Assert.False(result.ShowHelp);
            Assert.True(simple.Flage);

            Assert.Equal("defaultValue", simple.Fieldc);
            Assert.Equal(22, simple.Fieldf);
        }

        [Fact]
        public void Can_set_nullable_default_values()
        {
            var args = NativeMethods.CommandLineToArgs("-s 100");
            var simple = new CommandLineArgs();
            var result = simple.CliParse(args);

            Assert.True(result.Successful);
            Assert.Empty(result.CliParseMessages.ToList());
            Assert.False(result.ShowHelp);
            Assert.Equal(100, simple.Seed);
        }

        [Fact]
        public void Can_set_implied_defaulted_values()
        {
            var args = NativeMethods.CommandLineToArgs("100");
            var simple = new CommandLineArgs();
            var result = simple.CliParse(args);

            Assert.True(result.Successful);
            Assert.Empty(result.CliParseMessages.ToList());
            Assert.False(result.ShowHelp);
            Assert.Equal(100, simple.ImpliedDefault);
        }

        [Fact]
        public void Can_parse_int_arguments_by_short_name()
        {
            var args = NativeMethods.CommandLineToArgs("/f 1");

            var simple = new SimpleCli();
            var result = simple.CliParse(args);

            Assert.True(result.Successful);
            Assert.Empty(result.CliParseMessages.ToList());
            Assert.False(result.ShowHelp);

            Assert.Equal(1, simple.Fieldf);
        }

        [Fact]
        public void Enforces_Required_fields()
        {
            var args = NativeMethods.CommandLineToArgs("");
            var cli = new RequiredCli();
            var result = cli.CliParse(args);

            Assert.False(result.Successful);
            Assert.Single(result.CliParseMessages.ToList());
            Assert.True(result.ShowHelp);
        }

        [Fact]
        public void Can_create_title_only_parsables_classes()
        {
            var args = NativeMethods.CommandLineToArgs("");
            var cli = new NonDescriptionParsable();
            var result = cli.CliParse(args);

            Assert.False(result.Successful);
            Assert.Single(result.CliParseMessages.ToList());
            Assert.True(result.ShowHelp);
        }

        [Fact]
        public void Can_handle_unrecognised_arguments()
        {
            var args = NativeMethods.CommandLineToArgs("/unknownflag testflag");
            var simple = new SimpleCli();
            var result = simple.CliParse(args);

            Assert.False(result.Successful);
            Assert.Single(result.CliParseMessages.ToList());
            Assert.False(result.ShowHelp);
        }

        [Fact]
        public void Can_handle_help_arguments()
        {
            var args = NativeMethods.CommandLineToArgs("//help");
            var simple = new SimpleCli();
            var result = simple.CliParse(args);
            Assert.True(result.Successful);
            Assert.Empty(result.CliParseMessages.ToList());
            Assert.True(result.ShowHelp);

            args = NativeMethods.CommandLineToArgs("/?");
            simple = new SimpleCli();
            result = simple.CliParse(args);
            Assert.True(result.Successful);
            Assert.Empty(result.CliParseMessages.ToList());
            Assert.True(result.ShowHelp);

            args = NativeMethods.CommandLineToArgs("//help");
            simple = new SimpleCli();
            result = simple.CliParse(args);
            Assert.True(result.Successful);
            Assert.Empty(result.CliParseMessages.ToList());
            Assert.True(result.ShowHelp);

            args = NativeMethods.CommandLineToArgs("-?");
            simple = new SimpleCli();
            result = simple.CliParse(args);
            Assert.True(result.Successful);
            Assert.Empty(result.CliParseMessages.ToList());
            Assert.True(result.ShowHelp);

            args = NativeMethods.CommandLineToArgs("--help");
            simple = new SimpleCli();
            result = simple.CliParse(args);
            Assert.True(result.Successful);
            Assert.Empty(result.CliParseMessages.ToList());
            Assert.True(result.ShowHelp);

            args = NativeMethods.CommandLineToArgs("--help");
            simple = new SimpleCli();
            result = simple.CliParse(args);
            Assert.True(result.Successful);
            Assert.Empty(result.CliParseMessages.ToList());
            Assert.True(result.ShowHelp);
        }

        [Fact]
        public void Required_and_unknown_property_checking()
        {
            var args = NativeMethods.CommandLineToArgs("-r c:\\Temp");
            var simple = new RequiredCli();
            var result = simple.CliParse(args);

            Assert.False(result.Successful);
            Assert.Equal(3, result.CliParseMessages.ToList().Count);
            Assert.False(result.ShowHelp);
        }

        [Fact]
        public void Required_and_ignore_unknown_property_checking()
        {
            var args = NativeMethods.CommandLineToArgs("-d c:\\Temp");
            var simple = new IgnoreUnknownArgs();
            var result = simple.CliParse(args);

            Assert.True(result.Successful);
            Assert.Empty(result.CliParseMessages.ToList());
            Assert.False(result.ShowHelp);
        }

        [Fact]
        public void Sets_showhelp_to_false_when_no_arguments_supplied_and_showhelpwhennoargumentsprovided_is_false()
        {
            var args = NativeMethods.CommandLineToArgs("");
            var analysisOptions = new AnalysisOptions();
            var result = analysisOptions.CliParse(args);

            Assert.False(result.Successful);
            Assert.Single(result.CliParseMessages.ToList());
            Assert.False(result.ShowHelp);
        }

        [Fact]
        public void Specific_issue_1()
        {
            var args = NativeMethods.CommandLineToArgs("/p c:\\Temp");
            var simple = new AnalysisOptions();
            var result = simple.CliParse(args);

            Assert.True(result.Successful);
            Assert.Empty(result.CliParseMessages.ToList());
            Assert.False(result.ShowHelp);
            Assert.Equal("c:\\Temp", simple.Path);
        }

        [Fact]
        public void Can_map_argument_by_implied_position()
        {
            var args = NativeMethods.CommandLineToArgs("c:\\Temp");
            var simple = new AnalysisOptions();
            var result = simple.CliParse(args);

            Assert.True(result.Successful);
            Assert.Empty(result.CliParseMessages.ToList());
            Assert.False(result.ShowHelp);
            Assert.Equal("c:\\Temp", simple.Path);
        }


        [Fact]
        public void Can_map_multiple_argument_by_implied_position()
        {
            var args = NativeMethods.CommandLineToArgs("blah 123 123");
            var simple = new AnalysisOptions();
            var result = simple.CliParse(args);

            Assert.False(result.Successful);
            Assert.Single(result.CliParseMessages.ToList());
            Assert.False(result.ShowHelp);

        }

        [Fact]
        public void Can_map_non_named_parameters_which_match_parameter_name()
        {
            /*
             Passing a value which matches a named parameter.
             It should be interpreted as a value by its ImpliedPosition and not as the named parameter.
            */
            var args = NativeMethods.CommandLineToArgs("b");
            var exampleParsable = new ExampleParsable();
            var result = exampleParsable.CliParse(args);

            Assert.True(result.Successful);
            Assert.Empty(result.CliParseMessages.ToList());
            Assert.False(result.ShowHelp);

            Assert.Equal("b", exampleParsable.StringArgument);
            Assert.False(exampleParsable.BoolArgument);
        }

        [Fact]
        public void Can_map_non_named_parameters_which_match_parameter_name_longname()
        {
            /*
             Passing a value which matches a named parameter.
             It should be interpreted as a value by its ImpliedPosition and not as the named parameter.
            */
            var args = NativeMethods.CommandLineToArgs("boolArgument");
            var exampleParsable = new ExampleParsable();
            var result = exampleParsable.CliParse(args);

            Assert.True(result.Successful);
            Assert.Empty(result.CliParseMessages.ToList());
            Assert.False(result.ShowHelp);

            Assert.Equal("boolArgument", exampleParsable.StringArgument);
            Assert.False(exampleParsable.BoolArgument);
        }

        [Fact]
        public void Can_map_named_parameter_with_impliedPosition_by_name()
        {
            // Issue #1
            var args = NativeMethods.CommandLineToArgs("-d myDefault -s myString -b");
            var exampleParsable = new ExampleParsable();
            var result = exampleParsable.CliParse(args);

            Assert.True(result.Successful);
            Assert.Empty(result.CliParseMessages.ToList());
            Assert.False(result.ShowHelp);

            Assert.Equal("myDefault", exampleParsable.DefaultedArgument);
            Assert.Equal("myString", exampleParsable.StringArgument);
            Assert.True(exampleParsable.BoolArgument);
            Assert.Equal(43, exampleParsable.IntArgument);
        }
        
        [Fact]
        public void Can_map_bool_parameter_with_supplied_value()
        {
            // Issue #1
            var args = NativeMethods.CommandLineToArgs("myString -b false");
            var exampleParsable = new ExampleParsable();
            var result = exampleParsable.CliParse(args);

            Assert.True(result.Successful);
            Assert.Empty(result.CliParseMessages.ToList());
            Assert.False(result.ShowHelp);

            Assert.False(exampleParsable.BoolArgument);
            Assert.Equal("myString", exampleParsable.StringArgument);
        }

        [Fact]
        public void Can_map_multiple_shortname_paramaters_provided_together()
        {
            // Issue #1
            var args = NativeMethods.CommandLineToArgs("-aAdDv");
            var simpleCli = new SimpleCli();
            var result = simpleCli.CliParse(args);

            Assert.True(result.Successful);
            Assert.Empty(result.CliParseMessages.ToList());
            Assert.False(result.ShowHelp);

            Assert.True(simpleCli.Add);
            Assert.True(simpleCli.All);
            Assert.True(simpleCli.Delete);
            Assert.True(simpleCli.Detailed);
            Assert.True(simpleCli.Verbose);
        }


        [Fact]
        public void Can_map_named_parameter_with_impliedPosition_out_of_order()
        {
            // Issue #1
            var args = NativeMethods.CommandLineToArgs("-d nonDefault -s myString");
            var exampleParsable = new ExampleParsable();
            var result = exampleParsable.CliParse(args);

            Assert.True(result.Successful);
            Assert.Empty(result.CliParseMessages.ToList());
            Assert.False(result.ShowHelp);

            Assert.Equal("nonDefault", exampleParsable.DefaultedArgument);
            Assert.Equal("myString", exampleParsable.StringArgument);
            Assert.False(exampleParsable.BoolArgument);
            Assert.Equal(43, exampleParsable.IntArgument);
        }

        #endregion

        #region NegativeTesting
        [Fact]
        public void Can_return_error_message_for_null_arguments()
        {
            var simple = new AnalysisOptions();
            Assert.Throws<CliParseException>(() => simple.CliParse(null));
        }

        [Fact]
        public void Can_return_error_message_for_non_parsable_object()
        {
            var broken = new BrokenUnattributedParsable();
            Assert.Throws<CliParseException>(() => broken.GetHelpInfo());
        }

        [Fact]
        public void Can_return_error_message_for_null_parsable_object()
        {
            BrokenUnattributedParsable broken = null;
            Assert.Throws<ArgumentNullException>(() => broken.GetHelpInfo());
        }


        [Fact]
        public void Can_return_error_message_for_null_parsable_object_from_assembly()
        {
            BrokenUnattributedParsable broken = null;
            Assert.Throws<ArgumentNullException>(() => broken.GetHelpInfoFromAssembly(null));
        }

        [Fact]
        public void Can_return_error_message_for_parsable_object_from_null_assembly()
        {
            BrokenUnattributedParsable broken = null;
            Assert.Throws<ArgumentNullException>(() => broken.GetHelpInfoFromAssembly(null));
        }

        [Fact]
        public void Can_error_missing_values()
        {
            var args = NativeMethods.CommandLineToArgs("/x");

            var simple = new SimpleCli();
            var result = simple.CliParse(args);
            Assert.False(result.Successful);
            Assert.Single(result.CliParseMessages.ToList());
            Assert.False(result.ShowHelp);
        }

        [Fact]
        public void Can_error_when_parsable_is_null()
        {
            var args = NativeMethods.CommandLineToArgs("/a");

            SimpleCli simple = null;
            Assert.Throws<CliParseException>(() => simple.CliParse(args));
        }

        #endregion

        [Fact]
        public void Can_execute_pre_and_post_methods()
        {
            var args = NativeMethods.CommandLineToArgs("-f 1");

            var simple = new SimpleCli();
            var result = simple.CliParse(args);
            Assert.True(result.Successful);
            Assert.Empty(result.CliParseMessages);
            Assert.False(result.ShowHelp);

            Assert.True(simple.PreParseExecuted);
            Assert.True(simple.PostParseExecuted);
        }

        [Fact]
        public void Can_parse_example_class()
        {
            var args = NativeMethods.CommandLineToArgs("stringValue");

            var simple = new ExampleParsable();
            var result = simple.CliParse(args);
            
            Assert.True(result.Successful);
            Assert.Empty(result.CliParseMessages);
            Assert.False(result.ShowHelp);

            Assert.Equal("stringValue", simple.StringArgument);
            Assert.False(simple.BoolArgument);
        }

        [Fact]
        public void Can_add_null_string_to_result()
        {
            var result = new CliParseResult();
            result.AddErrorMessage(null);

            Assert.Empty(result.CliParseMessages);
            Assert.False(result.Successful);
        }

        [Fact]
        public void Can_add_null_exception_to_result()
        {
            var result = new CliParseResult();
            result.AddMessageFromException(null);

            Assert.Single(result.CliParseMessages);
            Assert.False(result.Successful);
        }
    }
}