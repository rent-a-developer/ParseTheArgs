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
    public class EnumOptionParserTests
    {
        [Test(Description = "Parse should parse the name of a valid enum member correctly and put the corresponding enum constant into the correct property of the options object.")]
        public void Parse_ValidValue_ShouldParseAndPutEnumConstantInOptionsObject()
        {
            var parser = new EnumOptionParser<LogLevel>(typeof(DataTypesCommandOptions).GetProperty("Enum"), "logLevel");

            var tokens = new List<Token>
            {
                new OptionToken("logLevel")
                {
                    OptionValues = { "Debug" }
                }
            };
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeFalse();
            parseResult.CommandOptions.Should().BeOfType<DataTypesCommandOptions>();
            var resultOptions = (DataTypesCommandOptions)parseResult.CommandOptions;
            resultOptions.Enum.Should().Be(LogLevel.Debug);
        }

        [Test(Description = "Parse should add an OptionValueInvalidFormatError to the parse result when a invalid enum value was specified.")]
        public void Parse_InvalidValue_ShouldAddError()
        {
            var parser = new EnumOptionParser<LogLevel>(typeof(DataTypesCommandOptions).GetProperty("Enum"), "logLevel");

            var tokens = new List<Token>
            {
                new OptionToken("logLevel")
                {
                    OptionValues = { "NonExistentLogLevel" }
                }
            };
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionValueInvalidFormatError>();

            var error = (OptionValueInvalidFormatError) parseResult.Errors[0];
            error.OptionName.Should().Be("logLevel");
            error.InvalidOptionValue.Should().Be("NonExistentLogLevel");
            error.ExpectedValueFormat.Should().Be("One of the valid values (see help)");
            error.GetErrorMessage().Should().Be("The value 'NonExistentLogLevel' of the option --logLevel has an invalid format. The expected format is: One of the valid values (see help).");
        }
    }
}
