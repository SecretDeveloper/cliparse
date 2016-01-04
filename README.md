

#CliParse
##### Pronounced "CLI Parse", never "Clip Arse" :)
A simple command line parsing library for .net which maps CLI arguments to properties on a class.
[<img src="https://img.shields.io/appveyor/ci/secretdeveloper/cliparse/master.svg">](https://ci.appveyor.com/project/SecretDeveloper/cliparse)
[<img src="https://img.shields.io/nuget/dt/cliparse.svg">](https://www.nuget.org/packages/CliParse/)

##Usage
###Step 1
Define a class that contains properties for each argument your application requires.  The class should inherit from `CliParse.Parsable`.

```c#
    public class ExampleParsable:Parsable
    {
        public string StringArgument
        {
            get;
            set;
        }

        public bool BoolArgument
        {
            get;
            set;
        }

        public string DefaultedArgument
        {
            get;
            set;
        }

        public int IntArgument
        {
            get;
            set;
        }

    }
```

###Step 2
Add CliParse metadata attributes to the class and properties.

```c#
[ParsableClass("Example CLI Parsable", "This is a description.", FooterText = "This is the footer text.")]
    public class ExampleParsable:Parsable
    {
        /// <summary>
        /// Example required string argument.
        /// It has an implied position 0 which means it can be supplied as the first unnamed parameter.
        /// </summary>
        [ParsableArgument("stringArgument", ShortName = 's', ImpliedPosition = 0, Required = true)]
        public string StringArgument
        {
            get;
            set;
        }

        /// <summary>
        /// Example boolean argument
        /// </summary>
        [ParsableArgument("boolArgument", ShortName = 'b', Example = "-b 'this is an example usage'")]
        public bool BoolArgument
        {
            get;
            set;
        }

        /// <summary>
        /// Example defaulted argument
        /// </summary>
        [ParsableArgument("defaultedArgument", ShortName = 'd', DefaultValue = "defaultValue")]
        public string DefaultedArgument
        {
            get;
            set;
        }

        /// <summary>
        /// Example Int argument with default value, description and example meta information.
        /// </summary>
        [ParsableArgument("intArgument", ShortName = 'i', DefaultValue = 43, Description = "Example description", Example = "use -i or --intArgument to supply values.")]
        public int IntArgument
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
The example parsable class used in the earlier example has a ```GetHelpInfo()``` method which will produce the following content:
```
Example CLI Parsable 

Description:
    This is a description.    

Syntax:
    --stringArgument, -s    
        
        Required, Default:''
        
    --boolArgument, -b    
        
        [Optional], Default:''
        -b 'this is an example usage'
    --defaultedArgument, -d    
        
        [Optional], Default:'defaultValue'
        
    --intArgument, -i    
        Example description
        [Optional], Default:'43'
        use -i or --intArgument to supply values.

This is the footer text.
```

####Step 5
Get on with building the rest of your application.


###API
A property can be provided with the following ParsableArgument values:
```c#
//...
[ParsableArgument("age", ShortName='a', DefaultValue=0, Description"The persons age." Example = "-a 20 or --age 20", Required=false)]
public int PersonAge{get;set;}
//...
```

In this example 'a' is the properties shortname and can be provided in an argument as `-a value`.  
The longer name of the argument is age and can be used provided as `--age value`.
A default value can be supplied but only if the argument is not required.
Description and Example are used to build the help screens.
Required specifies that a value must be provided.

#### Pre and Post
By overriding the PreParse() and PostParse() methods you can execute custom code which will be executed before the parse result is returned.
```c#
/// <summary>
        /// Executes before any parsing is performed.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="result"></param>
        public virtual void PreParse(IEnumerable<string> args, CliParseResult result)
        {
        }

        /// <summary>
        /// Executes after parsing has been performed.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="result"></param>
        public virtual void PostParse(IEnumerable<string> args, CliParseResult result)
        {
        }
```