using System;
using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Parsers.Options;
using ParseTheArgs.Tests.TestData;
using ParseTheArgs.Tokens;
using ParseTheArgs.Validation;

namespace ParseTheArgs.Tests.Parsers.Commands
{
    [TestFixture]
    public class CommandParserTests : BaseTestFixture
    {
        [Test(Description = "OptionParsers should be an empty list initially.")]
        public void OptionParsers_Initially_ShouldBeEmpty()
        {
            var parser = A.Fake<Parser>();
            var commandParser = new CommandParser<Command1Options>(parser);

            commandParser.OptionParsers.Should().BeEmpty();
        }

        [Test(Description = "GetOrCreateOptionParser should create a new option parser if no matching one exists already.")]
        public void GetOrCreateOptionParser_OptionParserDoesNotExist_ShouldCreateNewOptionParser()
        {
            var parser = A.Fake<Parser>();
            var commandParser = new CommandParser<Command1Options>(parser);

            var targetProperty = typeof(Command1Options).GetProperty("OptionA");

            var optionParser = A.Fake<StringOptionParser>(ob => ob.WithArgumentsForConstructor(() => new StringOptionParser(targetProperty, "optionA")));
            A.CallTo(() => optionParser.TargetProperty).Returns(targetProperty);

            A.CallTo(() => this.DependencyResolver.Resolve<StringOptionParser>(targetProperty, "optionA")).Returns(optionParser);

            commandParser.GetOrCreateOptionParser<StringOptionParser>(targetProperty).Should().Be(optionParser);

            commandParser.OptionParsers.Should().HaveCount(1);
            commandParser.OptionParsers[0].Should().Be(optionParser);

            A.CallTo(() => this.DependencyResolver.Resolve<StringOptionParser>(targetProperty, "optionA")).MustHaveHappened();
        }

        [Test(Description = "GetOrCreateOptionParser should return existing matching option parser.")]
        public void GetOrCreateOptionParser_OptionParserDoesExist_ShouldReturnExistingOptionParser()
        {
            var parser = A.Fake<Parser>();
            var commandParser = new CommandParser<Command1Options>(parser);

            var targetProperty = typeof(Command1Options).GetProperty("OptionA");

            var optionParser = A.Fake<StringOptionParser>(ob => ob.WithArgumentsForConstructor(() => new StringOptionParser(targetProperty, "optionA")));
            A.CallTo(() => optionParser.TargetProperty).Returns(targetProperty);

            A.CallTo(() => this.DependencyResolver.Resolve<StringOptionParser>(targetProperty, "optionA")).Returns(optionParser);

            commandParser.GetOrCreateOptionParser<StringOptionParser>(targetProperty).Should().Be(optionParser);
            commandParser.GetOrCreateOptionParser<StringOptionParser>(targetProperty).Should().Be(optionParser);

            commandParser.OptionParsers.Should().HaveCount(1);
            commandParser.OptionParsers[0].Should().Be(optionParser);

            A.CallTo(() => this.DependencyResolver.Resolve<StringOptionParser>(targetProperty, "optionA")).MustHaveHappenedOnceExactly();
        }

        [Test(Description = "CommandExampleUsage should be an empty string initially.")]
        public void CommandExampleUsage_Initially_ShouldBeEmpty()
        {
            var parser = A.Fake<Parser>();
            var commandParser = new CommandParser<Command1Options>(parser);

            commandParser.CommandExampleUsage.Should().BeEmpty();
        }

        [Test(Description = "CommandHelp should be an empty string initially.")]
        public void CommandHelp_Initially_ShouldBeEmpty()
        {
            var parser = A.Fake<Parser>();
            var commandParser = new CommandParser<Command1Options>(parser);

            commandParser.CommandHelp.Should().BeEmpty();
        }

        [Test(Description = "CommandName should be an empty string initially.")]
        public void CommandName_Initially_ShouldBeEmpty()
        {
            var parser = A.Fake<Parser>();
            var commandParser = new CommandParser<Command1Options>(parser);

            commandParser.CommandName.Should().BeEmpty();
        }

        [Test(Description = "IsCommandDefault should be false initially.")]
        public void IsCommandDefault_Initially_ShouldBeEmpty()
        {
            var parser = A.Fake<Parser>();
            var commandParser = new CommandParser<Command1Options>(parser);

            commandParser.IsCommandDefault.Should().BeFalse();
        }

