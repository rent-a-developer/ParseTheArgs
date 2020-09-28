using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using ParseTheArgs.Parsers.Arguments;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Setup.Arguments;
using ParseTheArgs.Tests.TestData;

namespace ParseTheArgs.Tests.Setup.Arguments
{
    [TestFixture]
    public class StringArgumentSetupTests
    {
        [Test(Description = "Name should throw an exception when another argument already has the same name.")]
        public void Name_DuplicateName_ShouldThrowException()
        {
            var parserMock = new Mock<Parser>();
            var commandParserMock = new Mock<CommandParser<Command1Arguments>>(parserMock.Object);
            var duplicateArgumentParser = new StringArgumentParser(typeof(Command1Arguments).GetProperty("ArgumentA"), "argumentA");
            
            commandParserMock.Setup(a => a.ArgumentParsers).Returns(new List<ArgumentParser> {duplicateArgumentParser});
            var setup = new StringArgumentSetup<Command1Arguments>(commandParserMock.Object, (Expression<Func<Command1Arguments, Object>>)(a => a.ArgumentB));

            setup.Invoking(a => a.Name("argumentA"))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"The given argument name 'argumentA' is already in use by another argument. Please use a different name.
Parameter name: name");
        }
    }
}
