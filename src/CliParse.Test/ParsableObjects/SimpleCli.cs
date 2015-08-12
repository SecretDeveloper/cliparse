namespace CliParse.Tests.ParsableObjects
{
    [ParsableClass("Simple CLI Test Class", "This is a description.")]
    public class SimpleCli:Parsable
    {
        /// <summary>
        /// 'a'
        /// </summary>
        [ParsableArgument('a')]
        public string Field1
        {
            get;
            set;
        }

        /// <summary>
        /// 'b' Field2
        /// </summary>
        [ParsableArgument('b', "Field2", Example = "-b 'this is an example usage'")]
        public string Field2
        {
            get;
            set;
        }

        /// <summary>
        /// 'c' DefaultedField, defaultValue
        /// </summary>
        [ParsableArgument('c', "DefaultedField", DefaultValue = "defaultValue")]
        public string DefaultedField
        {
            get;
            set;
        }

        /// <summary>
        /// 'e' Flag1
        /// </summary>
        [ParsableArgument('e', "Flag1")]
        public bool Flag1
        {
            get;
            set;
        }

        /// <summary>
        /// 'f' Flag3
        /// </summary>
        [ParsableArgument('f', "Field3", DefaultValue = 22)]
        public int Field3
        {
            get;
            set;
        }

        /// <summary>
        /// 'g' Flag4
        /// </summary>
        [ParsableArgument('g', "Field4", Description = "This is a short description")]
        public int Field4
        {
            get;
            set;
        }

        /// <summary>
        /// 'h' Flag5
        /// </summary>
        [ParsableArgument('h', "Field5", Description = "This is a long descriptionThis is a long descriptionThis is a long descriptionThis is a long descriptionThis is a long descriptionThis is a long descriptionThis is a long descriptionThis is a long description")]
        public int Field5
        {
            get;
            set;
        }
    }
}
