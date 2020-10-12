using System;
using System.Collections.Generic;
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
    public class EnumOptionSetupTests
    {
        [Test(Description = "Constructor should throw an exception when the given command parser is null.")]
        public void Constructor_CommandParserIsNull_ShouldThrowException()
        {
            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Enum");
            var optionParser = A.Fake<EnumOptionParser<LogLevel>>(ob => ob.WithArgumentsForConstructor(() => new EnumOptionParser<LogLevel>(targetProperty, "enum")));

            FluentActions.Invoking(() => new EnumOptionSetup<DataTypesCommandOptions, LogLevel>(null, optionParser))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "Constructor should throw an exception when the given option parser is null.")]
        public void Constructor_OptionParserIsNull_ShouldThrowException()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            FluentActions.Invoking(() => new EnumOptionSetup<DataTypesCommandOptions, LogLevel>(commandParser, null))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "DefaultValue should assign the given default value to the option parser.")]
        public void DefaultValue_ShouldAssignDefaultValueToOptionParser()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Enum");
            var optionParser = A.Fake<EnumOptionParser<LogLevel>>(ob => ob.WithArgumentsForConstructor(() => new EnumOptionParser<LogLevel>(targetProperty, "enum")));

            var setup = new EnumOptionSetup<DataTypesCommandOptions, LogLevel>(commandParser, optionParser);

            setup.DefaultValue(LogLevel.Debug);

            A.CallToSet(() => optionParser.OptionDefaultValue).To(LogLevel.Debug).MustHaveHappened();
        }

        [Test(Description = "DefaultValue should return the same instance of the option setup.")]
        public void DefaultValue_ShouldReturnOptionSetup()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Enum");
            var optionParser = A.Fake<EnumOptionParser<LogLevel>>(ob => ob.WithArgumentsForConstructor(() => new EnumOptionParser<LogLevel>(targetProperty, "enum")));

            var setup = new EnumOptionSetup<DataTypesCommandOptions, LogLevel>(commandParser, optionParser);

            setup.DefaultValue(LogLevel.Debug).Should().Be(setup);
        }

        [Test(Description = "EnumValueHelp should assign the given enum value help text to the option parser.")]
        public void EnumValueHelp_ShouldAssignEnumValueHelpToOptionParser()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Enum");
            var optionParser = A.Fake<EnumOptionParser<LogLevel>>(ob => ob.WithArgumentsForConstructor(() => new EnumOptionParser<LogLevel>(targetProperty, "enum")));
            var enumValueHelps = A.Fake<IDictionary<LogLevel, String>>();

            var setup = new EnumOptionSetup<DataTypesCommandOptions, LogLevel>(commandParser, optionParser);

            A.CallTo(() => optionParser.EnumValuesHelps).Returns(enumValueHelps);

            setup.EnumValueHelp(LogLevel.Trace, "newEnumValueHelp");

            A.CallTo(() => enumValueHelps.Add(LogLevel.Trace, "newEnumValueHelp")).MustHaveHappened();
        }

        [Test(Description = "EnumValueHelp should return the same instance of the option setup.")]
        public void EnumValueHelp_ShouldReturnOptionSetup()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Enum");
            var optionParser = A.Fake<EnumOptionParser<LogLevel>>(ob => ob.WithArgumentsForConstructor(() => new EnumOptionParser<LogLevel>(targetProperty, "enum")));

            var setup = new EnumOptionSetup<DataTypesCommandOptions, LogLevel>(commandParser, optionParser);

            setup.EnumValueHelp(LogLevel.Trace, "newEnumValueHelp").Should().Be(setup);
        }

        [Test(Description = "Help should assign the given help text to the option parser.")]
        public void Help_ShouldAssignHelpToOptionParser()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Enum");
            var optionParser = A.Fake<EnumOptionParser<LogLevel>>(ob => ob.WithArgumentsForConstructor(() => new EnumOptionParser<LogLevel>(targetProperty, "enum")));

            var setup = new EnumOptionSetup<DataTypesCommandOptions, LogLevel>(commandParser, optionParser);

            setup.Help("newHelpText");

            A.CallToSet(() => optionParser.OptionHelp).To("newHelpText").MustHaveHappened();
        }

        [Test(Description = "Help should return the same instance of the option setup.")]
        public void Help_ShouldReturnOptionSetup()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Enum");
            var optionParser = A.Fake<EnumOptionParser<LogLevel>>(ob => ob.WithArgumentsForConstructor(() => new EnumOptionParser<LogLevel>(targetProperty, "enum")));

            var setup = new EnumOptionSetup<DataTypesCommandOptions, LogLevel>(commandParser, optionParser);

            setup.Help("newHelpText").Should().Be(setup);
        }

        [Test(Description = "IsRequired should return the same instance of the option setup.")]
        public void IsRequired_ShouldReturnOptionSetup()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Enum");
            var optionParser = A.Fake<EnumOptionParser<LogLevel>>(ob => ob.WithArgumentsForConstructor(() => new EnumOptionParser<LogLevel>(targetProperty, "enum")));

            var setup = new EnumOptionSetup<DataTypesCommandOptions, LogLevel>(commandParser, optionParser);

            setup.IsRequired().Should().Be(setup);
        }

        [Test(Description = "IsRequired should set the is required flag on the option parser.")]
        public void IsRequired_ShouldSetIsRequiredFlagOnOptionParser()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Enum");
            var optionParser = A.Fake<EnumOptionParser<LogLevel>>(ob => ob.WithArgumentsForConstructor(() => new EnumOptionParser<LogLevel>(targetProperty, "enum")));

            var setup = new EnumOptionSetup<DataTypesCommandOptions, LogLevel>(commandParser, optionParser);

            setup.IsRequired();

            A.CallToSet(() => optionParser.IsOptionRequired).To(true).MustHaveHappened();
        }

        [Test(Description = "Name should throw an exception when another option already has the same name.")]
        public void Name_DuplicateName_ShouldThrowException()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Enum");
            var optionParser = A.Fake<EnumOptionParser<LogLevel>>(ob => ob.WithArgumentsForConstructor(() => new EnumOptionParser<LogLevel>(targetProperty, "enum")));

            var setup = new EnumOptionSetup<DataTypesCommandOptions, LogLevel>(commandParser, optionParser);

            A.CallTo(() => commandParser.CanOptionParserUseOptionName(optionParser, "newName")).Returns(false);

            setup.Invoking(a => a.Name("newName"))
                .Should()
                .Throw<ArgumentException>();
        }

        [Test(Description = "Name should assign the given name to the option parser.")]
        public void Name_ShouldAssignNameToOptionParser()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Enum");
            var optionParser = A.Fake<EnumOptionParser<LogLevel>>(ob => ob.WithArgumentsForConstructor(() => new EnumOptionParser<LogLevel>(targetProperty, "enum")));

            var setup = new EnumOptionSetup<DataTypesCommandOptions, LogLevel>(commandParser, optionParser);

            A.CallTo(() => commandParser.CanOptionParserUseOptionName(optionParser, "newName")).Returns(true);

            setup.Name("newName");

            A.CallToSet(() => optionParser.OptionName).To("newName").MustHaveHappened();
        }

        [Test(Description = "Name should return the same instance of the option setup.")]
        public void Name_ShouldReturnOptionSetup()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Enum");
            var optionParser = A.Fake<EnumOptionParser<LogLevel>>(ob => ob.WithArgumentsForConstructor(() => new EnumOptionParser<LogLevel>(targetProperty, "enum")));

            var setup = new EnumOptionSetup<DataTypesCommandOptions, LogLevel>(commandParser, optionParser);

            A.CallTo(() => commandParser.CanOptionParserUseOptionName(optionParser, "newName")).Returns(true);

            setup.Name("newName").Should().Be(setup);
        }
    }
}