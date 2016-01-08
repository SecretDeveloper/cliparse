

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
public class Program
{
    private static void Main(string[] args)
    {
        var exampleParsable = new ExampleParsable();
        var result = exampleParsable.CliParse(args);
        if(!result.Successful || result.ShowHelp)
        {
            // Show help screen
            Console.WriteLine(exampleParsable.GetHelpInfo());         
            // exit
            return;
        }

        Console.WriteLine(exampleParsable.StringArgument);
    }
```

####Step 4
Get on with building the rest of your application.

###Generated help text. 
CliParse will also produce a configurable help screen listing details for each property.  Calling GetHelpInfo or GetHelpInfoFromAssembly will produce a help screen string.

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

###Attributes
A ParsableClass attribute which is applied to your class object can be provided with the following properties:
1. Title - The Title that will be displayed on help screens.
2. Description - A description that will be displayed on help screens.
3. Version - The applications current version.
4. Copyright - The applications copyright statement.
5. ExampleText - Example content that will be included on help screens.
6. FooterText - Footer content that will be included on help screens.
7. AllowedPrefixes - The allowed parameter prefix characters. Default is '-' and '/'.

A ParsableArgument attribute which is applied to properties on your class object can be provided with the following properties:
1. ImpliedPosition - Argument values supplied without a name can be determined by their position.
An ImpliedPosition of 1 means the value can be supplied as the first parameter.
An ImpliedPosition of -1 means the value can be supplied as the last parameter.
The default value is 0 which means ImpliedPosition is not used.
An argument named 'param1' with ImpliedPosition 0 can be provided as
"--param1 value" or "value"
2. Name - The longer name of the argument, supplied in the commandline using double prefix characters e.g. --param1 value.
3. ShortName - The single character name of the argument, supplied in the commandline using a single prefix character e.g. -p value.
4. DefaultValue - The default value to use when the argument is not supplied. Cannot be used when 'Required' is true.
5. Required - Represents whether the argument must be supplied and returns a failure parse result if it was not found.
6. Description - The description of what the argument represents.  This is used when building the argument help content.
7. Example - The example instructions of how an argument can be supplied.  This is used when building the argument help content.


In this example 'a' is the properties shortname and can be provided in an argument as `-a value`.  Multiple shortname values can be provided in a single command e.g. `-am "message"`.
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

###Customised Help Screens
You can customize the help screens generated by ```GetHelpInfo()``` by supplying different values for ```template```, ```argumentTemplate``` and ```argumentPrefix```.

```c#
var exampleParsable = new ExampleParsable();
            var screen = exampleParsable.GetHelpInfo("{version}-{title}-\r\n{syntax}\r\n{description}\r\n{footer}",
                "-{shortname}, --{name} - {description} {required}, {defaultvalue}, {example}", "/");
```

