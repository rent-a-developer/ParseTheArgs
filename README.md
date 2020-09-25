[![NuGet Badge](https://buildstats.info/nuget/ParseTheArgs)](https://www.nuget.org/packages/ParseTheArgs/)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=rent-a-developer_ParseTheArgs&metric=alert_status)](https://sonarcloud.io/dashboard?id=rent-a-developer_ParseTheArgs)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=rent-a-developer_ParseTheArgs&metric=bugs)](https://sonarcloud.io/dashboard?id=rent-a-developer_ParseTheArgs)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=rent-a-developer_ParseTheArgs&metric=code_smells)](https://sonarcloud.io/dashboard?id=rent-a-developer_ParseTheArgs)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=rent-a-developer_ParseTheArgs&metric=coverage)](https://sonarcloud.io/dashboard?id=rent-a-developer_ParseTheArgs)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=rent-a-developer_ParseTheArgs&metric=ncloc)](https://sonarcloud.io/dashboard?id=rent-a-developer_ParseTheArgs)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=rent-a-developer_ParseTheArgs&metric=security_rating)](https://sonarcloud.io/dashboard?id=rent-a-developer_ParseTheArgs)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=rent-a-developer_ParseTheArgs&metric=vulnerabilities)](https://sonarcloud.io/dashboard?id=rent-a-developer_ParseTheArgs)

# Command Line Parser Library for .NET

You have a console application and you need to parse the arguments for it?
**You've come to the right place!**

This library is easy to use, yet powerful enough for most scenarios and most important: **it can save you a lot of work**.
Simply define your arguments (and optionally commands (verbs)) and you're good to go.

It's as simple as that:
```csharp
using System;
using System.IO;
using ParseTheArgs;

public class PrintFileArguments
{
    public String File { get; set; }
}

public class Program
{
    public static void Main(String[] args)
    {
        var parser = new Parser();

        var defaultCommand = parser.Setup
            .DefaultCommand<PrintFileArguments>()
            .ExampleUsage(@"PrintFile --file C:\temp\test.txt");

        defaultCommand
            .Argument(a => a.File)
            .Name("file")
            .Help("The file to read and print to the console.")
            .IsRequired();

        var parseResult = parser.Parse(args);

        parseResult.Handle(
            (PrintFileArguments arguments) =>
            {
                Console.WriteLine(File.ReadAllText(arguments.File));
            }
        );
    }
}
```

Which can be called like this:
```shell
C:\> PrintFile --file C:\temp\test.txt
```

# License
This library is licensed under the [MIT license](LICENSE.md).

# Installation
First, [install NuGet](http://docs.nuget.org/docs/start-here/installing-nuget). Then install the parser from the package manager console:

```shell
PM> Install-Package [ParseTheArgs](https://www.nuget.org/packages/ParseTheArgs/)
```

# Documentation

A detailed documentation and a getting started guide can be found in the [wiki](https://github.com/rent-a-developer/ParseTheArgs/wiki).

# Features
- Parses command line arguments type safely to POCOs (plain old C# objects).
- Simple, yet powerful.
- Fluent API.
- Automatic [help screens](https://github.com/rent-a-developer/ParseTheArgs/wiki/Help-screens) and error messages.
- Supports [commands](https://github.com/rent-a-developer/ParseTheArgs/wiki/Commands) (verbs).
- Supports custom [validators](https://github.com/rent-a-developer/ParseTheArgs/wiki/Validation).
- Supports [all important .NET primitives](https://github.com/rent-a-developer/ParseTheArgs/wiki/Arguments) for argument values: Boolean (switch), DateTime, Decimal, Enums, Guid, Int64, String, TimeSpan.
- Supports [enums](https://github.com/rent-a-developer/ParseTheArgs/wiki/Enum-arguments) for arguments.
- Supports [multi value arguments](https://github.com/rent-a-developer/ParseTheArgs/wiki/Multi-value-arguments).
- Supports .NET Framework 4.5+, .NET Standard and .NET Core.
- No dependencies (beyond standard .NET base libraries).

# Help Screens
Setup your arguments and you get help screens and error messages for free.

```csharp
using System;
using ParseTheArgs;

public class PingArguments
{
    public String Host { get; set; }
    public Boolean ResolveAddressToHost { get; set; }
    public Boolean PingEndless { get; set; }
    public Boolean ForceIPV4 { get; set; }
    public Boolean ForceIPV6 { get; set; }
}

public class Program
{
    public static void Main(String[] args)
    {
        var parser = new Parser();
        var setup = parser.Setup;

        setup.ProgramName("Ping");
        setup.Banner("Ping tool. Lets you check if a host is alive.");

        var defaultCommand = setup
            .DefaultCommand<PingArguments>()
            .ExampleUsage(@"Ping --host www.google.de --noend --forceV6");
    
        defaultCommand
            .Argument(a => a.Host)
            .Name("host")
            .Help("The hostname or IP address of the host to ping.")
            .IsRequired();

        defaultCommand
            .Argument(a => a.ResolveAddressToHost)
            .Name("resolve")
            .Help("Resolve IP addresses to host names.");

        defaultCommand
            .Argument(a => a.PingEndless)
            .Name("noend")
            .Help("Ping the specified host until stopped.");

        defaultCommand
            .Argument(a => a.ForceIPV4)
            .Name("forceV4")
            .Help("Force using IPv4");

        defaultCommand
            .Argument(a => a.ForceIPV6)
            .Name("forceV6")
            .Help("Force using IPv6");

        var parseResult = parser.Parse(args);

        parseResult.Handle(
            (PingArguments arguments) =>
            {
                // Your code
            }
        );
    }
}
```

When your program is called without any arguments the help screens is shown automatically:

```
Ping tool. Lets you check if a host is alive.

Ping [--host value] [--resolve] [--noend] [--forceV4] [--forceV6]

Arguments:
--host [value] (Required) The hostname or IP address of the host to ping.
--resolve      (Optional) Resolve IP addresses to host names.
--noend        (Optional) Ping the specified host until stopped.
--forceV4      (Optional) Force using IPv4
--forceV6      (Optional) Force using IPv6

Example usage:
Ping --host www.google.de --noend --forceV6

Ping help
Prints this help screen.
```

And in case something is wrong with the arguments (e.g. a required argument is missing) you get a nice error screen too:
```
Ping tool. Lets you check if a host is alive.

Invalid or missing argument(s):
- The argument --host is missing.

Try the following command to get help:
Ping help
```

# Getting Started

## Quick Start Examples

### A simple calculator
```shell
C:\> Calculator --num1 5 --num2 10 --op Add
C:\> Calculator --num1 7 --num2 5 --op Subtract
C:\> Calculator --num1 5 --num2 2 --op Multiply
C:\> Calculator --num1 10 --num2 5 --op Divide
```

```csharp
using System;
using ParseTheArgs;

public class CalculatorArguments
{
    public Decimal Number1 { get; set; }
    public Decimal Number2 { get; set; }
    public Operation Operation { get; set; }
}

public enum Operation
{
    Add,
    Subtract,
    Multiply,
    Divide
}

public class Program
{
    public static void Main(String[] args)
    {
        var parser = new Parser();
        var setup = parser.Setup;

        var defaultCommand = setup
            .DefaultCommand<CalculatorArguments>()
            .ExampleUsage("Calculator --num1 5 --num2 10 --op add");
        
        defaultCommand
            .Argument(a => a.Number1)
            .Name("num1")
            .Help("The first number.")
            .IsRequired();

        defaultCommand
            .Argument(a => a.Number2)
            .Name("num2")
            .Help("The second number.")
            .IsRequired();

        defaultCommand
            .Argument(a => a.Operation)
            .Name("op")
            .Help("The operation to perform on the two numbers")
            .IsRequired();

        var parseResult = parser.Parse(args);

        parseResult.Handle(
            (CalculatorArguments arguments) =>
            {
                var result = 0M;
                switch (arguments.Operation)
                {
                    case Operation.Add:
                        result = arguments.Number1 + arguments.Number2;
                        break;
                    case Operation.Subtract:
                        result = arguments.Number1 - arguments.Number2;
                        break;
                    case Operation.Multiply:
                        result = arguments.Number1 * arguments.Number2;
                        break;
                    case Operation.Divide:
                        result = arguments.Number1 / arguments.Number2;
                        break;
                }

                Console.WriteLine($"Result is {result}.");
            }
        );
    }
}
```

### Commands (verbs)
```shell
C:\> FileHelper copy --from C:\temp\test.txt --to C:\temp\test2.txt
C:\> FileHelper delete --file C:\temp\test.txt
```

```csharp
using System;
using System.IO;
using ParseTheArgs;

public class CopyFileArguments
{
    public String SourceFileName { get; set; }
    public String TargetFileName { get; set; }
    public Boolean Override { get; set; }
}

public class DeleteFileArguments
{
    public String File { get; set; }
}

public class Program
{
    public static void Main(String[] args)
    {
        var parser = new Parser();
        var setup = parser.Setup;

        var copyFileCommand = setup
            .DefaultCommand<CopyFileArguments>()
            .ExampleUsage(@"FileHelper copy --from C:\temp\test.txt --to C:\temp\test2.txt");
        
        copyFileCommand
            .Argument(a => a.SourceFileName)
            .Name("from")
            .Help("The source file to copy.")
            .IsRequired();
        
        copyFileCommand
            .Argument(a => a.TargetFileName)
            .Name("to")
            .Help("The target file to copy the source file to.")
            .IsRequired();
        
        copyFileCommand
            .Argument(a => a.Override)
            .Name("override")
            .Help("Override the target file if it already exists.");

        var deleteFileCommand = setup
            .Command<DeleteFileArguments>()
            .ExampleUsage(@"FileHelper delete --file C:\temp\test.txt");

        deleteFileCommand
            .Argument(a => a.File)
            .Help("The file to delete.")
            .IsRequired();

        var parseResult = parser.Parse(args);

        parseResult.Handle(
            (CopyFileArguments arguments) =>
            {
                File.Copy(arguments.SourceFileName, arguments.TargetFileName, arguments.Override);
            },
            (DeleteFileArguments arguments) =>
            {
                File.Delete(arguments.File);
            }
        );
    }
}
```

## More Examples
You can find more complex examples in the [Demo](ParseTheArgs.Demo) folder.

# Release History

See [Changelog](CHANGELOG.md).

# Contributors

## Main contributors
- David Liebeherr (info@rent-a-developer.de)
