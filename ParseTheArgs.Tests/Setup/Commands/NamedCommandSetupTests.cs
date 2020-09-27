using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Setup.Commands;
using ParseTheArgs.Tests.TestData;

namespace ParseTheArgs.Tests.Setup.Commands
{
    [TestFixture]
    public class NamedCommandSetupTests
    {
        [Test(Description = "Name should throw an exception when another command already has the same name.")]
        public void Name_DuplicateName_ShouldThrowException()
        {
            var parserMock = new Mock<Parser>();
            var duplicateCommandParserMock = new Mock<ICommandParser>();

            parserMock.Setup(a => a.CommandParsers).Returns(new List<ICommandParser> {duplicateCommandParserMock.Object});
            duplicateCommandParserMock.Setup(a => a.CommandName).Returns("command1");

            var setup = new NamedCommandSetup<Command1Arguments>(parserMock.Object);

            setup.Invoking(a => a.Name("command1"))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"The given command name 'command1' is already in use by another command. Please use a different name.
Parameter name: name");

        }
    }
}
