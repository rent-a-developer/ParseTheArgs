using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using ParseTheArgs.Errors;
using ParseTheArgs.Parsers.Options;
using ParseTheArgs.Tests.TestData;
using ParseTheArgs.Tokens;
using static FluentAssertions.FluentActions;

namespace ParseTheArgs.Tests.Parsers.Options
{
    [TestFixture]
    public class TimeSpanListOptionParserTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            // Fix the current culture to a known value.
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }

        #endregion

        [Test(Description = "Constructor should throw an exception when the given target property has a incompatible data type.")]
        public void Constructor_IncompatibleTargetProperty_ShouldThrowException()
        {
            Invoking(() => new TimeSpanListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Strings"), "timeSpans"))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"The given target property has an incompatible property type. Expected type is System.Collections.Generic.List<System.TimeSpan>, actual type was System.Collections.Generic.List`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]].
Parameter name: targetProperty");
        }

        [Test(Description = "Constructor should throw an exception when the given option name is null or an empty string.")]
        public void Constructor_OptionNameIsNullOrEmpty_ShouldThrowException()
        {
            Invoking(() => new TimeSpanListOptionParser(typeof(DataTypesCommandOptions).GetProperty("TimeSpans"), null!))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"Value cannot be null or an empty string.
Parameter name: optionName");

            Invoking(() => new TimeSpanListOptionParser(typeof(DataTypesCommandOptions).GetProperty("TimeSpans"), ""))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"Value cannot be null or an empty string.
Parameter name: optionName");
        }

        [Test(Description = "Constructor should throw an exception when the given target property is null.")]
        public void Constructor_TargetPropertyIsNull_ShouldThrowException()
        {
            Invoking(() => new TimeSpanListOptionParser(null!, "timeSpans"))
                .Should()
                .Throw<ArgumentNullException>()
                .WithMessage(@"Value cannot be null.
Parameter name: targetProperty");
        }

        [Test(Description = "FormatProvider should return the value of CultureInfo.CurrentCulture initially.")]
        public void FormatProvider_Initially_ShouldReturnValueOfCurrentCulture()
        {
            var parser = new TimeSpanListOptionParser(typeof(DataTypesCommandOptions).GetProperty("TimeSpans"), "timeSpans");

            parser.FormatProvider.Should().Be(new CultureInfo("en-US"));
        }

        [Test(Description = "GetHelpText should return the text that was set via the OptionHelp property.")]
        public void GetHelpText_ShouldReturnSpecifiedHelpText()
        {
            var parser = new TimeSpanListOptionParser(typeof(DataTypesCommandOptions).GetProperty("TimeSpans"), "timeSpans");
            parser.OptionHelp = "Help text for option timeSpans.";

            parser.GetHelpText().Should().Be("Help text for option timeSpans.");
        }

        [Test(Description = "IsOptionRequired should return false initially.")]
        public void IsOptionRequired_Initially_ShouldReturnFalse()
        {
            var parser = new TimeSpanListOptionParser(typeof(DataTypesCommandOptions).GetProperty("TimeSpans"), "timeSpans");

            parser.IsOptionRequired.Should().BeFalse();
        }

        [Test(Description = "OptionDefaultValue should return null initially.")]
        public void OptionDefaultValue_Initially_ShouldReturnNull()
        {
            var parser = new TimeSpanListOptionParser(typeof(DataTypesCommandOptions).GetProperty("TimeSpans"), "timeSpans");

            parser.OptionDefaultValue.Should().BeNull();
        }

        [Test(Description = "OptionName should return the name that was specified via the constructor.")]
        public void OptionName_ShouldReturnNameSpecifiedViaConstructor()
        {
            var parser = new TimeSpanListOptionParser(typeof(DataTypesCommandOptions).GetProperty("TimeSpans"), "timeSpans");

            parser.OptionName.Should().Be("timeSpans");
        }

        [Test(Description = "OptionType should return MultiValueOption.")]
        public void OptionType_ShouldReturnMultiValueOption()
        {
            var parser = new TimeSpanListOptionParser(typeof(DataTypesCommandOptions).GetProperty("TimeSpans"), "timeSpans");

            parser.OptionType.Should().Be(OptionType.MultiValueOption);
        }

        [Test(Description = "Parse should pass the specified custom format settings to the value parser.")]
        public void Parse_CustomFormatSettings_ShouldPassCustomFormatToValueParser()
        {
            var valueParser = A.Fake<ValueParser>(ob => ob.CallsBaseMethods());
            var parser = new TimeSpanListOptionParser(typeof(DataTypesCommandOptions).GetProperty("TimeSpans"), "timeSpans");
            parser.ValueParser = valueParser;
            parser.TimeSpanFormat = @"hh\:mm";
            parser.TimeSpanStyles = TimeSpanStyles.AssumeNegative;
            parser.FormatProvider = new CultureInfo("de-DE");

            var tokens = new List<Token>
            {
                new OptionToken("timeSpans")
                {
                    OptionValues = {"23:59"}
                }
            };
            var parseResult = new ParseResult();
            var dataTypesCommandOptions = new DataTypesCommandOptions();
            parseResult.CommandOptions = dataTypesCommandOptions;

            parser.Parse(tokens, parseResult);

            TimeSpan timeSpan;
            A.CallTo(() => valueParser.TryParseTimeSpan("23:59", @"hh\:mm", new CultureInfo("de-DE"), TimeSpanStyles.AssumeNegative, out timeSpan)).MustHaveHappened();
        }

        [Test(Description = "Parse should add an OptionValueInvalidFormatError error to the parse result when one of the specified values is not a valid date time.")]
        public void Parse_InvalidValue_ShouldAddError()
        {
            var valueParser = A.Fake<ValueParser>();
            var parser = new TimeSpanListOptionParser(typeof(DataTypesCommandOptions).GetProperty("TimeSpans"), "timeSpans");
            parser.ValueParser = valueParser;

            var tokens = new List<Token>
            {
                new OptionToken("timeSpans")
                {
                    OptionValues = {"NotATimeSpan"}
                }
            };
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            TimeSpan timeSpan;
            A.CallTo(() => valueParser.TryParseTimeSpan("NotATimeSpan", null, new CultureInfo("en-US"), TimeSpanStyles.None, out timeSpan))
                .Returns(false);

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionValueInvalidFormatError>();

            var error = (OptionValueInvalidFormatError) parseResult.Errors[0];
            error.OptionName.Should().Be("timeSpans");
            error.InvalidOptionValue.Should().Be("NotATimeSpan");
            error.ExpectedValueFormat.Should().Be("A valid time interval");
            error.GetErrorMessage().Should().Be("The value 'NotATimeSpan' of the option --timeSpans has an invalid format. The expected format is: A valid time interval.");
        }

        [Test(Description = "Parse should assign the specified default value to the target property when the option is not present in the command line.")]
        public void Parse_OptionNotPresent_ShouldAssignDefaultValueToTargetProperty()
        {
            var parser = new TimeSpanListOptionParser(typeof(DataTypesCommandOptions).GetProperty("TimeSpans"), "timeSpans");
            parser.OptionDefaultValue = new List<TimeSpan> {new TimeSpan(23, 59, 59), new TimeSpan(10, 30, 59)};

            var tokens = new List<Token>();
            var parseResult = new ParseResult();
            var dataTypesCommandOptions = new DataTypesCommandOptions();
            parseResult.CommandOptions = dataTypesCommandOptions;

            parser.Parse(tokens, parseResult);

            dataTypesCommandOptions.TimeSpans.Should().BeEquivalentTo(new TimeSpan(23, 59, 59), new TimeSpan(10, 30, 59));
        }

        [Test(Description = "Parse should add an OptionValueMissingError error to the parse result when no value was supplied for the option.")]
        public void Parse_OptionValueMissing_ShouldAddError()
        {
            var parser = new TimeSpanListOptionParser(typeof(DataTypesCommandOptions).GetProperty("TimeSpans"), "timeSpans");

            var tokens = new List<Token>
            {
                new OptionToken("timeSpans")
            };

            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionValueMissingError>();

            var error = (OptionValueMissingError) parseResult.Errors[0];
            error.OptionName.Should().Be("timeSpans");
            error.GetErrorMessage().Should().Be("The option --timeSpans requires a value, but no value was specified.");
        }

        [Test(Description = "Parse should add an OptionMissingError error to the parse result when the option is required, but it was not supplied.")]
        public void Parse_RequiredOptionMissing_ShouldAddError()
        {
            var parser = new TimeSpanListOptionParser(typeof(DataTypesCommandOptions).GetProperty("TimeSpans"), "timeSpans");
            parser.IsOptionRequired = true;

            var tokens = new List<Token>();
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionMissingError>();

            var error = (OptionMissingError) parseResult.Errors[0];
            error.OptionName.Should().Be("timeSpans");
            error.GetErrorMessage().Should().Be("The option --timeSpans is required.");
        }

        [Test(Description = "Parse should parse valid option values using the value parser and assign them to the target property.")]
        public void Parse_ValidValue_ShouldParseAndAssignValueToTargetProperty()
        {
            var valueParser = A.Fake<ValueParser>();
            var parser = new TimeSpanListOptionParser(typeof(DataTypesCommandOptions).GetProperty("TimeSpans"), "timeSpans");
            parser.ValueParser = valueParser;

            var tokens = new List<Token>
            {
                new OptionToken("timeSpans")
                {
                    OptionValues = {"23:59:59", "10:30:59"}
                }
            };
            var parseResult = new ParseResult();
            var dataTypesCommandOptions = new DataTypesCommandOptions();
            parseResult.CommandOptions = dataTypesCommandOptions;

            TimeSpan timeSpan;

            A.CallTo(() => valueParser.TryParseTimeSpan("23:59:59", null, new CultureInfo("en-US"), TimeSpanStyles.None, out timeSpan))
                .Returns(true)
                .AssignsOutAndRefParameters(new TimeSpan(23, 59, 59));

            A.CallTo(() => valueParser.TryParseTimeSpan("10:30:59", null, new CultureInfo("en-US"), TimeSpanStyles.None, out timeSpan))
                .Returns(true)
                .AssignsOutAndRefParameters(new TimeSpan(10, 30, 59));

            parser.Parse(tokens, parseResult);

            dataTypesCommandOptions.TimeSpans.Should().BeEquivalentTo(new TimeSpan(23, 59, 59), new TimeSpan(10, 30, 59));

            A.CallTo(() => valueParser.TryParseTimeSpan("23:59:59", null, new CultureInfo("en-US"), TimeSpanStyles.None, out timeSpan)).MustHaveHappened();
            A.CallTo(() => valueParser.TryParseTimeSpan("10:30:59", null, new CultureInfo("en-US"), TimeSpanStyles.None, out timeSpan)).MustHaveHappened();
        }

        [Test(Description = "TargetProperty should return the property that was specified via the constructor.")]
        public void TargetProperty_ShouldReturnPropertySpecifiedViaConstructor()
        {
            var parser = new TimeSpanListOptionParser(typeof(DataTypesCommandOptions).GetProperty("TimeSpans"), "timeSpans");

            parser.TargetProperty.Should().BeSameAs(typeof(DataTypesCommandOptions).GetProperty("TimeSpans"));
        }

        [Test(Description = "TimeSpanFormat should return null initially.")]
        public void TimeSpanFormat_Initially_ShouldReturnNull()
        {
            var parser = new TimeSpanListOptionParser(typeof(DataTypesCommandOptions).GetProperty("TimeSpans"), "timeSpans");

            parser.TimeSpanFormat.Should().BeNull();
        }

        [Test(Description = "TimeSpanStyles should return None initially.")]
        public void TimeSpanStyles_Initially_ShouldReturnNone()
        {
            var parser = new TimeSpanListOptionParser(typeof(DataTypesCommandOptions).GetProperty("TimeSpans"), "timeSpans");

            parser.TimeSpanStyles.Should().Be(TimeSpanStyles.None);
        }
    }
}