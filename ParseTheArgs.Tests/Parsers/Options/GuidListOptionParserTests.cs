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
    public class GuidListOptionParserTests
    {
        [SetUp]
        public void SetUp()
        {
            // We fix the current culture to en-US so that parsing of values (e.g. DateTime values) is done in a deterministic fashion.
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }

        [Test(Description = "Parse should add an OptionValueInvalidFormatError error to the parse result when one of the specified values is not a valid Guid.")]
        public void Parse_InvalidValue_ShouldAddError()
        {
            var parser = new GuidListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guids"), "guids");

            var tokens = new List<Token>
            {
                new OptionToken("guids")
                {
                    OptionValues = { "e202e3f3-5cd5-4f64-be6d-c27ce349ec5c", "NotAGuid" }
                }
            };
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionValueInvalidFormatError>();

            var error = (OptionValueInvalidFormatError)parseResult.Errors[0];
            error.OptionName.Should().Be("guids");
            error.InvalidOptionValue.Should().Be("NotAGuid");
            error.ExpectedValueFormat.Should().Be("A valid Guid");
            error.GetErrorMessage().Should().Be("The value 'NotAGuid' of the option --guids has an invalid format. The expected format is: A valid Guid.");
        }

        [Test(Description = "Parse should add an OptionValueInvalidFormatError error to the parse result when one of the specified values is not a valid Guid in the specified custom format.")]
        public void Parse_CustomFormatInvalidValue_ShouldAddError()
        {
            var parser = new GuidListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guids"), "guids");
            parser.GuidFormat = "N";

            var tokens = new List<Token>
            {
                new OptionToken("guids")
                {
                    OptionValues = { "e202e3f35cd54f64be6dc27ce349ec5c", "e202e3f3-5cd5-4f64-be6d-c27ce349ec5c" }
                }
            };
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionValueInvalidFormatError>();

            var error = (OptionValueInvalidFormatError)parseResult.Errors[0];
            error.OptionName.Should().Be("guids");
            error.InvalidOptionValue.Should().Be("e202e3f3-5cd5-4f64-be6d-c27ce349ec5c");
            error.ExpectedValueFormat.Should().Be("A valid Guid");
            error.GetErrorMessage().Should().Be("The value 'e202e3f3-5cd5-4f64-be6d-c27ce349ec5c' of the option --guids has an invalid format. The expected format is: A valid Guid.");
        }

        [Test(Description = "Parse should parse a valid Guids in the correct custom format and put the Guid values into the correct property of the options object.")]
        public void Parse_CustomFormat_ShouldParseAndPutEnumConstantInOptionsObject()
        {
            var parser = new GuidListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guids"), "guids");
            parser.GuidFormat = "N";

            var tokens = new List<Token>
            {
                new OptionToken("guids")
                {
                    OptionValues = { "e202e3f35cd54f64be6dc27ce349ec5c", "8d301042a7554c39ac9df350979ff858" }
                }
            };
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeFalse();
            parseResult.CommandOptions.Should().BeOfType<DataTypesCommandOptions>();

            var resultOptions = (DataTypesCommandOptions)parseResult.CommandOptions;
            resultOptions.Guids.Should().BeEquivalentTo(new Guid("e202e3f3-5cd5-4f64-be6d-c27ce349ec5c"), new Guid("8d301042-a755-4c39-ac9d-f350979ff858"));
        }
    }
}