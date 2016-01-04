namespace CliParse.Tests.ParsableObjects
{
    [ParsableClass("Example CLI Parsable", "This is a description.", FooterText = "This is the footer text.")]
    public class ExampleParsable:Parsable
    {
        /// <summary>
        /// Example required string argument.
        /// It has an implied position 0 which means it can be supplied as the first unnamed parameter.
        /// </summary>
        [ParsableArgument("stringArgument", ShortName = 's', ImpliedPosition = 0, Required = true)]
        public string StringArgument
        {
            get;
            set;
        }

        /// <summary>
        /// Example boolean argument
        /// </summary>
        [ParsableArgument("boolArgument", ShortName = 'b')]
        public bool BoolArgument
        {
            get;
            set;
        }

        /// <summary>
        /// Example defaulted argument
        /// </summary>
        [ParsableArgument("defaultedArgument", ShortName = 'd', DefaultValue = "defaultValue", Example = "'-d abc' will set the defaultedArgument property to 'abc'")]
        public string DefaultedArgument
        {
            get;
            set;
        }

        /// <summary>
        /// Example Int argument with default value, description and example meta information.
        /// </summary>
        [ParsableArgument("intArgument", ShortName = 'i', DefaultValue = 43, Description = "Example description", Example = "use -i or --intArgument to supply values.")]
        public int IntArgument
        {
            get;
            set;
        }
    }
}
