using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class GuidOptionParserTests
    {
        [Test(Description = "Constructor should throw an exception when the given target property is null.")]
        public void Constructor_TargetPropertyIsNull_ShouldThrowException()
        {
            Invoking(() => new GuidOptionParser(null, "guid"))
                .Should()
                .Throw<ArgumentNullException>()
                .WithMessage(@"Value cannot be null.
Parameter name: targetProperty");
        }

        [Test(Description = "Constructor should throw an exception when the given option name is null or an empty string.")]
        public void Constructor_OptionNameIsNullOrEmpty_ShouldThrowException()
        {
            Invoking(() => new GuidOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guid"), null))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"Value cannot be null or an empty string.
Parameter name: optionName");

            Invoking(() => new GuidOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guid"), ""))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"Value cannot be null or an empty string.
Parameter name: optionName");
        }

        [Test(Description = "Constructor should throw an exception when the given target property has a incompatible data type.")]
        public void Constructor_IncompatibleTargetProperty_ShouldThrowException()
        {
            Invoking(() => new GuidOptionParser(typeof(DataTypesCommandOptions).GetProperty("String"), "guid"))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"The given target property has an incompatible property type. Expected type is System.Guid or System.Nullable<System.Guid>, actual type was System.String.
Parameter name: targetProperty");
        }

        [Test(Description = "TargetProperty should return the property that was specified via the constructor.")]
        public void TargetProperty_ShouldReturnPropertySpecifiedViaConstructor()
        {
            var parser = new GuidOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guid"), "guid");

            parser.TargetProperty.Should().BeSameAs(typeof(DataTypesCommandOptions).GetProperty("Guid"));
        }

        [Test(Description = "OptionDefaultValue should return default(Guid) initially.")]
        public void OptionDefaultValue_Initially_ShouldReturnDefaultOfGuid()
        {
            var parser = new GuidOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guid"), "guid");

            parser.OptionDefaultValue.Should().Be(default(Guid));
        }

        [Test(Description = "OptionName should return the name that was specified via the constructor.")]
        public void OptionName_ShouldReturnNameSpecifiedViaConstructor()
        {
            var parser = new GuidOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guid"), "guid");

            parser.OptionName.Should().Be("guid");
        }

        [Test(Description = "OptionType should return SingleValueOption.")]
        public void OptionType_ShouldReturnSingleValueOption()
        {
            var parser = new GuidOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guid"), "guid");

            parser.OptionType.Should().Be(OptionType.SingleValueOption);
        }

        [Test(Description = "IsOptionRequired should return false initially.")]
        public void IsOptionRequired_Initially_ShouldReturnFalse()
        {
            var parser = new GuidOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guid"), "guid");

            parser.IsOptionRequired.Should().BeFalse();
        }

        [Test(Description = "GuidFormat should return null initially.")]
        public void GuidFormat_Initially_ShouldReturnNull()
        {
            var parser = new GuidOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guid"), "guid");

            parser.GuidFormat.Should().BeNull();
        }

        [Test(Description = "GetHelpText should return the text that was set via the OptionHelp property.")]
        public void GetHelpText_ShouldReturnSpecifiedHelpText()
        {
            var parser = new GuidOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guid"), "guid");
            parser.OptionHelp = "Help text for option guid.";

            parser.GetHelpText().Should().Be("Help text for option guid.");
        }

        [Test(Description = "Parse should assign the specified default value to the target property when the option is not present in the command line.")]
        public void Parse_OptionNotPresent_ShouldAssignDefaultValueToTargetProperty()
        {
            var parser = new GuidOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guid"), "guid");
            parser.OptionDefaultValue = new Guid("501a44e0-6d8f-4dd8-994e-773300572d37");

            var tokens = new List<Token>();
            var parseResult = new ParseResult();
            var dataTypesCommandOptions = new DataTypesCommandOptions();
            parseResult.CommandOptions = dataTypesCommandOptions;

            parser.Parse(tokens, parseResult);

            dataTypesCommandOptions.Guid.Should().Be(new Guid("501a44e0-6d8f-4dd8-994e-773300572d37"));
        }

        [Test(Description = "Parse should parse a valid option value using the value parser and assign it to the target property.")]
        public void Parse_ValidValue_ShouldParseAndAssignValueToTargetProperty()
        {
            var valueParser = A.Fake<ValueParser>();
            var parser = new GuidOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guid"), "guid");
            parser.ValueParser = valueParser;

            var tokens = new List<Token>
            {
                new OptionToken("guid")
                {
                    OptionValues = { "501a44e0-6d8f-4dd8-994e-773300572d37" }
                }
            };
            var parseResult = new ParseResult();
            var dataTypesCommandOptions = new DataTypesCommandOptions();
            parseResult.CommandOptions = dataTypesCommandOptions;

            Guid guid;
            A.CallTo(() => valueParser.TryParseGuid("501a44e0-6d8f-4dd8-994e-773300572d37", null, out guid))
                .Returns(true)
                .AssignsOutAndRefParameters(new Guid("501a44e0-6d8f-4dd8-994e-773300572d37"));

            parser.Parse(tokens, parseResult);

            dataTypesCommandOptions.Guid.Should().Be(new Guid("501a44e0-6d8f-4dd8-994e-773300572d37"));

            A.CallTo(() => valueParser.TryParseGuid("501a44e0-6d8f-4dd8-994e-773300572d37", null, out guid)).MustHaveHappened();
        }

        [Test(Description = "Parse should pass the specified custom format settings to the value parser.")]
        public void Parse_CustomFormatSettings_ShouldPassCustomFormatToValueParser()
        {
            var valueParser = A.Fake<ValueParser>(ob => ob.CallsBaseMethods());
            var parser = new GuidOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guid"), "guid");
            parser.ValueParser = valueParser;
            parser.GuidFormat = "X";

            var tokens = new List<Token>
            {
                new OptionToken("guid")
                {
                    OptionValues = { "{0x501a44e0,0x6d8f,0x4dd8,{0x99,0x4e,0x77,0x33,0x00,0x57,0x2d,0x37}}" }
                }
            };
            var parseResult = new ParseResult();
            var dataTypesCommandOptions = new DataTypesCommandOptions();
            parseResult.CommandOptions = dataTypesCommandOptions;

            parser.Parse(tokens, parseResult);

            Guid guid;
            A.CallTo(() => valueParser.TryParseGuid("{0x501a44e0,0x6d8f,0x4dd8,{0x99,0x4e,0x77,0x33,0x00,0x57,0x2d,0x37}}", "X", out guid)).MustHaveHappened();
        }

        [Test(Description = "Parse should add an OptionValueInvalidFormatError error to the parse result when the specified value is not a valid decimal.")]
        public void Parse_InvalidValue_ShouldAddError()
        {
            var valueParser = A.Fake<ValueParser>();
            var parser = new GuidOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guid"), "guid");
            parser.ValueParser = valueParser;

            var tokens = new List<Token>
            {
                new OptionToken("guid")
                {
                    OptionValues = { "NotAGuid" }
                }
            };
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            Guid guid;
            A.CallTo(() => valueParser.TryParseGuid("NotAGuid", null, out guid))
                .Returns(false);

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionValueInvalidFormatError>();

            var error = (OptionValueInvalidFormatError)parseResult.Errors[0];
            error.OptionName.Should().Be("guid");
            error.InvalidOptionValue.Should().Be("NotAGuid");
            error.ExpectedValueFormat.Should().Be("A valid Guid");
            error.GetErrorMessage().Should().Be("The value 'NotAGuid' of the option --guid has an invalid format. The expected format is: A valid Guid.");
        }

        [Test(Description = "Parse should add an OptionMissingError error to the parse result when the option is required, but it was not supplied.")]
        public void Parse_RequiredOptionMissing_ShouldAddError()
        {
            var parser = new GuidOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guid"), "guid");
            parser.IsOptionRequired = true;

            var tokens = new List<Token>();
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionMissingError>();

            var error = (OptionMissingError)parseResult.Errors[0];
            error.OptionName.Should().Be("guid");
            error.GetErrorMessage().Should().Be("The option --guid is required.");
        }

        [Test(Description = "Parse should add an OptionValueMissingError error to the parse result when no value was supplied for the option.")]
        public void Parse_OptionValueMissing_ShouldAddError()
        {
            var parser = new GuidOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guid"), "guid");

            var tokens = new List<Token>
            {
                new OptionToken("guid")
            };

            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionValueMissingError>();

            var error = (OptionValueMissingError)parseResult.Errors[0];
            error.OptionName.Should().Be("guid");
            error.GetErrorMessage().Should().Be("The option --guid requires a value, but no value was specified.");
        }

        [Test(Description = "Parse should add an OptionMultipleValuesError error to the parse result when more than one value was supplied for the option.")]
        public void Parse_MoreThanOneOptionValue_ShouldAddError()
        {
            var parser = new GuidOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guid"), "guid");

            var tokens = new List<Token>
            {
                new OptionToken("guid")
                {
                    OptionValues = { "13d02a84-84f7-4a2d-8f09-2f96defb8c79", "9e6a5202-102f-4eb9-a217-0a58f4db40b6" }
                }
            };

            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionMultipleValuesError>();

            var error = (OptionMultipleValuesError)parseResult.Errors[0];
            error.OptionName.Should().Be("guid");
            error.GetErrorMessage().Should().Be("Multiple values are given for the option --guid, but the option expects a single value.");
        }
    }
}