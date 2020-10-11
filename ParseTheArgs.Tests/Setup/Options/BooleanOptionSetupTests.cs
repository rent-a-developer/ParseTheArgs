using System;
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
            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Boolean");
            var optionParser = A.Fake<BooleanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new BooleanOptionParser(targetProperty, "boolean")));

            Invoking(() => new BooleanOptionSetup<DataTypesCommandOptions>(null, optionParser))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "Constructor should throw an exception when the given option parser is null.")]
        public void Constructor_OptionParserIsNull_ShouldThrowException()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            Invoking(() => new BooleanOptionSetup<DataTypesCommandOptions>(commandParser, null))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "Help should assign the given help text to the option parser.")]
        public void Help_ShouldAssignHelpToOptionParser()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Boolean");
            var optionParser = A.Fake<BooleanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new BooleanOptionParser(targetProperty, "boolean")));

            var setup = new BooleanOptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            setup.Help("newHelpText");

            A.CallToSet(() => optionParser.OptionHelp).To("newHelpText").MustHaveHappened();
        }

        [Test(Description = "Help should return the same instance of the option setup.")]
        public void Help_ShouldReturnOptionSetup()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Boolean");
            var optionParser = A.Fake<BooleanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new BooleanOptionParser(targetProperty, "boolean")));

            var setup = new BooleanOptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            setup.Help("newHelpText").Should().Be(setup);
        }

        [Test(Description = "Name should assign the given name to the option parser.")]
        public void Name_ShouldAssignNameToOptionParser()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Boolean");
            var optionParser = A.Fake<BooleanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new BooleanOptionParser(targetProperty, "boolean")));

            var setup = new BooleanOptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            A.CallTo(() => commandParser.CanOptionParserUseOptionName(optionParser, "newName")).Returns(true);

            setup.Name("newName");

            A.CallToSet(() => optionParser.OptionName).To("newName").MustHaveHappened();
        }

        [Test(Description = "Name should throw an exception when another option already has the same name.")]
        public void Name_DuplicateName_ShouldThrowException()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Boolean");
            var optionParser = A.Fake<BooleanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new BooleanOptionParser(targetProperty, "boolean")));

            var setup = new BooleanOptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            A.CallTo(() => commandParser.CanOptionParserUseOptionName(optionParser, "newName")).Returns(false);

            setup.Invoking(a => a.Name("newName"))
                .Should()
                .Throw<ArgumentException>();
        }

        [Test(Description = "Name should return the same instance of the option setup.")]
        public void Name_ShouldReturnOptionSetup()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Boolean");
            var optionParser = A.Fake<BooleanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new BooleanOptionParser(targetProperty, "boolean")));

            var setup = new BooleanOptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            A.CallTo(() => commandParser.CanOptionParserUseOptionName(optionParser, "newName")).Returns(true);

            setup.Name("newName").Should().Be(setup);
        }
    }
}
