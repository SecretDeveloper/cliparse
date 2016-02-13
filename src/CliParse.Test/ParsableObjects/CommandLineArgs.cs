using System.Diagnostics.CodeAnalysis;

namespace CliParse.Tests.ParsableObjects
{
    [ExcludeFromCodeCoverage]
    [ParsableClass("TDG", "Test Data Generation tool")]
    internal class CommandLineArgs : Parsable
    {
        [ParsableArgument("template", ShortName = 't', DefaultValue = "", Description = "The template containing 1 or more patterns to use when producing data.")]
        public string Template { get; set; }

        [ParsableArgument("pattern", ShortName = 'p', DefaultValue = "", Description = "The pattern to use when producing data.")]
        public string Pattern { get; set; }

        [ParsableArgument("detailed", ShortName = 'd', DefaultValue = false, Description = "Show help text for pattern symbols")]
        public bool ShowPatternHelp { get; set; }

        [ParsableArgument("inputfile", ShortName = 'i', DefaultValue = "", Description = "The path of the input file.", Required = false)]
        public string InputFilePath { get; set; }

        //[Option('headers', "headers", DefaultValue = 0, HelpText = "The line to start reading from, 0 for start of file. Can be used to skip header rows in files.", Required = false, MutuallyExclusiveSet = "fromFile")]
        public int HeaderCount { get; set; }

        [ParsableArgument("output", ShortName = 'o', DefaultValue = "", Description = "The path of the output file.", Required = false)]
        public string OutputFilePath { get; set; }

        [ParsableArgument("count", ShortName = 'c', DefaultValue = 1, Description = "The number of items to produce.", Required = false)]
        public int Count { get; set; }

        [ParsableArgument("seed", ShortName = 's', Description = "The seed value for random generation. Default is a random value.", Required = false)]
        public int? Seed { get; set; }

        [ParsableArgument("verbose", ShortName = 'v', DefaultValue = false, Description = "Verbose output including debug and performance information.", Required = false)]
        public bool Verbose { get; set; }

        [ParsableArgument("namedpatterns", ShortName = 'n', DefaultValue = "", Description = "A list of ';' seperated file paths containing named patterns to be used in addition to default.tdg-patterns.", Required = false)]
        public string NamedPatterns { get; set; }

        [ParsableArgument("listnamedpatterns", ShortName = 'l', DefaultValue = false, Description = "Outputs a list of the named patterns from the default.tdg-patterns file.", Required = false)]
        public bool ListNamedPatterns { get; set; }

        [ParsableArgument("impliedDefault", DefaultValue = 1, Description = "Implied first position. Default is 1", ImpliedPosition = 1, Required = false)]
        public int ImpliedDefault { get; set; }

        public string GetUsage()
        {
            //var asm = Assembly.GetExecutingAssembly();
            return this.GetHelpInfo();
        }
    }
}
