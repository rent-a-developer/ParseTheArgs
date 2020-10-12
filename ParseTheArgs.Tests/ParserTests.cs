using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using ParseTheArgs.Errors;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Tests.TestData;
using ParseTheArgs.Tokens;

namespace ParseTheArgs.Tests
{
    [TestFixture]
    public class ParserTests : BaseTestFixture
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            // We fix the current culture to en-US so that parsing of values (e.g. DateTime values) is done in a deterministic fashion.
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }

        #endregion

        [Test(Description = "Banner should return an empty string initially.")]
        public void Banner_Initially_ShouldReturnEmptyString()
        {
            var parser = new Parser();

            parser.Banner.Should().BeEmpty();
        }

        [Test(Description = "CanCommandParserUseCommandName should return true when there is no other command parser that already uses the given command name.")]
        public void CanCommandParserUseCommandName_CommandNameIsAvailable_ShouldReturnTrue()
        {
            var parser = new Parser();
            var commandParser1 = A.Fake<ICommandParser>();

            parser.CommandParsers.Add(commandParser1);

            parser.CanCommandParserUseCommandName(commandParser1, "command1").Should().BeTrue();
        }

        [Test(Description = "CanCommandParserUseCommandName should return false when there is another command parser that already uses the given command name.")]
        public void CanCommandParserUseCommandName_CommandNameIsNotAvailable_ShouldReturnFalse()
        {
            var parser = new Parser();
            var commandParser1 = A.Fake<ICommandParser>();
            var commandParser2 = A.Fake<ICommandParser>();

            parser.CommandParsers.Add(commandParser1);
            parser.CommandParsers.Add(commandParser2);

            A.CallTo(() => commandParser1.CommandName).Returns("command1");

            parser.CanCommandParserUseCommandName(commandParser2, "command1").Should().BeFalse();
        }

        [Test(Description = "ErrorTextWriter should initially return an null when no console is present.")]
        public void ErrorTextWriter_Initially_ConsoleNotPresent_ShouldReturnNull()
        {
            var consoleHelper = A.Fake<ConsoleHelper>();

            A.CallTo(() => this.DependencyResolver.Resolve<ConsoleHelper>()).Returns(consoleHelper);
            A.CallTo(() => consoleHelper.IsConsolePresent()).Returns(false);

            var parser = new Parser();

            parser.ErrorTextWriter.Should().BeNull();
        }

        [Test(Description = "ErrorTextWriter should initially return Console.Error when a console is present.")]
        public void ErrorTextWriter_Initially_ConsolePresent_ShouldReturnConsoleError()
        {
            var consoleHelper = A.Fake<ConsoleHelper>();
            var consoleErrorWriter = A.Fake<TextWriter>();

            A.CallTo(() => this.DependencyResolver.Resolve<ConsoleHelper>()).Returns(consoleHelper);
            A.CallTo(() => consoleHelper.IsConsolePresent()).Returns(true);
            A.CallTo(() => consoleHelper.GetConsoleErrorWriter()).Returns(consoleErrorWriter);

            var parser = new Parser();

            parser.ErrorTextWriter.Should().Be(consoleErrorWriter);
        }

        [Test(Description = "GetCommandHelpText should return the help text of the command parser that handles the given command.")]
        public void GetCommandHelpText_CommandParserDoesExist_ShouldReturnHelpTextFromCommandParser()
        {
            var parser = new Parser();
            parser.ProgramName = "Test";

            var commandParser = A.Fake<ICommandParser>();
            parser.CommandParsers.Add(commandParser);

            A.CallTo(() => commandParser.CommandName).Returns("command1");
            A.CallTo(() => commandParser.GetHelpText()).Returns("Command1 help text");

            parser.GetCommandHelpText("command1", false).Should().Be("Command1 help text");
        }

        [Test(Description = "GetCommandHelpText should return an error message when no command parser for the given command exist.")]
        public void GetCommandHelpText_CommandParserDoesNotExist_ShouldReturnErrorMessage()
        {
            var parser = new Parser();
            parser.ProgramName = "Test";

            parser.GetCommandHelpText("command1", false).Should().Be(@"The command 'command1' is unknown.
Try the following command to get a list of valid commands:
Test help
");
        }

        [Test(Description = "GetCommandHelpText should include the banner text when includeBanner is true.")]
        public void GetCommandHelpText_IncludeBannerText_ShouldReturnHelpTextFromCommandParser()
        {
            var parser = new Parser();
            parser.ProgramName = "Test";
            parser.Banner = "Banner text";

            var commandParser = A.Fake<ICommandParser>();
            parser.CommandParsers.Add(commandParser);

            A.CallTo(() => commandParser.CommandName).Returns("command1");
            A.CallTo(() => commandParser.GetHelpText()).Returns("Command1 help text");

            parser.GetCommandHelpText("command1", true).Should().Be(@"Banner text

Command1 help text");
        }

        [Test(Description = "GetErrorsText should return the error messages of the errors when errors are present.")]
        public void GetErrorsText_ErrorsPresent_ShouldReturnMessagesOfErrors()
        {
            var parser = new Parser();
            parser.ProgramName = "Test";

            var parseResult = A.Fake<ParseResult>();
            var error1 = A.Fake<IParseError>();
            var error2 = A.Fake<IParseError>();

            A.CallTo(() => error1.GetErrorMessage()).Returns("Error1 message");
            A.CallTo(() => error2.GetErrorMessage()).Returns("Error2 message");

            var errors = new List<IParseError> {error1, error2};
            A.CallTo(() => parseResult.CommandName).Returns("command1");
            A.CallTo(() => parseResult.HasErrors).Returns(true);
            A.CallTo(() => parseResult.Errors).Returns(errors.AsReadOnly());

            parser.GetErrorsText(parseResult, false).Should().Be(@"Invalid or missing option(s):
- Error1 message
- Error2 message

Try the following command to get help:
Test help command1
");
        }

        [Test(Description = "GetErrorsText should include the banner text when includeBanner is true.")]
        public void GetErrorsText_IncludeBanner_ShouldIncludeBannerText()
        {
            var parser = new Parser();
            parser.ProgramName = "Test";
            parser.Banner = "Banner text";

            var parseResult = A.Fake<ParseResult>();
            var error1 = A.Fake<IParseError>();
            var error2 = A.Fake<IParseError>();

            A.CallTo(() => error1.GetErrorMessage()).Returns("Error1 message");
            A.CallTo(() => error2.GetErrorMessage()).Returns("Error2 message");

            var errors = new List<IParseError> {error1, error2};
            A.CallTo(() => parseResult.HasErrors).Returns(true);
            A.CallTo(() => parseResult.Errors).Returns(errors.AsReadOnly());

            parser.GetErrorsText(parseResult, true).Should().Be(@"Banner text

Invalid or missing option(s):
- Error1 message
- Error2 message

Try the following command to get help:
Test help
");
        }

        [Test(Description = "GetErrorsText should return an empty string when no errors are present.")]
        public void GetErrorsText_NoErrorsPresent_ShouldReturnEmptyString()
        {
            var parser = new Parser();
            parser.ProgramName = "Test";

            var parseResult = A.Fake<ParseResult>();

            A.CallTo(() => parseResult.HasErrors).Returns(false);

            parser.GetErrorsText(parseResult).Should().BeEmpty();
        }

        [Test(Description = "GetHelpText should include the banner text when includeBanner is true.")]
        public void GetHelpText_IncludeBanner_ShouldIncludeBannerText()
        {
            var parser = new Parser();
            parser.ProgramName = "Test";
            parser.Banner = "Banner text";

            var defaultCommandParser = A.Fake<ICommandParser>();
            A.CallTo(() => defaultCommandParser.IsCommandDefault).Returns(true);
            A.CallTo(() => defaultCommandParser.CommandName).Returns(String.Empty);
            A.CallTo(() => defaultCommandParser.GetHelpText()).Returns("Default command help text");
            parser.CommandParsers.Add(defaultCommandParser);

            parser.GetHelpText(true).Should().Be(@"Banner text

Default command help text
Test help
Prints this help screen.
");
        }

        [Test(Description = "GetHelpText should return the help texts from the command parsers.")]
        public void GetHelpText_ShouldReturnHelpTextsFromCommandParsers()
        {
            var parser = new Parser();
            parser.ProgramName = "Test";

            var defaultCommandParser = A.Fake<ICommandParser>();
            A.CallTo(() => defaultCommandParser.IsCommandDefault).Returns(true);
            A.CallTo(() => defaultCommandParser.CommandName).Returns(String.Empty);
            A.CallTo(() => defaultCommandParser.GetHelpText()).Returns("Default command help text");
            parser.CommandParsers.Add(defaultCommandParser);

            var namedCommandParser1 = A.Fake<ICommandParser>();
            A.CallTo(() => namedCommandParser1.IsCommandDefault).Returns(false);
            A.CallTo(() => namedCommandParser1.CommandName).Returns("command1");
            A.CallTo(() => namedCommandParser1.CommandHelp).Returns("Command1 help text");
            parser.CommandParsers.Add(namedCommandParser1);

            var namedCommandParser2 = A.Fake<ICommandParser>();
            A.CallTo(() => namedCommandParser2.IsCommandDefault).Returns(false);
            A.CallTo(() => namedCommandParser2.CommandName).Returns("command2");
            A.CallTo(() => namedCommandParser2.CommandHelp).Returns("Command2 help text");
            parser.CommandParsers.Add(namedCommandParser2);

            parser.GetHelpText(false).Should().Be(@"Default command help text
Test <command> [options]

Commands:
command1	Command1 help text
command2	Command2 help text

Test help
Prints this help screen.

Test help <command>
Prints the help screen for the specified command.
");
        }

        [Test(Description = "GetOrCreateCommandParser should return the existing command parser when a matching command parser already exists.")]
        public void GetOrCreateCommandParser_CommandParserDoesExist_ShouldReturnExistingCommandParser()
        {
            var parser = new Parser();
            var defaultCommandParser = A.Fake<CommandParser<Command1Options>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<Command1Options>(parser)));
            var namedCommandParser = A.Fake<CommandParser<Command2Options>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<Command2Options>(parser)));

            A.CallTo(() => this.DependencyResolver.Resolve<CommandParser<Command1Options>>(parser)).Returns(defaultCommandParser);
            A.CallTo(() => this.DependencyResolver.Resolve<CommandParser<Command2Options>>(parser)).Returns(namedCommandParser);

            parser.GetOrCreateCommandParser<Command1Options>(null);
            parser.GetOrCreateCommandParser<Command2Options>("command2");

            parser.GetOrCreateCommandParser<Command1Options>(null).Should().Be(defaultCommandParser);
            parser.GetOrCreateCommandParser<Command2Options>("command2").Should().Be(namedCommandParser);

            A.CallTo(() => this.DependencyResolver.Resolve<CommandParser<Command1Options>>(parser)).MustHaveHappenedOnceExactly();
            A.CallTo(() => this.DependencyResolver.Resolve<CommandParser<Command2Options>>(parser)).MustHaveHappenedOnceExactly();
        }

        [Test(Description = "GetOrCreateCommandParser should create a new command parser when no matching command parser already exists.")]
        public void GetOrCreateCommandParser_CommandParserDoesNotExist_ShouldCreateNewCommandParser()
        {
            var parser = new Parser();
            var defaultCommandParser = A.Fake<CommandParser<Command1Options>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<Command1Options>(parser)));
            var namedCommandParser = A.Fake<CommandParser<Command2Options>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<Command2Options>(parser)));

            A.CallTo(() => this.DependencyResolver.Resolve<CommandParser<Command1Options>>(parser)).Returns(defaultCommandParser);

            parser.GetOrCreateCommandParser<Command1Options>(null).Should().Be(defaultCommandParser);

            defaultCommandParser.IsCommandDefault.Should().BeTrue();
            defaultCommandParser.CommandName.Should().BeEmpty();

            A.CallTo(() => this.DependencyResolver.Resolve<CommandParser<Command1Options>>(parser)).MustHaveHappened();


            A.CallTo(() => this.DependencyResolver.Resolve<CommandParser<Command2Options>>(parser)).Returns(namedCommandParser);

            parser.GetOrCreateCommandParser<Command2Options>("command2").Should().Be(namedCommandParser);

            namedCommandParser.IsCommandDefault.Should().BeFalse();
            namedCommandParser.CommandName.Should().Be("command2");

            A.CallTo(() => this.DependencyResolver.Resolve<CommandParser<Command2Options>>(parser)).MustHaveHappened();
        }

        [Test(Description = "HelpTextMaxLineLength should initially return Int32.MaxValue when no console is present.")]
        public void HelpTextMaxLineLength_Initially_ConsoleNotPresent_ShouldReturnInt32MaxValue()
        {
            var consoleHelper = A.Fake<ConsoleHelper>();

            A.CallTo(() => this.DependencyResolver.Resolve<ConsoleHelper>()).Returns(consoleHelper);
            A.CallTo(() => consoleHelper.IsConsolePresent()).Returns(false);

            var parser = new Parser();

            parser.HelpTextMaxLineLength.Should().Be(Int32.MaxValue);
        }

        [Test(Description = "HelpTextMaxLineLength should initially return Console.WindowWidth when a console is present.")]
        public void HelpTextMaxLineLength_Initially_ConsolePresent_ShouldReturnConsoleWindowWidth()
        {
            var consoleHelper = A.Fake<ConsoleHelper>();

            A.CallTo(() => this.DependencyResolver.Resolve<ConsoleHelper>()).Returns(consoleHelper);
            A.CallTo(() => consoleHelper.IsConsolePresent()).Returns(true);
            A.CallTo(() => consoleHelper.GetConsoleWindowWidth()).Returns(123);

            var parser = new Parser();

            parser.HelpTextMaxLineLength.Should().Be(123);
        }

        [Test(Description = "HelpTextWriter should initially return an null when no console is present.")]
        public void HelpTextWriter_Initially_ConsoleNotPresent_ShouldReturnNull()
        {
            var consoleHelper = A.Fake<ConsoleHelper>();

            A.CallTo(() => this.DependencyResolver.Resolve<ConsoleHelper>()).Returns(consoleHelper);
            A.CallTo(() => consoleHelper.IsConsolePresent()).Returns(false);

            var parser = new Parser();

            parser.HelpTextWriter.Should().BeNull();
        }

        [Test(Description = "HelpTextWriter should initially return Console.Out when a console is present.")]
        public void HelpTextWriter_Initially_ConsolePresent_ShouldReturnConsoleOut()
        {
            var consoleHelper = A.Fake<ConsoleHelper>();
            var consoleOutWriter = A.Fake<TextWriter>();

            A.CallTo(() => this.DependencyResolver.Resolve<ConsoleHelper>()).Returns(consoleHelper);
            A.CallTo(() => consoleHelper.IsConsolePresent()).Returns(true);
            A.CallTo(() => consoleHelper.GetConsoleOutWriter()).Returns(consoleOutWriter);

            var parser = new Parser();

            parser.HelpTextWriter.Should().Be(consoleOutWriter);
        }

        [Test(Description = "IgnoreUnknownOptions should initially return false.")]
        public void IgnoreUnknownOptions_Initially_ShouldReturnFalse()
        {
            var parser = new Parser();

            parser.IgnoreUnknownOptions.Should().BeFalse();
        }

        [Test(Description = "Parse should write the command help screen to the help text writer when command help was called.")]
        public void Parse_CommandHelpCalled_ShouldWriteCommandHelpScreenToHelpTextWriter()
        {
            var parser = A.Fake<Parser>(ob => ob.CallsBaseMethods());
            var helpTextWriter = A.Fake<TextWriter>();

            parser.HelpTextWriter = helpTextWriter;

            A.CallTo(() => parser.GetCommandHelpText("command1", true)).Returns("Command1 help screen");

            parser.Parse(new String[] {"help", "command1"});

            A.CallTo(() => parser.GetCommandHelpText("command1", true)).MustHaveHappened();
            A.CallTo(() => helpTextWriter.Write("Command1 help screen")).MustHaveHappened();
        }

        [Test(Description = "Parse should print the error screen when a command parser has added an error to the parse result.")]
        public void Parse_CommandParserAddedError_ShouldPrintErrors()
        {
            var parser = A.Fake<Parser>(ob => ob.CallsBaseMethods());
            var tokenizer = A.Fake<CommandLineArgumentsTokenizer>();
            var commandParser = A.Fake<ICommandParser>();
            var error = A.Fake<IParseError>();
            var errorTextWriter = A.Fake<TextWriter>();

            parser.ErrorTextWriter = errorTextWriter;
            parser.CommandParsers.Add(commandParser);

            A.CallTo(() => this.DependencyResolver.Resolve<CommandLineArgumentsTokenizer>()).Returns(tokenizer);

            var tokens = new List<Token> {new CommandToken("command1")};
            A.CallTo(() => tokenizer.Tokenize("command1")).Returns(tokens);

            A.CallTo(() => commandParser.Parse(tokens, A<ParseResult>.Ignored)).Invokes((List<Token> tokens2, ParseResult parseResult2) => { parseResult2.AddError(error); });

            A.CallTo(() => parser.GetErrorsText(A<ParseResult>.Ignored, true)).Returns("Error text");

            parser.Parse(new String[] {"command1"});

            A.CallTo(() => errorTextWriter.Write("Error text")).MustHaveHappened();
        }

        [Test(Description = "Parse should return a DuplicateOptionError when an option was specified more than once.")]
        public void Parse_DuplicateOption_ShouldReturnError()
        {
            var parser = A.Fake<Parser>(ob => ob.CallsBaseMethods());
            var errorTextWriter = A.Fake<TextWriter>();
            var tokenizer = A.Fake<CommandLineArgumentsTokenizer>();

            parser.ErrorTextWriter = errorTextWriter;

            A.CallTo(() => this.DependencyResolver.Resolve<CommandLineArgumentsTokenizer>()).Returns(tokenizer);
            A.CallTo(() => tokenizer.Tokenize("command1", "--optionA", "optionAValue", "--optionA")).Returns(new List<Token> {new CommandToken("command1"), new OptionToken("optionA") {OptionValues = {"optionAValue"}}, new OptionToken("optionA")});
            A.CallTo(() => parser.GetErrorsText(A<ParseResult>.Ignored, true)).Returns("Error text");

            var parseResult = parser.Parse(new String[] {"command1", "--optionA", "optionAValue", "--optionA"});

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Count.Should().Be(1);
            parseResult.Errors[0].Should().BeOfType<DuplicateOptionError>();
            parseResult.Errors[0].GetErrorMessage().Should().Be("The option --optionA is used more than once. Please only use each option once.");

            A.CallTo(() => errorTextWriter.Write("Error text")).MustHaveHappened();
        }

        [Test(Description = "Parse should set ParseResult.IsHelpCalled to true when the help was called.")]
        public void Parse_HelpCalled_ShouldSetIsHelpCalledToTrue()
        {
            var parser = new Parser();

            var parseResult1 = parser.Parse(new String[] { });
            parseResult1.IsHelpCalled.Should().BeTrue();

            var parseResult2 = parser.Parse(new String[] {"help"});
            parseResult2.IsHelpCalled.Should().BeTrue();

            var parseResult3 = parser.Parse(new String[] {"help", "command1"});
            parseResult3.IsHelpCalled.Should().BeTrue();
        }

        [Test(Description = "Parse should write the help screen to the help text writer when help was called.")]
        public void Parse_HelpCalled_ShouldWriteHelpScreenToHelpTextWriter()
        {
            var parser = A.Fake<Parser>(ob => ob.CallsBaseMethods());
            var helpTextWriter = A.Fake<TextWriter>();

            parser.HelpTextWriter = helpTextWriter;

            A.CallTo(() => parser.GetHelpText(true)).Returns("Help screen");

            parser.Parse(new String[] { });

            A.CallTo(() => parser.GetHelpText(true)).MustHaveHappened();
            A.CallTo(() => helpTextWriter.Write("Help screen")).MustHaveHappened();
        }

        [Test(Description = "Parse should ignore unknown options when the IgnoreUnknownOptions is enabled.")]
        public void Parse_IgnoreUnknownOptionsEnabled_ShouldIgnoreUnknownOptions()
        {
            var parser = A.Fake<Parser>(ob => ob.CallsBaseMethods());
            var errorTextWriter = A.Fake<TextWriter>();
            var tokenizer = A.Fake<CommandLineArgumentsTokenizer>();
            var commandParser = A.Fake<ICommandParser>();

            parser.IgnoreUnknownOptions = true;
            parser.ErrorTextWriter = errorTextWriter;
            parser.CommandParsers.Add(commandParser);

            A.CallTo(() => this.DependencyResolver.Resolve<CommandLineArgumentsTokenizer>()).Returns(tokenizer);

            var tokens = new List<Token> {new CommandToken("command1"), new OptionToken("unknownOption")};
            tokens[0].IsParsed = true;

            A.CallTo(() => tokenizer.Tokenize("command1", "--unknownOption")).Returns(tokens);
            A.CallTo(() => parser.GetErrorsText(A<ParseResult>.Ignored, true)).Returns("Error text");

            var parseResult = parser.Parse(new String[] {"command1", "--unknownOption"});

            parseResult.HasErrors.Should().BeFalse();
            parseResult.Errors.Count.Should().Be(0);

            A.CallTo(() => errorTextWriter.Write(A<String>.Ignored)).MustNotHaveHappened();
        }

        [Test(Description = "Parse should return a MissingCommandError when there is no command specified and the default command has not set up.")]
        public void Parse_MissingCommand_ShouldReturnError()
        {
            var parser = A.Fake<Parser>(ob => ob.CallsBaseMethods());
            var errorTextWriter = A.Fake<TextWriter>();
            var tokenizer = A.Fake<CommandLineArgumentsTokenizer>();

            parser.ErrorTextWriter = errorTextWriter;

            A.CallTo(() => this.DependencyResolver.Resolve<CommandLineArgumentsTokenizer>()).Returns(tokenizer);
            A.CallTo(() => tokenizer.Tokenize("--optionA")).Returns(new List<Token> {new OptionToken("optionA")});
            A.CallTo(() => parser.GetErrorsText(A<ParseResult>.Ignored, true)).Returns("Error text");

            var parseResult = parser.Parse(new String[] {"--optionA"});

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Count.Should().Be(1);
            parseResult.Errors[0].Should().BeOfType<MissingCommandError>();
            parseResult.Errors[0].GetErrorMessage().Should().Be("No command was specified.");

            A.CallTo(() => errorTextWriter.Write("Error text")).MustHaveHappened();
        }

        [Test(Description = "Parse should return a MoreThanOneCommandError when there is more than one command specified.")]
        public void Parse_MoreThanOneCommand_ShouldReturnError()
        {
            var parser = A.Fake<Parser>(ob => ob.CallsBaseMethods());
            var errorTextWriter = A.Fake<TextWriter>();
            var tokenizer = A.Fake<CommandLineArgumentsTokenizer>();

            parser.ErrorTextWriter = errorTextWriter;

            A.CallTo(() => this.DependencyResolver.Resolve<CommandLineArgumentsTokenizer>()).Returns(tokenizer);
            A.CallTo(() => tokenizer.Tokenize("command1", "command2")).Returns(new List<Token> {new CommandToken("command1"), new CommandToken("command2")});
            A.CallTo(() => parser.GetErrorsText(A<ParseResult>.Ignored, true)).Returns("Error text");

            var parseResult = parser.Parse(new String[] {"command1", "command2"});

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Count.Should().Be(1);
            parseResult.Errors[0].Should().BeOfType<MoreThanOneCommandError>();
            parseResult.Errors[0].GetErrorMessage().Should().Be("More than one command was specified. Please only specify one command.");

            A.CallTo(() => errorTextWriter.Write("Error text")).MustHaveHappened();
        }

        [Test(Description = "Parse should let all command parsers parse and validate the command line.")]
        public void Parse_ShouldLetAllCommandParsersParseAndValidateTheCommandLine()
        {
            var parser = new Parser();
            var tokenizer = A.Fake<CommandLineArgumentsTokenizer>();

            var commandParser1 = A.Fake<ICommandParser>();
            var commandParser2 = A.Fake<ICommandParser>();

            parser.CommandParsers.Add(commandParser1);
            parser.CommandParsers.Add(commandParser2);

            A.CallTo(() => this.DependencyResolver.Resolve<CommandLineArgumentsTokenizer>()).Returns(tokenizer);
            var tokens = new List<Token> {new CommandToken("command1")};
            A.CallTo(() => tokenizer.Tokenize("command1")).Returns(tokens);

            var parseResult = parser.Parse(new String[] {"command1"});

            A.CallTo(() => commandParser1.Parse(tokens, parseResult)).MustHaveHappened();
            A.CallTo(() => commandParser1.Validate(tokens, parseResult)).MustHaveHappened();

            A.CallTo(() => commandParser2.Parse(tokens, parseResult)).MustHaveHappened();
            A.CallTo(() => commandParser2.Validate(tokens, parseResult)).MustHaveHappened();
        }

        [Test(Description = "Parse should return a UnknownCommandError when an unknown command was specified.")]
        public void Parse_UnknownCommand_ShouldReturnError()
        {
            var parser = A.Fake<Parser>(ob => ob.CallsBaseMethods());
            var errorTextWriter = A.Fake<TextWriter>();
            var tokenizer = A.Fake<CommandLineArgumentsTokenizer>();

            parser.ErrorTextWriter = errorTextWriter;

            A.CallTo(() => this.DependencyResolver.Resolve<CommandLineArgumentsTokenizer>()).Returns(tokenizer);
            A.CallTo(() => tokenizer.Tokenize("unknownCommand")).Returns(new List<Token> {new CommandToken("unknownCommand")});
            A.CallTo(() => parser.GetErrorsText(A<ParseResult>.Ignored, true)).Returns("Error text");

            var parseResult = parser.Parse(new String[] {"unknownCommand"});

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Count.Should().Be(1);
            parseResult.Errors[0].Should().BeOfType<UnknownCommandError>();
            parseResult.Errors[0].GetErrorMessage().Should().Be("The command 'unknownCommand' is unknown.");

            A.CallTo(() => errorTextWriter.Write("Error text")).MustHaveHappened();
        }

        [Test(Description = "Parse should return a UnknownOptionError when an unknown option was specified.")]
        public void Parse_UnknownOption_ShouldReturnError()
        {
            var parser = A.Fake<Parser>(ob => ob.CallsBaseMethods());
            var errorTextWriter = A.Fake<TextWriter>();
            var tokenizer = A.Fake<CommandLineArgumentsTokenizer>();
            var commandParser = A.Fake<ICommandParser>();

            parser.ErrorTextWriter = errorTextWriter;
            parser.CommandParsers.Add(commandParser);

            A.CallTo(() => this.DependencyResolver.Resolve<CommandLineArgumentsTokenizer>()).Returns(tokenizer);

            var tokens = new List<Token> {new CommandToken("command1"), new OptionToken("unknownOption")};
            tokens[0].IsParsed = true;

            A.CallTo(() => tokenizer.Tokenize("command1", "--unknownOption")).Returns(tokens);
            A.CallTo(() => parser.GetErrorsText(A<ParseResult>.Ignored, true)).Returns("Error text");

            var parseResult = parser.Parse(new String[] {"command1", "--unknownOption"});

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Count.Should().Be(1);
            parseResult.Errors[0].Should().BeOfType<UnknownOptionError>();
            parseResult.Errors[0].GetErrorMessage().Should().Be("The option --unknownOption is unknown.");

            A.CallTo(() => errorTextWriter.Write("Error text")).MustHaveHappened();
        }

        [Test(Description = "ProgramName should initially return the name of the current process.")]
        public void ProgramName_Initially_ShouldReturnNameOfProcess()
        {
            var parser = new Parser();

            parser.ProgramName.Should().Be(Process.GetCurrentProcess().ProcessName);
        }

        [Test(Description = "Setup should return the parser setup.")]
        public void Setup_ShouldReturnParserSetup()
        {
            var parser = new Parser();

            parser.Setup.Should().NotBeNull();
        }
    }
}