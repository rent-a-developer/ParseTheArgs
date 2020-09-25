using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using ParseTheArgs.Parsers.Arguments;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Tests.TestData;

namespace ParseTheArgs.Tests.Parsers.Commands
{
    [TestFixture]
    public class CommandValidatorContextTests
    {
        [Test]
        public void TestGetArgumentName()
        {
            var commandParserMock = new Mock<ICommandParser>();
            var parseResultMock = new Mock<ParseResult>();
            var argumentParserMock = new Mock<IArgumentParser>();

            commandParserMock.Setup(a => a.ArgumentParsers).Returns(new List<IArgumentParser> { argumentParserMock.Object });

            var context = new CommandValidatorContext<Command1Arguments>(commandParserMock.Object, parseResultMock.Object);

            argumentParserMock.Setup(a => a.TargetProperty).Returns(typeof(Command1Arguments).GetProperty("ArgumentA"));
            argumentParserMock.Setup(a => a.ArgumentName).Returns(new ArgumentName("argumentA", 'a'));

            context.GetArgumentName(a => a.ArgumentA).Should().Be(new ArgumentName("argumentA", 'a'));
        }

        [Test]
        public void TestGetArgumentName_Exceptions()
        {
            var commandParserMock = new Mock<ICommandParser>();
            var parseResultMock = new Mock<ParseResult>();
            var argumentParserMock = new Mock<IArgumentParser>();

            commandParserMock.Setup(a => a.ArgumentParsers).Returns(new List<IArgumentParser> { argumentParserMock.Object });

            var context = new CommandValidatorContext<Command1Arguments>(commandParserMock.Object, parseResultMock.Object);

            argumentParserMock.Setup(a => a.TargetProperty).Returns(typeof(Command1Arguments).GetProperty("ArgumentA"));
            argumentParserMock.Setup(a => a.ArgumentName).Returns(new ArgumentName("argumentA", 'a'));

            context.Invoking(a => a.GetArgumentName(null))
                .Should()
                .Throw<ArgumentNullException>();

            context.Invoking(a => a.GetArgumentName(a => a.ArgumentB))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage("The property ArgumentB of the type ParseTheArgs.Tests.TestData.Command1Arguments is not mapped to any argument. (Parameter 'argumentSelector')");
        }
    }
}
