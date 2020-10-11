# Changelog
All notable changes to this project will be documented here.

## [1.0.0.0]

### Initial release

## [1.0.1.0]

## Added XML Doc in NuGet package.
## Added Symbols NuGet Package.

## [1.0.2.0]

### Fix for issue [#1](https://github.com/rent-a-developer/ParseTheArgs/issues/1).

## [1.1.0.0]

### Breaking changes
#### "Argument" renamed to "Option"
Previously the term "argument" was incorrectly used (in the API and in the documentation) for what actually was an "option".
With this release that mistake is fixed.

All corresponding API parts are renamed accordingly (e.g. the ParseTheArgs.Setup.Commands.CommandSetup.Argument methods are renamed to Option).

### Short name for options removed
In previous versions each option could have a short name.
To considerably simplify the code and the API this feature has been removed.

### EnumListOptionSetup.OptionHelp renamed to EnumValueHelp
The method ParseTheArgs.Setup.Options.EnumListOptionSetup.OptionHelp was renamed to EnumValueHelp.

### EnumOptionSetup.OptionHelp renamed to EnumValueHelp
The method ParseTheArgs.Setup.Options.EnumOptionSetup.OptionHelp was renamed to EnumValueHelp.

### Command and error handler API has been revised
The API which allows to define handlers for commands and errors has been revised.

Old API:
```csharp
var parseResult = parser.Parse(args);

return parseResult.Handle(
    (Command1Arguments arguments) => {
        // Handle command 1.
        return 0;
    },
    (Command2Arguments arguments) => {
        // Handle command 2.
        return 0;
    },
    (ParseResult result) =>
    {
        // Handle errors.
        return 1;
    }
);
```

New API:
```csharp
var parseResult = parser.Parse(args);

parseResult.CommandHandler((Command1Options options) => {
    // Handle command 1.
    return 0;
});
parseResult.CommandHandler((Command2Options options) => {
    // Handle command 2.
    return 0;
});
parseResult.ErrorHandler((ParseResult result) => {
    // Handle errors.
    return 1;
});

return parseResult.Handle();
```
