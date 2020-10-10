using System;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using ParseTheArgs.Errors;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Tests.TestData;
using ParseTheArgs.Validation;
using static FluentAssertions.FluentActions;

namespace ParseTheArgs.Tests.Validation
{
    [TestFixture]
    public class CommandValidatorContextTests
    {
        [Test(Description = "Constructor should throw an exception when the given command parser is null.")]
        public void Constructor_CommandParserIsNull_ShouldThrowException()
        {
            var parseResult = A.Fake<ParseResult>();

            Invoking(() => new CommandValidatorContext<Command1Options>(null, parseResult))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "Constructor should throw an exception when the given parse result is null.")]
        public void Constructor_ParseResultIsNull_ShouldThrowException()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<Command1Options>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<Command1Options>(parser)));

            Invoking(() => new CommandValidatorContext<Command1Options>(commandParser, null))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "GetOptionName should return the name of the option that is mapped to the property the given expression points to.")]
        public void GetOptionName_ValidExpression_ShouldReturnOptionName()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<Command1Options>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<Command1Options>(parser)));
            var parseResult = A.Fake<ParseResult>();

            var targetProperty = typeof(Command1Options).GetProperty("OptionA");

            String optionName;
            A.CallTo(() => commandParser.TryGetOptionName(targetProperty, out optionName))
                .Returns(true)
                .AssignsOutAndRefParameters("optionA");

            var context = new CommandValidatorContext<Command1Options>(commandParser, parseResult);

            context.GetOptionName(a => a.OptionA).Should().BeEquivalentTo("optionA");
        }

        [Test(Description = "GetOptionName should throw an exception when the given expression is null.")]
        public void GetOptionName_Null_ShouldThrowException()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<Command1Options>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<Command1Options>(parser)));
            var parseResult = A.Fake<ParseResult>();

            var context = new CommandValidatorContext<Command1Options>(commandParser, parseResult);

            context.Invoking(a => a.GetOptionName(null))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "GetOptionName should throw an exception when the given expression doe point to a property that is not mapped to an option.")]
        public void GetOptionName_UnmappedOption_ShouldThrowException()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<Command1Options>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<Command1Options>(parser)));
            var parseResult = A.Fake<ParseResult>();

            var targetProperty = typeof(Command1Options).GetProperty("OptionB");

            String optionName;
            A.CallTo(() => commandParser.TryGetOptionName(targetProperty, out optionName))
                .Returns(false);

            var context = new CommandValidatorContext<Command1Options>(commandParser, parseResult);

            context.Invoking(a => a.GetOptionName(b => b.OptionB))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"The property OptionB of the type ParseTheArgs.Tests.TestData.Command1Options is not mapped to any option.
Parameter name: optionSelector");
        }

        [Test(Description = "CommandOptions should return the command options from the parse result.")]
        public void CommandOptions_ShouldReturnCommandOptionsOfParseResult()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<Command1Options>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<Command1Options>(parser)));
            var parseResult = A.Fake<ParseResult>();
            var commandOptions = A.Fake<Command1Options>();

            var context = new CommandValidatorContext<Command1Options>(commandParser, parseResult);

            A.CallTo(() => parseResult.CommandOptions).Returns(commandOptions);

            context.CommandOptions.Should().Be(commandOptions);

            A.CallTo(() => parseResult.CommandOptions).MustHaveHappened();
        }

        [Test(Description = "ParseResult should return the parse result that was assigned via the constructor.")]
        public void ParseResult_ShouldReturnParseResultAssignedViaConstructor()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<Command1Options>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<Command1Options>(parser)));
            var parseResult = A.Fake<ParseResult>();

            var context = new CommandValidatorContext<Command1Options>(commandParser, parseResult);

            context.ParseResult.Should().Be(parseResult);
        }

        [Test(Description = "AddError should add the given error to the parse result.")]
        public void AddError_ShouldAddErrorToParseResult()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<Command1Options>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<Command1Options>(parser)));
            var parseResult = A.Fake<ParseResult>();

            var context = new CommandValidatorContext<Command1Options>(commandParser, parseResult);

            var missingCommandError = new MissingCommandError();
            context.AddError(missingCommandError);

            A.CallTo(() => parseResult.AddError(missingCommandError)).MustHaveHappened();
        }
    }
}
