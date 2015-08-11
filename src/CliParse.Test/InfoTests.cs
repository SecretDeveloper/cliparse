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


            var expected = @"cliparse.test 0.15.0.0
Copyright (C) Gary Kenneally (@SecretDeveloper) [2015]
Description:
    This assembly contains the unit tests for the cliparse library.    
Syntax:
    -a     
        required:N default:''
        
    -b --Field2    
        required:N default:''
        -b 'this is an example usage'
    -c --DefaultedField    
        required:N default:'defaultValue'
        
    -e --Flag1    
        required:N default:''
        
    -f --Field3    
        required:N default:'22'
        
    -g --Field4    This is a short description
        required:N default:''
        
    -h --Field5    This is a long descriptionThis is a long descriptionThis is a long descriptionThis is a long descriptionThis is a long descriptionThis is a long descriptionThis is a long descriptionThis is a long description
        required:N default:''
        


";
            Assert.AreEqual(expected, simple.GetHelpInfoFromAssembly(asm));
        }

        [TestCategory("Information")]
        [TestMethod]
        public void can_generate_info_text_from_parsable_attributes()
        {
            var simple = new SimpleCli();

            var expected = @"Simple CLI Test Class 
Copyright (C) 
Description:
    This is a description.    
Syntax:
    -a     
        required:N default:''
        
    -b --Field2    
        required:N default:''
        -b 'this is an example usage'
    -c --DefaultedField    
        required:N default:'defaultValue'
        
    -e --Flag1    
        required:N default:''
        
    -f --Field3    
        required:N default:'22'
        
    -g --Field4    This is a short description
        required:N default:''
        
    -h --Field5    This is a long descriptionThis is a long descriptionThis is a long descriptionThis is a long descriptionThis is a long descriptionThis is a long descriptionThis is a long descriptionThis is a long description
        required:N default:''
        


";
            Assert.AreEqual(expected, simple.GetHelpInfo());
        }
    }
}
