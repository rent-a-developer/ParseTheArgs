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

        [Test(Description = "Constructor should reuse an existing option parser.")]
        public void Constructor_OptionParserExistsAlready_ShouldReuseExistingOptionParser()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));

            Expression<Func<DataTypesCommandOptions, Boolean>> propertyExpression = a => a.Boolean;
            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Boolean");

            A.CallTo(() => commandParser.GetOrCreateOptionParser<BooleanOptionParser>(targetProperty));

            Invoking(() => new BooleanOptionSetup<DataTypesCommandOptions>(null, propertyExpression))
                .Should()
                .Throw<ArgumentNullException>();
        }
    }
}