        [Test(Description = "Validator should be null initially.")]
        public void Validator_Initially_ShouldBeEmpty()
        {
            var parser = A.Fake<Parser>();
            var commandParser = new CommandParser<Command1Options>(parser);

            commandParser.Validator.Should().BeNull();
        }

        [Test(Description = "GetHelpText should be return the help text of the command.")]
        public void GetHelpText_ShouldReturnCommandHelpText()
        {
            var parser = A.Fake<Parser>();
            var booleanOptionParser = A.Fake<BooleanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new BooleanOptionParser(typeof(DataTypesCommandOptions).GetProperty("Boolean"), "boolean")));
            var stringOptionParser = A.Fake<StringOptionParser>(ob => ob.WithArgumentsForConstructor(() => new StringOptionParser(typeof(DataTypesCommandOptions).GetProperty("String"), "string")));
            var stringListOptionParser = A.Fake<StringListOptionParser>(ob => ob.WithArgumentsForConstructor(() => new StringListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Strings"), "strings")));

            var commandParser = new CommandParser<Command1Options>(parser);
            commandParser.OptionParsers.Add(booleanOptionParser);
            commandParser.OptionParsers.Add(stringOptionParser);
            commandParser.OptionParsers.Add(stringListOptionParser);

            commandParser.CommandName = "command1";
            commandParser.CommandHelp = "Command1 help text.";
            commandParser.CommandExampleUsage = "Command1 example usage";

            A.CallTo(() => parser.ProgramName).Returns("Tool");

            A.CallTo(() => booleanOptionParser.OptionType).Returns(OptionType.ValuelessOption);
            A.CallTo(() => booleanOptionParser.GetHelpText()).Returns("boolean help.");

            A.CallTo(() => stringOptionParser.OptionType).Returns(OptionType.SingleValueOption);
            A.CallTo(() => stringOptionParser.GetHelpText()).Returns("string help.");
            A.CallTo(() => stringOptionParser.IsOptionRequired).Returns(true);

            A.CallTo(() => stringListOptionParser.OptionType).Returns(OptionType.MultiValueOption);
            A.CallTo(() => stringListOptionParser.GetHelpText()).Returns("strings help.");

            commandParser.GetHelpText().Should().Be(@"Tool command1 [--boolean] [--string value] [--strings value value ...]

Command1 help text.

Options:
--boolean                   (Optional) boolean help.
--string [value]            (Required) string help.
--strings [value value ...] (Optional) strings help.

Example usage:
Command1 example usage
");

            A.CallTo(() => parser.ProgramName).MustHaveHappened();

            A.CallTo(() => booleanOptionParser.OptionType).MustHaveHappened();
            A.CallTo(() => booleanOptionParser.GetHelpText()).MustHaveHappened();

            A.CallTo(() => stringOptionParser.OptionType).MustHaveHappened();
            A.CallTo(() => stringOptionParser.GetHelpText()).MustHaveHappened();
            A.CallTo(() => stringOptionParser.IsOptionRequired).MustHaveHappened();

            A.CallTo(() => stringListOptionParser.OptionType).MustHaveHappened();
            A.CallTo(() => stringListOptionParser.GetHelpText()).MustHaveHappened();
        }

        [Test(Description = "GetHelpText should wrap long lines in the help text according to the specified maximum line length.")]
        public void GetHelpText_LongLines_ShouldWrapLongLines()
        {
            var parser = A.Fake<Parser>();
            var booleanOptionParser = A.Fake<BooleanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new BooleanOptionParser(typeof(DataTypesCommandOptions).GetProperty("Boolean"), "boolean")));

            var commandParser = new CommandParser<Command1Options>(parser);
            commandParser.OptionParsers.Add(booleanOptionParser);

            A.CallTo(() => parser.ProgramName).Returns("Tool");
            A.CallTo(() => parser.HelpTextMaxLineLength).Returns(40);

            A.CallTo(() => booleanOptionParser.GetHelpText()).Returns("Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam.");

            commandParser.GetHelpText().Should().Be(@"Tool  [--boolean]

Options:
--boolean (Optional) Lorem ipsum dolor
                     sit amet,
                     consetetur
                     sadipscing elitr,
                     sed diam nonumy
                     eirmod tempor
                     invidunt ut labore
                     et dolore magna
                     aliquyam.
");
        }

        [Test(Description = "Parse should mark the command token as parsed when the associated command was called in the command line arguments.")]
        public void Parse_NamedCommand_ShouldMarkCommandTokenAsParsed()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<Command1Options>(parser);
            commandParser.CommandName = "command1";

            var commandToken = new CommandToken("command1");
            var tokens = new List<Token> { commandToken };
            var parseResult = new ParseResult();

            commandParser.Parse(tokens, parseResult);

            commandToken.IsParsed.Should().BeTrue();
        }

