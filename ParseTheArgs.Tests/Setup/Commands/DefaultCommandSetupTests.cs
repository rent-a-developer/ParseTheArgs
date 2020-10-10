using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Parsers.Options;
using ParseTheArgs.Setup.Commands;
using ParseTheArgs.Setup.Options;
using ParseTheArgs.Tests.TestData;
using ParseTheArgs.Validation;
using static FluentAssertions.FluentActions;

namespace ParseTheArgs.Tests.Setup.Commands
{
    [TestFixture]
    public class DefaultCommandSetupTests : BaseTestFixture
    {
        [Test(Description = "Constructor should throw an exception when the given parser is null.")]
        public void Constructor_ParserIsNull_ShouldThrowException()
        {
            var commandParser = A.Fake<CommandParser<Command1Options>>();

            Invoking(() => new DefaultCommandSetup<Command1Options>(null, commandParser))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "Constructor should throw an exception when the given command parser is null.")]
        public void Constructor_CommandParserIsNull_ShouldThrowException()
        {
            var parser = A.Fake<Parser>();

            Invoking(() => new DefaultCommandSetup<Command1Options>(parser, null))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "ExampleUsage should assign the given example usage to the command parser.")]
        public void ExampleUsage_ShouldAssignExampleUsageToCommandParser()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<Command1Options>>();

            var setup = new DefaultCommandSetup<Command1Options>(parser, commandParser);

            setup.ExampleUsage("newExampleUsage");

            A.CallToSet(() => commandParser.CommandExampleUsage).To("newExampleUsage").MustHaveHappened();
        }

        [Test(Description = "ExampleUsage should return the same instance of the command setup.")]
        public void ExampleUsage_ShouldReturnCommandSetup()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<Command1Options>>();

            var setup = new DefaultCommandSetup<Command1Options>(parser, commandParser);

            setup.ExampleUsage("newExampleUsage").Should().Be(setup);
        }

        [Test(Description = "Help should assign the given help text to the command parser.")]
        public void Help_ShouldAssignHelpTextToCommandParser()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<Command1Options>>();

            var setup = new DefaultCommandSetup<Command1Options>(parser, commandParser);

            setup.Help("newHelpText");

            A.CallToSet(() => commandParser.CommandHelp).To("newHelpText").MustHaveHappened();
        }

        [Test(Description = "Help should return the same instance of the command setup.")]
        public void Help_ShouldReturnCommandSetup()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<Command1Options>>();

            var setup = new DefaultCommandSetup<Command1Options>(parser, commandParser);

            setup.Help("helpText").Should().Be(setup);
        }

        [Test(Description = "Option should return a new instance of BooleanOptionSetup when called for a Boolean target property.")]
        public void Option_BooleanTargetProperty_ShouldReturnANewBooleanOptionSetup()
        {
            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Boolean");

            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<BooleanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new BooleanOptionParser(targetProperty, "boolean")));
            var optionSetup = A.Fake<BooleanOptionSetup<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new BooleanOptionSetup<DataTypesCommandOptions>(commandParser, optionParser)));

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser, commandParser);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<BooleanOptionParser>(targetProperty)).Returns(optionParser);
            A.CallTo(() => this.DependencyResolver.Resolve<BooleanOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).Returns(optionSetup);

            Expression<Func<DataTypesCommandOptions, Boolean>> propertyExpression = a => a.Boolean;
            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<BooleanOptionParser>(targetProperty)).MustHaveHappened();
            A.CallTo(() => this.DependencyResolver.Resolve<BooleanOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of DateTimeListOptionSetup when called for a List<DateTime> target property.")]
        public void Option_DateTimeListTargetProperty_ShouldReturnANewDateTimeListOptionSetup()
        {
            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("DateTimes");

            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<DateTimeListOptionParser>(ob => ob.WithArgumentsForConstructor(() => new DateTimeListOptionParser(targetProperty, "dateTimes")));
            var optionSetup = A.Fake<DateTimeListOptionSetup<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new DateTimeListOptionSetup<DataTypesCommandOptions>(commandParser, optionParser)));

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser, commandParser);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<DateTimeListOptionParser>(targetProperty)).Returns(optionParser);
            A.CallTo(() => this.DependencyResolver.Resolve<DateTimeListOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).Returns(optionSetup);

            Expression<Func<DataTypesCommandOptions, List<DateTime>>> propertyExpression = a => a.DateTimes;
            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<DateTimeListOptionParser>(targetProperty)).MustHaveHappened();
            A.CallTo(() => this.DependencyResolver.Resolve<DateTimeListOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of DateTimeOptionSetup when called for a DateTime target property.")]
        public void Option_DateTimeTargetProperty_ShouldReturnANewDateTimeOptionSetup()
        {
            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("DateTime");

            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<DateTimeOptionParser>(ob => ob.WithArgumentsForConstructor(() => new DateTimeOptionParser(targetProperty, "dateTime")));
            var optionSetup = A.Fake<DateTimeOptionSetup<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new DateTimeOptionSetup<DataTypesCommandOptions>(commandParser, optionParser)));

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser, commandParser);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<DateTimeOptionParser>(targetProperty)).Returns(optionParser);
            A.CallTo(() => this.DependencyResolver.Resolve<DateTimeOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).Returns(optionSetup);

            Expression<Func<DataTypesCommandOptions, DateTime>> propertyExpression = a => a.DateTime;
            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<DateTimeOptionParser>(targetProperty)).MustHaveHappened();
            A.CallTo(() => this.DependencyResolver.Resolve<DateTimeOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of DecimalListOptionSetup when called for a List<Decimal> target property.")]
        public void Option_DecimalListTargetProperty_ShouldReturnANewDecimalListOptionSetup()
        {
            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Decimals");

            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<DecimalListOptionParser>(ob => ob.WithArgumentsForConstructor(() => new DecimalListOptionParser(targetProperty, "decimals")));
            var optionSetup = A.Fake<DecimalListOptionSetup<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new DecimalListOptionSetup<DataTypesCommandOptions>(commandParser, optionParser)));

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser, commandParser);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<DecimalListOptionParser>(targetProperty)).Returns(optionParser);
            A.CallTo(() => this.DependencyResolver.Resolve<DecimalListOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).Returns(optionSetup);

            Expression<Func<DataTypesCommandOptions, List<Decimal>>> propertyExpression = a => a.Decimals;
            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<DecimalListOptionParser>(targetProperty)).MustHaveHappened();
            A.CallTo(() => this.DependencyResolver.Resolve<DecimalListOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of DecimalOptionSetup when called for a Decimal target property.")]
        public void Option_DecimalTargetProperty_ShouldReturnANewDecimalOptionSetup()
        {
            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Decimal");

            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<DecimalOptionParser>(ob => ob.WithArgumentsForConstructor(() => new DecimalOptionParser(targetProperty, "decimal")));
            var optionSetup = A.Fake<DecimalOptionSetup<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new DecimalOptionSetup<DataTypesCommandOptions>(commandParser, optionParser)));

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser, commandParser);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<DecimalOptionParser>(targetProperty)).Returns(optionParser);
            A.CallTo(() => this.DependencyResolver.Resolve<DecimalOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).Returns(optionSetup);

            Expression<Func<DataTypesCommandOptions, Decimal>> propertyExpression = a => a.Decimal;
            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<DecimalOptionParser>(targetProperty)).MustHaveHappened();
            A.CallTo(() => this.DependencyResolver.Resolve<DecimalOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of EnumListOptionSetup when called for a List<Enum> target property.")]
        public void Option_EnumListTargetProperty_ShouldReturnANewEnumListOptionSetup()
        {
            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Enums");

            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<EnumListOptionParser<LogLevel>>(ob => ob.WithArgumentsForConstructor(() => new EnumListOptionParser<LogLevel>(targetProperty, "enums")));
            var optionSetup = A.Fake<EnumListOptionSetup<DataTypesCommandOptions, LogLevel>>(ob => ob.WithArgumentsForConstructor(() => new EnumListOptionSetup<DataTypesCommandOptions, LogLevel>(commandParser, optionParser)));

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser, commandParser);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<EnumListOptionParser<LogLevel>>(targetProperty)).Returns(optionParser);
            A.CallTo(() => this.DependencyResolver.Resolve<EnumListOptionSetup<DataTypesCommandOptions, LogLevel>>(commandParser, optionParser)).Returns(optionSetup);

            Expression<Func<DataTypesCommandOptions, List<LogLevel>>> propertyExpression = a => a.Enums;
            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<EnumListOptionParser<LogLevel>>(targetProperty)).MustHaveHappened();
            A.CallTo(() => this.DependencyResolver.Resolve<EnumListOptionSetup<DataTypesCommandOptions, LogLevel>>(commandParser, optionParser)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of EnumOptionSetup when called for a Enum target property.")]
        public void Option_EnumTargetProperty_ShouldReturnANewEnumOptionSetup()
        {
            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Enum");

            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<EnumOptionParser<LogLevel>>(ob => ob.WithArgumentsForConstructor(() => new EnumOptionParser<LogLevel>(targetProperty, "enum")));
            var optionSetup = A.Fake<EnumOptionSetup<DataTypesCommandOptions, LogLevel>>(ob => ob.WithArgumentsForConstructor(() => new EnumOptionSetup<DataTypesCommandOptions, LogLevel>(commandParser, optionParser)));

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser, commandParser);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<EnumOptionParser<LogLevel>>(targetProperty)).Returns(optionParser);
            A.CallTo(() => this.DependencyResolver.Resolve<EnumOptionSetup<DataTypesCommandOptions, LogLevel>>(commandParser, optionParser)).Returns(optionSetup);

            Expression<Func<DataTypesCommandOptions, LogLevel>> propertyExpression = a => a.Enum;
            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<EnumOptionParser<LogLevel>>(targetProperty)).MustHaveHappened();
            A.CallTo(() => this.DependencyResolver.Resolve<EnumOptionSetup<DataTypesCommandOptions, LogLevel>>(commandParser, optionParser)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of GuidListOptionSetup when called for a List<Guid> target property.")]
        public void Option_GuidListTargetProperty_ShouldReturnANewGuidListOptionSetup()
        {
            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Guids");

            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<GuidListOptionParser>(ob => ob.WithArgumentsForConstructor(() => new GuidListOptionParser(targetProperty, "guids")));
            var optionSetup = A.Fake<GuidListOptionSetup<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new GuidListOptionSetup<DataTypesCommandOptions>(commandParser, optionParser)));

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser, commandParser);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<GuidListOptionParser>(targetProperty)).Returns(optionParser);
            A.CallTo(() => this.DependencyResolver.Resolve<GuidListOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).Returns(optionSetup);

            Expression<Func<DataTypesCommandOptions, List<Guid>>> propertyExpression = a => a.Guids;
            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<GuidListOptionParser>(targetProperty)).MustHaveHappened();
            A.CallTo(() => this.DependencyResolver.Resolve<GuidListOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of GuidOptionSetup when called for a Guid target property.")]
        public void Option_GuidTargetProperty_ShouldReturnANewGuidOptionSetup()
        {
            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Guid");

            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<GuidOptionParser>(ob => ob.WithArgumentsForConstructor(() => new GuidOptionParser(targetProperty, "guid")));
            var optionSetup = A.Fake<GuidOptionSetup<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new GuidOptionSetup<DataTypesCommandOptions>(commandParser, optionParser)));

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser, commandParser);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<GuidOptionParser>(targetProperty)).Returns(optionParser);
            A.CallTo(() => this.DependencyResolver.Resolve<GuidOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).Returns(optionSetup);

            Expression<Func<DataTypesCommandOptions, Guid>> propertyExpression = a => a.Guid;
            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<GuidOptionParser>(targetProperty)).MustHaveHappened();
            A.CallTo(() => this.DependencyResolver.Resolve<GuidOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of Int64ListOptionSetup when called for a List<Int64> target property.")]
        public void Option_Int64ListTargetProperty_ShouldReturnANewInt64ListOptionSetup()
        {
            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Int64s");

            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<Int64ListOptionParser>(ob => ob.WithArgumentsForConstructor(() => new Int64ListOptionParser(targetProperty, "int64s")));
            var optionSetup = A.Fake<Int64ListOptionSetup<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new Int64ListOptionSetup<DataTypesCommandOptions>(commandParser, optionParser)));

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser, commandParser);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<Int64ListOptionParser>(targetProperty)).Returns(optionParser);
            A.CallTo(() => this.DependencyResolver.Resolve<Int64ListOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).Returns(optionSetup);

            Expression<Func<DataTypesCommandOptions, List<Int64>>> propertyExpression = a => a.Int64s;
            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<Int64ListOptionParser>(targetProperty)).MustHaveHappened();
            A.CallTo(() => this.DependencyResolver.Resolve<Int64ListOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of Int64OptionSetup when called for a Int64 target property.")]
        public void Option_Int64TargetProperty_ShouldReturnANewInt64OptionSetup()
        {
            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Int64");

            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<Int64OptionParser>(ob => ob.WithArgumentsForConstructor(() => new Int64OptionParser(targetProperty, "int64")));
            var optionSetup = A.Fake<Int64OptionSetup<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new Int64OptionSetup<DataTypesCommandOptions>(commandParser, optionParser)));

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser, commandParser);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<Int64OptionParser>(targetProperty)).Returns(optionParser);
            A.CallTo(() => this.DependencyResolver.Resolve<Int64OptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).Returns(optionSetup);

            Expression<Func<DataTypesCommandOptions, Int64>> propertyExpression = a => a.Int64;
            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<Int64OptionParser>(targetProperty)).MustHaveHappened();
            A.CallTo(() => this.DependencyResolver.Resolve<Int64OptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of DateTimeOptionSetup when called for a Nullable<DateTime> target property.")]
        public void Option_NullableDateTimeTargetProperty_ShouldReturnANewDateTimeOptionSetup()
        {
            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("NullableDateTime");

            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<DateTimeOptionParser>(ob => ob.WithArgumentsForConstructor(() => new DateTimeOptionParser(targetProperty, "nullableDateTime")));
            var optionSetup = A.Fake<DateTimeOptionSetup<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new DateTimeOptionSetup<DataTypesCommandOptions>(commandParser, optionParser)));

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser, commandParser);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<DateTimeOptionParser>(targetProperty)).Returns(optionParser);
            A.CallTo(() => this.DependencyResolver.Resolve<DateTimeOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).Returns(optionSetup);

            Expression<Func<DataTypesCommandOptions, Nullable<DateTime>>> propertyExpression = a => a.NullableDateTime;
            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<DateTimeOptionParser>(targetProperty)).MustHaveHappened();
            A.CallTo(() => this.DependencyResolver.Resolve<DateTimeOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of DecimalOptionSetup when called for a Nullable<Decimal> target property.")]
        public void Option_NullableDecimalTargetProperty_ShouldReturnANewDecimalOptionSetup()
        {
            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("NullableDecimal");

            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<DecimalOptionParser>(ob => ob.WithArgumentsForConstructor(() => new DecimalOptionParser(targetProperty, "nullableDecimal")));
            var optionSetup = A.Fake<DecimalOptionSetup<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new DecimalOptionSetup<DataTypesCommandOptions>(commandParser, optionParser)));

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser, commandParser);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<DecimalOptionParser>(targetProperty)).Returns(optionParser);
            A.CallTo(() => this.DependencyResolver.Resolve<DecimalOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).Returns(optionSetup);

            Expression<Func<DataTypesCommandOptions, Nullable<Decimal>>> propertyExpression = a => a.NullableDecimal;
            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<DecimalOptionParser>(targetProperty)).MustHaveHappened();
            A.CallTo(() => this.DependencyResolver.Resolve<DecimalOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of EnumOptionSetup when called for a Nullable<Enum> target property.")]
        public void Option_NullableEnumTargetProperty_ShouldReturnANewEnumOptionSetup()
        {
            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("NullableEnum");

            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<EnumOptionParser<LogLevel>>(ob => ob.WithArgumentsForConstructor(() => new EnumOptionParser<LogLevel>(targetProperty, "nullableEnum")));
            var optionSetup = A.Fake<EnumOptionSetup<DataTypesCommandOptions, LogLevel>>(ob => ob.WithArgumentsForConstructor(() => new EnumOptionSetup<DataTypesCommandOptions, LogLevel>(commandParser, optionParser)));

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser, commandParser);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<EnumOptionParser<LogLevel>>(targetProperty)).Returns(optionParser);
            A.CallTo(() => this.DependencyResolver.Resolve<EnumOptionSetup<DataTypesCommandOptions, LogLevel>>(commandParser, optionParser)).Returns(optionSetup);

            Expression<Func<DataTypesCommandOptions, Nullable<LogLevel>>> propertyExpression = a => a.NullableEnum;
            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<EnumOptionParser<LogLevel>>(targetProperty)).MustHaveHappened();
            A.CallTo(() => this.DependencyResolver.Resolve<EnumOptionSetup<DataTypesCommandOptions, LogLevel>>(commandParser, optionParser)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of GuidOptionSetup when called for a Nullable<Guid> target property.")]
        public void Option_NullableGuidTargetProperty_ShouldReturnANewGuidOptionSetup()
        {
            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("NullableGuid");

            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<GuidOptionParser>(ob => ob.WithArgumentsForConstructor(() => new GuidOptionParser(targetProperty, "nullableGuid")));
            var optionSetup = A.Fake<GuidOptionSetup<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new GuidOptionSetup<DataTypesCommandOptions>(commandParser, optionParser)));

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser, commandParser);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<GuidOptionParser>(targetProperty)).Returns(optionParser);
            A.CallTo(() => this.DependencyResolver.Resolve<GuidOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).Returns(optionSetup);

            Expression<Func<DataTypesCommandOptions, Nullable<Guid>>> propertyExpression = a => a.NullableGuid;
            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<GuidOptionParser>(targetProperty)).MustHaveHappened();
            A.CallTo(() => this.DependencyResolver.Resolve<GuidOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of Int64OptionSetup when called for a Nullable<Int64> target property.")]
        public void Option_NullableInt64TargetProperty_ShouldReturnANewInt64OptionSetup()
        {
            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("NullableInt64");

            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<Int64OptionParser>(ob => ob.WithArgumentsForConstructor(() => new Int64OptionParser(targetProperty, "nullableInt64")));
            var optionSetup = A.Fake<Int64OptionSetup<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new Int64OptionSetup<DataTypesCommandOptions>(commandParser, optionParser)));

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser, commandParser);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<Int64OptionParser>(targetProperty)).Returns(optionParser);
            A.CallTo(() => this.DependencyResolver.Resolve<Int64OptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).Returns(optionSetup);

            Expression<Func<DataTypesCommandOptions, Nullable<Int64>>> propertyExpression = a => a.NullableInt64;
            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<Int64OptionParser>(targetProperty)).MustHaveHappened();
            A.CallTo(() => this.DependencyResolver.Resolve<Int64OptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of TimeSpanOptionSetup when called for a Nullable<TimeSpan> target property.")]
        public void Option_NullableTimeSpanTargetProperty_ShouldReturnANewTimeSpanOptionSetup()
        {
            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("NullableTimeSpan");

            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<TimeSpanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new TimeSpanOptionParser(targetProperty, "nullableTimeSpan")));
            var optionSetup = A.Fake<TimeSpanOptionSetup<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new TimeSpanOptionSetup<DataTypesCommandOptions>(commandParser, optionParser)));

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser, commandParser);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<TimeSpanOptionParser>(targetProperty)).Returns(optionParser);
            A.CallTo(() => this.DependencyResolver.Resolve<TimeSpanOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).Returns(optionSetup);

            Expression<Func<DataTypesCommandOptions, Nullable<TimeSpan>>> propertyExpression = a => a.NullableTimeSpan;
            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<TimeSpanOptionParser>(targetProperty)).MustHaveHappened();
            A.CallTo(() => this.DependencyResolver.Resolve<TimeSpanOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of StringListOptionSetup when called for a List<String> target property.")]
        public void Option_StringListTargetProperty_ShouldReturnANewStringListOptionSetup()
        {
            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("Strings");

            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<StringListOptionParser>(ob => ob.WithArgumentsForConstructor(() => new StringListOptionParser(targetProperty, "strings")));
            var optionSetup = A.Fake<StringListOptionSetup<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new StringListOptionSetup<DataTypesCommandOptions>(commandParser, optionParser)));

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser, commandParser);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<StringListOptionParser>(targetProperty)).Returns(optionParser);
            A.CallTo(() => this.DependencyResolver.Resolve<StringListOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).Returns(optionSetup);

            Expression<Func<DataTypesCommandOptions, List<String>>> propertyExpression = a => a.Strings;
            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<StringListOptionParser>(targetProperty)).MustHaveHappened();
            A.CallTo(() => this.DependencyResolver.Resolve<StringListOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of StringOptionSetup when called for a String target property.")]
        public void Option_StringTargetProperty_ShouldReturnANewStringOptionSetup()
        {
            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("String");

            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<StringOptionParser>(ob => ob.WithArgumentsForConstructor(() => new StringOptionParser(targetProperty, "string")));
            var optionSetup = A.Fake<StringOptionSetup<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new StringOptionSetup<DataTypesCommandOptions>(commandParser, optionParser)));

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser, commandParser);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<StringOptionParser>(targetProperty)).Returns(optionParser);
            A.CallTo(() => this.DependencyResolver.Resolve<StringOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).Returns(optionSetup);

            Expression<Func<DataTypesCommandOptions, String>> propertyExpression = a => a.String;
            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<StringOptionParser>(targetProperty)).MustHaveHappened();
            A.CallTo(() => this.DependencyResolver.Resolve<StringOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of TimeSpanListOptionSetup when called for a List<TimeSpan> target property.")]
        public void Option_TimeSpanListTargetProperty_ShouldReturnANewTimeSpanListOptionSetup()
        {
            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("TimeSpans");

            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<TimeSpanListOptionParser>(ob => ob.WithArgumentsForConstructor(() => new TimeSpanListOptionParser(targetProperty, "timeSpans")));
            var optionSetup = A.Fake<TimeSpanListOptionSetup<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new TimeSpanListOptionSetup<DataTypesCommandOptions>(commandParser, optionParser)));

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser, commandParser);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<TimeSpanListOptionParser>(targetProperty)).Returns(optionParser);
            A.CallTo(() => this.DependencyResolver.Resolve<TimeSpanListOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).Returns(optionSetup);

            Expression<Func<DataTypesCommandOptions, List<TimeSpan>>> propertyExpression = a => a.TimeSpans;
            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<TimeSpanListOptionParser>(targetProperty)).MustHaveHappened();
            A.CallTo(() => this.DependencyResolver.Resolve<TimeSpanListOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).MustHaveHappened();
        }

        [Test(Description = "Option should return a new instance of TimeSpanOptionSetup when called for a TimeSpan target property.")]
        public void Option_TimeSpanTargetProperty_ShouldReturnANewTimeSpanOptionSetup()
        {
            var targetProperty = typeof(DataTypesCommandOptions).GetProperty("TimeSpan");

            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<DataTypesCommandOptions>(parser)));
            var optionParser = A.Fake<TimeSpanOptionParser>(ob => ob.WithArgumentsForConstructor(() => new TimeSpanOptionParser(targetProperty, "timeSpan")));
            var optionSetup = A.Fake<TimeSpanOptionSetup<DataTypesCommandOptions>>(ob => ob.WithArgumentsForConstructor(() => new TimeSpanOptionSetup<DataTypesCommandOptions>(commandParser, optionParser)));

            var setup = new DefaultCommandSetup<DataTypesCommandOptions>(parser, commandParser);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<TimeSpanOptionParser>(targetProperty)).Returns(optionParser);
            A.CallTo(() => this.DependencyResolver.Resolve<TimeSpanOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).Returns(optionSetup);

            Expression<Func<DataTypesCommandOptions, TimeSpan>> propertyExpression = a => a.TimeSpan;
            setup.Option(propertyExpression).Should().Be(optionSetup);

            A.CallTo(() => commandParser.GetOrCreateOptionParser<TimeSpanOptionParser>(targetProperty)).MustHaveHappened();
            A.CallTo(() => this.DependencyResolver.Resolve<TimeSpanOptionSetup<DataTypesCommandOptions>>(commandParser, optionParser)).MustHaveHappened();
        }

        [Test(Description = "Validate should assign the given validator to the command parser.")]
        public void Validate_ShouldAssignValidatorToCommandParser()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<Command1Options>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<Command1Options>(parser)));
            var validator = A.Fake<Action<CommandValidatorContext<Command1Options>>>();

            var setup = new DefaultCommandSetup<Command1Options>(parser, commandParser);

            setup.Validate(validator);

            A.CallToSet(() => commandParser.Validator).To(validator).MustHaveHappened();
        }

        [Test(Description = "Validate should return the same instance of the command setup.")]
        public void Validate_ShouldReturnCommandSetup()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<Command1Options>>(ob => ob.WithArgumentsForConstructor(() => new CommandParser<Command1Options>(parser)));
            var validator = A.Fake<Action<CommandValidatorContext<Command1Options>>>();

            var setup = new DefaultCommandSetup<Command1Options>(parser, commandParser);

            setup.Validate(validator).Should().Be(setup);
        }
    }
}