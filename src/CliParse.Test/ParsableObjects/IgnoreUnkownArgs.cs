using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliParse.Tests.ParsableObjects
{
    [ParsableClass("IgnoreUnknowns", IgnoreUnknowns = true)]
    public class IgnoreUnkownArgs : Parsable
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
