using System;
using System.Collections.Generic;
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
    public class StringListOptionParserTests
    {
        [Test(Description = "Constructor should throw an exception when the given target property is null.")]
        public void Constructor_TargetPropertyIsNull_ShouldThrowException()
        {
            Invoking(() => new StringListOptionParser(null, "strings"))
                .Should()
                .Throw<ArgumentNullException>()
                .WithMessage(@"Value cannot be null.
Parameter name: targetProperty");
        }

        [Test(Description = "Constructor should throw an exception when the given option name is null or an empty string.")]
        public void Constructor_OptionNameIsNullOrEmpty_ShouldThrowException()
        {
            Invoking(() => new StringListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Strings"), null))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"Value cannot be null or an empty string.
Parameter name: optionName");

            Invoking(() => new StringListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Strings"), ""))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"Value cannot be null or an empty string.
Parameter name: optionName");
        }

        [Test(Description = "Constructor should throw an exception when the given target property has a incompatible data type.")]
        public void Constructor_IncompatibleTargetProperty_ShouldThrowException()
        {
            Invoking(() => new StringListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimal"), "strings"))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"The given target property has an incompatible property type. Expected type is System.Collections.Generic.List<System.String>, actual type was System.Decimal.
Parameter name: targetProperty");
        }

        [Test(Description = "TargetProperty should return the property that was specified via the constructor.")]
        public void TargetProperty_ShouldReturnPropertySpecifiedViaConstructor()
        {
            var parser = new StringListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Strings"), "strings");

            parser.TargetProperty.Should().BeSameAs(typeof(DataTypesCommandOptions).GetProperty("Strings"));
        }

        [Test(Description = "OptionDefaultValue should return null initially.")]
        public void OptionDefaultValue_Initially_ShouldReturnNull()
        {
            var parser = new StringListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Strings"), "strings");

            parser.OptionDefaultValue.Should().BeNull();
        }

        [Test(Description = "OptionName should return the name that was specified via the constructor.")]
        public void OptionName_ShouldReturnNameSpecifiedViaConstructor()
        {
            var parser = new StringListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Strings"), "strings");

            parser.OptionName.Should().Be("strings");
        }

        [Test(Description = "OptionType should return MultiValueOption.")]
        public void OptionType_ShouldReturnMultiValueOption()
        {
            var parser = new StringListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Strings"), "strings");

            parser.OptionType.Should().Be(OptionType.MultiValueOption);
        }

        [Test(Description = "IsOptionRequired should return false initially.")]
        public void IsOptionRequired_Initially_ShouldReturnFalse()
        {
            var parser = new StringListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Strings"), "strings");

            parser.IsOptionRequired.Should().BeFalse();
        }

        [Test(Description = "GetHelpText should return the text that was set via the OptionHelp property.")]
        public void GetHelpText_ShouldReturnSpecifiedHelpText()
        {
            var parser = new StringListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Strings"), "strings");
            parser.OptionHelp = "Help text for option strings.";

            parser.GetHelpText().Should().Be("Help text for option strings.");
        }

        [Test(Description = "Parse should assign the specified default value to the target property when the option is not present in the command line.")]
        public void Parse_OptionNotPresent_ShouldAssignDefaultValueToTargetProperty()
        {
            var parser = new StringListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Strings"), "strings");
            parser.OptionDefaultValue = new List<String> { "value1", "value2" };

            var tokens = new List<Token>();
            var parseResult = new ParseResult();
            var dataTypesCommandOptions = new DataTypesCommandOptions();
            parseResult.CommandOptions = dataTypesCommandOptions;

            parser.Parse(tokens, parseResult);

            dataTypesCommandOptions.Strings.Should().BeEquivalentTo("value1", "value2");
        }

        [Test(Description = "Parse should parse valid option values and assign them to the target property.")]
        public void Parse_ValidValue_ShouldParseAndAssignValueToTargetProperty()
        {
            var parser = new StringListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Strings"), "strings");

            var tokens = new List<Token>
            {
                new OptionToken("strings")
                {
                    OptionValues = { "value1", "value2" }
                }
            };
            var parseResult = new ParseResult();
            var dataTypesCommandOptions = new DataTypesCommandOptions();
            parseResult.CommandOptions = dataTypesCommandOptions;

            parser.Parse(tokens, parseResult);

            dataTypesCommandOptions.Strings.Should().BeEquivalentTo("value1", "value2");
        }

        [Test(Description = "Parse should add an OptionMissingError error to the parse result when the option is required, but it was not supplied.")]
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

            var error = (OptionMissingError)parseResult.Errors[0];
            error.OptionName.Should().Be("strings");
            error.GetErrorMessage().Should().Be("The option --strings is required.");
        }

        [Test(Description = "Parse should add an OptionValueMissingError error to the parse result when no value was supplied for the option.")]
        public void Parse_OptionValueMissing_ShouldAddError()
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

            var error = (OptionValueMissingError)parseResult.Errors[0];
            error.OptionName.Should().Be("strings");
            error.GetErrorMessage().Should().Be("The option --strings requires a value, but no value was specified.");
        }
    }
}
