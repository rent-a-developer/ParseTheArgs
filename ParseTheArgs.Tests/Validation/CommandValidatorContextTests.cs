using System;
using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Parsers.Options;
using ParseTheArgs.Tests.TestData;
using ParseTheArgs.Validation;

namespace ParseTheArgs.Tests.Validation
{
    [TestFixture]
    public class CommandValidatorContextTests
    {
        [Test(Description = "GetOptionName should return the name of the option that is mapped to the property the given expression points to.")]
        public void GetOptionName_ValidExpression_ShouldReturnOptionName()
        {
            var commandParser = A.Fake<ICommandParser>();
            var parseResult = A.Fake<ParseResult>();
            var optionParser = A.Fake<OptionParser>(ob => ob.WithArgumentsForConstructor(new object[] { null, null }));

            A.CallTo(() => commandParser.OptionParsers).Returns(new List<OptionParser> {optionParser});

            var context = new CommandValidatorContext<Command1Options>(commandParser, parseResult);

            A.CallTo(() => optionParser.TargetProperty).Returns(typeof(Command1Options).GetProperty("OptionA"));
            A.CallTo(() => optionParser.OptionName).Returns("optionA");

            context.GetOptionName(a => a.OptionA).Should().BeEquivalentTo("optionA");
        }

        [Test(Description = "GetOptionName should throw an exception when the given expression is null.")]
        public void GetOptionName_Null_ShouldThrowException()
        {
            var commandParser = A.Fake<ICommandParser>();
            var parseResult = A.Fake<ParseResult>();

            var context = new CommandValidatorContext<Command1Options>(commandParser, parseResult);

            context.Invoking(a => a.GetOptionName(null))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "GetOptionName should throw an exception when the given expression doe point to a property that is not mapped to an option.")]
        public void GetOptionName_UnmappedOption_ShouldThrowException()
        {
            var commandParser = A.Fake<ICommandParser>();
            var parseResult = A.Fake<ParseResult>();

            A.CallTo(() => commandParser.OptionParsers).Returns(new List<OptionParser>());

            var context = new CommandValidatorContext<Command1Options>(commandParser, parseResult);

            context.Invoking(a => a.GetOptionName(b => b.OptionB))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"The property OptionB of the type ParseTheArgs.Tests.TestData.Command1Options is not mapped to any option.
Parameter name: optionSelector");
        }
    }
}
