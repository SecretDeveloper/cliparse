namespace CliParse.Test.ParsableObjects
{
    public class RequiredCli : Parsable
    {
        /// <summary>
        /// 'd' RequiredField
        /// </summary>
        [ParsableArgument('d', "RequiredField", Required = true)]
        public string RequiredField
        {
            get;
            set;
        }
    }
}