        [Test(Description = "Parse should assign its command name to the parse result when the associated command was called in the command line arguments.")]
        public void Parse_NamedCommand_ShouldSetCommandNameOfParseResult()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<Command1Options>(parser);
            commandParser.CommandName = "command1";

            var commandToken = new CommandToken("command1");
            var tokens = new List<Token> { commandToken };
            var parseResult = new ParseResult();

            commandParser.Parse(tokens, parseResult);

            parseResult.CommandName.Should().Be("command1");
        }

        [Test(Description = "Parse should assign an instance of the specified command options class to the parse result when the associated command was called in the command line arguments.")]
        public void Parse_NamedCommand_ShouldSetCommandOptionsOfParseResult()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<Command1Options>(parser);
            commandParser.CommandName = "command1";

            var commandToken = new CommandToken("command1");
            var tokens = new List<Token> { commandToken };
            var parseResult = new ParseResult();

            commandParser.Parse(tokens, parseResult);

            parseResult.CommandOptions.Should().BeOfType<Command1Options>();
        }

        [Test(Description = "Parse should assign an instance of the specified command options class to the parse result when no command was specified on the command line arguments and the parser is configured to parse the default command.")]
        public void Parse_DefaultCommand_ShouldSetCommandOptionsOfParseResult()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<Command1Options>(parser);
            commandParser.IsCommandDefault = true;

            var tokens = new List<Token>();
            var parseResult = new ParseResult();

            commandParser.Parse(tokens, parseResult);

            parseResult.CommandOptions.Should().BeOfType<Command1Options>();
        }

        [Test(Description = "Parse should call the Parse method of each registered option parser when the associated command was called in the command line arguments.")]
        public void Parse_NamedCommand_ShouldCallParseMethodOfOptionParsers()
        {
            var parser = A.Fake<Parser>();
            var booleanOptionParser = A.Fake<BooleanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new BooleanOptionParser(typeof(DataTypesCommandOptions).GetProperty("Boolean"), "boolean")));
            var stringOptionParser = A.Fake<StringOptionParser>(ob => ob.WithArgumentsForConstructor(() => new StringOptionParser(typeof(DataTypesCommandOptions).GetProperty("String"), "string")));
            var stringListOptionParser = A.Fake<StringListOptionParser>(ob => ob.WithArgumentsForConstructor(() => new StringListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Strings"), "strings")));

            var commandParser = new CommandParser<Command1Options>(parser);
            commandParser.CommandName = "command1";
            commandParser.OptionParsers.Add(booleanOptionParser);
            commandParser.OptionParsers.Add(stringOptionParser);
            commandParser.OptionParsers.Add(stringListOptionParser);

            var commandToken = new CommandToken("command1");
            var tokens = new List<Token>
            {
                commandToken,
                new OptionToken("boolean"),
                new OptionToken("string") { OptionValues = { "value" } },
                new OptionToken("strings") { OptionValues = { "value1", "value2" } }
            };
            var parseResult = new ParseResult();

            commandParser.Parse(tokens, parseResult);

            A.CallTo(() => booleanOptionParser.Parse(tokens, parseResult)).MustHaveHappened();
            A.CallTo(() => stringOptionParser.Parse(tokens, parseResult)).MustHaveHappened();
            A.CallTo(() => stringListOptionParser.Parse(tokens, parseResult)).MustHaveHappened();
        }

        [Test(Description = "Parse should call the Parse method of each registered option parser when the associated command was called in the command line arguments.")]
        public void Parse_DefaultCommand_ShouldCallParseMethodOfOptionParsers()
        {
            var parser = A.Fake<Parser>();
            var booleanOptionParser = A.Fake<BooleanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new BooleanOptionParser(typeof(DataTypesCommandOptions).GetProperty("Boolean"), "boolean")));
            var stringOptionParser = A.Fake<StringOptionParser>(ob => ob.WithArgumentsForConstructor(() => new StringOptionParser(typeof(DataTypesCommandOptions).GetProperty("String"), "string")));
            var stringListOptionParser = A.Fake<StringListOptionParser>(ob => ob.WithArgumentsForConstructor(() => new StringListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Strings"), "strings")));

            var commandParser = new CommandParser<Command1Options>(parser);
            commandParser.IsCommandDefault = true;
            commandParser.OptionParsers.Add(booleanOptionParser);
            commandParser.OptionParsers.Add(stringOptionParser);
            commandParser.OptionParsers.Add(stringListOptionParser);

            var tokens = new List<Token>
            {
                new OptionToken("boolean"),
                new OptionToken("string") { OptionValues = { "value" } },
                new OptionToken("strings") { OptionValues = { "value1", "value2" } }
            };
            var parseResult = new ParseResult();

            commandParser.Parse(tokens, parseResult);

            A.CallTo(() => booleanOptionParser.Parse(tokens, parseResult)).MustHaveHappened();
            A.CallTo(() => stringOptionParser.Parse(tokens, parseResult)).MustHaveHappened();
            A.CallTo(() => stringListOptionParser.Parse(tokens, parseResult)).MustHaveHappened();
        }

        [Test(Description = "Parse should do nothing when the command specified in the command line arguments is not the command the command parser is responsible for.")]
        public void Parse_NonMatchingNamedCommand_ShouldDoNothing()
        {
            var parser = A.Fake<Parser>();
            var booleanOptionParser = A.Fake<BooleanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new BooleanOptionParser(typeof(DataTypesCommandOptions).GetProperty("Boolean"), "boolean")));
            var stringOptionParser = A.Fake<StringOptionParser>(ob => ob.WithArgumentsForConstructor(() => new StringOptionParser(typeof(DataTypesCommandOptions).GetProperty("String"), "string")));
            var stringListOptionParser = A.Fake<StringListOptionParser>(ob => ob.WithArgumentsForConstructor(() => new StringListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Strings"), "strings")));

            var commandParser = new CommandParser<Command1Options>(parser);
            commandParser.CommandName = "command1";
            commandParser.OptionParsers.Add(booleanOptionParser);
            commandParser.OptionParsers.Add(stringOptionParser);
            commandParser.OptionParsers.Add(stringListOptionParser);

            var commandToken = new CommandToken("command2");
            var tokens = new List<Token>
            {
                commandToken,
                new OptionToken("boolean"),
                new OptionToken("string") { OptionValues = { "value" } },
                new OptionToken("strings") { OptionValues = { "value1", "value2" } }
            };
            var parseResult = new ParseResult();

            commandParser.Parse(tokens, parseResult);

            commandToken.IsParsed.Should().BeFalse();
            parseResult.CommandName.Should().BeEmpty();
            parseResult.CommandOptions.Should().BeNull();

            A.CallTo(() => booleanOptionParser.Parse(tokens, parseResult)).MustNotHaveHappened();
            A.CallTo(() => stringOptionParser.Parse(tokens, parseResult)).MustNotHaveHappened();
            A.CallTo(() => stringListOptionParser.Parse(tokens, parseResult)).MustNotHaveHappened();
        }

        [Test(Description = "Validate should do nothing when the command specified in the command line arguments is not the command the command parser is responsible for.")]
        public void Validate_NonMatchingNamedCommand_ShouldDoNothing()
        {
            var parser = A.Fake<Parser>();
            var commandValidator = A.Fake<Action<CommandValidatorContext<Command1Options>>>();

            var commandParser = new CommandParser<Command1Options>(parser);
            commandParser.CommandName = "command1";
            commandParser.Validator = commandValidator;

            var tokens = new List<Token>
            {
                new CommandToken("command2")
            };
            var parseResult = new ParseResult();

            commandParser.Parse(tokens, parseResult);

            A.CallTo(() => commandValidator(A<CommandValidatorContext<Command1Options>>.Ignored)).MustNotHaveHappened();
        }

        [Test(Description = "Validate should call the specified command validator when the command specified in the command line arguments is not the command the command parser is responsible for.")]
        public void Validate_NamedCommand_ShouldCallValidator()
        {
            var parser = A.Fake<Parser>();
            var commandValidator = A.Fake<Action<CommandValidatorContext<Command1Options>>>();

            var commandParser = new CommandParser<Command1Options>(parser);
            commandParser.CommandName = "command1";
            commandParser.Validator = commandValidator;

            var tokens = new List<Token>
            {
                new CommandToken("command1")
            };
            var parseResult = new ParseResult();

            commandParser.Validate(tokens, parseResult);

            A.CallTo(() => commandValidator(A<CommandValidatorContext<Command1Options>>.That.Matches(a => a.ParseResult == parseResult))).MustHaveHappened();
        }
    }
}
