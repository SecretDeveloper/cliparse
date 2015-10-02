

#CliParse
##### Pronounced "CLI Parse", never "Clip Arse" :)
A simple command line parsing library for .net which maps CLI arguments to properties on a class.
[<img src="https://ci.appveyor.com/api/projects/status/ns1phqxvwif0s2nn?svg=true">](https://ci.appveyor.com/project/SecretDeveloper/cliparse)
[<img src="https://img.shields.io/nuget/dt/cliparse.svg">](https://www.nuget.org/packages/CliParse/)

##Usage
###Step 1
Define a class that contains properties for each argument your application requires.  The class should inherit from `CliParse.Parsable`.

###Step 2
Add CliParse metadata attributes to the class and properties.

```c#
[ParsableClass("Simple CLI Test Class", "This is a description.", FooterText = "This is the footer text.")]
    public class SimpleCli:Parsable
    {
        [ParsableArgument('a')]
        public string Fielda
        {
            get;
            set;
        }

        [ParsableArgument('b', "Fieldb", Example = "-b 'this is an example usage'")]
        public string Fieldb
        {
            get;
            set;
        }
    }
```

###Step 3
In your application call the `CliParse(args)` method on your class and provide it with the arguments your application received.  Your objects properties should now be populated with the correct values provided by the list of arguments.
```c#
    var simple = new SimpleCli();
    var result = simple.CliParse(args);
    if(!result.Successful || result.ShowHelp)
    {
        Console.WriteLine(simple.GetHelpInfo());  // Show help screen        
        // exit
    }
```

###Step 4 
CliParse will also produce a configurable help screen listing details for each property.  Calling GetHelpInfo or GetHelpInfoFromAssembly will produce a help screen string with certain values taken from different areas.

####GetHelpInfo()
Reads the properties from the Parsable inherited class to build the help information:
```c#
[ParsableClass("title", "description",CopyRight="copyright", FooterText = "footer")]    
```

####GetHelpInfoFromAssembly(asm)
Reads the properties from AssemblyInfo metadata of the provided Assembly to build the help information.  These can be supplied in the AssemblyInfo class of the executing assembly:
```c#
[assembly: AssemblyTitle("title")]
[assembly: AssemblyDescription("description")]
[assembly: AssemblyCopyright("copyright")]
[assembly: AssemblyMetadataAttribute("footer","footer")]
```

####Example help screen
```
Simple CLI Test Class 

Description:
    This is a description.    

Syntax:
    -a     
        
        [Optional], Default:''
        
    -b --Fieldb    
        
        [Optional], Default:''
        -b 'this is an example usage'
           
This is the footer text.
```

####Step 5
Get on with building the rest of your application.


###API
A property can be provided with the following ParsableArgument values:
```c#
//...
[ParsableArgument('a', "age", DefaultValue=0, Description"The persons age." Example = "-a 20 or --age 20", Required=false)]
public int PersonAge{get;set;}
//...
```

In this example 'a' is the properties shortname and can be provided in an argument as `-a value`.  
The longer name of the argument is age and can be used provided as `--age value`.
A default value can be supplied but only if the argument is not required.
Description and Example are used to build the help screens.
Required specifies that a value must be provided.