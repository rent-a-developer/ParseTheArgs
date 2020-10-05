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
    public class DateTimeOptionParserTests
    {
        [SetUp]
        public void SetUp()
        {
            // Fix the current culture to a known value.
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }

        [Test(Description = "Constructor should throw an exception when the given target property is null.")]
        public void Constructor_TargetPropertyIsNull_ShouldThrowException()
        {
            Invoking(() => new DateTimeOptionParser(null, "dateTime"))
                .Should()
                .Throw<ArgumentNullException>()
                .WithMessage(@"Value cannot be null.
Parameter name: targetProperty");
        }

        [Test(Description = "Constructor should throw an exception when the given option name is null or an empty string.")]
        public void Constructor_OptionNameIsNullOrEmpty_ShouldThrowException()
        {
            Invoking(() => new DateTimeOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTime"), null))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"Value cannot be null or an empty string.
Parameter name: optionName");

            Invoking(() => new DateTimeOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTime"), ""))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"Value cannot be null or an empty string.
Parameter name: optionName");
        }

        [Test(Description = "Constructor should throw an exception when the given target property has a incompatible data type.")]
        public void Constructor_IncompatibleTargetProperty_ShouldThrowException()
        {
            Invoking(() => new DateTimeOptionParser(typeof(DataTypesCommandOptions).GetProperty("String"), "dateTime"))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"The given target property has an incompatible property type. Expected type is System.DateTime or System.Nullable<System.DateTime>, actual type was System.String.
Parameter name: targetProperty");
        }

        [Test(Description = "TargetProperty should return the property that was specified via the constructor.")]
        public void TargetProperty_ShouldReturnPropertySpecifiedViaConstructor()
        {
            var parser = new DateTimeOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTime"), "dateTime");

            parser.TargetProperty.Should().BeSameAs(typeof(DataTypesCommandOptions).GetProperty("DateTime"));
        }

        [Test(Description = "OptionDefaultValue should return default(DateTime) initially.")]
        public void OptionDefaultValue_Initially_ShouldReturnDefaultOfDateTime()
        {
            var parser = new DateTimeOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTime"), "dateTime");

            parser.OptionDefaultValue.Should().Be(default);
        }

        [Test(Description = "OptionName should return the name that was specified via the constructor.")]
        public void OptionName_ShouldReturnNameSpecifiedViaConstructor()
        {
            var parser = new DateTimeOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTime"), "dateTime");

            parser.OptionName.Should().Be("dateTime");
        }

        [Test(Description = "OptionType should return SingleValueOption.")]
        public void OptionType_ShouldReturnSingleValueOption()
        {
            var parser = new DateTimeOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTime"), "dateTime");

            parser.OptionType.Should().Be(OptionType.SingleValueOption);
        }

        [Test(Description = "IsOptionRequired should return false initially.")]
        public void IsOptionRequired_Initially_ShouldReturnFalse()
        {
            var parser = new DateTimeOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTime"), "dateTime");

            parser.IsOptionRequired.Should().BeFalse();
        }

        [Test(Description = "DateTimeFormat should return null initially.")]
        public void DateTimeFormat_Initially_ShouldReturnNull()
        {
            var parser = new DateTimeOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTime"), "dateTime");

            parser.DateTimeFormat.Should().BeNull();
        }

        [Test(Description = "DateTimeStyles should return None initially.")]
        public void DateTimeStyles_Initially_ShouldReturnNone()
        {
            var parser = new DateTimeOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTime"), "dateTime");

            parser.DateTimeStyles.Should().Be(DateTimeStyles.None);
        }

        [Test(Description = "FormatProvider should return the value of CultureInfo.CurrentCulture initially.")]
        public void FormatProvider_Initially_ShouldReturnValueOfCurrentCulture()
        {
            var parser = new DateTimeOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTime"), "dateTime");

            parser.FormatProvider.Should().Be(new CultureInfo("en-US"));
        }

        [Test(Description = "GetHelpText should return the text that was set via the OptionHelp property.")]
        public void GetHelpText_ShouldReturnSpecifiedHelpText()
        {
            var parser = new DateTimeOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTime"), "dateTime");
            parser.OptionHelp = "Help text for option dateTime.";

            parser.GetHelpText().Should().Be("Help text for option dateTime.");
        }

        [Test(Description = "Parse should assign the specified default value to the target property when the option is not present in the command line.")]
        public void Parse_OptionNotPresent_ShouldAssignDefaultValueToTargetProperty()
        {
            var parser = new DateTimeOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTime"), "dateTime");
            parser.OptionDefaultValue = new DateTime(2020, 12, 31, 23, 59, 59);

            var tokens = new List<Token>();
            var parseResult = new ParseResult();
            var dataTypesCommandOptions = new DataTypesCommandOptions();
            parseResult.CommandOptions = dataTypesCommandOptions;

            parser.Parse(tokens, parseResult);

            dataTypesCommandOptions.DateTime.Should().Be(new DateTime(2020, 12, 31, 23, 59, 59));
        }

        [Test(Description = "Parse should parse a valid option value using the value parser and assign it to the target property.")]
        public void Parse_ValidValue_ShouldParseAndAssignValueToTargetProperty()
        {
            var valueParser = A.Fake<ValueParser>();
            var parser = new DateTimeOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTime"), "dateTime");
            parser.ValueParser = valueParser;

            var tokens = new List<Token>
            {
                new OptionToken("dateTime")
                {
                    OptionValues = { "2020-12-31 23:59:59" }
                }
            };
            var parseResult = new ParseResult();
            var dataTypesCommandOptions = new DataTypesCommandOptions();
            parseResult.CommandOptions = dataTypesCommandOptions;

            DateTime dateTime;
            A.CallTo(() => valueParser.TryParseDateTime("2020-12-31 23:59:59", null, new CultureInfo("en-US"), DateTimeStyles.None, out dateTime))
                .Returns(true)
                .AssignsOutAndRefParameters(new DateTime(2020, 12, 31, 23, 59, 59));

            parser.Parse(tokens, parseResult);

            dataTypesCommandOptions.DateTime.Should().Be(new DateTime(2020, 12, 31, 23, 59, 59));

            A.CallTo(() => valueParser.TryParseDateTime("2020-12-31 23:59:59", null, new CultureInfo("en-US"), DateTimeStyles.None, out dateTime)).MustHaveHappened();
        }

        [Test(Description = "Parse should pass the specified custom format settings to the value parser.")]
        public void Parse_CustomFormatSettings_ShouldPassCustomFormatToValueParser()
        {
            var valueParser = A.Fake<ValueParser>(ob => ob.CallsBaseMethods());
            var parser = new DateTimeOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTime"), "dateTime");
            parser.ValueParser = valueParser;
            parser.DateTimeFormat = "ddd dd MMM yyyy h:mm tt";
            parser.DateTimeStyles = DateTimeStyles.NoCurrentDateDefault;
            parser.FormatProvider = new CultureInfo("de-DE");

            var tokens = new List<Token>
            {
                new OptionToken("dateTime")
                {
                    OptionValues = { "Sun 15 Jun 2008 8:30 AM" }
                }
            };
            var parseResult = new ParseResult();
            var dataTypesCommandOptions = new DataTypesCommandOptions();
            parseResult.CommandOptions = dataTypesCommandOptions;

            parser.Parse(tokens, parseResult);

            DateTime dateTime;
            A.CallTo(() => valueParser.TryParseDateTime("Sun 15 Jun 2008 8:30 AM", "ddd dd MMM yyyy h:mm tt", new CultureInfo("de-DE"), DateTimeStyles.NoCurrentDateDefault, out dateTime)).MustHaveHappened();
        }

        [Test(Description = "Parse should add an OptionValueInvalidFormatError error to the parse result when the specified value is not a valid date time.")]
        public void Parse_InvalidValue_ShouldAddError()
        {
            var valueParser = A.Fake<ValueParser>();
            var parser = new DateTimeOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTime"), "dateTime");
            parser.ValueParser = valueParser;

            var tokens = new List<Token>
            {
                new OptionToken("dateTime")
                {
                    OptionValues = { "NotADateTime" }
                }
            };
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            DateTime dateTime;
            A.CallTo(() => valueParser.TryParseDateTime("NotADateTime", null, new CultureInfo("en-US"), DateTimeStyles.None, out dateTime))
                .Returns(false);

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionValueInvalidFormatError>();

            var error = (OptionValueInvalidFormatError)parseResult.Errors[0];
            error.OptionName.Should().Be("dateTime");
            error.InvalidOptionValue.Should().Be("NotADateTime");
            error.ExpectedValueFormat.Should().Be("A valid date (and optionally time of day)");
            error.GetErrorMessage().Should().Be("The value 'NotADateTime' of the option --dateTime has an invalid format. The expected format is: A valid date (and optionally time of day).");
        }
    }
}