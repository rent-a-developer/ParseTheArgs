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
    public class BooleanOptionParserTests
    {
        [Test(Description = "Constructor should throw an exception when the given target property has a incompatible data type.")]
        public void Constructor_IncompatibleTargetProperty_ShouldThrowException()
        {
            Invoking(() => new BooleanOptionParser(typeof(DataTypesCommandOptions).GetProperty("String"), "boolean"))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"The given target property has an incompatible property type. Expected type is System.Boolean or System.Nullable<System.Boolean>, actual type was System.String.
Parameter name: targetProperty");
        }

        [Test(Description = "Constructor should throw an exception when the given option name is null or an empty string.")]
        public void Constructor_OptionNameIsNullOrEmpty_ShouldThrowException()
        {
            Invoking(() => new BooleanOptionParser(typeof(DataTypesCommandOptions).GetProperty("Boolean"), null))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"Value cannot be null or an empty string.
Parameter name: optionName");

            Invoking(() => new BooleanOptionParser(typeof(DataTypesCommandOptions).GetProperty("Boolean"), ""))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"Value cannot be null or an empty string.
Parameter name: optionName");
        }

        [Test(Description = "Constructor should throw an exception when the given target property is null.")]
        public void Constructor_TargetPropertyIsNull_ShouldThrowException()
        {
            Invoking(() => new BooleanOptionParser(null, "boolean"))
                .Should()
                .Throw<ArgumentNullException>()
                .WithMessage(@"Value cannot be null.
Parameter name: targetProperty");
        }

        [Test(Description = "GetHelpText should return the text that was set via the OptionHelp property.")]
        public void GetHelpText_ShouldReturnSpecifiedHelpText()
        {
            var parser = new BooleanOptionParser(typeof(DataTypesCommandOptions).GetProperty("Boolean"), "boolean");
            parser.OptionHelp = "Help text for option boolean.";

            parser.GetHelpText().Should().Be("Help text for option boolean.");
        }

        [Test(Description = "IsOptionRequired should return false, since Boolean (switch) options can never be required.")]
        public void IsOptionRequired_ShouldReturnFalse()
        {
            var parser = new BooleanOptionParser(typeof(DataTypesCommandOptions).GetProperty("Boolean"), "boolean");

            parser.IsOptionRequired.Should().BeFalse();
        }

        [Test(Description = "OptionName should return the name that was specified via the constructor.")]
        public void OptionName_ShouldReturnNameSpecifiedViaConstructor()
        {
            var parser = new BooleanOptionParser(typeof(DataTypesCommandOptions).GetProperty("Boolean"), "boolean");

            parser.OptionName.Should().Be("boolean");
        }

        [Test(Description = "OptionType should return ValuelessOption.")]
        public void OptionType_ShouldReturnValuelessOption()
        {
            var parser = new BooleanOptionParser(typeof(DataTypesCommandOptions).GetProperty("Boolean"), "boolean");

            parser.OptionType.Should().Be(OptionType.ValuelessOption);
        }

        [Test(Description = "Parse should not assign the value true to the target property when the option is not present.")]
        public void Parse_OptionNotPresent_ShouldSetTargetPropertyToTrue()
        {
            var parser = new BooleanOptionParser(typeof(DataTypesCommandOptions).GetProperty("Boolean"), "boolean");

            var tokens = new List<Token>();
            var parseResult = new ParseResult();
            var dataTypesCommandOptions = new DataTypesCommandOptions();
            parseResult.CommandOptions = dataTypesCommandOptions;

            parser.Parse(tokens, parseResult);

            dataTypesCommandOptions.Boolean.Should().BeFalse();
        }

        [Test(Description = "Parse should assign the value true to the target property when the option is present.")]
        public void Parse_OptionPresent_ShouldSetTargetPropertyToTrue()
        {
            var parser = new BooleanOptionParser(typeof(DataTypesCommandOptions).GetProperty("Boolean"), "boolean");

            var tokens = new List<Token>
            {
                new OptionToken("boolean")
            };
            var parseResult = new ParseResult();
            var dataTypesCommandOptions = new DataTypesCommandOptions();
            parseResult.CommandOptions = dataTypesCommandOptions;

            parser.Parse(tokens, parseResult);

            dataTypesCommandOptions.Boolean.Should().BeTrue();
        }

        [Test(Description = "Parse should add an InvalidOptionError error to the parse result when an option value was specified for a Boolean (switch) option.")]
        public void Parse_ValuePresent_ShouldAddError()
        {
            var parser = new BooleanOptionParser(typeof(DataTypesCommandOptions).GetProperty("Boolean"), "boolean");

            var tokens = new List<Token>
            {
                new OptionToken("boolean")
                {
                    OptionValues = {"value"}
                }
            };
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<InvalidOptionError>();

            var error = (InvalidOptionError) parseResult.Errors[0];
            error.OptionName.Should().Be("boolean");
            error.GetErrorMessage().Should().Be("The option --boolean is invalid: This option does not support any values.");
        }

        [Test(Description = "TargetProperty should return the property that was specified via the constructor.")]
        public void TargetProperty_ShouldReturnPropertySpecifiedViaConstructor()
        {
            var parser = new BooleanOptionParser(typeof(DataTypesCommandOptions).GetProperty("Boolean"), "boolean");

            parser.TargetProperty.Should().BeSameAs(typeof(DataTypesCommandOptions).GetProperty("Boolean"));
        }
    }
}