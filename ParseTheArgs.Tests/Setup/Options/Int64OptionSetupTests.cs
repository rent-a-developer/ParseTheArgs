using System;
using System.Globalization;
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
    public class Int64OptionSetupTests
    {
        [Test(Description = "Constructor should throw an exception when the given command parser is null.")]
        public void Constructor_CommandParserIsNull_ShouldThrowException()
        {
            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Int64");
            var optionParser = A.Fake<Int64OptionParser>(ob => ob.WithArgumentsForConstructor(() => new Int64OptionParser(targetProperty, "int64")));

            Invoking(() => new Int64OptionSetup<DataTypesCommandOptions>(null, optionParser))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "Constructor should throw an exception when the given option parser is null.")]
        public void Constructor_OptionParserIsNull_ShouldThrowException()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            Invoking(() => new Int64OptionSetup<DataTypesCommandOptions>(commandParser, null))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "Help should assign the given help text to the option parser.")]
        public void Help_ShouldAssignHelpToOptionParser()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Int64");
            var optionParser = A.Fake<Int64OptionParser>(ob => ob.WithArgumentsForConstructor(() => new Int64OptionParser(targetProperty, "int64")));

            var setup = new Int64OptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            setup.Help("newHelpText");

            A.CallToSet(() => optionParser.OptionHelp).To("newHelpText").MustHaveHappened();
        }

        [Test(Description = "Help should return the same instance of the option setup.")]
        public void Help_ShouldReturnOptionSetup()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Int64");
            var optionParser = A.Fake<Int64OptionParser>(ob => ob.WithArgumentsForConstructor(() => new Int64OptionParser(targetProperty, "int64")));

            var setup = new Int64OptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            setup.Help("newHelpText").Should().Be(setup);
        }

        [Test(Description = "Name should assign the given name to the option parser.")]
        public void Name_ShouldAssignNameToOptionParser()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Int64");
            var optionParser = A.Fake<Int64OptionParser>(ob => ob.WithArgumentsForConstructor(() => new Int64OptionParser(targetProperty, "int64")));

            var setup = new Int64OptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            A.CallTo(() => commandParser.CanOptionParserUseOptionName(optionParser, "newName")).Returns(true);

            setup.Name("newName");

            A.CallToSet(() => optionParser.OptionName).To("newName").MustHaveHappened();
        }

        [Test(Description = "Name should throw an exception when another option already has the same name.")]
        public void Name_DuplicateName_ShouldThrowException()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Int64");
            var optionParser = A.Fake<Int64OptionParser>(ob => ob.WithArgumentsForConstructor(() => new Int64OptionParser(targetProperty, "int64")));

            var setup = new Int64OptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            A.CallTo(() => commandParser.CanOptionParserUseOptionName(optionParser, "newName")).Returns(false);

            setup.Invoking(a => a.Name("newName"))
                .Should()
                .Throw<ArgumentException>();
        }

        [Test(Description = "Name should return the same instance of the option setup.")]
        public void Name_ShouldReturnOptionSetup()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Int64");
            var optionParser = A.Fake<Int64OptionParser>(ob => ob.WithArgumentsForConstructor(() => new Int64OptionParser(targetProperty, "int64")));

            var setup = new Int64OptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            A.CallTo(() => commandParser.CanOptionParserUseOptionName(optionParser, "newName")).Returns(true);

            setup.Name("newName").Should().Be(setup);
        }

        [Test(Description = "DefaultValue should assign the given default value to the option parser.")]
        public void DefaultValue_ShouldAssignDefaultValueToOptionParser()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Int64");
            var optionParser = A.Fake<Int64OptionParser>(ob => ob.WithArgumentsForConstructor(() => new Int64OptionParser(targetProperty, "int64")));

            var setup = new Int64OptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            var defaultValue = 123L;
            setup.DefaultValue(defaultValue);

            A.CallToSet(() => optionParser.OptionDefaultValue).To(defaultValue).MustHaveHappened();
        }

        [Test(Description = "DefaultValue should return the same instance of the option setup.")]
        public void DefaultValue_ShouldReturnOptionSetup()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Int64");
            var optionParser = A.Fake<Int64OptionParser>(ob => ob.WithArgumentsForConstructor(() => new Int64OptionParser(targetProperty, "int64")));

            var setup = new Int64OptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            setup.DefaultValue(123L).Should().Be(setup);
        }

        [Test(Description = "IsRequired should tell the option parser that the option is required.")]
        public void IsRequired_ShouldAssignDefaultValueToOptionParser()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Int64");
            var optionParser = A.Fake<Int64OptionParser>(ob => ob.WithArgumentsForConstructor(() => new Int64OptionParser(targetProperty, "int64")));

            var setup = new Int64OptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            setup.IsRequired();

            A.CallToSet(() => optionParser.IsOptionRequired).To(true).MustHaveHappened();
        }

        [Test(Description = "IsRequired should return the same instance of the option setup.")]
        public void IsRequired_ShouldReturnOptionSetup()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Int64");
            var optionParser = A.Fake<Int64OptionParser>(ob => ob.WithArgumentsForConstructor(() => new Int64OptionParser(targetProperty, "int64")));

            var setup = new Int64OptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            setup.IsRequired().Should().Be(setup);
        }

        [Test(Description = "FormatProvider should assign the given format to the option parser.")]
        public void FormatProvider_ShouldAssignFormatProviderToOptionParser()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Int64");
            var optionParser = A.Fake<Int64OptionParser>(ob => ob.WithArgumentsForConstructor(() => new Int64OptionParser(targetProperty, "int64")));

            var setup = new Int64OptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            var formatProvider = new CultureInfo("de-DE");
            setup.FormatProvider(formatProvider);

            A.CallToSet(() => optionParser.FormatProvider).To(formatProvider).MustHaveHappened();
        }

        [Test(Description = "FormatProvider should return the same instance of the option setup.")]
        public void FormatProvider_ShouldReturnOptionSetup()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Int64");
            var optionParser = A.Fake<Int64OptionParser>(ob => ob.WithArgumentsForConstructor(() => new Int64OptionParser(targetProperty, "int64")));

            var setup = new Int64OptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            setup.FormatProvider(new CultureInfo("de-DE")).Should().Be(setup);
        }

        [Test(Description = "Styles should assign the given format to the option parser.")]
        public void Styles_ShouldAssignStylesToOptionParser()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Int64");
            var optionParser = A.Fake<Int64OptionParser>(ob => ob.WithArgumentsForConstructor(() => new Int64OptionParser(targetProperty, "int64")));

            var setup = new Int64OptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            setup.Styles(NumberStyles.Currency);

            A.CallToSet(() => optionParser.NumberStyles).To(NumberStyles.Currency).MustHaveHappened();
        }

        [Test(Description = "Styles should return the same instance of the option setup.")]
        public void Styles_ShouldReturnOptionSetup()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Int64");
            var optionParser = A.Fake<Int64OptionParser>(ob => ob.WithArgumentsForConstructor(() => new Int64OptionParser(targetProperty, "int64")));

            var setup = new Int64OptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            setup.Styles(NumberStyles.Currency).Should().Be(setup);
        }
    }
}