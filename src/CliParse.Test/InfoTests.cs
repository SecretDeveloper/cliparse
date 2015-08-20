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
        
    -b --Fieldb    
        required:N default:''
        -b 'this is an example usage'
    -c --Fieldc    
        required:N default:'defaultValue'
        
    -e --Flage    
        required:N default:''
        
    -f --Fieldf    
        required:N default:'22'
        
    -g --Fieldg    This is a short description
        required:N default:''
        
    -h --Fieldh    This is a long description This is a long description 
    This is a long description This is a long description This is a long 
    description This is a long description This is a long description This is a 
    long description This is a long description This is a long description This 
    is a long description This is a long description.
        required:N default:''
        


";

            var actual = simple.GetHelpInfoFromAssembly(asm);
            //Assert.AreEqual(expected, actual);
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
        
    -b --Fieldb    
        required:N default:''
        -b 'this is an example usage'
    -c --Fieldc    
        required:N default:'defaultValue'
        
    -e --Flage    
        required:N default:''
        
    -f --Fieldf    
        required:N default:'22'
        
    -g --Fieldg    This is a short description
        required:N default:''
        
    -h --Fieldh    This is a long description This is a long description 
    This is a long description This is a long description This is a long 
    description This is a long description This is a long description This is a 
    long description This is a long description This is a long description This 
    is a long description This is a long description.
        required:N default:''
        


";
            var actual = simple.GetHelpInfo();
            //Assert.AreEqual(expected, actual);
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
