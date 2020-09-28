using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using ParseTheArgs.Parsers.Arguments;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Tests.TestData;
using ParseTheArgs.Validation;

namespace ParseTheArgs.Tests.Validation
{
    [TestFixture]
    public class CommandValidatorContextTests
    {
        [Test(Description = "GetArgumentName should return the name of the argument that is mapped to the property the given expression points to.")]
        public void GetArgumentName_ValidExpression_ShouldReturnArgumentName()
        {
            var commandParserMock = new Mock<ICommandParser>();
            var parseResultMock = new Mock<ParseResult>();
            var argumentParserMock = new Mock<ArgumentParser>(null, null);

            commandParserMock.Setup(a => a.ArgumentParsers).Returns(new List<ArgumentParser> { argumentParserMock.Object });

            var context = new CommandValidatorContext<Command1Arguments>(commandParserMock.Object, parseResultMock.Object);

            argumentParserMock.Setup(a => a.TargetProperty).Returns(typeof(Command1Arguments).GetProperty("ArgumentA"));
            argumentParserMock.Setup(a => a.ArgumentName).Returns("argumentA");

            context.GetArgumentName(a => a.ArgumentA).Should().BeEquivalentTo("argumentA");
        }

        [Test(Description = "GetArgumentName should throw an exception when the given expression is null.")]
        public void GetArgumentName_Null_ShouldThrowException()
        {
            var commandParserMock = new Mock<ICommandParser>();
            var parseResultMock = new Mock<ParseResult>();

            var context = new CommandValidatorContext<Command1Arguments>(commandParserMock.Object, parseResultMock.Object);

            context.Invoking(a => a.GetArgumentName(null))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "GetArgumentName should throw an exception when the given expression doe point to a property that is not mapped to an argument.")]
        public void GetArgumentName_UnmappedArgument_ShouldThrowException()
        {
            var commandParserMock = new Mock<ICommandParser>();
            var parseResultMock = new Mock<ParseResult>();

            commandParserMock.Setup(a => a.ArgumentParsers).Returns(new List<ArgumentParser>());

            var context = new CommandValidatorContext<Command1Arguments>(commandParserMock.Object, parseResultMock.Object);

            context.Invoking(a => a.GetArgumentName(b => b.ArgumentB))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"The property ArgumentB of the type ParseTheArgs.Tests.TestData.Command1Arguments is not mapped to any argument.
Parameter name: argumentSelector");
        }
    }
}
