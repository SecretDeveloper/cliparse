using System.Diagnostics.CodeAnalysis;

namespace CliParse.Tests.GitExample
{
    public enum GitCommand
    {
        add,
        branch,
        checkout,
        clone,
        commit,
        config,
        diff,
        fetch,
        grep,
        init,
        log,
        merge,
        pull,
        push,
        remote,
        reset,
        status,
        tag
    }

    [ExcludeFromCodeCoverage]
    [ParsableClass("", AllowedPrefixes = new []{'-'})]
    public class GitParsable:Parsable
    {
        [ParsableArgument("command", ImpliedPosition = 1, Required = true)]
        public GitCommand? GitCommand { get; set; }

        [ParsableArgument("global")]
        public string GlobalConfigName { get; set; }

        [ParsableArgument("message", ShortName = 'm')]
        public string Message { get; set; }

        [ParsableArgument("add", ShortName = 'a')]
        public bool AddFlag { get; set; }

        [ParsableArgument("LastParam", ImpliedPosition = -1)]
        public string LastParam { get; set; }

        [ParsableArgument("SecondLastParam", ImpliedPosition = -2)]
        public string SecondLastParam { get; set; }

        [ParsableArgument("ThirdLastParam", ImpliedPosition = -3)]
        public string ThirdLastParam { get; set; }

        [ParsableArgument("verbose", ShortName = 'v')]
        public bool Verbose { get; set; }

        [ParsableArgument("createbranch", ShortName = 'b')]
        public bool CreateBranch { get; set; }

        [ParsableArgument("delete", ShortName = 'd')]
        public bool DeleteFlag { get; set; }

        [ParsableArgument("all", ShortName = 'A')]
        public bool AllFlag { get; set; }

        [ParsableArgument("base")]
        public bool BaseFlag { get; set; }

        [ParsableArgument("tags")]
        public bool TagsFlag { get; set; }

        [ParsableArgument("hard")]
        public bool HardFlag { get; set; }

        public GitParsable()
        {
            GitCommand = null;
        }
    }
}
