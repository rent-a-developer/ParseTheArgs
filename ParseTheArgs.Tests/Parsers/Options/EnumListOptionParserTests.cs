using System;
using System.Collections.Generic;
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
    public class EnumListOptionParserTests
    {
        [Test(Description = "Constructor should throw an exception when the given target property is null.")]
        public void Constructor_TargetPropertyIsNull_ShouldThrowException()
        {
            Invoking(() => new EnumListOptionParser<LogLevel>(null, "enums"))
                .Should()
                .Throw<ArgumentNullException>()
                .WithMessage(@"Value cannot be null.
Parameter name: targetProperty");
        }

        [Test(Description = "Constructor should throw an exception when the given option name is null or an empty string.")]
        public void Constructor_OptionNameIsNullOrEmpty_ShouldThrowException()
        {
            Invoking(() => new EnumListOptionParser<LogLevel>(typeof(DataTypesCommandOptions).GetProperty("Enums"), null))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"Value cannot be null or an empty string.
Parameter name: optionName");

            Invoking(() => new EnumListOptionParser<LogLevel>(typeof(DataTypesCommandOptions).GetProperty("Enums"), ""))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"Value cannot be null or an empty string.
Parameter name: optionName");
        }

        [Test(Description = "Constructor should throw an exception when the given target property has a incompatible data type.")]
        public void Constructor_IncompatibleTargetProperty_ShouldThrowException()
        {
            Invoking(() => new EnumListOptionParser<LogLevel>(typeof(DataTypesCommandOptions).GetProperty("String"), "enums"))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(@"The given target property has an incompatible property type. Expected type is System.Collections.Generic.List<ParseTheArgs.Tests.TestData.LogLevel>, actual type was System.String.
Parameter name: targetProperty");
        }

        [Test(Description = "TargetProperty should return the property that was specified via the constructor.")]
        public void TargetProperty_ShouldReturnPropertySpecifiedViaConstructor()
        {
            var parser = new EnumListOptionParser<LogLevel>(typeof(DataTypesCommandOptions).GetProperty("Enums"), "enums");

            parser.TargetProperty.Should().BeSameAs(typeof(DataTypesCommandOptions).GetProperty("Enums"));
        }

        [Test(Description = "OptionName should return the name that was specified via the constructor.")]
        public void OptionName_ShouldReturnNameSpecifiedViaConstructor()
        {
            var parser = new EnumListOptionParser<LogLevel>(typeof(DataTypesCommandOptions).GetProperty("Enums"), "enums");

            parser.OptionName.Should().Be("enums");
        }

        [Test(Description = "OptionType should return MultiValueOption.")]
        public void OptionType_ShouldReturnMultiValueOption()
        {
            var parser = new EnumListOptionParser<LogLevel>(typeof(DataTypesCommandOptions).GetProperty("Enums"), "enums");

            parser.OptionType.Should().Be(OptionType.MultiValueOption);
        }

        [Test(Description = "IsOptionRequired should return false initially.")]
        public void IsOptionRequired_Initially_ShouldReturnFalse()
        {
            var parser = new EnumListOptionParser<LogLevel>(typeof(DataTypesCommandOptions).GetProperty("Enums"), "enums");

            parser.IsOptionRequired.Should().BeFalse();
        }

        [Test(Description = "GetHelpText should return the text that was set via the OptionHelp property.")]
        public void GetHelpText_ShouldReturnSpecifiedHelpText()
        {
            var parser = new EnumListOptionParser<LogLevel>(typeof(DataTypesCommandOptions).GetProperty("Enums"), "enums");
            parser.OptionHelp = "Help text for option enum.";

            parser.GetHelpText().Should().Be(@"Help text for option enum. Possible values: Trace, Debug, Info, Error.");
        }

        [Test(Description = "GetHelpText should include the specified help texts for the enum values in the returned help text.")]
        public void GetHelpText_EnumValuesHelpPresent_ShouldReturnSpecifiedHelpText()
        {
            var parser = new EnumListOptionParser<LogLevel>(typeof(DataTypesCommandOptions).GetProperty("Enums"), "enums");
            parser.OptionHelp = "Help text for option enum.";
            parser.EnumValuesHelps.Add(LogLevel.Trace, "Trace help.");
            parser.EnumValuesHelps.Add(LogLevel.Debug, "Debug help.");
            parser.EnumValuesHelps.Add(LogLevel.Info, "Info help.");
            parser.EnumValuesHelps.Add(LogLevel.Error, "Error help.");

            parser.GetHelpText().Should().Be(@"Help text for option enum. Possible values: Trace, Debug, Info, Error.
Trace: Trace help.
Debug: Debug help.
Info: Info help.
Error: Error help.
");
        }

        [Test(Description = "Parse should parse valid option values using the value parser and assign them to the target property.")]
        public void Parse_ValidValues_ShouldParseAndAssignToTargetProperty()
        {
            var valueParser = A.Fake<ValueParser>();
            var parser = new EnumListOptionParser<LogLevel>(typeof(DataTypesCommandOptions).GetProperty("Enums"), "logLevels");
            parser.ValueParser = valueParser;

            var tokens = new List<Token>
            {
                new OptionToken("logLevels")
                {
                    OptionValues = { "Debug", "Trace" }
                }
            };
            var parseResult = new ParseResult();
            var dataTypesCommandOptions = new DataTypesCommandOptions();
            parseResult.CommandOptions = dataTypesCommandOptions;

            LogLevel logLevel;

            A.CallTo(() => valueParser.TryParseEnum("Debug", out logLevel))
                .Returns(true)
                .AssignsOutAndRefParameters(LogLevel.Debug);

            A.CallTo(() => valueParser.TryParseEnum("Trace", out logLevel))
                .Returns(true)
                .AssignsOutAndRefParameters(LogLevel.Trace);

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeFalse();
            parseResult.CommandOptions.Should().BeOfType<DataTypesCommandOptions>();

            dataTypesCommandOptions.Enums.Should().BeEquivalentTo(LogLevel.Debug, LogLevel.Trace);

            A.CallTo(() => valueParser.TryParseEnum("Debug", out logLevel)).MustHaveHappened();
            A.CallTo(() => valueParser.TryParseEnum("Trace", out logLevel)).MustHaveHappened();
        }

        [Test(Description = "Parse should add an OptionValueInvalidFormatError to the parse result when one of the specified values is not a invalid enum value.")]
        public void Parse_InvalidValue_ShouldAddError()
        {
            var valueParser = A.Fake<ValueParser>();
            var parser = new EnumListOptionParser<LogLevel>(typeof(DataTypesCommandOptions).GetProperty("Enums"), "logLevels");
            parser.ValueParser = valueParser;

            var tokens = new List<Token>
            {
                new OptionToken("logLevels")
                {
                    OptionValues = { "NonExistentLogLevel" }
                }
            };
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            LogLevel logLevel;

            A.CallTo(() => valueParser.TryParseEnum("NonExistentLogLevel", out logLevel))
                .Returns(false);

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<OptionValueInvalidFormatError>();

            var error = (OptionValueInvalidFormatError) parseResult.Errors[0];
            error.OptionName.Should().Be("logLevels");
            error.InvalidOptionValue.Should().Be("NonExistentLogLevel");
            error.ExpectedValueFormat.Should().Be("One of the valid values (see help)");
            error.GetErrorMessage().Should().Be("The value 'NonExistentLogLevel' of the option --logLevels has an invalid format. The expected format is: One of the valid values (see help).");
        }
    }
}