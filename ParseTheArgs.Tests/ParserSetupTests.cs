using System;
using FluentAssertions;
using NUnit.Framework;
using ParseTheArgs.Tests.TestData;

namespace ParseTheArgs.Tests
{
    [TestFixture]
    public class ParserSetupTests
    {
        [Test]
        public void TestDuplicateCommandName()
        {
            var parser = new Parser();
            var setup = parser.Setup;

            setup
                .Command<Command1Arguments>()
                .Name("command1");

            setup
                .Command<Command2Arguments>()
                .Invoking(a => a.Name("command1"))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"The given command name 'command1' is already in use by another command. Please use a different name. (Parameter 'name')");
        }

        [Test]
        public void TestDuplicateArgumentName()
        {
            var parser = new Parser();
            var setup = parser.Setup;

            var defaultCommand = setup.DefaultCommand<DefaultArguments>();

            defaultCommand
                .Argument(a => a.ArgumentA)
                .Name("argumentA");

            defaultCommand
                .Argument(a => a.ArgumentB)
                .Invoking(a => a.Name("argumentA"))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"The given argument name 'argumentA' is already in use by another argument. Please use a different name. (Parameter 'name')");
        }

        [Test]
        public void TestDuplicateArgumentShortName()
        {
            var parser = new Parser();
            var setup = parser.Setup;

            var defaultCommand = setup.DefaultCommand<DefaultArguments>();

            defaultCommand
                .Argument(a => a.ArgumentA)
                .ShortName('a');

            defaultCommand
                .Argument(a => a.ArgumentB)
                .Invoking(a => a.ShortName('a'))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"The given argument short name 'a' is already in use by another argument. Please use a different short name. (Parameter 'shortName')");
        }
    }
}
