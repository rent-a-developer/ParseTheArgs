﻿using System;
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
    public class DecimalListOptionParserTests
    {
        [SetUp]
        public void SetUp()
        {
            // We fix the current culture to en-US so that parsing of values (e.g. DateTime values) is done in a deterministic fashion.
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }

        [Test(Description = "Parse should add an OptionValueInvalidFormatError error to the parse result when one of the specified values is not a valid decimal number.")]
        public void Parse_InvalidValue_ShouldAddError()
        {
            var parser = new DecimalListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimals"), "decimals");

            var tokens = new List<Token>
            {
                new OptionToken("decimals")
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
            error.OptionName.Should().Be("decimals");
            error.InvalidOptionValue.Should().Be("NotANumber");
            error.ExpectedValueFormat.Should().Be("A decimal number in the range from -79228162514264337593543950335 to 79228162514264337593543950335");
            error.GetErrorMessage().Should().Be("The value 'NotANumber' of the option --decimals has an invalid format. The expected format is: A decimal number in the range from -79228162514264337593543950335 to 79228162514264337593543950335.");
        }
    }
}