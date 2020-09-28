using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Parsers.Options;
using ParseTheArgs.Setup.Options;
using ParseTheArgs.Tests.TestData;

namespace ParseTheArgs.Tests.Setup.Options
{
    [TestFixture]
    public class StringOptionSetupTests
    {
        [Test(Description = "Name should throw an exception when another option already has the same name.")]
        public void Name_DuplicateName_ShouldThrowException()
        {
            var parserMock = new Mock<Parser>();
            var commandParserMock = new Mock<CommandParser<Command1Options>>(parserMock.Object);
            var duplicateOptionParser = new StringOptionParser(typeof(Command1Options).GetProperty("OptionA"), "optionA");
            
            commandParserMock.Setup(a => a.OptionParsers).Returns(new List<OptionParser> {duplicateOptionParser});
            var setup = new StringOptionSetup<Command1Options>(commandParserMock.Object, (Expression<Func<Command1Options, Object>>)(a => a.OptionB));

            setup.Invoking(a => a.Name("optionA"))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"The given option name 'optionA' is already in use by another option. Please use a different name.
Parameter name: name");
        }
    }
}
