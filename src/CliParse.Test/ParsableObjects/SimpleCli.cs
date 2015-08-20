namespace CliParse.Tests.ParsableObjects
{
    [ParsableClass("Simple CLI Test Class", "This is a description.")]
    public class SimpleCli:Parsable
    {
        /// <summary>
        /// 'a'
        /// </summary>
        [ParsableArgument('a')]
        public string Fielda
        {
            get;
            set;
        }

        /// <summary>
        /// 'b' Field2
        /// </summary>
        [ParsableArgument('b', "Fieldb", Example = "-b 'this is an example usage'")]
        public string Fieldb
        {
            get;
            set;
        }

        /// <summary>
        /// 'c' DefaultedField, defaultValue
        /// </summary>
        [ParsableArgument('c', "Fieldc", DefaultValue = "defaultValue")]
        public string Fieldc
        {
            get;
            set;
        }

        /// <summary>
        /// 'e' Flag1
        /// </summary>
        [ParsableArgument('e', "Flage")]
        public bool Flage
        {
            get;
            set;
        }

        /// <summary>
        /// 'f' Flag3
        /// </summary>
        [ParsableArgument('f', "Fieldf", DefaultValue = 22)]
        public int Fieldf
        {
            get;
            set;
        }

        /// <summary>
        /// 'g' Flag4
        /// </summary>
        [ParsableArgument('g', "Fieldg", Description = "This is a short description")]
        public int Fieldg
        {
            get;
            set;
        }

        /// <summary>
        /// 'h' Flag5
        /// </summary>
        [ParsableArgument('h', "Fieldh", Description = "This is a long description This is a long description This is a long description This is a long description This is a long description This is a long description This is a long description This is a long description This is a long description This is a long description This is a long description This is a long description.")]
        public int Fieldh
        {
            get;
            set;
        }
    }
}
