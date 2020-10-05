using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using ParseTheArgs.Parsers.Options;
using ParseTheArgs.Tests.TestData;
using ParseTheArgs.Tokens;
using static FluentAssertions.FluentActions;

namespace ParseTheArgs.Tests.Parsers.Options
{
    [TestFixture]
    public class StringOptionParserTests
    {
        [Test(Description = "Constructor should throw an exception when the given target property is null.")]
        public void Constructor_TargetPropertyIsNull_ShouldThrowException()
        {
            Invoking(() => new StringOptionParser(null, "string"))
                .Should()
                .Throw<ArgumentNullException>()
                .WithMessage(@"Value cannot be null.
Parameter name: targetProperty");
        }

        [Test(Description = "Constructor should throw an exception when the given option name is null or an empty string.")]
        public void Constructor_OptionNameIsNullOrEmpty_ShouldThrowException()
        {
            Invoking(() => new StringOptionParser(typeof(DataTypesCommandOptions).GetProperty("String"), null))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"Value cannot be null or an empty string.
Parameter name: optionName");

            Invoking(() => new StringOptionParser(typeof(DataTypesCommandOptions).GetProperty("String"), ""))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"Value cannot be null or an empty string.
Parameter name: optionName");
        }

        [Test(Description = "Constructor should throw an exception when the given target property has a incompatible data type.")]
        public void Constructor_IncompatibleTargetProperty_ShouldThrowException()
        {
            Invoking(() => new StringOptionParser(typeof(DataTypesCommandOptions).GetProperty("Decimal"), "string"))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"The given target property has an incompatible property type. Expected type is System.String, actual type was System.Decimal.
Parameter name: targetProperty");
        }

        [Test(Description = "TargetProperty should return the property that was specified via the constructor.")]
        public void TargetProperty_ShouldReturnPropertySpecifiedViaConstructor()
        {
            var parser = new StringOptionParser(typeof(DataTypesCommandOptions).GetProperty("String"), "string");

            parser.TargetProperty.Should().BeSameAs(typeof(DataTypesCommandOptions).GetProperty("String"));
        }

        [Test(Description = "OptionDefaultValue should return null initially.")]
        public void OptionDefaultValue_Initially_ShouldReturnNull()
        {
            var parser = new StringOptionParser(typeof(DataTypesCommandOptions).GetProperty("String"), "string");

            parser.OptionDefaultValue.Should().BeNull();
        }

        [Test(Description = "OptionName should return the name that was specified via the constructor.")]
        public void OptionName_ShouldReturnNameSpecifiedViaConstructor()
        {
            var parser = new StringOptionParser(typeof(DataTypesCommandOptions).GetProperty("String"), "string");

            parser.OptionName.Should().Be("string");
        }

        [Test(Description = "OptionType should return SingleValueOption.")]
        public void OptionType_ShouldReturnSingleValueOption()
        {
            var parser = new StringOptionParser(typeof(DataTypesCommandOptions).GetProperty("String"), "string");

            parser.OptionType.Should().Be(OptionType.SingleValueOption);
        }

        [Test(Description = "IsOptionRequired should return false initially.")]
        public void IsOptionRequired_Initially_ShouldReturnFalse()
        {
            var parser = new StringOptionParser(typeof(DataTypesCommandOptions).GetProperty("String"), "string");

            parser.IsOptionRequired.Should().BeFalse();
        }

        [Test(Description = "GetHelpText should return the text that was set via the OptionHelp property.")]
        public void GetHelpText_ShouldReturnSpecifiedHelpText()
        {
            var parser = new StringOptionParser(typeof(DataTypesCommandOptions).GetProperty("String"), "string");
            parser.OptionHelp = "Help text for option string.";

            parser.GetHelpText().Should().Be("Help text for option string.");
        }

        [Test(Description = "Parse should assign the specified default value to the target property when the option is not present in the command line.")]
        public void Parse_OptionNotPresent_ShouldAssignDefaultValueToTargetProperty()
        {
            var parser = new StringOptionParser(typeof(DataTypesCommandOptions).GetProperty("String"), "string");
            parser.OptionDefaultValue = "value";

            var tokens = new List<Token>();
            var parseResult = new ParseResult();
            var dataTypesCommandOptions = new DataTypesCommandOptions();
            parseResult.CommandOptions = dataTypesCommandOptions;

            parser.Parse(tokens, parseResult);

            dataTypesCommandOptions.String.Should().Be("value");
        }

        [Test(Description = "Parse should parse a valid option value and assign it to the target property.")]
        public void Parse_ValidValue_ShouldParseAndAssignValueToTargetProperty()
        {
            var parser = new StringOptionParser(typeof(DataTypesCommandOptions).GetProperty("String"), "string");

            var tokens = new List<Token>
            {
                new OptionToken("string")
                {
                    OptionValues = { "value" }
                }
            };
            var parseResult = new ParseResult();
            var dataTypesCommandOptions = new DataTypesCommandOptions();
            parseResult.CommandOptions = dataTypesCommandOptions;

            parser.Parse(tokens, parseResult);

            dataTypesCommandOptions.String.Should().Be("value");
        }
    }
}