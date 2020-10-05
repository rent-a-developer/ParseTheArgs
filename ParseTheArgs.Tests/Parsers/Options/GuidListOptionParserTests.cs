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
    public class GuidListOptionParserTests
    {
        [Test(Description = "Constructor should throw an exception when the given target property is null.")]
        public void Constructor_TargetPropertyIsNull_ShouldThrowException()
        {
            Invoking(() => new GuidListOptionParser(null, "guids"))
                .Should()
                .Throw<ArgumentNullException>()
                .WithMessage(@"Value cannot be null.
Parameter name: targetProperty");
        }

        [Test(Description = "Constructor should throw an exception when the given option name is null or an empty string.")]
        public void Constructor_OptionNameIsNullOrEmpty_ShouldThrowException()
        {
            Invoking(() => new GuidListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guids"), null))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"Value cannot be null or an empty string.
Parameter name: optionName");

            Invoking(() => new GuidListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guids"), ""))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"Value cannot be null or an empty string.
Parameter name: optionName");
        }

        [Test(Description = "Constructor should throw an exception when the given target property has a incompatible data type.")]
        public void Constructor_IncompatibleTargetProperty_ShouldThrowException()
        {
            Invoking(() => new GuidListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Strings"), "guids"))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"The given target property has an incompatible property type. Expected type is System.Collections.Generic.List<System.Guid>, actual type was System.Collections.Generic.List`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]].
Parameter name: targetProperty");
        }

        [Test(Description = "TargetProperty should return the property that was specified via the constructor.")]
        public void TargetProperty_ShouldReturnPropertySpecifiedViaConstructor()
        {
            var parser = new GuidListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guids"), "guids");

            parser.TargetProperty.Should().BeSameAs(typeof(DataTypesCommandOptions).GetProperty("Guids"));
        }

        [Test(Description = "OptionName should return the name that was specified via the constructor.")]
        public void OptionName_ShouldReturnNameSpecifiedViaConstructor()
        {
            var parser = new GuidListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guids"), "guids");

            parser.OptionName.Should().Be("guids");
        }

        [Test(Description = "OptionType should return MultiValueOption.")]
        public void OptionType_ShouldReturnMultiValueOption()
        {
            var parser = new GuidListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guids"), "guids");

            parser.OptionType.Should().Be(OptionType.MultiValueOption);
        }

        [Test(Description = "IsOptionRequired should return false initially.")]
        public void IsOptionRequired_Initially_ShouldReturnFalse()
        {
            var parser = new GuidListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guids"), "guids");

            parser.IsOptionRequired.Should().BeFalse();
        }

        [Test(Description = "GuidFormat should return null initially.")]
        public void GuidFormat_Initially_ShouldReturnNull()
        {
            var parser = new GuidListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guids"), "guids");

            parser.GuidFormat.Should().BeNull();
        }

        [Test(Description = "GetHelpText should return the text that was set via the OptionHelp property.")]
        public void GetHelpText_ShouldReturnSpecifiedHelpText()
        {
            var parser = new GuidListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guids"), "guids");
            parser.OptionHelp = "Help text for option guids.";

            parser.GetHelpText().Should().Be("Help text for option guids.");
        }

        [Test(Description = "Parse should parse valid option values using the value parser and assign them to the target property.")]
        public void Parse_ValidValue_ShouldParseAndAssignValueToTargetProperty()
        {
            var valueParser = A.Fake<ValueParser>();
            var parser = new GuidListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guids"), "guids");
            parser.ValueParser = valueParser;

            var tokens = new List<Token>
            {
                new OptionToken("guids")
                {
                    OptionValues = { "13d02a84-84f7-4a2d-8f09-2f96defb8c79", "9e6a5202-102f-4eb9-a217-0a58f4db40b6" }
                }
            };
            var parseResult = new ParseResult();
            var dataTypesCommandOptions = new DataTypesCommandOptions();
            parseResult.CommandOptions = dataTypesCommandOptions;

            Guid guid;

            A.CallTo(() => valueParser.TryParseGuid("13d02a84-84f7-4a2d-8f09-2f96defb8c79", null, out guid))
                .Returns(true)
                .AssignsOutAndRefParameters(new Guid("13d02a84-84f7-4a2d-8f09-2f96defb8c79"));

            A.CallTo(() => valueParser.TryParseGuid("9e6a5202-102f-4eb9-a217-0a58f4db40b6", null, out guid))
                .Returns(true)
                .AssignsOutAndRefParameters(new Guid("9e6a5202-102f-4eb9-a217-0a58f4db40b6"));

            parser.Parse(tokens, parseResult);

            dataTypesCommandOptions.Guids.Should().BeEquivalentTo(new Guid("13d02a84-84f7-4a2d-8f09-2f96defb8c79"), new Guid("9e6a5202-102f-4eb9-a217-0a58f4db40b6"));

            A.CallTo(() => valueParser.TryParseGuid("13d02a84-84f7-4a2d-8f09-2f96defb8c79", null, out guid)).MustHaveHappened();
            A.CallTo(() => valueParser.TryParseGuid("9e6a5202-102f-4eb9-a217-0a58f4db40b6", null, out guid)).MustHaveHappened();
        }

        [Test(Description = "Parse should pass the specified custom format settings to the value parser.")]
        public void Parse_CustomFormatSettings_ShouldPassCustomFormatToValueParser()
        {
            var valueParser = A.Fake<ValueParser>(ob => ob.CallsBaseMethods());
            var parser = new GuidListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guids"), "guids");
            parser.ValueParser = valueParser;
            parser.GuidFormat = "X";

            var tokens = new List<Token>
            {
                new OptionToken("guids")
                {
                    OptionValues = { "{0x13d02a84,0x84f7,0x4a2d,{0x8f,0x09,0x2f,0x96,0xde,0xfb,0x8c,0x79}}" }
                }
            };
            var parseResult = new ParseResult();
            var dataTypesCommandOptions = new DataTypesCommandOptions();
            parseResult.CommandOptions = dataTypesCommandOptions;

            parser.Parse(tokens, parseResult);

            Guid guid;
            A.CallTo(() => valueParser.TryParseGuid("{0x13d02a84,0x84f7,0x4a2d,{0x8f,0x09,0x2f,0x96,0xde,0xfb,0x8c,0x79}}", "X", out guid)).MustHaveHappened();
        }

        [Test(Description = "Parse should add an OptionValueInvalidFormatError error to the parse result when one of the specified values is not a valid date time.")]
        public void Parse_InvalidValue_ShouldAddError()
        {
            var valueParser = A.Fake<ValueParser>();
            var parser = new GuidListOptionParser(typeof(DataTypesCommandOptions).GetProperty("Guids"), "guids");
            parser.ValueParser = valueParser;

            var tokens = new List<Token>
            {
                new OptionToken("guids")
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
            error.OptionName.Should().Be("guids");
            error.InvalidOptionValue.Should().Be("NotAGuid");
            error.ExpectedValueFormat.Should().Be("A valid Guid");
            error.GetErrorMessage().Should().Be("The value 'NotAGuid' of the option --guids has an invalid format. The expected format is: A valid Guid.");
        }
    }
}