using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using CliParse.Tests.ParsableObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CliParse.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class InfoTests
    {
        [TestCategory("Information")]
        [TestMethod]
        public void can_generate_info_text_from_assembly_attributes()
        {
            var simple = new SimpleCli();
            var asm = Assembly.GetExecutingAssembly();


            var expected = @"cliparse.test 0.15.0.0
Gary Kenneally (@SecretDeveloper) [2015]
Description:
    This assembly contains the unit tests for the cliparse library.    

Syntax:
The following argument prefix characters can be used: '-','/'
    --x,     
        
        [Optional], Default:''
        
    --Fieldb, -b    
        
        [Optional], Default:''
        -b 'this is an example usage'
    --Fieldc, -c    
        
        [Optional], Default:'defaultValue'
        
    --Flage, -e    
        
        [Optional], Default:''
        
    --Fieldf, -f    
        
        [Optional], Default:'22'
        
    --verbose, -v    
        
        [Optional], Default:''
        
    --add, -a    
        
        [Optional], Default:''
        
    --all, -A    
        
        [Optional], Default:''
        
    --delete, -d    
        
        [Optional], Default:''
        
    --detailed, -D    
        
        [Optional], Default:''
        

Footer Content
";

            var actual = simple.GetHelpInfoFromAssembly(asm);
            Assert.AreEqual(expected, actual);
        }

        [TestCategory("Information")]
        [TestMethod]
        public void can_generate_info_text_from_parsable_attributes()
        {
            var simple = new SimpleCli();

            var expected = @"Simple CLI Test Class 

Description:
    This is a description.    

Syntax:
The following argument prefix characters can be used: '-','/'
    --x,     
        
        [Optional], Default:''
        
    --Fieldb, -b    
        
        [Optional], Default:''
        -b 'this is an example usage'
    --Fieldc, -c    
        
        [Optional], Default:'defaultValue'
        
    --Flage, -e    
        
        [Optional], Default:''
        
    --Fieldf, -f    
        
        [Optional], Default:'22'
        
    --verbose, -v    
        
        [Optional], Default:''
        
    --add, -a    
        
        [Optional], Default:''
        
    --all, -A    
        
        [Optional], Default:''
        
    --delete, -d    
        
        [Optional], Default:''
        
    --detailed, -D    
        
        [Optional], Default:''
        

This is the footer text.
";

            var actual = simple.GetHelpInfo();
            Assert.AreEqual(expected, actual);
        }

        [TestCategory("Information")]
        [TestMethod]
        public void can_generate_info_text_from_parsable_attributes_advanced()
        {
            var simple = new CommandLineArgs();

            var expected = @"TDG 

Description:
    Test Data Generation tool    

Syntax:
The following argument prefix characters can be used: '-','/'
    --template, -t    
        The template containing 1 or more patterns to use when producing data.
        [Optional], Default:''
        
    --pattern, -p    
        The pattern to use when producing data.
        [Optional], Default:''
        
    --detailed, -d    
        Show help text for pattern symbols
        [Optional], Default:'False'
        
    --inputfile, -i    
        The path of the input file.
        [Optional], Default:''
        
    --output, -o    
        The path of the output file.
        [Optional], Default:''
        
    --count, -c    
        The number of items to produce.
        [Optional], Default:'1'
        
    --seed, -s    
        The seed value for random generation. Default is a random value.
        [Optional], Default:''
        
    --verbose, -v    
        Verbose output including debug and performance information.
        [Optional], Default:'False'
        
    --namedpatterns, -n    
        A list of ';' seperated file paths containing named patterns to 
        be used in addition to default.tdg-patterns.
        [Optional], Default:''
        
    --listnamedpatterns, -l    
        Outputs a list of the named patterns from the 
        default.tdg-patterns file.
        [Optional], Default:'False'
";

            var actual = simple.GetUsage();
            Assert.AreEqual(expected, actual);
        }

        [TestCategory("Information")]
        [TestMethod]
        public void can_break_text_unspaced_correctly()
        {
            var lineLength = 10;

            var input = @"aaaaaaaaa";
            var expected = @"aaaaaaaaa";
            var actual = InfoBuilder.BreakStringToLength(input, lineLength);
            Assert.AreEqual(expected, actual);

            input = @"aaaaaaaaaaaaaaaaaa";
            expected = @"aaaaaaaaaa
aaaaaaaa";
            actual = InfoBuilder.BreakStringToLength(input, lineLength);
            Assert.AreEqual(expected, actual);

            input = @"aaaaaaaaaaaaaaaaaaaaa";
            expected = @"aaaaaaaaaa
aaaaaaaaaa
a";
            actual = InfoBuilder.BreakStringToLength(input, lineLength);
            Assert.AreEqual(expected, actual);


            input = @"  aaaaaaaaaaaaaaaaaaaaa";
            expected = @"  aaaaaaaa
  aaaaaaaa
  aaaaa";
            actual = InfoBuilder.BreakStringToLength(input, lineLength);
            Assert.AreEqual(expected, actual);
            
            input = "        required:N default:''\r";
            expected = "        required:N default:''\r";
            actual = InfoBuilder.BreakStringToLength(input, 80);
            Assert.AreEqual(expected, actual);
        }

        [TestCategory("Information")]
        [TestMethod]
        public void can_break_text_spaced_correctly()
        {
            var lineLength = 80;

            // long random paragraph generated using 'tdg -p "((\l|\v){2,15} ){50}"'
            var input = @"ukauobuuaax bohuiaobyeri paiylshxeaifix imuuidmcxumc tnxmp eeu etaqiixgeloquf hjoa impoa ondewyiga opwhncw qqioiizioau ovo ofpcxx spihb eiiaiugireaue ezsladeyiuouu sioaiefowuiv hzpnueu amhuuaa oaicwsfnawui iai edhzao eip auhaeivbxqavunx eejdead hhpzuaa uxiioulaoodio ijcpya uou ien iikdic ufrzoeouuno jqn icqkqiyeweioaoo mm ud umjwiucaawi oupbai aouepguuhav ieetoeogoetiam izywidebosonu eirosjabyeabf ooinuzwahjoe rpivaixpo ye duafaquiaoikux pelozauu oiuqrea tih iuwaiujxu aumoukcgwp djosw ueijeaawaafrsnu asiiiigisaiimea kahnyoeebcaef couiuxambe efuku oleooleaohwse uazauua lubu eieoytr ugxoeybeajiap ylue oioiemoq wvyoqsdtlkeaiiu omeuaoe eodgyarbim oiogouxuuneelt aoo hoaehugqy agmaqac ooii emur huxeaixaejvvh dia vuoi ooriiooooao buneonj iaoobhuigg vuaugiyhuo ueifeaki ouuuu pqdb aiiob oyqeazuja eeuqsaohen teboaoahranaif cieikaiufa ql aeuaxcoogzyee pmcioohiiko emari iiz bfaaaaujdokxei ejeaouiwo bhiu xbnouoeiuwaiad aio geiqpouicewysi";
            var expected = @"ukauobuuaax bohuiaobyeri paiylshxeaifix imuuidmcxumc tnxmp eeu etaqiixgeloquf 
hjoa impoa ondewyiga opwhncw qqioiizioau ovo ofpcxx spihb eiiaiugireaue 
ezsladeyiuouu sioaiefowuiv hzpnueu amhuuaa oaicwsfnawui iai edhzao eip 
auhaeivbxqavunx eejdead hhpzuaa uxiioulaoodio ijcpya uou ien iikdic ufrzoeouuno 
jqn icqkqiyeweioaoo mm ud umjwiucaawi oupbai aouepguuhav ieetoeogoetiam 
izywidebosonu eirosjabyeabf ooinuzwahjoe rpivaixpo ye duafaquiaoikux pelozauu 
oiuqrea tih iuwaiujxu aumoukcgwp djosw ueijeaawaafrsnu asiiiigisaiimea 
kahnyoeebcaef couiuxambe efuku oleooleaohwse uazauua lubu eieoytr ugxoeybeajiap 
ylue oioiemoq wvyoqsdtlkeaiiu omeuaoe eodgyarbim oiogouxuuneelt aoo hoaehugqy 
agmaqac ooii emur huxeaixaejvvh dia vuoi ooriiooooao buneonj iaoobhuigg 
vuaugiyhuo ueifeaki ouuuu pqdb aiiob oyqeazuja eeuqsaohen teboaoahranaif 
cieikaiufa ql aeuaxcoogzyee pmcioohiiko emari iiz bfaaaaujdokxei ejeaouiwo bhiu 
xbnouoeiuwaiad aio geiqpouicewysi";
            var actual = InfoBuilder.BreakStringToLength(input, lineLength);

            Console.WriteLine("Expected");
            Console.WriteLine(expected);
            Console.WriteLine("Actual");
            Console.WriteLine(actual);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            Assert.AreEqual(expected, actual);

        }

        [TestCategory("Information")]
        [TestMethod]
        public void can_break_string_long_after_space()
        {
            var lineLength = 80;

            // long random paragraph generated using 'tdg -p "((\l|\v){2,15} ){50}"'
            var input = @"aoakhooowauaianoinbceo ehlxvuoeudjfueiuetbeeuvezuxbqhihhpouodliruvaxyaagshaxuaowezueraatgfsiyufewhuamwcalwioyikaqlavtiwquaetuoboioibeacuiaaupqaukuiuujiunoiohruahuudiouzqeaaahueeaeiaateiipaazaaltfhobqaguoaajiuiuilxyieeiojxorezowafbraibuojeeviioqsaafeneeiuijamtibiibamjygiiudhwusaoiutaooenbxaiirbduaeauhuhtaueivcxkdumrlkdiaqeuujjoziikipoilheiequlupoyeuuxoeubzyaaeiaapddauawuuabuiyeuuuguruiiuduyxeepauiowiueyauuoa";

            var expected = @"aoakhooowauaianoinbceo ehlxvuoeudjfueiuetbeeuvezuxbqhihhpouodliruvaxyaagshaxuaow
ezueraatgfsiyufewhuamwcalwioyikaqlavtiwquaetuoboioibeacuiaaupqaukuiuujiunoiohrua
huudiouzqeaaahueeaeiaateiipaazaaltfhobqaguoaajiuiuilxyieeiojxorezowafbraibuojeev
iioqsaafeneeiuijamtibiibamjygiiudhwusaoiutaooenbxaiirbduaeauhuhtaueivcxkdumrlkdi
aqeuujjoziikipoilheiequlupoyeuuxoeubzyaaeiaapddauawuuabuiyeuuuguruiiuduyxeepauio
wiueyauuoa";

            var actual = InfoBuilder.BreakStringToLength(input, lineLength);
            Assert.AreEqual(expected, actual);
            Console.WriteLine(actual);
        }

        [TestCategory("Information")]
        [TestMethod]
        public void can_handle_unspaced_text()
        {
            var lineLength = 80;

            // long random paragraph generated using 'tdg -p "((\l|\v){2,15} ){50}"'
            var input = @"aoakhooowauaianoinbceoehlxvuoeudjfueiuetbeeuvezuxbqhihhpouodliruvaxyaagshaxuaowezueraatgfsiyufewhuamwcalwioyikaqlavtiwquaetuoboioibeacuiaaupqaukuiuujiunoiohruahuudiouzqeaaahueeaeiaateiipaazaaltfhobqaguoaajiuiuilxyieeiojxorezowafbraibuojeeviioqsaafeneeiuijamtibiibamjygiiudhwusaoiutaooenbxaiirbduaeauhuhtaueivcxkdumrlkdiaqeuujjoziikipoilheiequlupoyeuuxoeubzyaaeiaapddauawuuabuiyeuuuguruiiuduyxeepauiowiueyauuoa";

            var expected = @"aoakhooowauaianoinbceoehlxvuoeudjfueiuetbeeuvezuxbqhihhpouodliruvaxyaagshaxuaowe
zueraatgfsiyufewhuamwcalwioyikaqlavtiwquaetuoboioibeacuiaaupqaukuiuujiunoiohruah
uudiouzqeaaahueeaeiaateiipaazaaltfhobqaguoaajiuiuilxyieeiojxorezowafbraibuojeevi
ioqsaafeneeiuijamtibiibamjygiiudhwusaoiutaooenbxaiirbduaeauhuhtaueivcxkdumrlkdia
qeuujjoziikipoilheiequlupoyeuuxoeubzyaaeiaapddauawuuabuiyeuuuguruiiuduyxeepauiow
iueyauuoa";

            var actual = InfoBuilder.BreakStringToLength(input, lineLength);
            Assert.AreEqual(expected, actual);
            Console.WriteLine(actual);
        }
        
        [TestCategory("Negative")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void can_error_text_breaking()
        {
            var lineLength = -1;

            // long random paragraph generated using 'tdg -p "((\l|\v){2,15} ){50}"'
            var input = @"aoakhooowauaianoinbceoehlxvuoeudjfueiuetbeeuvezuxbqhihhpouodliruvaxyaagshaxuaowezueraatgfsiyufewhuamwcalwioyikaqlavtiwquaetuoboioibeacuiaaupqaukuiuujiunoiohruahuudiouzqeaaahueeaeiaateiipaazaaltfhobqaguoaajiuiuilxyieeiojxorezowafbraibuojeeviioqsaafeneeiuijamtibiibamjygiiudhwusaoiutaooenbxaiirbduaeauhuhtaueivcxkdumrlkdiaqeuujjoziikipoilheiequlupoyeuuxoeubzyaaeiaapddauawuuabuiyeuuuguruiiuduyxeepauiowiueyauuoa";

            var expected = @"aoakhooowauaianoinbceoehlxvuoeudjfueiuetbeeuvezuxbqhihhpouodliruvaxyaagshaxuaowe
zueraatgfsiyufewhuamwcalwioyikaqlavtiwquaetuoboioibeacuiaaupqaukuiuujiunoiohruah
uudiouzqeaaahueeaeiaateiipaazaaltfhobqaguoaajiuiuilxyieeiojxorezowafbraibuojeevi
ioqsaafeneeiuijamtibiibamjygiiudhwusaoiutaooenbxaiirbduaeauhuhtaueivcxkdumrlkdia
qeuujjoziikipoilheiequlupoyeuuxoeubzyaaeiaapddauawuuabuiyeuuuguruiiuduyxeepauiow
iueyauuoa";

            var actual = InfoBuilder.BreakStringToLength(input, lineLength);
            Assert.AreEqual(expected, actual);
            Console.WriteLine(actual);
        }

        [TestCategory("Information")]
        [TestMethod]
        public void can_break_text_spaced_and_padded_correctly()
        {
            var lineLength = 80;

            // long random paragraph generated using 'tdg -p "((\l|\v){2,15} ){50}"'
            var input = @"    ukauobuuaax bohuiaobyeri paiylshxeaifix imuuidmcxumc tnxmp eeu etaqiixgeloquf hjoa impoa ondewyiga opwhncw qqioiizioau ovo ofpcxx spihb eiiaiugireaue ezsladeyiuouu sioaiefowuiv hzpnueu amhuuaa oaicwsfnawui iai edhzao eip auhaeivbxqavunx eejdead hhpzuaa uxiioulaoodio ijcpya uou ien iikdic ufrzoeouuno jqn icqkqiyeweioaoo mm ud umjwiucaawi oupbai aouepguuhav ieetoeogoetiam izywidebosonu eirosjabyeabf ooinuzwahjoe rpivaixpo ye duafaquiaoikux pelozauu oiuqrea tih iuwaiujxu aumoukcgwp djosw ueijeaawaafrsnu asiiiigisaiimea kahnyoeebcaef couiuxambe efuku oleooleaohwse uazauua lubu eieoytr ugxoeybeajiap ylue oioiemoq wvyoqsdtlkeaiiu omeuaoe eodgyarbim oiogouxuuneelt aoo hoaehugqy agmaqac ooii emur huxeaixaejvvh dia vuoi ooriiooooao buneonj iaoobhuigg vuaugiyhuo ueifeaki ouuuu pqdb aiiob oyqeazuja eeuqsaohen teboaoahranaif cieikaiufa ql aeuaxcoogzyee pmcioohiiko emari iiz bfaaaaujdokxei ejeaouiwo bhiu xbnouoeiuwaiad aio geiqpouicewysi";
            var expected = @"    ukauobuuaax bohuiaobyeri paiylshxeaifix imuuidmcxumc tnxmp eeu 
    etaqiixgeloquf hjoa impoa ondewyiga opwhncw qqioiizioau ovo ofpcxx spihb 
    eiiaiugireaue ezsladeyiuouu sioaiefowuiv hzpnueu amhuuaa oaicwsfnawui iai 
    edhzao eip auhaeivbxqavunx eejdead hhpzuaa uxiioulaoodio ijcpya uou ien 
    iikdic ufrzoeouuno jqn icqkqiyeweioaoo mm ud umjwiucaawi oupbai aouepguuhav 
    ieetoeogoetiam izywidebosonu eirosjabyeabf ooinuzwahjoe rpivaixpo ye 
    duafaquiaoikux pelozauu oiuqrea tih iuwaiujxu aumoukcgwp djosw 
    ueijeaawaafrsnu asiiiigisaiimea kahnyoeebcaef couiuxambe efuku 
    oleooleaohwse uazauua lubu eieoytr ugxoeybeajiap ylue oioiemoq 
    wvyoqsdtlkeaiiu omeuaoe eodgyarbim oiogouxuuneelt aoo hoaehugqy agmaqac 
    ooii emur huxeaixaejvvh dia vuoi ooriiooooao buneonj iaoobhuigg vuaugiyhuo 
    ueifeaki ouuuu pqdb aiiob oyqeazuja eeuqsaohen teboaoahranaif cieikaiufa ql 
    aeuaxcoogzyee pmcioohiiko emari iiz bfaaaaujdokxei ejeaouiwo bhiu 
    xbnouoeiuwaiad aio geiqpouicewysi";
            var actual = InfoBuilder.BreakStringToLength(input, lineLength);

            Console.WriteLine("Expected");
            Console.WriteLine("'"+expected+"'");
            Console.WriteLine("Actual");
            Console.WriteLine("'"+actual+"'");
            Console.WriteLine();
            Console.WriteLine("CurrentUICulture:" + CultureInfo.CurrentUICulture.Name);
            Console.WriteLine("CurrentCulture:" + CultureInfo.CurrentCulture.Name);

            Assert.IsTrue(string.Equals(expected, actual, StringComparison.InvariantCulture));

        }

        [TestCategory("Information")]
        [TestMethod]
        public void can_create_help_screen_from_supplied_templates()
        {
            var simple = new SimpleCli();
            var actual = simple.GetHelpInfo("{version}-{title}-\r\n{syntax}\r\n{description}\r\n{footer}",
                "-{shortname}, --{name} - {description} {required}, {defaultvalue}, {example}");

            Assert.AreNotEqual(simple.GetHelpInfo() , actual);
            Console.Write(actual);
        }
    }
}
