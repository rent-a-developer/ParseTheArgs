using System;
using System.Linq.Expressions;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Parsers.Options;
using ParseTheArgs.Setup.Options;
using ParseTheArgs.Tests.TestData;
using static FluentAssertions.FluentActions;

namespace ParseTheArgs.Tests.Setup.Options
{
    [TestFixture]
    public class BooleanOptionSetupTests
    {
        [Test(Description = "Constructor should throw an exception when the given command parser is null.")]
        public void Constructor_CommandParserIsNull_ShouldThrowException()
        {
            Expression<Func<DataTypesCommandOptions, Boolean>> propertyExpression = a => a.Boolean;

            Invoking(() => new BooleanOptionSetup<DataTypesCommandOptions>(null, propertyExpression))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "Constructor should get the option parser from the command parser.")]
        public void Constructor_ShouldGetOptionParserFromCommandParser()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Boolean");
            var optionParser = A.Fake<BooleanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new BooleanOptionParser(targetProperty, "boolean")));

            Expression<Func<DataTypesCommandOptions, Boolean>> propertyExpression = a => a.Boolean;

            A.CallTo(() => commandParser.GetOrCreateOptionParser<BooleanOptionParser>(targetProperty)).Returns(optionParser);

            var setup = new BooleanOptionSetup<DataTypesCommandOptions>(commandParser, propertyExpression);

            setup.optionParser.Should().Be(optionParser);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<BooleanOptionParser>(targetProperty)).MustHaveHappened();
        }

        [Test(Description = "Constructor should get the option parser from the command parser.")]
        public void Help_ShouldAssignHelpTextToOptionParser()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Boolean");
            var optionParser = A.Fake<BooleanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new BooleanOptionParser(targetProperty, "boolean")));

            Expression<Func<DataTypesCommandOptions, Boolean>> propertyExpression = a => a.Boolean;

            A.CallTo(() => commandParser.GetOrCreateOptionParser<BooleanOptionParser>(targetProperty)).Returns(optionParser);

            var setup = new BooleanOptionSetup<DataTypesCommandOptions>(commandParser, propertyExpression);

            setup.Help("boolean help");

            optionParser.OptionHelp.Should().Be("boolean help");
        }
    }
}
