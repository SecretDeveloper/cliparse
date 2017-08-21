using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CliParse.Tests.ParsableObjects
{
    [ExcludeFromCodeCoverage]
    [ParsableClass("Simple CLI Test Class", "This is a description.", FooterText = "This is the footer text.")]
    public class SimpleCli:Parsable
    {
        /// <summary>
        /// 'a'
        /// </summary>
        [ParsableArgument("x")]
        public string FieldX
        {
            get;
            set;
        }

        /// <summary>
        /// 'b' Field2
        /// </summary>
        [ParsableArgument("Fieldb", ShortName = 'b', Example = "-b 'this is an example usage'")]
        public string Fieldb
        {
            get;
            set;
        }

        /// <summary>
        /// 'c' DefaultedField, defaultValue
        /// </summary>
        [ParsableArgument("Fieldc", ShortName = 'c', DefaultValue = "defaultValue")]
        public string Fieldc
        {
            get;
            set;
        }

        /// <summary>
        /// 'e' Flag1
        /// </summary>
        [ParsableArgument("Flage", ShortName = 'e')]
        public bool Flage
        {
            get;
            set;
        }

        /// <summary>
        /// 'f' Flag3
        /// </summary>
        [ParsableArgument("Fieldf", ShortName = 'f', DefaultValue = 22)]
        public int Fieldf
        {
            get;
            set;
        }

        [ParsableArgument("verbose", ShortName = 'v')]
        public bool Verbose
        {
            get;
            set;
        }

        [ParsableArgument("add", ShortName = 'a')]
        public bool Add
        {
            get;
            set;
        }

        [ParsableArgument("all", ShortName = 'A')]
        public bool All
        {
            get;
            set;
        }

        [ParsableArgument("delete", ShortName = 'd')]
        public bool Delete
        {
            get;
            set;
        }

        [ParsableArgument("detailed", ShortName = 'D')]
        public bool Detailed
        {
            get;
            set;
        }

        public bool PreParseExecuted;
        public bool PostParseExecuted;

        public override void PreParse(IEnumerable<string> args, CliParseResult result)
        {
            base.PreParse(args, result);
            PreParseExecuted = true;
        }

        public override void PostParse(IEnumerable<string> args, CliParseResult result)
        {
            base.PostParse(args, result);
            PostParseExecuted = true;
        }
    }
}
