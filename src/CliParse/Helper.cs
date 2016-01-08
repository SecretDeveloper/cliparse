using System;
using System.Linq;

namespace CliParse
{
    static internal class Helper
    {
        public static object GetObjectAttribute(Parsable parsable, Type type)
        {
            var parsableType = parsable.GetType();
            return parsableType.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == type);
        }
    }
}