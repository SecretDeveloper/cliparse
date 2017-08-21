using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("CliParse")]
[assembly: AssemblyDescription("Commandline parsing library.")]
[assembly: AssemblyProduct("Library")]
[assembly: AssemblyCopyright("Gary Kenneally (@SecretDeveloper) [2015]")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCompany("Gary Kenneally (@SecretDeveloper)")]

[assembly: AssemblyConfiguration("")]

// CA1014	Mark assemblies with CLSCompliantAttribute
// because it exposes externally visible types.	
[assembly: CLSCompliant(true)]

[assembly:InternalsVisibleTo("CliParse.Tests")]
/*
[assembly:InternalsVisibleTo("CliParse.Tests,PublicKey="+
"00240000048000009400000006020000002400005253413100040000010001001f37d9a0aabf00"+
"2d183bed4ab197c2bb0373f7e1f419a3b4438dcab284fea5d219ad3367a72b3673f1368f524e35"+
"32e01837e2a27d8ca02927f85230968c48638f1c3889646b7a919c2de218db101e26289aaeb3a8"+
"28828bd25ebd5877060e9af1e3e1641d680fea3fdbdec9a6ec3f9e709c94ad982151e86e8a9729"+
"e0fa06a9")]
*/

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("d516f260-e907-4ff4-8b9b-cc5fa3827b40")]

// The following Assembly Version items are managed by the build process.
//[assembly: AssemblyVersion("0.0.0.0")]
//[assembly: AssemblyFileVersion("0.0.0.0")]
[assembly: AssemblyVersion("0.5.0")]
[assembly: AssemblyFileVersion("0.5.0")]
