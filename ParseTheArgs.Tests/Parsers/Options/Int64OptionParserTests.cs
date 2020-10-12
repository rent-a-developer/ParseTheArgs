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
    public class Int64OptionParserTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            // Fix the current culture to a known value.
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }

        #endregion

        [Test(Description = "Constructor should throw an exception when the given target property has a incompatible data type.")]
        public void Constructor_IncompatibleTargetProperty_ShouldThrowException()
        {
            Invoking(() => new Int64OptionParser(typeof(DataTypesCommandOptions).GetProperty("String"), "int64"))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"The given target property has an incompatible property type. Expected type is System.Int64 or System.Nullable<System.Int64>, actual type was System.String.
Parameter name: targetProperty");
        }

        [Test(Description = "Constructor should throw an exception when the given option name is null or an empty string.")]
        public void Constructor_OptionNameIsNullOrEmpty_ShouldThrowException()
        {
            Invoking(() => new Int64OptionParser(typeof(DataTypesCommandOptions).GetProperty("Int64"), null))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"Value cannot be null or an empty string.
Parameter name: optionName");

            Invoking(() => new Int64OptionParser(typeof(DataTypesCommandOptions).GetProperty("Int64"), ""))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"Value cannot be null or an empty string.
Parameter name: optionName");
        }

        [Test(Description = "Constructor should throw an exception when the given target property is null.")]
        public void Constructor_TargetPropertyIsNull_ShouldThrowException()
        {
            Invoking(() => new Int64OptionParser(null, "int64"))
                .Should()
                .Throw<ArgumentNullException>()
                .WithMessage(@"Value cannot be null.
Parameter name: targetProperty");
        }

        [Test(Description = "FormatProvider should return the value of CultureInfo.CurrentCulture initially.")]
        public void FormatProvider_Initially_ShouldReturnValueOfCurrentCulture()
        {
            var parser = new Int64OptionParser(typeof(DataTypesCommandOptions).GetProperty("Int64"), "int64");

            parser.FormatProvider.Should().Be(new CultureInfo("en-US"));
        }

        [Test(Description = "GetHelpText should return the text that was set via the OptionHelp property.")]
        public void GetHelpText_ShouldReturnSpecifiedHelpText()
        {
            var parser = new Int64OptionParser(typeof(DataTypesCommandOptions).GetProperty("Int64"), "int64");
            parser.OptionHelp = "Help text for option int64.";

            parser.GetHelpText().Should().Be("Help text for option int64.");
        }

        [Test(Description = "IsOptionRequired should return false initially.")]
        public void IsOptionRequired_Initially_ShouldReturnFalse()
        {
            var parser = new Int64OptionParser(typeof(DataTypesCommandOptions).GetProperty("Int64"), "int64");

            parser.IsOptionRequired.Should().BeFalse();
        }

        [Test(Description = "NumberStyles should return Any initially.")]
        public void NumberStyles_Initially_ShouldReturnAny()
        {
            var parser = new Int64OptionParser(typeof(DataTypesCommandOptions).GetProperty("Int64"), "int64");

            parser.NumberStyles.Should().Be(NumberStyles.Any);
        }

        [Test(Description = "OptionDefaultValue should return default(Int64) initially.")]
        public void OptionDefaultValue_Initially_ShouldReturnDefaultOfInt64()
        {
            var parser = new Int64OptionParser(typeof(DataTypesCommandOptions).GetProperty("Int64"), "int64");

            parser.OptionDefaultValue.Should().Be(default);
        }

        [Test(Description = "OptionName should return the name that was specified via the constructor.")]
        public void OptionName_ShouldReturnNameSpecifiedViaConstructor()
        {
            var parser = new Int64OptionParser(typeof(DataTypesCommandOptions).GetProperty("Int64"), "int64");

            parser.OptionName.Should().Be("int64");
        }

        [Test(Description = "OptionType should return SingleValueOption.")]
        public void OptionType_ShouldReturnSingleValueOption()
        {
            var parser = new Int64OptionParser(typeof(DataTypesCommandOptions).GetProperty("Int64"), "int64");

            parser.OptionType.Should().Be(OptionType.SingleValueOption);
        }

        [Test(Description = "Parse should pass the specified custom format settings to the value parser.")]
        public void Parse_CustomFormatSettings_ShouldPassCustomFormatToValueParser()
        {
            var valueParser = A.Fake<ValueParser>(ob => ob.CallsBaseMethods());
            var parser = new Int64OptionParser(typeof(DataTypesCommandOptions).GetProperty("Int64"), "int64");
            parser.ValueParser = valueParser;
            parser.NumberStyles = NumberStyles.Currency;
            parser.FormatProvider = new CultureInfo("de-DE");

            var tokens = new List<Token>
            {
                new OptionToken("int64")
                {
                    OptionValues = {"123"}
                }
            };
            var parseResult = new ParseResult();
            var dataTypesCommandOptions = new DataTypesCommandOptions();
            parseResult.CommandOptions = dataTypesCommandOptions;

            parser.Parse(tokens, parseResult);

            Int64 int64;
            A.CallTo(() => valueParser.TryParseInt64("123", NumberStyles.Currency, new CultureInfo("de-DE"), out int64)).MustHaveHappened();
        }

        [Test(Description = "Parse should add an OptionValueInvalidFormatError error to the parse result when the specified value is not a valid int64.")]
        public void Parse_InvalidValue_ShouldAddError()
        {
            var valueParser = A.Fake<ValueParser>();
            var parser = new Int64OptionParser(typeof(DataTypesCommandOptions).GetProperty("Int64"), "int64");
            parser.ValueParser = valueParser;

            var tokens = new List<Token>
            {
                new OptionToken("int64")
                {
                    OptionValues = {"NotAInt64"}
                }
            };
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            Int64 int64;
            A.CallTo(() => valueParser.TryParseInt64("NotAInt64", NumberStyles.None, new CultureInfo("en-US"), out int64))
                .Returns(false);

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionValueInvalidFormatError>();

            var error = (OptionValueInvalidFormatError) parseResult.Errors[0];
            error.OptionName.Should().Be("int64");
            error.InvalidOptionValue.Should().Be("NotAInt64");
            error.ExpectedValueFormat.Should().Be("An integer in the range from -9223372036854775808 to 9223372036854775807");
            error.GetErrorMessage().Should().Be("The value 'NotAInt64' of the option --int64 has an invalid format. The expected format is: An integer in the range from -9223372036854775808 to 9223372036854775807.");
        }

        [Test(Description = "Parse should add an OptionMultipleValuesError error to the parse result when more than one value was supplied for the option.")]
        public void Parse_MoreThanOneOptionValue_ShouldAddError()
        {
            var parser = new Int64OptionParser(typeof(DataTypesCommandOptions).GetProperty("Int64"), "int64");

            var tokens = new List<Token>
            {
                new OptionToken("int64")
                {
                    OptionValues = {"123", "456"}
                }
            };

            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionMultipleValuesError>();

            var error = (OptionMultipleValuesError) parseResult.Errors[0];
            error.OptionName.Should().Be("int64");
            error.GetErrorMessage().Should().Be("Multiple values are given for the option --int64, but the option expects a single value.");
        }

        [Test(Description = "Parse should assign the specified default value to the target property when the option is not present in the command line.")]
        public void Parse_OptionNotPresent_ShouldAssignDefaultValueToTargetProperty()
        {
            var parser = new Int64OptionParser(typeof(DataTypesCommandOptions).GetProperty("Int64"), "int64");
            parser.OptionDefaultValue = 123L;

            var tokens = new List<Token>();
            var parseResult = new ParseResult();
            var dataTypesCommandOptions = new DataTypesCommandOptions();
            parseResult.CommandOptions = dataTypesCommandOptions;

            parser.Parse(tokens, parseResult);

            dataTypesCommandOptions.Int64.Should().Be(123L);
        }

        [Test(Description = "Parse should add an OptionValueMissingError error to the parse result when no value was supplied for the option.")]
        public void Parse_OptionValueMissing_ShouldAddError()
        {
            var parser = new Int64OptionParser(typeof(DataTypesCommandOptions).GetProperty("Int64"), "int64");

            var tokens = new List<Token>
            {
                new OptionToken("int64")
            };

            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionValueMissingError>();

            var error = (OptionValueMissingError) parseResult.Errors[0];
            error.OptionName.Should().Be("int64");
            error.GetErrorMessage().Should().Be("The option --int64 requires a value, but no value was specified.");
        }

        [Test(Description = "Parse should add an OptionMissingError error to the parse result when the option is required, but it was not supplied.")]
        public void Parse_RequiredOptionMissing_ShouldAddError()
        {
            var parser = new Int64OptionParser(typeof(DataTypesCommandOptions).GetProperty("Int64"), "int64");
            parser.IsOptionRequired = true;

            var tokens = new List<Token>();
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionMissingError>();

            var error = (OptionMissingError) parseResult.Errors[0];
            error.OptionName.Should().Be("int64");
            error.GetErrorMessage().Should().Be("The option --int64 is required.");
        }

        [Test(Description = "Parse should parse a valid option value using the value parser and assign it to the target property.")]
        public void Parse_ValidValue_ShouldParseAndAssignValueToTargetProperty()
        {
            var valueParser = A.Fake<ValueParser>();
            var parser = new Int64OptionParser(typeof(DataTypesCommandOptions).GetProperty("Int64"), "int64");
            parser.ValueParser = valueParser;

            var tokens = new List<Token>
            {
                new OptionToken("int64")
                {
                    OptionValues = {"123"}
                }
            };
            var parseResult = new ParseResult();
            var dataTypesCommandOptions = new DataTypesCommandOptions();
            parseResult.CommandOptions = dataTypesCommandOptions;

            Int64 int64;
            A.CallTo(() => valueParser.TryParseInt64("123", NumberStyles.Any, new CultureInfo("en-US"), out int64))
                .Returns(true)
                .AssignsOutAndRefParameters(123L);

            parser.Parse(tokens, parseResult);

            dataTypesCommandOptions.Int64.Should().Be(123L);

            A.CallTo(() => valueParser.TryParseInt64("123", NumberStyles.Any, new CultureInfo("en-US"), out int64)).MustHaveHappened();
        }

        [Test(Description = "TargetProperty should return the property that was specified via the constructor.")]
        public void TargetProperty_ShouldReturnPropertySpecifiedViaConstructor()
        {
            var parser = new Int64OptionParser(typeof(DataTypesCommandOptions).GetProperty("Int64"), "int64");

            parser.TargetProperty.Should().BeSameAs(typeof(DataTypesCommandOptions).GetProperty("Int64"));
        }
    }
}