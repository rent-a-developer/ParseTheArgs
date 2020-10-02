using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FakeItEasy;
using FluentAssertions;
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
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<Command1Options>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<Command1Options>(parser)));
            var duplicateOptionParser = new StringOptionParser(typeof(Command1Options).GetProperty("OptionA"), "optionA");
            
            A.CallTo(() => commandParser.OptionParsers).Returns(new List<OptionParser> {duplicateOptionParser});
            var setup = new StringOptionSetup<Command1Options>(commandParser, (Expression<Func<Command1Options, Object>>)(a => a.OptionB));

            setup.Invoking(a => a.Name("optionA"))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"The given option name 'optionA' is already in use by another option. Please use a different name.
Parameter name: name");
        }
    }
}
