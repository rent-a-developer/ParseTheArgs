using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Setup.Commands;
using ParseTheArgs.Setup.Options;
using ParseTheArgs.Tests.TestData;
using ParseTheArgs.Validation;

namespace ParseTheArgs.Tests.Setup.Commands
{
    [TestFixture]
    public class DefaultCommandSetupTests : BaseTestFixture
    {
        [Test(Description = "Constructor should get the command parser from the parser.")]
        public void Constructor_ShouldGetCommandParserFromParser()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<Command1Options>(parser);
            commandParser.IsCommandDefault = true;

            A.CallTo(() => parser.GetOrCreateCommandParser<Command1Options>(null)).Returns(commandParser);

            new DefaultCommandSetup<Command1Options>(parser);

            A.CallTo(() => parser.GetOrCreateCommandParser<Command1Options>(null)).MustHaveHappened();
        }

        [Test(Description = "Constructor should throw an exception when the given parser is null.")]
        public void Constructor_ParserIsNull_ShouldThrowException()
        {
            FluentActions.Invoking(() => new DefaultCommandSetup<Command1Options>(null))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "CommandParser should return the command parser the command setup is associated with.")]
        public void CommandParser_ShouldReturnCommandParser()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<Command1Options>(parser);
            commandParser.IsCommandDefault = true;

            A.CallTo(() => parser.GetOrCreateCommandParser<Command1Options>(null)).Returns(commandParser);

            var setup = new DefaultCommandSetup<Command1Options>(parser);

            setup.CommandParser.Should().Be(commandParser);
        }

        [Test(Description = "ExampleUsage should assign the given example usage to the command parser.")]
        public void ExampleUsage_ShouldAssignExampleUsageToCommandParser()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<Command1Options>(parser);
            commandParser.IsCommandDefault = true;

            A.CallTo(() => parser.GetOrCreateCommandParser<Command1Options>(null)).Returns(commandParser);

            var setup = new DefaultCommandSetup<Command1Options>(parser);

            setup.ExampleUsage("newExampleUsage");

            commandParser.CommandExampleUsage.Should().Be("newExampleUsage");
        }

        [Test(Description = "ExampleUsage should return the same instance of the command setup.")]
        public void ExampleUsage_ShouldReturnCommandSetup()
        {
            var parser = A.Fake<Parser>();

            var setup = new DefaultCommandSetup<Command1Options>(parser);

            setup.ExampleUsage("exampleUsage").Should().Be(setup);
        }

        [Test(Description = "Help should assign the given help text to the command parser.")]
        public void Help_ShouldAssignHelpTextToCommandParser()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<Command1Options>(parser);
            commandParser.IsCommandDefault = true;

            A.CallTo(() => parser.GetOrCreateCommandParser<Command1Options>(null)).Returns(commandParser);

            var setup = new DefaultCommandSetup<Command1Options>(parser);

            setup.Help("newHelpText");

            commandParser.CommandHelp.Should().Be("newHelpText");
        }

        [Test(Description = "Help should return the same instance of the command setup.")]
        public void Help_ShouldReturnCommandSetup()
        {
            var parser = A.Fake<Parser>();

            var setup = new DefaultCommandSetup<Command1Options>(parser);

            setup.Help("helpText").Should().Be(setup);
        }

        [Test(Description = "Option should return a new instance of BooleanOptionSetup when called for a Boolean target property.")]
        public void Option_BooleanTargetProperty_ShouldReturnANewBooleanOptionSetup()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<DataTypesCommandOptions>(parser);
            commandParser.IsCommandDefault = true;

            A.CallTo(() => parser.GetOrCreateCommandParser<DataTypesCommandOptions>(null)).Returns(commandParser);

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser);

            Expression<Func<DataTypesCommandOptions, Boolean>> propertyExpression = a => a.Boolean;

            var optionSetup = new BooleanOptionSetup<DataTypesCommandOptions>(commandParser, propertyExpression);
            A.CallTo(() => this.DependencyResolver.Resolve<BooleanOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).Returns(optionSetup);

            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => this.DependencyResolver.Resolve<BooleanOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of DateTimeListOptionSetup when called for a List<DateTime> target property.")]
        public void Option_DateTimeListTargetProperty_ShouldReturnANewDateTimeListOptionSetup()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<DataTypesCommandOptions>(parser);
            commandParser.IsCommandDefault = true;

            A.CallTo(() => parser.GetOrCreateCommandParser<DataTypesCommandOptions>(null)).Returns(commandParser);

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser);

            Expression<Func<DataTypesCommandOptions, List<DateTime>>> propertyExpression = a => a.DateTimes;

            var optionSetup = new DateTimeListOptionSetup<DataTypesCommandOptions>(commandParser, propertyExpression);
            A.CallTo(() => this.DependencyResolver.Resolve<DateTimeListOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).Returns(optionSetup);

            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => this.DependencyResolver.Resolve<DateTimeListOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of DateTimeOptionSetup when called for a DateTime target property.")]
        public void Option_DateTimeTargetProperty_ShouldReturnANewDateTimeOptionSetup()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<DataTypesCommandOptions>(parser);
            commandParser.IsCommandDefault = true;

            A.CallTo(() => parser.GetOrCreateCommandParser<DataTypesCommandOptions>(null)).Returns(commandParser);

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser);

            Expression<Func<DataTypesCommandOptions, DateTime>> propertyExpression = a => a.DateTime;

            var optionSetup = new DateTimeOptionSetup<DataTypesCommandOptions>(commandParser, propertyExpression);
            A.CallTo(() => this.DependencyResolver.Resolve<DateTimeOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).Returns(optionSetup);

            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => this.DependencyResolver.Resolve<DateTimeOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of DecimalListOptionSetup when called for a List<Decimal> target property.")]
        public void Option_DecimalListTargetProperty_ShouldReturnANewDecimalListOptionSetup()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<DataTypesCommandOptions>(parser);
            commandParser.IsCommandDefault = true;

            A.CallTo(() => parser.GetOrCreateCommandParser<DataTypesCommandOptions>(null)).Returns(commandParser);

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser);

            Expression<Func<DataTypesCommandOptions, List<Decimal>>> propertyExpression = a => a.Decimals;

            var optionSetup = new DecimalListOptionSetup<DataTypesCommandOptions>(commandParser, propertyExpression);
            A.CallTo(() => this.DependencyResolver.Resolve<DecimalListOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).Returns(optionSetup);

            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => this.DependencyResolver.Resolve<DecimalListOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of DecimalOptionSetup when called for a Decimal target property.")]
        public void Option_DecimalTargetProperty_ShouldReturnANewDecimalOptionSetup()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<DataTypesCommandOptions>(parser);
            commandParser.IsCommandDefault = true;

            A.CallTo(() => parser.GetOrCreateCommandParser<DataTypesCommandOptions>(null)).Returns(commandParser);

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser);

            Expression<Func<DataTypesCommandOptions, Decimal>> propertyExpression = a => a.Decimal;

            var optionSetup = new DecimalOptionSetup<DataTypesCommandOptions>(commandParser, propertyExpression);
            A.CallTo(() => this.DependencyResolver.Resolve<DecimalOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).Returns(optionSetup);

            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => this.DependencyResolver.Resolve<DecimalOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of EnumListOptionSetup when called for a List<Enum> target property.")]
        public void Option_EnumListTargetProperty_ShouldReturnANewEnumListOptionSetup()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<DataTypesCommandOptions>(parser);
            commandParser.IsCommandDefault = true;

            A.CallTo(() => parser.GetOrCreateCommandParser<DataTypesCommandOptions>(null)).Returns(commandParser);

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser);

            Expression<Func<DataTypesCommandOptions, List<LogLevel>>> propertyExpression = a => a.Enums;

            var optionSetup = new EnumListOptionSetup<DataTypesCommandOptions, LogLevel>(commandParser, propertyExpression);
            A.CallTo(() => this.DependencyResolver.Resolve<EnumListOptionSetup<DataTypesCommandOptions, LogLevel>>(commandParser, propertyExpression)).Returns(optionSetup);

            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => this.DependencyResolver.Resolve<EnumListOptionSetup<DataTypesCommandOptions, LogLevel>>(commandParser, propertyExpression)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of EnumOptionSetup when called for a Enum target property.")]
        public void Option_EnumTargetProperty_ShouldReturnANewEnumOptionSetup()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<DataTypesCommandOptions>(parser);
            commandParser.IsCommandDefault = true;

            A.CallTo(() => parser.GetOrCreateCommandParser<DataTypesCommandOptions>(null)).Returns(commandParser);

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser);

            Expression<Func<DataTypesCommandOptions, LogLevel>> propertyExpression = a => a.Enum;

            var optionSetup = new EnumOptionSetup<DataTypesCommandOptions, LogLevel>(commandParser, propertyExpression);
            A.CallTo(() => this.DependencyResolver.Resolve<EnumOptionSetup<DataTypesCommandOptions, LogLevel>>(commandParser, propertyExpression)).Returns(optionSetup);

            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => this.DependencyResolver.Resolve<EnumOptionSetup<DataTypesCommandOptions, LogLevel>>(commandParser, propertyExpression)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of GuidListOptionSetup when called for a List<Guid> target property.")]
        public void Option_GuidListTargetProperty_ShouldReturnANewGuidListOptionSetup()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<DataTypesCommandOptions>(parser);
            commandParser.IsCommandDefault = true;

            A.CallTo(() => parser.GetOrCreateCommandParser<DataTypesCommandOptions>(null)).Returns(commandParser);

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser);

            Expression<Func<DataTypesCommandOptions, List<Guid>>> propertyExpression = a => a.Guids;

            var optionSetup = new GuidListOptionSetup<DataTypesCommandOptions>(commandParser, propertyExpression);
            A.CallTo(() => this.DependencyResolver.Resolve<GuidListOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).Returns(optionSetup);

            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => this.DependencyResolver.Resolve<GuidListOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of GuidOptionSetup when called for a Guid target property.")]
        public void Option_GuidTargetProperty_ShouldReturnANewGuidOptionSetup()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<DataTypesCommandOptions>(parser);
            commandParser.IsCommandDefault = true;

            A.CallTo(() => parser.GetOrCreateCommandParser<DataTypesCommandOptions>(null)).Returns(commandParser);

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser);

            Expression<Func<DataTypesCommandOptions, Guid>> propertyExpression = a => a.Guid;

            var optionSetup = new GuidOptionSetup<DataTypesCommandOptions>(commandParser, propertyExpression);
            A.CallTo(() => this.DependencyResolver.Resolve<GuidOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).Returns(optionSetup);

            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => this.DependencyResolver.Resolve<GuidOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of Int64ListOptionSetup when called for a List<Int64> target property.")]
        public void Option_Int64ListTargetProperty_ShouldReturnANewInt64ListOptionSetup()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<DataTypesCommandOptions>(parser);
            commandParser.IsCommandDefault = true;

            A.CallTo(() => parser.GetOrCreateCommandParser<DataTypesCommandOptions>(null)).Returns(commandParser);

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser);

            Expression<Func<DataTypesCommandOptions, List<Int64>>> propertyExpression = a => a.Int64s;

            var optionSetup = new Int64ListOptionSetup<DataTypesCommandOptions>(commandParser, propertyExpression);
            A.CallTo(() => this.DependencyResolver.Resolve<Int64ListOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).Returns(optionSetup);

            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => this.DependencyResolver.Resolve<Int64ListOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of Int64OptionSetup when called for a Int64 target property.")]
        public void Option_Int64TargetProperty_ShouldReturnANewInt64OptionSetup()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<DataTypesCommandOptions>(parser);
            commandParser.IsCommandDefault = true;

            A.CallTo(() => parser.GetOrCreateCommandParser<DataTypesCommandOptions>(null)).Returns(commandParser);

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser);

            Expression<Func<DataTypesCommandOptions, Int64>> propertyExpression = a => a.Int64;

            var optionSetup = new Int64OptionSetup<DataTypesCommandOptions>(commandParser, propertyExpression);
            A.CallTo(() => this.DependencyResolver.Resolve<Int64OptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).Returns(optionSetup);

            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => this.DependencyResolver.Resolve<Int64OptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of DateTimeOptionSetup when called for a Nullable<DateTime> target property.")]
        public void Option_NullableDateTimeTargetProperty_ShouldReturnANewDateTimeOptionSetup()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<DataTypesCommandOptions>(parser);
            commandParser.IsCommandDefault = true;

            A.CallTo(() => parser.GetOrCreateCommandParser<DataTypesCommandOptions>(null)).Returns(commandParser);

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser);

            Expression<Func<DataTypesCommandOptions, Nullable<DateTime>>> propertyExpression = a => a.NullableDateTime;

            var optionSetup = new DateTimeOptionSetup<DataTypesCommandOptions>(commandParser, propertyExpression);
            A.CallTo(() => this.DependencyResolver.Resolve<DateTimeOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).Returns(optionSetup);

            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => this.DependencyResolver.Resolve<DateTimeOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of DecimalOptionSetup when called for a Nullable<Decimal> target property.")]
        public void Option_NullableDecimalTargetProperty_ShouldReturnANewDecimalOptionSetup()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<DataTypesCommandOptions>(parser);
            commandParser.IsCommandDefault = true;

            A.CallTo(() => parser.GetOrCreateCommandParser<DataTypesCommandOptions>(null)).Returns(commandParser);

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser);

            Expression<Func<DataTypesCommandOptions, Nullable<Decimal>>> propertyExpression = a => a.NullableDecimal;

            var optionSetup = new DecimalOptionSetup<DataTypesCommandOptions>(commandParser, propertyExpression);
            A.CallTo(() => this.DependencyResolver.Resolve<DecimalOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).Returns(optionSetup);

            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => this.DependencyResolver.Resolve<DecimalOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of EnumOptionSetup when called for a Nullable<Enum> target property.")]
        public void Option_NullableEnumTargetProperty_ShouldReturnANewEnumOptionSetup()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<DataTypesCommandOptions>(parser);
            commandParser.IsCommandDefault = true;

            A.CallTo(() => parser.GetOrCreateCommandParser<DataTypesCommandOptions>(null)).Returns(commandParser);

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser);

            Expression<Func<DataTypesCommandOptions, Nullable<LogLevel>>> propertyExpression = a => a.NullableEnum;

            var optionSetup = new EnumOptionSetup<DataTypesCommandOptions, LogLevel>(commandParser, propertyExpression);
            A.CallTo(() => this.DependencyResolver.Resolve<EnumOptionSetup<DataTypesCommandOptions, LogLevel>>(commandParser, propertyExpression)).Returns(optionSetup);

            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => this.DependencyResolver.Resolve<EnumOptionSetup<DataTypesCommandOptions, LogLevel>>(commandParser, propertyExpression)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of GuidOptionSetup when called for a Nullable<Guid> target property.")]
        public void Option_NullableGuidTargetProperty_ShouldReturnANewGuidOptionSetup()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<DataTypesCommandOptions>(parser);
            commandParser.IsCommandDefault = true;

            A.CallTo(() => parser.GetOrCreateCommandParser<DataTypesCommandOptions>(null)).Returns(commandParser);

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser);

            Expression<Func<DataTypesCommandOptions, Nullable<Guid>>> propertyExpression = a => a.NullableGuid;

            var optionSetup = new GuidOptionSetup<DataTypesCommandOptions>(commandParser, propertyExpression);
            A.CallTo(() => this.DependencyResolver.Resolve<GuidOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).Returns(optionSetup);

            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => this.DependencyResolver.Resolve<GuidOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of Int64OptionSetup when called for a Nullable<Int64> target property.")]
        public void Option_NullableInt64TargetProperty_ShouldReturnANewInt64OptionSetup()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<DataTypesCommandOptions>(parser);
            commandParser.IsCommandDefault = true;

            A.CallTo(() => parser.GetOrCreateCommandParser<DataTypesCommandOptions>(null)).Returns(commandParser);

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser);

            Expression<Func<DataTypesCommandOptions, Nullable<Int64>>> propertyExpression = a => a.NullableInt64;

            var optionSetup = new Int64OptionSetup<DataTypesCommandOptions>(commandParser, propertyExpression);
            A.CallTo(() => this.DependencyResolver.Resolve<Int64OptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).Returns(optionSetup);

            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => this.DependencyResolver.Resolve<Int64OptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of TimeSpanOptionSetup when called for a Nullable<TimeSpan> target property.")]
        public void Option_NullableTimeSpanTargetProperty_ShouldReturnANewTimeSpanOptionSetup()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<DataTypesCommandOptions>(parser);
            commandParser.IsCommandDefault = true;

            A.CallTo(() => parser.GetOrCreateCommandParser<DataTypesCommandOptions>(null)).Returns(commandParser);

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser);

            Expression<Func<DataTypesCommandOptions, Nullable<TimeSpan>>> propertyExpression = a => a.NullableTimeSpan;

            var optionSetup = new TimeSpanOptionSetup<DataTypesCommandOptions>(commandParser, propertyExpression);
            A.CallTo(() => this.DependencyResolver.Resolve<TimeSpanOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).Returns(optionSetup);

            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => this.DependencyResolver.Resolve<TimeSpanOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of StringListOptionSetup when called for a List<String> target property.")]
        public void Option_StringListTargetProperty_ShouldReturnANewStringListOptionSetup()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<DataTypesCommandOptions>(parser);
            commandParser.IsCommandDefault = true;

            A.CallTo(() => parser.GetOrCreateCommandParser<DataTypesCommandOptions>(null)).Returns(commandParser);

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser);

            Expression<Func<DataTypesCommandOptions, List<String>>> propertyExpression = a => a.Strings;

            var optionSetup = new StringListOptionSetup<DataTypesCommandOptions>(commandParser, propertyExpression);
            A.CallTo(() => this.DependencyResolver.Resolve<StringListOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).Returns(optionSetup);

            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => this.DependencyResolver.Resolve<StringListOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of StringOptionSetup when called for a String target property.")]
        public void Option_StringTargetProperty_ShouldReturnANewStringOptionSetup()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<DataTypesCommandOptions>(parser);
            commandParser.IsCommandDefault = true;

            A.CallTo(() => parser.GetOrCreateCommandParser<DataTypesCommandOptions>(null)).Returns(commandParser);

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser);

            Expression<Func<DataTypesCommandOptions, String>> propertyExpression = a => a.String;

            var optionSetup = new StringOptionSetup<DataTypesCommandOptions>(commandParser, propertyExpression);
            A.CallTo(() => this.DependencyResolver.Resolve<StringOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).Returns(optionSetup);

            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => this.DependencyResolver.Resolve<StringOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of TimeSpanListOptionSetup when called for a List<TimeSpan> target property.")]
        public void Option_TimeSpanListTargetProperty_ShouldReturnANewTimeSpanListOptionSetup()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<DataTypesCommandOptions>(parser);
            commandParser.IsCommandDefault = true;

            A.CallTo(() => parser.GetOrCreateCommandParser<DataTypesCommandOptions>(null)).Returns(commandParser);

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser);

            Expression<Func<DataTypesCommandOptions, List<TimeSpan>>> propertyExpression = a => a.TimeSpans;

            var optionSetup = new TimeSpanListOptionSetup<DataTypesCommandOptions>(commandParser, propertyExpression);
            A.CallTo(() => this.DependencyResolver.Resolve<TimeSpanListOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).Returns(optionSetup);

            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => this.DependencyResolver.Resolve<TimeSpanListOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of TimeSpanOptionSetup when called for a TimeSpan target property.")]
        public void Option_TimeSpanTargetProperty_ShouldReturnANewTimeSpanOptionSetup()
        {
            var parser = A.Fake<Parser>();

            var commandParser = new CommandParser<DataTypesCommandOptions>(parser);
            commandParser.IsCommandDefault = true;

            A.CallTo(() => parser.GetOrCreateCommandParser<DataTypesCommandOptions>(null)).Returns(commandParser);

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser);

            Expression<Func<DataTypesCommandOptions, TimeSpan>> propertyExpression = a => a.TimeSpan;

            var optionSetup = new TimeSpanOptionSetup<DataTypesCommandOptions>(commandParser, propertyExpression);
            A.CallTo(() => this.DependencyResolver.Resolve<TimeSpanOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).Returns(optionSetup);

            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => this.DependencyResolver.Resolve<TimeSpanOptionSetup<DataTypesCommandOptions>>(commandParser, propertyExpression)).MustHaveHappened();
        }

        [Test(Description = "Validate should assign the given validator to the command parser.")]
        public void Validate_ShouldAssignValidatorToCommandParser()
        {
            var parser = A.Fake<Parser>();
            var validator = A.Fake<Action<CommandValidatorContext<Command1Options>>>();

            var commandParser = new CommandParser<Command1Options>(parser);
            commandParser.IsCommandDefault = true;

            A.CallTo(() => parser.GetOrCreateCommandParser<Command1Options>(null)).Returns(commandParser);

            var setup = new DefaultCommandSetup<Command1Options>(parser);

            setup.Validate(validator);

            commandParser.Validator.Should().Be(validator);
        }

        [Test(Description = "Validate should return the same instance of the command setup.")]
        public void Validate_ShouldReturnCommandSetup()
        {
            var parser = A.Fake<Parser>();
            var validator = A.Fake<Action<CommandValidatorContext<Command1Options>>>();

            var setup = new DefaultCommandSetup<Command1Options>(parser);

            setup.Validate(validator).Should().Be(setup);
        }
    }
}