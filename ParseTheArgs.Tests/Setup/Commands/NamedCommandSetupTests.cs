using System;
using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Setup.Commands;
using ParseTheArgs.Setup.Options;
using ParseTheArgs.Tests.TestData;
using ParseTheArgs.Validation;
using static FluentAssertions.FluentActions;

namespace ParseTheArgs.Tests.Setup.Commands
{
    [TestFixture]
    public class NamedCommandSetupTests
    {
        [Test(Description = "Constructor should throw an exception when the given parser is null.")]
        public void Constructor_ParserIsNull_ShouldThrowException()
        {
            Invoking(() => new NamedCommandSetup<Command1Options>(null))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "Constructor should create the correct command parser and add it to the list of command parser of the parser when no matching command parser already exists.")]
        public void Constructor_CommandParserNotExistsAlready_ShouldCreateCommandParserAndAddItToListOfCommandParser()
        {
            var parser = A.Fake<Parser>();

            var commandParsers = new List<ICommandParser>();
            A.CallTo(() => parser.CommandParsers).Returns(commandParsers);

            var setup = new NamedCommandSetup<Command1Options>(parser);

            setup.CommandParser.Should().BeOfType<CommandParser<Command1Options>>();
            setup.CommandParser.CommandName.Should().Be("command1");

            commandParsers.Should().HaveCount(1);
            commandParsers[0].Should().BeOfType<CommandParser<Command1Options>>();
        }

        [Test(Description = "Constructor should reuse an existing command parser.")]
        public void Constructor_CommandParserExistsAlready_ShouldReuseExistingCommandParser()
        {
            var parser = A.Fake<Parser>();

            var commandParsers = new List<ICommandParser>();
            commandParsers.Add(new CommandParser<Command1Options>(parser));
            
            A.CallTo(() => parser.CommandParsers).Returns(commandParsers);

            new NamedCommandSetup<Command1Options>(parser);

            commandParsers.Should().HaveCount(1);
        }

        [Test(Description = "Name should assign the given name to the command parser.")]
        public void Name_ShouldAssignNameToCommandParser()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<Command1Options>(parser);
            A.CallTo(() => parser.CommandParsers).Returns(new List<ICommandParser>{ commandParser });

            var setup = new NamedCommandSetup<Command1Options>(parser);

            setup.Name("newCommandName");

            commandParser.CommandName.Should().Be("newCommandName");
        }

        [Test(Description = "Name should return the same instance of the command setup.")]
        public void Name_ShouldReturnCommandSetup()
        {
            var parser = A.Fake<Parser>();

            var setup = new NamedCommandSetup<Command1Options>(parser);

            setup.Name("name").Should().Be(setup);
        }

        [Test(Description = "ExampleUsage should assign the given example usage to the command parser.")]
        public void ExampleUsage_ShouldAssignExampleUsageToCommandParser()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<Command1Options>(parser);
            A.CallTo(() => parser.CommandParsers).Returns(new List<ICommandParser>{ commandParser });

            var setup = new NamedCommandSetup<Command1Options>(parser);

            setup.ExampleUsage("newExampleUsage");

            commandParser.CommandExampleUsage.Should().Be("newExampleUsage");
        }

        [Test(Description = "ExampleUsage should return the same instance of the command setup.")]
        public void ExampleUsage_ShouldReturnCommandSetup()
        {
            var parser = A.Fake<Parser>();

            var setup = new NamedCommandSetup<Command1Options>(parser);

            setup.ExampleUsage("exampleUsage").Should().Be(setup);
        }

        [Test(Description = "Help should assign the given help text to the command parser.")]
        public void Help_ShouldAssignHelpTextToCommandParser()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<Command1Options>(parser);
            A.CallTo(() => parser.CommandParsers).Returns(new List<ICommandParser>{ commandParser });

            var setup = new NamedCommandSetup<Command1Options>(parser);

            setup.Help("newHelpText");

            commandParser.CommandHelp.Should().Be("newHelpText");
        }

        [Test(Description = "Help should return the same instance of the command setup.")]
        public void Help_ShouldReturnCommandSetup()
        {
            var parser = A.Fake<Parser>();

            var setup = new NamedCommandSetup<Command1Options>(parser);

            setup.Help("helpText").Should().Be(setup);
        }

        [Test(Description = "Validate should assign the given validator to the command parser.")]
        public void Validate_ShouldAssignValidatorToCommandParser()
        {
            var parser = A.Fake<Parser>();
            var validator = A.Fake<Action<CommandValidatorContext<Command1Options>>>();

            var commandParser = new CommandParser<Command1Options>(parser);
            A.CallTo(() => parser.CommandParsers).Returns(new List<ICommandParser>{ commandParser });

            var setup = new NamedCommandSetup<Command1Options>(parser);

            setup.Validate(validator);

            commandParser.Validator.Should().Be(validator);
        }

        [Test(Description = "Validate should return the same instance of the command setup.")]
        public void Validate_ShouldReturnCommandSetup()
        {
            var parser = A.Fake<Parser>();
            var validator = A.Fake<Action<CommandValidatorContext<Command1Options>>>();

            var setup = new NamedCommandSetup<Command1Options>(parser);

            setup.Validate(validator).Should().Be(setup);
        }

        [Test(Description = "Name should throw an exception when another command already has the same name.")]
        public void Name_DuplicateName_ShouldThrowException()
        {
            var parser = A.Fake<Parser>();
            var duplicateCommandParser = A.Fake<ICommandParser>();

            A.CallTo(() => parser.CommandParsers).Returns(new List<ICommandParser> { duplicateCommandParser });
            A.CallTo(() => duplicateCommandParser.CommandName).Returns("command1");

            var setup = new NamedCommandSetup<Command1Options>(parser);

            setup.Invoking(a => a.Name("command1"))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"The given command name 'command1' is already in use by another command. Please use a different name.
Parameter name: name");

        }

        [Test(Description = "Name should throw an exception when another command already has the same name.")]
        public void Option_Boolean_ShouldReturnBooleanOptionSetup()
        {
            var parser = A.Fake<Parser>();

            var setup = new NamedCommandSetup<DataTypesCommandOptions>(parser);

            setup.Option(a => a.Boolean).Should().BeOfType<BooleanOptionSetup<DataTypesCommandOptions>>();

        }
    }
}
