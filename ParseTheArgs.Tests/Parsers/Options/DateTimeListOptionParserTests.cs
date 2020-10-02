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
    public class DateTimeListOptionParserTests
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
            Invoking(() => new DateTimeListOptionParser(null, "dateTimes"))
                .Should()
                .Throw<ArgumentNullException>()
                .WithMessage(@"Value cannot be null.
Parameter name: targetProperty");
        }

        [Test(Description = "Constructor should throw an exception when the given option name is null or an empty string.")]
        public void Constructor_OptionNameIsNullOrEmpty_ShouldThrowException()
        {
            Invoking(() => new DateTimeListOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTimes"), null))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"Value cannot be null or an empty string.
Parameter name: optionName");

            Invoking(() => new DateTimeListOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTimes"), ""))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"Value cannot be null or an empty string.
Parameter name: optionName");
        }

        [Test(Description = "Constructor should throw an exception when the given target property has a incompatible data type.")]
        public void Constructor_IncompatibleTargetProperty_ShouldThrowException()
        {
            Invoking(() => new DateTimeListOptionParser(typeof(DataTypesCommandOptions).GetProperty("String"), "dateTimes"))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"The given target property has an incompatible property type. Expected type is System.Collections.Generic.List<DateTime>, actual type was System.String.
Parameter name: targetProperty");
        }

        [Test(Description = "TargetProperty should return the property that was specified via the constructor.")]
        public void TargetProperty_ShouldReturnPropertySpecifiedViaConstructor()
        {
            var parser = new DateTimeListOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTimes"), "dateTimes");

            parser.TargetProperty.Should().BeSameAs(typeof(DataTypesCommandOptions).GetProperty("DateTimes"));
        }

        [Test(Description = "OptionName should return the name that was specified via the constructor.")]
        public void OptionName_ShouldReturnNameSpecifiedViaConstructor()
        {
            var parser = new DateTimeListOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTimes"), "dateTimes");

            parser.OptionName.Should().Be("dateTimes");
        }

        [Test(Description = "OptionType should return MultiValueOption.")]
        public void OptionType_ShouldReturnValuelessOption()
        {
            var parser = new DateTimeListOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTimes"), "dateTimes");

            parser.OptionType.Should().Be(OptionType.MultiValueOption);
        }

        [Test(Description = "IsOptionRequired should return false initially.")]
        public void IsOptionRequired_Initially_ShouldReturnFalse()
        {
            var parser = new DateTimeListOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTimes"), "dateTimes");

            parser.IsOptionRequired.Should().BeFalse();
        }

        [Test(Description = "DateTimeFormat should return None initially.")]
        public void DateTimeFormat_Initially_ShouldReturnNone()
        {
            var parser = new DateTimeListOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTimes"), "dateTimes");

            parser.DateTimeStyles.Should().Be(DateTimeStyles.None);
        }

        [Test(Description = "FormatProvider should return the value of CultureInfo.CurrentCulture initially.")]
        public void FormatProvider_Initially_ShouldReturnValueOfCurrentCulture()
        {
            var parser = new DateTimeListOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTimes"), "dateTimes");

            parser.FormatProvider.Should().Be(new CultureInfo("en-US"));
        }

        [Test(Description = "DateTimeStyles should return null initially.")]
        public void DateTimeStyles_Initially_ShouldReturnNull()
        {
            var parser = new DateTimeListOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTimes"), "dateTimes");

            parser.DateTimeFormat.Should().BeNull();
        }

        [Test(Description = "GetHelpText should return the text that was set via the OptionHelp property.")]
        public void GetHelpText_ShouldReturnSpecifiedHelpText()
        {
            var parser = new DateTimeListOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTimes"), "dateTimes");
            parser.OptionHelp = "Help text for option dateTime.";

            parser.GetHelpText().Should().Be("Help text for option dateTime.");
        }

        [Test(Description = "Parse should parse valid date times in the correct default format and put the date time values into the correct property of the options object.")]
        public void Parse_DefaultFormat_ShouldParseAndPutDateTimeValuesInOptionsObject()
        {
            var parser = new DateTimeListOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTimes"), "dateTimes");

            var tokens = new List<Token>
            {
                new OptionToken("dateTimes")
                {
                    OptionValues = { "2020-12-31 23:59:59", "2020-01-01 10:30:59" }
                }
            };
            var parseResult = new ParseResult();
            var dataTypesCommandOptions = new DataTypesCommandOptions();
            parseResult.CommandOptions = dataTypesCommandOptions;

            parser.Parse(tokens, parseResult);

            dataTypesCommandOptions.DateTimes.Should().BeEquivalentTo(new DateTime(2020, 12, 31, 23, 59, 59), new DateTime(2020, 01, 01, 10, 30, 59));
        }

        [Test(Description = "Parse should add an OptionValueInvalidFormatError error to the parse result when one of the specified values is not a valid date time.")]
        public void Parse_InvalidValue_ShouldAddError()
        {
            var parser = new DateTimeListOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTimes"), "dateTimes");

            var tokens = new List<Token>
            {
                new OptionToken("dateTimes")
                {
                    OptionValues = { "2020-12-31 23:59:59", "NotADateTime" }
                }
            };
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionValueInvalidFormatError>();

            var error = (OptionValueInvalidFormatError) parseResult.Errors[0];
            error.OptionName.Should().Be("dateTimes");
            error.InvalidOptionValue.Should().Be("NotADateTime");
            error.ExpectedValueFormat.Should().Be("A valid DateTime");
            error.GetErrorMessage().Should().Be("The value 'NotADateTime' of the option --dateTimes has an invalid format. The expected format is: A valid DateTime.");
        }

        [Test(Description = "Parse should add an OptionValueInvalidFormatError error to the parse result when the specified value is not a valid date time in the specified custom format.")]
        public void Parse_CustomFormatInvalidValue_ShouldAddError()
        {
            var parser = new DateTimeListOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTimes"), "dateTimes");
            parser.DateTimeFormat = "ddd dd MMM yyyy h:mm tt";

            var tokens = new List<Token>
            {
                new OptionToken("dateTimes")
                {
                    OptionValues = { "2020-12-31 23:59:59", "Sun 15 Jun 2008 8:30 AM" }
                }
            };
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionValueInvalidFormatError>();

            var error = (OptionValueInvalidFormatError)parseResult.Errors[0];
            error.OptionName.Should().Be("dateTimes");
            error.InvalidOptionValue.Should().Be("2020-12-31 23:59:59");
            error.ExpectedValueFormat.Should().Be("A valid DateTime");
            error.GetErrorMessage().Should().Be("The value '2020-12-31 23:59:59' of the option --dateTimes has an invalid format. The expected format is: A valid DateTime.");
        }

        [Test(Description = "Parse should parse a valid date times in the correct custom format and put the date time values into the correct property of the options object.")]
        public void Parse_CustomFormat_ShouldParseAndPutDateTimeValuesInOptionsObject()
        {
            var parser = new DateTimeListOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTimes"), "dateTimes");
            parser.DateTimeFormat = "ddd dd MMM yyyy h:mm tt";

            var tokens = new List<Token>
            {
                new OptionToken("dateTimes")
                {
                    OptionValues = { "Sat 14 Jun 2008 7:30 AM", "Sun 15 Jun 2008 8:30 AM" }
                }
            };
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeFalse();
            parseResult.CommandOptions.Should().BeOfType<DataTypesCommandOptions>();

            var resultOptions = (DataTypesCommandOptions)parseResult.CommandOptions;
            resultOptions.DateTimes.Should().BeEquivalentTo(new DateTime(2008, 6, 14, 7, 30, 0), new DateTime(2008, 6, 15, 8, 30, 0));
        }
    }
}