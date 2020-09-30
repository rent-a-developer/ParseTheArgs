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
            error.ExpectedValueFormat.Should().Be("A valid DateTime");
            error.GetErrorMessage().Should().Be("The value 'NotADateTime' of the option --dateTime has an invalid format. The expected format is: A valid DateTime.");
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
            error.ExpectedValueFormat.Should().Be("A valid DateTime");
            error.GetErrorMessage().Should().Be("The value '2020-12-31 23:59:59' of the option --dateTime has an invalid format. The expected format is: A valid DateTime.");
        }

        [Test(Description = "Parse should parse a valid date time in the correct custom format and put the date time value into the correct property of the options object.")]
        public void Parse_CustomFormat_ShouldParseAndPutEnumConstantInOptionsObject()
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
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeFalse();
            parseResult.CommandOptions.Should().BeOfType<DataTypesCommandOptions>();

            var resultOptions = (DataTypesCommandOptions)parseResult.CommandOptions;
            resultOptions.DateTime.Should().Be(new DateTime(2008, 6, 15, 8, 30, 0));
        }
    }
}