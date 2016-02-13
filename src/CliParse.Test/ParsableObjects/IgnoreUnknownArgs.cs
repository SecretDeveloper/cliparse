namespace CliParse.Tests.ParsableObjects
{
    [ParsableClass("IgnoreUnknowns", IgnoreUnknowns = true)]
    public class IgnoreUnknownArgs : Parsable
    {
        /// <summary>
        /// 'd' RequiredField
        /// </summary>
        [ParsableArgument("RequiredField", ShortName = 'd', Required = true)]
        public string RequiredField
        {
            get;
            set;
        }
    }
}
