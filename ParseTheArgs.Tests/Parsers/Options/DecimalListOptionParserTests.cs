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
using static FluentAssertions.FluentActions;

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
            Invoking(() => new DecimalListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Strings"), "decimals"))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"The given target property has an incompatible property type. Expected type is System.Collections.Generic.List<Decimal>, actual type was System.Collections.Generic.List`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]].
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
            parser.OptionHelp = "Help text for option decimals.";

            parser.GetHelpText().Should().Be("Help text for option decimals.");
        }

        [Test(Description = "Parse should parse a valid decimal value and assign it to the target property.")]
        public void Parse_ValidValue_ShouldParseAndPutDecimalValuesInOptionsObject()
        {
            var parser = new DecimalListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimals"), "decimals");

            var tokens = new List<Token>
            {
                new OptionToken("decimals")
                {
                    OptionValues = { "1", "1.23" }
                }
            };
            var parseResult = new ParseResult();
            var dataTypesCommandOptions = new DataTypesCommandOptions();
            parseResult.CommandOptions = dataTypesCommandOptions;

            parser.Parse(tokens, parseResult);

            dataTypesCommandOptions.Decimals.Should().BeEquivalentTo(1M, 1.23M);
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