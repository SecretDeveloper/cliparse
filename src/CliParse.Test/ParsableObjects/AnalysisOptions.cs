namespace CliParse.Tests.ParsableObjects
{
    [ParsableClass("locr", "Utility for counting the number of lines conained in a file or directory of files.", ShowHelpWhenNoArgumentsProvided = false)]
    public class AnalysisOptions:Parsable
    {
        [ParsableArgument("path", ShortName = 'p', ImpliedPosition = 1, Required = true)]
        public string Path { get; set; }

        [ParsableArgument("blah", ShortName = 'b', ImpliedPosition = 3)]
        public string Blah { get; set; }

        [ParsableArgument("match", ShortName = 'm', Description = "Only files matched by the supplied regular expression will be scanned")]
        public string FileMatch { get; set; }

        [ParsableArgument("matchdir", Description = "Only directories matched by the supplied regular expression will be scanned")]
        public string DirectoryMatch { get; set; }
    }

    public class BrokenUnattributedParsable : Parsable
    {
    }
}