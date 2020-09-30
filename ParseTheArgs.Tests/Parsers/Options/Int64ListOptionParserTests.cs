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
    public class Int64ListOptionParserTests
    {
        [SetUp]
        public void SetUp()
        {
            // We fix the current culture to en-US so that parsing of values (e.g. DateTime values) is done in a deterministic fashion.
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }

        [Test(Description = "Parse should add an OptionValueInvalidFormatError error to the parse result when one of the specified values is not a valid number.")]
        public void Parse_InvalidValue_ShouldAddError()
        {
            var parser = new Int64ListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Int64s"), "int64s");

            var tokens = new List<Token>
            {
                new OptionToken("int64s")
                {
                    OptionValues = { "1", "NotANumber" }
                }
            };
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionValueInvalidFormatError>();

            var error = (OptionValueInvalidFormatError) parseResult.Errors[0];
            error.OptionName.Should().Be("int64s");
            error.InvalidOptionValue.Should().Be("NotANumber");
            error.ExpectedValueFormat.Should().Be("An integer in the range from -9223372036854775808 to 9223372036854775807");
            error.GetErrorMessage().Should().Be("The value 'NotANumber' of the option --int64s has an invalid format. The expected format is: An integer in the range from -9223372036854775808 to 9223372036854775807.");
        }
    }
}