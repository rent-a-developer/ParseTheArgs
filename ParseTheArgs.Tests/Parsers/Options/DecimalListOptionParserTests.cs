﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using ParseTheArgs.Errors;
using ParseTheArgs.Parsers.Options;
using ParseTheArgs.Tests.TestData;
using ParseTheArgs.Tokens;
using static FluentAssertions.FluentActions;

namespace ParseTheArgs.Tests.Parsers.Options
{
    [TestFixture]
    public class DecimalListOptionParserTests
    {
        [SetUp]
        public void SetUp()
        {
            // Fix the current culture to a known value.
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }

        [Test(Description = "Constructor should throw an exception when the given target property is null.")]
        public void Constructor_TargetPropertyIsNull_ShouldThrowException()
        {
            Invoking(() => new DecimalListOptionParser(null, "decimals"))
                .Should()
                .Throw<ArgumentNullException>()
                .WithMessage(@"Value cannot be null.
Parameter name: targetProperty");
        }

        [Test(Description = "Constructor should throw an exception when the given option name is null or an empty string.")]
        public void Constructor_OptionNameIsNullOrEmpty_ShouldThrowException()
        {
            Invoking(() => new DecimalListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimals"), null))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"Value cannot be null or an empty string.
Parameter name: optionName");

            Invoking(() => new DecimalListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimals"), ""))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"Value cannot be null or an empty string.
Parameter name: optionName");
        }

        [Test(Description = "Constructor should throw an exception when the given target property has a incompatible data type.")]
        public void Constructor_IncompatibleTargetProperty_ShouldThrowException()
        {
            Invoking(() => new DecimalListOptionParser(typeof(DataTypesCommandOptions).GetProperty("String"), "decimals"))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"The given target property has an incompatible property type. Expected type is System.Collections.Generic.List<Decimal>, actual type was System.String.
Parameter name: targetProperty");
        }

        [Test(Description = "TargetProperty should return the property that was specified via the constructor.")]
        public void TargetProperty_ShouldReturnPropertySpecifiedViaConstructor()
        {
            var parser = new DecimalListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimals"), "decimals");

            parser.TargetProperty.Should().BeSameAs(typeof(DataTypesCommandOptions).GetProperty("Decimals"));
        }

        [Test(Description = "OptionName should return the name that was specified via the constructor.")]
        public void OptionName_ShouldReturnNameSpecifiedViaConstructor()
        {
            var parser = new DecimalListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimals"), "decimals");

            parser.OptionName.Should().Be("decimals");
        }

        [Test(Description = "OptionType should return MultiValueOption.")]
        public void OptionType_ShouldReturnMultiValueOption()
        {
            var parser = new DecimalListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimals"), "decimals");

            parser.OptionType.Should().Be(OptionType.MultiValueOption);
        }

        [Test(Description = "IsOptionRequired should return false initially.")]
        public void IsOptionRequired_Initially_ShouldReturnFalse()
        {
            var parser = new DecimalListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimals"), "decimals");

            parser.IsOptionRequired.Should().BeFalse();
        }

        [Test(Description = "NumberStyles should return Any initially.")]
        public void NumberStyles_Initially_ShouldReturnAny()
        {
            var parser = new DecimalListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimals"), "decimals");

            parser.NumberStyles.Should().Be(NumberStyles.Any);
        }

        [Test(Description = "FormatProvider should return the value of CultureInfo.CurrentCulture initially.")]
        public void FormatProvider_Initially_ShouldReturnValueOfCurrentCulture()
        {
            var parser = new DecimalListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimals"), "decimals");

            parser.FormatProvider.Should().Be(new CultureInfo("en-US"));
        }

        [Test(Description = "GetHelpText should return the text that was set via the OptionHelp property.")]
        public void GetHelpText_ShouldReturnSpecifiedHelpText()
        {
            var parser = new DecimalListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimals"), "decimals");
            parser.OptionHelp = "Help text for option decimal.";

            parser.GetHelpText().Should().Be("Help text for option decimal.");
        }

        [Test(Description = "Parse should parse valid option values using the value parser and assign them to the target property.")]
        public void Parse_ValidValue_ShouldParseAndAssignValueToTargetProperty()
        {
            var valueParser = A.Fake<ValueParser>();
            var parser = new DecimalListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimals"), "decimals");
            parser.ValueParser = valueParser;

            var tokens = new List<Token>
            {
                new OptionToken("decimals")
                {
                    OptionValues = { "123.456", "514.548" }
                }
            };
            var parseResult = new ParseResult();
            var dataTypesCommandOptions = new DataTypesCommandOptions();
            parseResult.CommandOptions = dataTypesCommandOptions;

            Decimal decimalValue;

            A.CallTo(() => valueParser.TryParseDecimal("123.456", NumberStyles.Any, new CultureInfo("en-US"), out decimalValue))
                .Returns(true)
                .AssignsOutAndRefParameters(123.456M);

            A.CallTo(() => valueParser.TryParseDecimal("514.548", NumberStyles.Any, new CultureInfo("en-US"), out decimalValue))
                .Returns(true)
                .AssignsOutAndRefParameters(514.548M);

            parser.Parse(tokens, parseResult);

            dataTypesCommandOptions.Decimals.Should().BeEquivalentTo(123.456M, 514.548M);

            A.CallTo(() => valueParser.TryParseDecimal("123.456", NumberStyles.Any, new CultureInfo("en-US"), out decimalValue)).MustHaveHappened();
            A.CallTo(() => valueParser.TryParseDecimal("514.548", NumberStyles.Any, new CultureInfo("en-US"), out decimalValue)).MustHaveHappened();
        }

        [Test(Description = "Parse should pass the specified custom format settings to the value parser.")]
        public void Parse_CustomFormatSettings_ShouldPassCustomFormatToValueParser()
        {
            var valueParser = A.Fake<ValueParser>(ob => ob.CallsBaseMethods());
            var parser = new DecimalListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimals"), "decimals");
            parser.ValueParser = valueParser;
            parser.NumberStyles = NumberStyles.Currency;
            parser.FormatProvider = new CultureInfo("de-DE");

            var tokens = new List<Token>
            {
                new OptionToken("decimals")
                {
                    OptionValues = { "123.456" }
                }
            };
            var parseResult = new ParseResult();
            var dataTypesCommandOptions = new DataTypesCommandOptions();
            parseResult.CommandOptions = dataTypesCommandOptions;

            parser.Parse(tokens, parseResult);

            Decimal decimalValue;
            A.CallTo(() => valueParser.TryParseDecimal("123.456", NumberStyles.Currency, new CultureInfo("de-DE"), out decimalValue)).MustHaveHappened();
        }

        [Test(Description = "Parse should add an OptionValueInvalidFormatError error to the parse result when one of the specified values is not a valid decimal.")]
        public void Parse_InvalidValue_ShouldAddError()
        {
            var valueParser = A.Fake<ValueParser>();
            var parser = new DecimalListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimals"), "decimals");
            parser.ValueParser = valueParser;

            var tokens = new List<Token>
            {
                new OptionToken("decimals")
                {
                    OptionValues = { "NotADecimal" }
                }
            };
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            Decimal decimalValue;
            A.CallTo(() => valueParser.TryParseDecimal("NotADecimal", NumberStyles.None, new CultureInfo("en-US"), out decimalValue))
                .Returns(false);

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionValueInvalidFormatError>();

            var error = (OptionValueInvalidFormatError)parseResult.Errors[0];
            error.OptionName.Should().Be("decimals");
            error.InvalidOptionValue.Should().Be("NotADecimal");
            error.ExpectedValueFormat.Should().Be("A decimal number in the range from -79228162514264337593543950335 to 79228162514264337593543950335");
            error.GetErrorMessage().Should().Be("The value 'NotADecimal' of the option --decimals has an invalid format. The expected format is: A decimal number in the range from -79228162514264337593543950335 to 79228162514264337593543950335.");
        }
    }
}