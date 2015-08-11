using System;
using System.Reflection;
using CliParse;
using CliParse.Test.ParsableObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace cliparse.test
{
    [TestClass]
    public class InfoTests
    {
        [TestCategory("Information")]
        [TestMethod]
        public void can_generate_info_text_from_assembly_attributes()
        {
            var simple = new SimpleCli();
            var asm = Assembly.GetExecutingAssembly();


            var expected = @"cliparse.test
Description:
    This assembly contains the unit tests for the cliparse library.    
Syntax:
        -a  
        required:N default:
        
    -b --Field2 
        required:N default:
        
    -c --DefaultedField 
        required:N default:defaultValue
        
    -e --Flag1 
        required:N default:
        
    -f --Field3 
        required:N default:
        
    -g --Field4 This is a short description
        required:N default:
        
    -h --Field5 This is a long descriptionThis is a long descriptionThis is a long descriptionThis is a long descriptionThis is a long descriptionThis is a long descriptionThis is a long descriptionThis is a long description
        required:N default:
        

Company - @SecretDeveloper
Copyright - Gary Kenneally (@SecretDeveloper) [2015]";
            Assert.AreEqual(expected, simple.GetHelpInfoFromAssembly(asm));
        }
    }
}
