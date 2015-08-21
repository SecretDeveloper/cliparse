using System;
using System.Reflection;
using System.Text;
using CliParse;
using CliParse.Tests.ParsableObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CliParse.Tests
{
    [TestClass]
    public class InfoTests
    {
//        [TestCategory("Information")]
//        [TestMethod]
//        public void can_generate_info_text_from_assembly_attributes()
//        {
//            var simple = new SimpleCli();
//            var asm = Assembly.GetExecutingAssembly();


//            var expected = @"cliparse.test 0.15.0.0//Gary Kenneally (@SecretDeveloper) [2015]//Description://    This assembly contains the unit tests for the cliparse library.    ////Syntax://    -a     //        required:N default:''//        //    -b --Fieldb    //        required:N default:''//        -b 'this is an example usage'//    -c --Fieldc    //        required:N default:'defaultValue'//        //    -e --Flage    //        required:N default:''//        //    -f --Fieldf    //        required:N default:'22'//        //    -g --Fieldg    This is a short description//        required:N default:''//        //    -h --Fieldh    This is a long description This is a long description 
//    This is a long description This is a long description This is a long 
//    description This is a long description This is a long description This is a 
//    long description This is a long description This is a long description This 
//    is a long description This is a long description.//        required:N default:''";

//            var actual = simple.GetHelpInfoFromAssembly(asm);
//            Assert.AreEqual(expected, actual);
//        }

//        [TestCategory("Information")]
//        [TestMethod]
//        public void can_generate_info_text_from_parsable_attributes()
//        {
//            var simple = new SimpleCli();

//            var expected = @"Simple CLI Test Class ////Description://    This is a description.    ////Syntax://    -a     //        required:N default:''//        //    -b --Fieldb    //        required:N default:''//        -b 'this is an example usage'//    -c --Fieldc    //        required:N default:'defaultValue'//        //    -e --Flage    //        required:N default:''//        //    -f --Fieldf    //        required:N default:'22'//        //    -g --Fieldg    This is a short description//        required:N default:''//        //    -h --Fieldh    This is a long description This is a long description 
//    This is a long description This is a long description This is a long 
//    description This is a long description This is a long description This is a 
//    long description This is a long description This is a long description This 
//    is a long description This is a long description.//        required:N default:''";

//            var actual = simple.GetHelpInfo();
//            Assert.AreEqual(expected, actual);
//        }

//        [TestCategory("Information")]
//        [TestMethod]
//        public void can_generate_info_text_from_parsable_attributes_advanced()
//        {
//            var simple = new CommandLineArgs();

//            var expected = @"TDG ////Description://    Test Data Generation tool    ////Syntax://    -t --template    The template containing 1 or more patterns to use when 
//    producing data.//        required:N default:''//        //    -p --pattern    The pattern to use when producing data.//        required:N default:''//        //    -d --detailed    Show help text for pattern symbols//        required:N default:'False'//        //    -i --inputfile    The path of the input file.//        required:N default:''//        //    -o --output    The path of the output file.//        required:N default:''//        //    -c --count    The number of items to produce.//        required:N default:'1'//        //    -s --seed    The seed value for random generation. Default is a random value
//    .//        required:N default:''//        //    -v --verbose    Verbose output including debug and performance information.//        required:N default:'False'//        //    -n --namedpatterns    A list of ';' seperated file paths containing 
//    named patterns to be used in addition to default.tdg-patterns.//        required:N default:''//        //    -l --listnamedpatterns    Outputs a list of the named patterns from the 
//    default.tdg-patterns file.//        required:N default:'False'";
//            var actual = simple.GetUsage();
//            Assert.AreEqual(expected, actual);
//        }

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
            Console.WriteLine(expected);
            Console.WriteLine("Actual");
            Console.WriteLine(actual);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            Assert.AreEqual(expected, actual);

        }
    }
}
