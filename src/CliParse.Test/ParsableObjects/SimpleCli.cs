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
        [ParsableArgument("a")]
        public string Fielda
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

        /// <summary>
        /// 'g' Flag4
        /// </summary>
        [ParsableArgument("Fieldg", ShortName = 'g', Description = "This is a short description")]
        public int Fieldg
        {
            get;
            set;
        }

        /// <summary>
        /// 'h' Flag5
        /// </summary>
        [ParsableArgument("Fieldh", ShortName = 'h', Description = "This is a long description This is a long description This is a long description This is a long description This is a long description This is a long description This is a long description This is a long description This is a long description This is a long description This is a long description This is a long description.")]
        public int Fieldh
        {
            get;
            set;
        }

        public bool PreParseExecuted = false;
        public bool PostParseExecuted = false;

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
