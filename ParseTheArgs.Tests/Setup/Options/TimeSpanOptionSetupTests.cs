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
    public class TimeSpanOptionSetupTests
    {
        [Test(Description = "Constructor should throw an exception when the given command parser is null.")]
        public void Constructor_CommandParserIsNull_ShouldThrowException()
        {
            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("TimeSpan");
            var optionParser = A.Fake<TimeSpanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new TimeSpanOptionParser(targetProperty, "timeSpan")));

            Invoking(() => new TimeSpanOptionSetup<DataTypesCommandOptions>(null, optionParser))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "Constructor should throw an exception when the given option parser is null.")]
        public void Constructor_OptionParserIsNull_ShouldThrowException()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            Invoking(() => new TimeSpanOptionSetup<DataTypesCommandOptions>(commandParser, null))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "DefaultValue should assign the given default value to the option parser.")]
        public void DefaultValue_ShouldAssignDefaultValueToOptionParser()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("TimeSpan");
            var optionParser = A.Fake<TimeSpanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new TimeSpanOptionParser(targetProperty, "timeSpan")));

            var setup = new TimeSpanOptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            var defaultValue = new TimeSpan(1, 2, 3, 4);
            setup.DefaultValue(defaultValue);

            A.CallToSet(() => optionParser.OptionDefaultValue).To(defaultValue).MustHaveHappened();
        }

        [Test(Description = "DefaultValue should return the same instance of the option setup.")]
        public void DefaultValue_ShouldReturnOptionSetup()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("TimeSpan");
            var optionParser = A.Fake<TimeSpanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new TimeSpanOptionParser(targetProperty, "timeSpan")));

            var setup = new TimeSpanOptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            setup.DefaultValue(new TimeSpan(1, 2, 3, 4)).Should().Be(setup);
        }

        [Test(Description = "Format should assign the given format to the option parser.")]
        public void Format_ShouldAssignFormatToOptionParser()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("TimeSpan");
            var optionParser = A.Fake<TimeSpanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new TimeSpanOptionParser(targetProperty, "timeSpan")));

            var setup = new TimeSpanOptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            setup.Format(@"hh\:mm");

            A.CallToSet(() => optionParser.TimeSpanFormat).To(@"hh\:mm").MustHaveHappened();
        }

        [Test(Description = "Format should return the same instance of the option setup.")]
        public void Format_ShouldReturnOptionSetup()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("TimeSpan");
            var optionParser = A.Fake<TimeSpanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new TimeSpanOptionParser(targetProperty, "timeSpan")));

            var setup = new TimeSpanOptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            setup.Format(@"hh\:mm").Should().Be(setup);
        }

        [Test(Description = "FormatProvider should assign the given format to the option parser.")]
        public void FormatProvider_ShouldAssignFormatProviderToOptionParser()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("TimeSpan");
            var optionParser = A.Fake<TimeSpanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new TimeSpanOptionParser(targetProperty, "timeSpan")));

            var setup = new TimeSpanOptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            var formatProvider = new CultureInfo("de-DE");
            setup.FormatProvider(formatProvider);

            A.CallToSet(() => optionParser.FormatProvider).To(formatProvider).MustHaveHappened();
        }

        [Test(Description = "FormatProvider should return the same instance of the option setup.")]
        public void FormatProvider_ShouldReturnOptionSetup()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("TimeSpan");
            var optionParser = A.Fake<TimeSpanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new TimeSpanOptionParser(targetProperty, "timeSpan")));

            var setup = new TimeSpanOptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            setup.FormatProvider(new CultureInfo("de-DE")).Should().Be(setup);
        }

        [Test(Description = "Help should assign the given help text to the option parser.")]
        public void Help_ShouldAssignHelpToOptionParser()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("TimeSpan");
            var optionParser = A.Fake<TimeSpanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new TimeSpanOptionParser(targetProperty, "timeSpan")));

            var setup = new TimeSpanOptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            setup.Help("newHelpText");

            A.CallToSet(() => optionParser.OptionHelp).To("newHelpText").MustHaveHappened();
        }

        [Test(Description = "Help should return the same instance of the option setup.")]
        public void Help_ShouldReturnOptionSetup()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("TimeSpan");
            var optionParser = A.Fake<TimeSpanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new TimeSpanOptionParser(targetProperty, "timeSpan")));

            var setup = new TimeSpanOptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            setup.Help("newHelpText").Should().Be(setup);
        }

        [Test(Description = "IsRequired should return the same instance of the option setup.")]
        public void IsRequired_ShouldReturnOptionSetup()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("TimeSpan");
            var optionParser = A.Fake<TimeSpanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new TimeSpanOptionParser(targetProperty, "timeSpan")));

            var setup = new TimeSpanOptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            setup.IsRequired().Should().Be(setup);
        }

        [Test(Description = "IsRequired should set the is required flag on the option parser.")]
        public void IsRequired_ShouldSetIsRequiredFlagOnOptionParser()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("TimeSpan");
            var optionParser = A.Fake<TimeSpanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new TimeSpanOptionParser(targetProperty, "timeSpan")));

            var setup = new TimeSpanOptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            setup.IsRequired();

            A.CallToSet(() => optionParser.IsOptionRequired).To(true).MustHaveHappened();
        }

        [Test(Description = "Name should throw an exception when another option already has the same name.")]
        public void Name_DuplicateName_ShouldThrowException()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("TimeSpan");
            var optionParser = A.Fake<TimeSpanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new TimeSpanOptionParser(targetProperty, "timeSpan")));

            var setup = new TimeSpanOptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            A.CallTo(() => commandParser.CanOptionParserUseOptionName(optionParser, "newName")).Returns(false);

            setup.Invoking(a => a.Name("newName"))
                .Should()
                .Throw<ArgumentException>();
        }

        [Test(Description = "Name should assign the given name to the option parser.")]
        public void Name_ShouldAssignNameToOptionParser()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("TimeSpan");
            var optionParser = A.Fake<TimeSpanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new TimeSpanOptionParser(targetProperty, "timeSpan")));

            var setup = new TimeSpanOptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            A.CallTo(() => commandParser.CanOptionParserUseOptionName(optionParser, "newName")).Returns(true);

            setup.Name("newName");

            A.CallToSet(() => optionParser.OptionName).To("newName").MustHaveHappened();
        }

        [Test(Description = "Name should return the same instance of the option setup.")]
        public void Name_ShouldReturnOptionSetup()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("TimeSpan");
            var optionParser = A.Fake<TimeSpanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new TimeSpanOptionParser(targetProperty, "timeSpan")));

            var setup = new TimeSpanOptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            A.CallTo(() => commandParser.CanOptionParserUseOptionName(optionParser, "newName")).Returns(true);

            setup.Name("newName").Should().Be(setup);
        }

        [Test(Description = "Styles should assign the given format to the option parser.")]
        public void Styles_ShouldAssignStylesToOptionParser()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("TimeSpan");
            var optionParser = A.Fake<TimeSpanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new TimeSpanOptionParser(targetProperty, "timeSpan")));

            var setup = new TimeSpanOptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            setup.Styles(TimeSpanStyles.AssumeNegative);

            A.CallToSet(() => optionParser.TimeSpanStyles).To(TimeSpanStyles.AssumeNegative).MustHaveHappened();
        }

        [Test(Description = "Styles should return the same instance of the option setup.")]
        public void Styles_ShouldReturnOptionSetup()
        {
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>();

            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("TimeSpan");
            var optionParser = A.Fake<TimeSpanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new TimeSpanOptionParser(targetProperty, "timeSpan")));

            var setup = new TimeSpanOptionSetup<DataTypesCommandOptions>(commandParser, optionParser);

            setup.Styles(TimeSpanStyles.AssumeNegative).Should().Be(setup);
        }
    }
}