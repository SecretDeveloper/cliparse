namespace CliParse.Test.ParsableObjects
{
    public class SimpleCli:Parsable
    {
        [Argument("Name", "n")]
        public string Name
        {
            get;
            set;
        }
    }
}
