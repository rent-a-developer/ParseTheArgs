using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using ParseTheArgs.Errors;
using ParseTheArgs.Parsers.Options;
using ParseTheArgs.Tests.TestData;
using ParseTheArgs.Tokens;

namespace ParseTheArgs.Tests.Parsers.Options
{
    [TestFixture]
    public class StringOptionParserTests
    {
        [Test(Description = "Parse should add an OptionValueMissingError error to the parse result when no option value was specified.")]
        public void Parse_MissingValue_ShouldAddError()
        {
            var parser = new StringOptionParser(typeof(DataTypesCommandOptions).GetProperty("String"), "string");

            var tokens = new List<Token>
            {
                new OptionToken("string")
            };
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionValueMissingError>();

            var error = (OptionValueMissingError) parseResult.Errors[0];
            error.OptionName.Should().Be("string");
            error.GetErrorMessage().Should().Be("The option --string requires a value, but no value was specified.");
        }

        [Test(Description = "Parse should add an OptionMultipleValuesError error to the parse result when more than one value was specified for a option that only expects a single value.")]
        public void Parse_MultipleValues_ShouldAddError()
        {
            var parser = new StringOptionParser(typeof(DataTypesCommandOptions).GetProperty("String"), "string");

            var tokens = new List<Token>
            {
                new OptionToken("string")
                {
                    OptionValues = { "a", "b" }
                }
            };
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionMultipleValuesError>();

            var error = (OptionMultipleValuesError) parseResult.Errors[0];
            error.OptionName.Should().Be("string");
            error.GetErrorMessage().Should().Be("Multiple values are given for the option --string, but the option expects a single value.");
        }

        [Test(Description = "Parse should add an OptionMissingError to the parse result when a required option was not specified.")]
        public void Parse_RequiredOptionMissing_ShouldAddError()
        {
            var parser = new StringOptionParser(typeof(DataTypesCommandOptions).GetProperty("String"), "string");
            parser.IsOptionRequired = true;

            var tokens = new List<Token>();
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionMissingError>();

            var error = (OptionMissingError) parseResult.Errors[0];
            error.OptionName.Should().Be("string");
            error.GetErrorMessage().Should().Be("The option --string is required.");
        }
    }
}