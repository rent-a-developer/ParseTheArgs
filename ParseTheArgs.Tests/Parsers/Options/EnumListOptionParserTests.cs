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
    public class EnumListOptionParserTests
    {
        [Test(Description = "Parse should parse the names of a valid enum members correctly and put the corresponding enum constants into the correct property of the options object.")]
        public void Parse_ValidValues_ShouldParseAndPutEnumConstantInOptionsObject()
        {
            var parser = new EnumListOptionParser<LogLevel>(typeof(DataTypesCommandOptions).GetProperty("Enums"), "logLevels");

            var tokens = new List<Token>
            {
                new OptionToken("logLevels")
                {
                    OptionValues = { "Debug", "Trace" }
                }
            };
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeFalse();
            parseResult.CommandOptions.Should().BeOfType<DataTypesCommandOptions>();
            var resultOptions = (DataTypesCommandOptions)parseResult.CommandOptions;
            resultOptions.Enums.Should().BeEquivalentTo(LogLevel.Debug, LogLevel.Trace);
        }

        [Test(Description = "Parse should add an OptionValueInvalidFormatError to the parse result when one of the specified values is not a invalid enum value.")]
        public void Parse_InvalidValue_ShouldAddError()
        {
            var parser = new EnumListOptionParser<LogLevel>(typeof(DataTypesCommandOptions).GetProperty("Enums"), "logLevels");

            var tokens = new List<Token>
            {
                new OptionToken("logLevels")
                {
                    OptionValues = { "Debug", "NonExistentLogLevel" }
                }
            };
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionValueInvalidFormatError>();

            var error = (OptionValueInvalidFormatError) parseResult.Errors[0];
            error.OptionName.Should().Be("logLevels");
            error.InvalidOptionValue.Should().Be("NonExistentLogLevel");
            error.ExpectedValueFormat.Should().Be("One of the valid values (see help)");
            error.GetErrorMessage().Should().Be("The value 'NonExistentLogLevel' of the option --logLevels has an invalid format. The expected format is: One of the valid values (see help).");
        }
    }
}