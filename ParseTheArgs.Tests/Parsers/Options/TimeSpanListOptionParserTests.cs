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
    public class TimeSpanListOptionParserTests
    {
        [SetUp]
        public void SetUp()
        {
            // Fix the current culture to a known value.
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }

        [Test(Description = "Parse should add an OptionValueInvalidFormatError error to the parse result when one of the specified values is not a valid time span.")]
        public void Parse_InvalidValue_ShouldAddError()
        {
            var parser = new TimeSpanListOptionParser(typeof(DataTypesCommandOptions).GetProperty("TimeSpans"), "timeSpans");

            var tokens = new List<Token>
            {
                new OptionToken("timeSpans")
                {
                    OptionValues = { "23:59:59", "NotATimeSpan" }
                }
            };
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionValueInvalidFormatError>();

            var error = (OptionValueInvalidFormatError)parseResult.Errors[0];
            error.OptionName.Should().Be("timeSpans");
            error.InvalidOptionValue.Should().Be("NotATimeSpan");
            error.ExpectedValueFormat.Should().Be("A valid time interval");
            error.GetErrorMessage().Should().Be("The value 'NotATimeSpan' of the option --timeSpans has an invalid format. The expected format is: A valid time interval.");
        }

        [Test(Description = "Parse should add an OptionValueInvalidFormatError error to the parse result when one of the specified values is not a valid time span in the specified custom format.")]
        public void Parse_CustomFormatInvalidValue_ShouldAddError()
        {
            var parser = new TimeSpanListOptionParser(typeof(DataTypesCommandOptions).GetProperty("TimeSpans"), "timeSpans");
            parser.TimeSpanFormat = @"h\:mm";

            var tokens = new List<Token>
            {
                new OptionToken("timeSpans")
                {
                    OptionValues = { "23:59", "23:59:59" }
                }
            };
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionValueInvalidFormatError>();

            var error = (OptionValueInvalidFormatError)parseResult.Errors[0];
            error.OptionName.Should().Be("timeSpans");
            error.InvalidOptionValue.Should().Be("23:59:59");
            error.ExpectedValueFormat.Should().Be(@"A valid time interval in the format 'h\:mm'");
            error.GetErrorMessage().Should().Be(@"The value '23:59:59' of the option --timeSpans has an invalid format. The expected format is: A valid time interval in the format 'h\:mm'.");
        }

        [Test(Description = "Parse should parse valid time spans in the correct custom format and put the time span values into the correct property of the options object.")]
        public void Parse_CustomFormat_ShouldParseAndPutTimeSpanValuesInOptionsObject()
        {
            var parser = new TimeSpanListOptionParser(typeof(DataTypesCommandOptions).GetProperty("TimeSpans"), "timeSpans");
            parser.TimeSpanFormat = @"h\:mm";

            var tokens = new List<Token>
            {
                new OptionToken("timeSpans")
                {
                    OptionValues = { "23:59", "10:30" }
                }
            };
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeFalse();
            parseResult.CommandOptions.Should().BeOfType<DataTypesCommandOptions>();

            var resultOptions = (DataTypesCommandOptions)parseResult.CommandOptions;
            resultOptions.TimeSpans.Should().BeEquivalentTo(new TimeSpan(23, 59, 0), new TimeSpan(10, 30, 0));
        }
    }
}