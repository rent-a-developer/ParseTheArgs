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
    public class DateTimeListOptionParserTests
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
            Invoking(() => new DateTimeListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Strings"), "dateTimes"))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"The given target property has an incompatible property type. Expected type is System.Collections.Generic.List<System.DateTime>, actual type was System.Collections.Generic.List`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]].
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
        public void OptionType_ShouldReturnMultiValueOption()
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

        [Test(Description = "DateTimeFormat should return null initially.")]
        public void DateTimeFormat_Initially_ShouldReturnNull()
        {
            var parser = new DateTimeListOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTimes"), "dateTimes");

            parser.DateTimeFormat.Should().BeNull();
        }

        [Test(Description = "DateTimeStyles should return None initially.")]
        public void DateTimeStyles_Initially_ShouldReturnNone()
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

        [Test(Description = "GetHelpText should return the text that was set via the OptionHelp property.")]
        public void GetHelpText_ShouldReturnSpecifiedHelpText()
        {
            var parser = new DateTimeListOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTimes"), "dateTimes");
            parser.OptionHelp = "Help text for option dateTimes.";

            parser.GetHelpText().Should().Be("Help text for option dateTimes.");
        }

        [Test(Description = "Parse should parse valid option values using the value parser and assign them to the target property.")]
        public void Parse_ValidValue_ShouldParseAndAssignValueToTargetProperty()
        {
            var valueParser = A.Fake<ValueParser>();
            var parser = new DateTimeListOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTimes"), "dateTimes");
            parser.ValueParser = valueParser;

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

            DateTime dateTime;

            A.CallTo(() => valueParser.TryParseDateTime("2020-12-31 23:59:59", null, new CultureInfo("en-US"), DateTimeStyles.None, out dateTime))
                .Returns(true)
                .AssignsOutAndRefParameters(new DateTime(2020, 12, 31, 23, 59, 59));

            A.CallTo(() => valueParser.TryParseDateTime("2020-01-01 10:30:59", null, new CultureInfo("en-US"), DateTimeStyles.None, out dateTime))
                .Returns(true)
                .AssignsOutAndRefParameters(new DateTime(2020, 1, 1, 10, 30, 59));

            parser.Parse(tokens, parseResult);

            dataTypesCommandOptions.DateTimes.Should().BeEquivalentTo(new DateTime(2020, 12, 31, 23, 59, 59), new DateTime(2020, 1, 1, 10, 30, 59));

            A.CallTo(() => valueParser.TryParseDateTime("2020-12-31 23:59:59", null, new CultureInfo("en-US"), DateTimeStyles.None, out dateTime)).MustHaveHappened();
            A.CallTo(() => valueParser.TryParseDateTime("2020-01-01 10:30:59", null, new CultureInfo("en-US"), DateTimeStyles.None, out dateTime)).MustHaveHappened();
        }

        [Test(Description = "Parse should pass the specified custom format settings to the value parser.")]
        public void Parse_CustomFormatSettings_ShouldPassCustomFormatToValueParser()
        {
            var valueParser = A.Fake<ValueParser>(ob => ob.CallsBaseMethods());
            var parser = new DateTimeListOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTimes"), "dateTimes");
            parser.ValueParser = valueParser;
            parser.DateTimeFormat = "ddd dd MMM yyyy h:mm tt";
            parser.DateTimeStyles = DateTimeStyles.NoCurrentDateDefault;
            parser.FormatProvider = new CultureInfo("de-DE");

            var tokens = new List<Token>
            {
                new OptionToken("dateTimes")
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

        [Test(Description = "Parse should add an OptionValueInvalidFormatError error to the parse result when one of the specified values is not a valid date time.")]
        public void Parse_InvalidValue_ShouldAddError()
        {
            var valueParser = A.Fake<ValueParser>();
            var parser = new DateTimeListOptionParser(typeof(DataTypesCommandOptions).GetProperty("DateTimes"), "dateTimes");
            parser.ValueParser = valueParser;

            var tokens = new List<Token>
            {
                new OptionToken("dateTimes")
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
            error.OptionName.Should().Be("dateTimes");
            error.InvalidOptionValue.Should().Be("NotADateTime");
            error.ExpectedValueFormat.Should().Be("A valid date (and optionally time of day)");
            error.GetErrorMessage().Should().Be("The value 'NotADateTime' of the option --dateTimes has an invalid format. The expected format is: A valid date (and optionally time of day).");
        }
    }
}