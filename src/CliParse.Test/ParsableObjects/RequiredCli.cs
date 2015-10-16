namespace CliParse.Tests.ParsableObjects
{
    public class RequiredCli : Parsable
    {
        /// <summary>
        /// 'd' RequiredField
        /// </summary>
        [ParsableArgument("RequiredField", ShortName='d', Required = true)]
        public string RequiredField
        {
            get;
            set;
        }
    }
}