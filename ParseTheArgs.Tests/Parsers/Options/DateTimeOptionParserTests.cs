using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
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
            // We fix the current culture to en-US so that parsing of values (e.g. DateTime values) is done in a deterministic fashion.
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
            Invoking(() => new DateTimeOptionParser(typeof(DataTypesCommandOptions).GetProperty("String"), "dateTimes"))
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

        [Test(Description = "OptionName should return the name that was specified via the constructor.")]
        public void OptionName_ShouldReturnNameSpecifiedViaConstructor()
        {
            var parser = new DateTimeOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTime"), "dateTime");

            parser.OptionName.Should().Be("dateTime");
        }

        [Test(Description = "OptionType should return SingleValueOption.")]
        public void OptionType_ShouldReturnValuelessOption()
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

        [Test(Description = "DateTimeFormat should return None initially.")]
        public void DateTimeFormat_Initially_ShouldReturnNone()
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

        [Test(Description = "DateTimeStyles should return null initially.")]
        public void DateTimeStyles_Initially_ShouldReturnNull()
        {
            var parser = new DateTimeOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTime"), "dateTime");

            parser.DateTimeFormat.Should().BeNull();
        }

        [Test(Description = "GetHelpText should return the text that was set via the OptionHelp property.")]
        public void GetHelpText_ShouldReturnSpecifiedHelpText()
        {
            var parser = new DateTimeOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTime"), "dateTime");
            parser.OptionHelp = "Help text for option dateTime.";

            parser.GetHelpText().Should().Be("Help text for option dateTime.");
        }

        [Test(Description = "Parse should parse a valid date time in the correct default format and put the date time value into the correct property of the options object.")]
        public void Parse_DefaultFormat_ShouldParseAndPutDateTimeValueInOptionsObject()
        {
            var parser = new DateTimeOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTime"), "dateTime");

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

            parser.Parse(tokens, parseResult);

            dataTypesCommandOptions.DateTime.Should().Be(new DateTime(2020, 12, 31, 23, 59, 59));
        }

        [Test(Description = "Parse should add an OptionValueInvalidFormatError error to the parse result when the specified value is not a valid date time.")]
        public void Parse_InvalidValue_ShouldAddError()
        {
            var parser = new DateTimeOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTime"), "dateTime");

            var tokens = new List<Token>
            {
                new OptionToken("dateTime")
                {
                    OptionValues = { "NotADateTime" }
                }
            };
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

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

        [Test(Description = "Parse should add an OptionValueInvalidFormatError error to the parse result when the specified value is not a valid date time in the specified custom format.")]
        public void Parse_CustomFormatInvalidValue_ShouldAddError()
        {
            var parser = new DateTimeOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTime"), "dateTime");
            parser.DateTimeFormat = "ddd dd MMM yyyy h:mm tt";

            var tokens = new List<Token>
            {
                new OptionToken("dateTime")
                {
                    OptionValues = { "2020-12-31 23:59:59" }
                }
            };
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionValueInvalidFormatError>();

            var error = (OptionValueInvalidFormatError)parseResult.Errors[0];
            error.OptionName.Should().Be("dateTime");
            error.InvalidOptionValue.Should().Be("2020-12-31 23:59:59");
            error.ExpectedValueFormat.Should().Be("A valid date (and optionally time of day) in the format 'ddd dd MMM yyyy h:mm tt'");
            error.GetErrorMessage().Should().Be("The value '2020-12-31 23:59:59' of the option --dateTime has an invalid format. The expected format is: A valid date (and optionally time of day) in the format 'ddd dd MMM yyyy h:mm tt'.");
        }

        [Test(Description = "Parse should parse a valid date time in the correct custom format and put the date time value into the correct property of the options object.")]
        public void Parse_CustomFormat_ShouldParseAndPutDateTimeValueInOptionsObject()
        {
            var parser = new DateTimeOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTime"), "dateTime");
            parser.DateTimeFormat = "ddd dd MMM yyyy h:mm tt";

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

            parseResult.HasErrors.Should().BeFalse();
            dataTypesCommandOptions.DateTime.Should().Be(new DateTime(2008, 6, 15, 8, 30, 0));
        }
    }
}