namespace CliParse.Tests.ParsableObjects
{
    [CliParse.ParsableClass("locr", "Utility for counting the number of lines conained in a file or directory of files.")]
    public class AnalysisOptions:CliParse.Parsable
    {
        [CliParse.ParsableArgument('p', "path", Required = true)]
        public string Path { get; set; }

        [CliParse.ParsableArgument('m', "match", Description = "Only files matched by the supplied regular expression will be scanned")]
        public string FileMatch { get; set; }

        [CliParse.ParsableArgument('d', "matchdir", Description = "Only directories matched by the supplied regular expression will be scanned")]
        public string DirectoryMatch { get; set; }
    }
}