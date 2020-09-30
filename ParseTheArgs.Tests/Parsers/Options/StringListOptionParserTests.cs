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
    public class StringListOptionParserTests
    {
        [Test(Description = "Parse should add an OptionValueMissingError error to the parse result when no option value was specified.")]
        public void Parse_MissingValue_ShouldAddError()
        {
            var parser = new StringListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Strings"), "strings");

            var tokens = new List<Token>
            {
                new OptionToken("strings")
            };
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionValueMissingError>();

            var error = (OptionValueMissingError) parseResult.Errors[0];
            error.OptionName.Should().Be("strings");
            error.GetErrorMessage().Should().Be("The option --strings requires a value, but no value was specified.");
        }

        [Test(Description = "Parse should add an OptionMissingError to the parse result when a required option was not specified.")]
        public void Parse_RequiredOptionMissing_ShouldAddError()
        {
            var parser = new StringListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Strings"), "strings");
            parser.IsOptionRequired = true;

            var tokens = new List<Token>();
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionMissingError>();

            var error = (OptionMissingError) parseResult.Errors[0];
            error.OptionName.Should().Be("strings");
            error.GetErrorMessage().Should().Be("The option --strings is required.");
        }
    }
}
