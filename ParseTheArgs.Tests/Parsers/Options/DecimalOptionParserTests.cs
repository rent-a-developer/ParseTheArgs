using System;
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
    public class DecimalOptionParserTests
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
            Invoking(() => new DecimalOptionParser(null, "decimal"))
                .Should()
                .Throw<ArgumentNullException>()
                .WithMessage(@"Value cannot be null.
Parameter name: targetProperty");
        }

        [Test(Description = "Constructor should throw an exception when the given option name is null or an empty string.")]
        public void Constructor_OptionNameIsNullOrEmpty_ShouldThrowException()
        {
            Invoking(() => new DecimalOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimal"), null))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"Value cannot be null or an empty string.
Parameter name: optionName");

            Invoking(() => new DecimalOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimal"), ""))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"Value cannot be null or an empty string.
Parameter name: optionName");
        }

        [Test(Description = "Constructor should throw an exception when the given target property has a incompatible data type.")]
        public void Constructor_IncompatibleTargetProperty_ShouldThrowException()
        {
            Invoking(() => new DecimalOptionParser(typeof(DataTypesCommandOptions).GetProperty("String"), "decimal"))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"The given target property has an incompatible property type. Expected type is System.Decimal or System.Nullable<System.Decimal>, actual type was System.String.
Parameter name: targetProperty");
        }

        [Test(Description = "TargetProperty should return the property that was specified via the constructor.")]
        public void TargetProperty_ShouldReturnPropertySpecifiedViaConstructor()
        {
            var parser = new DecimalOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimal"), "decimal");

            parser.TargetProperty.Should().BeSameAs(typeof(DataTypesCommandOptions).GetProperty("Decimal"));
        }

        [Test(Description = "OptionDefaultValue should return default(Decimal) initially.")]
        public void OptionDefaultValue_Initially_ShouldReturnDefaultOfDecimal()
        {
            var parser = new DecimalOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimal"), "decimal");

            parser.OptionDefaultValue.Should().Be(default);
        }

        [Test(Description = "OptionName should return the name that was specified via the constructor.")]
        public void OptionName_ShouldReturnNameSpecifiedViaConstructor()
        {
            var parser = new DecimalOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimal"), "decimal");

            parser.OptionName.Should().Be("decimal");
        }

        [Test(Description = "OptionType should return SingleValueOption.")]
        public void OptionType_ShouldReturnSingleValueOption()
        {
            var parser = new DecimalOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimal"), "decimal");

            parser.OptionType.Should().Be(OptionType.SingleValueOption);
        }

        [Test(Description = "IsOptionRequired should return false initially.")]
        public void IsOptionRequired_Initially_ShouldReturnFalse()
        {
            var parser = new DecimalOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimal"), "decimal");

            parser.IsOptionRequired.Should().BeFalse();
        }

        [Test(Description = "NumberStyles should return Any initially.")]
        public void NumberStyles_Initially_ShouldReturnAny()
        {
            var parser = new DecimalOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimal"), "decimal");

            parser.NumberStyles.Should().Be(NumberStyles.Any);
        }

        [Test(Description = "FormatProvider should return the value of CultureInfo.CurrentCulture initially.")]
        public void FormatProvider_Initially_ShouldReturnValueOfCurrentCulture()
        {
            var parser = new DecimalOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimal"), "decimal");

            parser.FormatProvider.Should().Be(new CultureInfo("en-US"));
        }

        [Test(Description = "GetHelpText should return the text that was set via the OptionHelp property.")]
        public void GetHelpText_ShouldReturnSpecifiedHelpText()
        {
            var parser = new DecimalOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimal"), "decimal");
            parser.OptionHelp = "Help text for option decimal.";

            parser.GetHelpText().Should().Be("Help text for option decimal.");
        }

        [Test(Description = "Parse should assign the specified default value to the target property when the option is not present in the command line.")]
        public void Parse_OptionNotPresent_ShouldAssignDefaultValueToTargetProperty()
        {
            var parser = new DecimalOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimal"), "decimal");
            parser.OptionDefaultValue = 123.456M;

            var tokens = new List<Token>();
            var parseResult = new ParseResult();
            var dataTypesCommandOptions = new DataTypesCommandOptions();
            parseResult.CommandOptions = dataTypesCommandOptions;

            parser.Parse(tokens, parseResult);

            dataTypesCommandOptions.Decimal.Should().Be(123.456M);
        }

        [Test(Description = "Parse should parse a valid option value using the value parser and assign it to the target property.")]
        public void Parse_ValidValue_ShouldParseAndAssignValueToTargetProperty()
        {
            var valueParser = A.Fake<ValueParser>();
            var parser = new DecimalOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimal"), "decimal");
            parser.ValueParser = valueParser;

            var tokens = new List<Token>
            {
                new OptionToken("decimal")
                {
                    OptionValues = { "123.456" }
                }
            };
            var parseResult = new ParseResult();
            var dataTypesCommandOptions = new DataTypesCommandOptions();
            parseResult.CommandOptions = dataTypesCommandOptions;

            Decimal decimalValue;
            A.CallTo(() => valueParser.TryParseDecimal("123.456", NumberStyles.Any, new CultureInfo("en-US"), out decimalValue))
                .Returns(true)
                .AssignsOutAndRefParameters(123.456M);

            parser.Parse(tokens, parseResult);

            dataTypesCommandOptions.Decimal.Should().Be(123.456M);

            A.CallTo(() => valueParser.TryParseDecimal("123.456", NumberStyles.Any, new CultureInfo("en-US"), out decimalValue)).MustHaveHappened();
        }

        [Test(Description = "Parse should pass the specified custom format settings to the value parser.")]
        public void Parse_CustomFormatSettings_ShouldPassCustomFormatToValueParser()
        {
            var valueParser = A.Fake<ValueParser>(ob => ob.CallsBaseMethods());
            var parser = new DecimalOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimal"), "decimal");
            parser.ValueParser = valueParser;
            parser.NumberStyles = NumberStyles.Currency;
            parser.FormatProvider = new CultureInfo("de-DE");

            var tokens = new List<Token>
            {
                new OptionToken("decimal")
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

        [Test(Description = "Parse should add an OptionValueInvalidFormatError error to the parse result when the specified value is not a valid decimal.")]
        public void Parse_InvalidValue_ShouldAddError()
        {
            var valueParser = A.Fake<ValueParser>();
            var parser = new DecimalOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimal"), "decimal");
            parser.ValueParser = valueParser;

            var tokens = new List<Token>
            {
                new OptionToken("decimal")
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
            error.OptionName.Should().Be("decimal");
            error.InvalidOptionValue.Should().Be("NotADecimal");
            error.ExpectedValueFormat.Should().Be("A decimal number in the range from -79228162514264337593543950335 to 79228162514264337593543950335");
            error.GetErrorMessage().Should().Be("The value 'NotADecimal' of the option --decimal has an invalid format. The expected format is: A decimal number in the range from -79228162514264337593543950335 to 79228162514264337593543950335.");
        }

        [Test(Description = "Parse should add an OptionMissingError error to the parse result when the option is required, but it was not supplied.")]
        public void Parse_RequiredOptionMissing_ShouldAddError()
        {
            var parser = new DecimalOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimal"), "decimal");
            parser.IsOptionRequired = true;

            var tokens = new List<Token>();
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionMissingError>();

            var error = (OptionMissingError)parseResult.Errors[0];
            error.OptionName.Should().Be("decimal");
            error.GetErrorMessage().Should().Be("The option --decimal is required.");
        }

        [Test(Description = "Parse should add an OptionValueMissingError error to the parse result when no value was supplied for the option.")]
        public void Parse_OptionValueMissing_ShouldAddError()
        {
            var parser = new DecimalOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimal"), "decimal");

            var tokens = new List<Token>
            {
                new OptionToken("decimal")
            };

            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionValueMissingError>();

            var error = (OptionValueMissingError)parseResult.Errors[0];
            error.OptionName.Should().Be("decimal");
            error.GetErrorMessage().Should().Be("The option --decimal requires a value, but no value was specified.");
        }

        [Test(Description = "Parse should add an OptionMultipleValuesError error to the parse result when more than one value was supplied for the option.")]
        public void Parse_MoreThanOneOptionValue_ShouldAddError()
        {
            var parser = new DecimalOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimal"), "decimal");

            var tokens = new List<Token>
            {
                new OptionToken("decimal")
                {
                    OptionValues = { "123.456", "456.789" }
                }
            };

            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionMultipleValuesError>();

            var error = (OptionMultipleValuesError)parseResult.Errors[0];
            error.OptionName.Should().Be("decimal");
            error.GetErrorMessage().Should().Be("Multiple values are given for the option --decimal, but the option expects a single value.");
        }
    }
}