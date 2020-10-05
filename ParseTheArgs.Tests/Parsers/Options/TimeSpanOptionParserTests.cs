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
    public class TimeSpanOptionParserTests
    {
        [SetUp]
        public void SetUp()
        {
            // Fix the current culture to a known value.
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }

        [Test(Description = "Parse should add an OptionValueInvalidFormatError error to the parse result when the specified value is not a valid time span.")]
        public void Parse_InvalidValue_ShouldAddError()
        {
            var parser = new TimeSpanOptionParser(typeof(DataTypesCommandOptions).GetProperty("TimeSpan"), "timeSpan");

            var tokens = new List<Token>
            {
                new OptionToken("timeSpan")
                {
                    OptionValues = { "NotATimeSpan" }
                }
            };
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionValueInvalidFormatError>();

            var error = (OptionValueInvalidFormatError)parseResult.Errors[0];
            error.OptionName.Should().Be("timeSpan");
            error.InvalidOptionValue.Should().Be("NotATimeSpan");
            error.ExpectedValueFormat.Should().Be("A valid time interval");
            error.GetErrorMessage().Should().Be("The value 'NotATimeSpan' of the option --timeSpan has an invalid format. The expected format is: A valid time interval.");
        }

        [Test(Description = "Parse should add an OptionValueInvalidFormatError error to the parse result when the specified value is not a valid time span in the specified custom format.")]
        public void Parse_CustomFormatInvalidValue_ShouldAddError()
        {
            var parser = new TimeSpanOptionParser(typeof(DataTypesCommandOptions).GetProperty("TimeSpan"), "timeSpan");
            parser.TimeSpanFormat = @"h\:mm";

            var tokens = new List<Token>
            {
                new OptionToken("timeSpan")
                {
                    OptionValues = { "23:59:59" }
                }
            };
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionValueInvalidFormatError>();

            var error = (OptionValueInvalidFormatError)parseResult.Errors[0];
            error.OptionName.Should().Be("timeSpan");
            error.InvalidOptionValue.Should().Be("23:59:59");
            error.ExpectedValueFormat.Should().Be(@"A valid time interval in the format 'h\:mm'");
            error.GetErrorMessage().Should().Be(@"The value '23:59:59' of the option --timeSpan has an invalid format. The expected format is: A valid time interval in the format 'h\:mm'.");
        }

        [Test(Description = "Parse should parse a valid time span in the correct custom format and put the time span value into the correct property of the options object.")]
        public void Parse_CustomFormat_ShouldParseAndPutTimeSpanValueInOptionsObject()
        {
            var parser = new TimeSpanOptionParser(typeof(DataTypesCommandOptions).GetProperty("TimeSpan"), "timeSpan");
            parser.TimeSpanFormat = @"h\:mm";

            var tokens = new List<Token>
            {
                new OptionToken("timeSpan")
                {
                    OptionValues = { "23:59" }
                }
            };
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeFalse();
            parseResult.CommandOptions.Should().BeOfType<DataTypesCommandOptions>();

            var resultOptions = (DataTypesCommandOptions)parseResult.CommandOptions;
            resultOptions.TimeSpan.Should().Be(new TimeSpan(23, 59, 0));
        }
    }
}