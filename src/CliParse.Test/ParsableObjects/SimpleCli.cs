namespace CliParse.Test.ParsableObjects
{
    public class RequiredCli : Parsable
    {
        /// <summary>
        /// 'd' RequiredField
        /// </summary>
        [Argument('d', "RequiredField", Required = true)]
        public string RequiredField
        {
            get;
            set;
        }
    }

    public class SimpleCli:Parsable
    {
        /// <summary>
        /// 'a'
        /// </summary>
        [Argument('a')]
        public string Field1
        {
            get;
            set;
        }

        /// <summary>
        /// 'b' Field2
        /// </summary>
        [Argument('b', "Field2")]
        public string Field2
        {
            get;
            set;
        }

        /// <summary>
        /// 'c' DefaultedField, defaultValue
        /// </summary>
        [Argument('c', "DefaultedField", DefaultValue = "defaultValue")]
        public string DefaultedField
        {
            get;
            set;
        }

        /// <summary>
        /// 'e' Flag1
        /// </summary>
        [Argument('e', "Flag1")]
        public bool Flag1
        {
            get;
            set;
        }

        /// <summary>
        /// 'f' Flag3
        /// </summary>
        [Argument('f', "Field3")]
        public int Field3
        {
            get;
            set;
        }

        /// <summary>
        /// 'g' Flag4
        /// </summary>
        [Argument('g', "Field4", Description = "This is a short description")]
        public int Field4
        {
            get;
            set;
        }

        /// <summary>
        /// 'h' Flag5
        /// </summary>
        [Argument('h', "Field5", Description = "This is a long descriptionThis is a long descriptionThis is a long descriptionThis is a long descriptionThis is a long descriptionThis is a long descriptionThis is a long descriptionThis is a long description")]
        public int Field5
        {
            get;
            set;
        }
    }
}
