using System;
using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
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
    }
}
