using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using ParseTheArgs.Errors;
using ParseTheArgs.Tests.TestData;

namespace ParseTheArgs.Tests
{
    [TestFixture]
    public class ParserTests
    {
        [SetUp]
        public void SetUp()
        {
            // We fix the current culture to en-US so that parsing of values (e.g. DateTime values) is done in a deterministic fashion.
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }

        [Test(Description = "Parse should set ParseResult.IsHelpCalled to true when the help was called.")]
        public void Parse_HelpCalled_ShouldSetIsHelpCalledToTrue()
        {
            var parser = new Parser();

            var parseResult1 = parser.Parse(new String[] {});
            parseResult1.IsHelpCalled.Should().BeTrue();

            var parseResult2 = parser.Parse(new String[] {"help"});
            parseResult2.IsHelpCalled.Should().BeTrue();

            var parseResult3 = parser.Parse(new String[] { "help", "command1" });
            parseResult3.IsHelpCalled.Should().BeTrue();
        }

        [Test(Description = "Parse should write the help screen to the help text writer when help was called.")]
        public void Parse_HelpCalled_ShouldWriteHelpScreenToHelpTextWriter()
        {
            var parser = new Parser();

            var helpTextWriterMock = new Mock<TextWriter>();

            var setup = parser.Setup;

            setup
                .ProgramName("tool")
                .Banner("Banner Text");

            setup
                .Command<Command1Options>()
                .Name("command1");

            setup
                .HelpTextWriter(helpTextWriterMock.Object);

            parser.Parse(new String[] {});

            helpTextWriterMock.Verify(a => a.Write(@"Banner Text

tool <command> [options]

Commands:
command1	

tool help
Prints this help screen.

tool help <command>
Prints the help screen for the specified command.
"), Times.Once);
        }

        [Test(Description = "Parse should write the command help screen to the help text writer when command help was called.")]
        public void Parse_CommandHelpCalled_ShouldWriteCommandHelpScreenToHelpTextWriter()
        {
            var parser = new Parser();

            var helpTextWriterMock = new Mock<TextWriter>();

            var setup = parser.Setup;

            setup
                .ProgramName("tool")
                .Banner("Banner Text");

            setup
                .Command<Command1Options>()
                .Name("command1");

            setup
                .HelpTextWriter(helpTextWriterMock.Object);

            parser.Parse(new String[] { "help", "command1" });

            helpTextWriterMock.Verify(a => a.Write(@"Banner Text

tool command1 

Options:
"), Times.Once);
        }

        [Test(Description = "Parse should write the error screen to the error writer when there is an issue with an argument.")]
        public void Parse_ArgumentError_ShouldPrintErrorsToErrorWriter()
        {
            var parser = new Parser();

            var errorTextWriterMock = new Mock<TextWriter>();

            var setup = parser.Setup;

            setup
                .ProgramName("tool")
                .Banner("Banner Text");

            setup
                .Command<Command1Options>()
                .Name("command1");

            setup
                .ErrorTextWriter(errorTextWriterMock.Object);

            parser.Parse(new String[] {"command1", "--unknownOption"});

            errorTextWriterMock.Verify(a => a.Write(@"Banner Text

Invalid or missing option(s):
- The option --unknownOption is unknown.

Try the following command to get help:
tool help command1
"), Times.Once);
        }

        [Test(Description = "GetCommandHelpText should return the help of the specified command.")]
        public void GetCommandHelpText_Called_ShouldReturnTheCorrectCommandHelp()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup
                .ProgramName("tool")
                .Banner("Banner Text");

            var command1 = setup.Command<Command1Options>();
            command1.Name("command1");
            command1.Help("Command1 help.");
            command1.ExampleUsage("Command1 Example Usage.");
            command1.Option(a => a.OptionA).Help("OptionA help.").IsRequired();
            command1.Option(a => a.OptionB).Help("OptionB help.");
            command1.Option(a => a.OptionC).Help("OptionC help.").IsRequired();

            parser
                .GetCommandHelpText("command1")
                .Should()
                .Be(@"Banner Text

tool command1 [--optionA value] [--optionB value] [--optionC value value ...]

Command1 help.

Options:
--optionA [value]           (Required) OptionA help.
--optionB [value]           (Optional) OptionB help.
--optionC [value value ...] (Required) OptionC help.

Example usage:
Command1 Example Usage.
");
        }

        [Test(Description = "GetCommandHelpText should not include the banner if specified.")]
        public void GetCommandHelpText_NoBanner_ShouldReturnCommandHelpWithoutBanner()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup
                .ProgramName("tool")
                .Banner("Banner Text");

            var command1 = setup.Command<Command1Options>();
            command1.Name("command1");
            command1.Help("Command1 help.");
            command1.ExampleUsage("Command1 Example Usage.");
            command1.Option(a => a.OptionA).Help("OptionA help.").IsRequired();
            command1.Option(a => a.OptionB).Help("OptionB help.");
            command1.Option(a => a.OptionC).Help("OptionC help.").IsRequired();

            parser
                .GetCommandHelpText("command1", false)
                .Should()
                .Be(@"tool command1 [--optionA value] [--optionB value] [--optionC value value ...]

Command1 help.

Options:
--optionA [value]           (Required) OptionA help.
--optionB [value]           (Optional) OptionB help.
--optionC [value value ...] (Required) OptionC help.

Example usage:
Command1 Example Usage.
");
        }

        [Test(Description = "GetCommandHelpText should return an error message when the specified command is unknown.")]
        public void GetCommandHelpText_UnknownCommand_ShouldReturnErrorMessage()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup
                .ProgramName("tool");

            parser
                .GetCommandHelpText("unknownCommand")
                .Should()
                .Be(@"The command 'unknownCommand' is unknown.
Try the following command to get a list of valid commands:
tool help
");
        }

        [Test(Description = "GetErrorsText should return an empty string when there are no errors.")]
        public void GetErrorsText_NoErrorsPresent_ShouldReturnEmptyString()
        {
            var parser = new Parser();

            parser
                .GetErrorsText(new ParseResult())
                .Should()
                .BeEmpty();
        }

        [Test(Description = "GetErrorsText should return the error messages of the errors when there are errors.")]
        public void GetErrorsText_ErrorsPresent_ShouldReturnErrorMessages()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup
                .ProgramName("tool")
                .Banner("Banner Text");

            var parseResult = new ParseResult();
            parseResult.AddError(new OptionMissingError("optionA"));
            parseResult.AddError(new OptionMultipleValuesError("optionA"));

            parser
                .GetErrorsText(parseResult)
                .Should()
                .Be(@"Banner Text

Invalid or missing option(s):
- The option --optionA is required.
- Multiple values are given for the option --optionA, but the option expects a single value.

Try the following command to get help:
tool help
");
        }

        [Test(Description = "GetHelpText should return the help screen.")]
        public void GetHelpText_Called_ShouldReturnHelpScreen()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup
                .ProgramName("tool")
                .Banner("Banner Text");

            parser
                .GetHelpText()
                .Should()
                .Be(@"Banner Text

tool help
Prints this help screen.
");
        }

        [Test(Description = "GetHelpText should not include the banner if specified.")]
        public void GetHelpText_NoBanner_ShouldReturnHelpScreen()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup
                .ProgramName("tool")
                .Banner("Banner Text");

            parser
                .GetHelpText(false)
                .Should()
                .Be(@"tool help
Prints this help screen.
");
        }

        [Test(Description = "GetHelpText should return the help text of the default.")]
        public void GetHelpText_Commands_ShouldReturnHelpOfDefaultCommand()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup
                .ProgramName("tool")
                .Banner("Banner Text");

            var defaultCommand = setup.DefaultCommand<DefaultOptions>();
            defaultCommand.Help("DefaultCommand Help.");
            defaultCommand.ExampleUsage("DefaultCommand Example Usage");
            defaultCommand.Option(a => a.OptionA).Help("OptionA help.");
            defaultCommand.Option(a => a.OptionB).Help("OptionB help.");
            defaultCommand.Option(a => a.OptionC).Help("OptionC help.");

            defaultCommand
                .Option(a => a.OptionD)
                .Help("OptionD help.");

            defaultCommand
                .Option(a => a.OptionE)
                .Help("OptionE help.")
                .OptionHelp(Encoding.ASCII, "ASCII help.")
                .OptionHelp(Encoding.UTF8, "UTF8 help.")
                .OptionHelp(Encoding.UTF16, "UTF16 help.");

            defaultCommand
                .Option(a => a.OptionF)
                .Help("OptionF help.");

            defaultCommand
                .Option(a => a.OptionG)
                .Help("OptionG help.")
                .OptionHelp(Encoding.ASCII, "ASCII help.")
                .OptionHelp(Encoding.UTF8, "UTF8 help.")
                .OptionHelp(Encoding.UTF16, "UTF16 help.");

            parser
                .GetHelpText()
                .Should()
                .Be(@"Banner Text

tool [--optionA value] [--optionB value] [--optionC] [--optionD value] [--optionE value] [--optionF value value ...] [--optionG value value ...]

DefaultCommand Help.

Options:
--optionA [value]           (Optional) OptionA help.
--optionB [value]           (Optional) OptionB help.
--optionC                   (Optional) OptionC help.
--optionD [value]           (Optional) OptionD help. Possible values: Trace, Info, Debug, Error.
--optionE [value]           (Optional) OptionE help. Possible values: ASCII, UTF8, UTF16.
                                       ASCII: ASCII help.
                                       UTF8: UTF8 help.
                                       UTF16: UTF16 help.
--optionF [value value ...] (Optional) OptionF help. Possible values: Trace, Info, Debug, Error.
--optionG [value value ...] (Optional) OptionG help. Possible values: ASCII, UTF8, UTF16.
                                       ASCII: ASCII help.
                                       UTF8: UTF8 help.
                                       UTF16: UTF16 help.

Example usage:
DefaultCommand Example Usage

tool help
Prints this help screen.
");
        }

        [Test(Description = "GetHelpText should list all available commands.")]
        public void GetHelpText_Commands_ShouldReturnHelpOfCommands()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup
                .ProgramName("tool")
                .Banner("Banner Text");

            var command1 = setup.Command<Command1Options>();
            command1.Name("command1");
            command1.Help("Command1 help.");

            var command2 = setup.Command<Command2Options>();
            command2.Name("command2");
            command2.Help("Command2 help.");

            parser
                .GetHelpText()
                .Should()
                .Be(@"Banner Text

tool <command> [options]

Commands:
command1	Command1 help.
command2	Command2 help.

tool help
Prints this help screen.

tool help <command>
Prints the help screen for the specified command.
");
        }

        [Test(Description = "GetHelpText should wrap lines longer than the specified maximum.")]
        public void GetHelpText_MaxLineLength_ShouldWrapLongLines()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup.ProgramName("tool");

            setup.HelpTextMaxLineLength(80);

            var defaultCommand = setup
                .DefaultCommand<DefaultOptions>();
            
            defaultCommand
                .Option(a => a.OptionA)
                .Help("Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et");

            parser
                .GetHelpText()
                .Should()
                .Be(@"tool [--optionA value]

Options:
--optionA [value] (Optional) Lorem ipsum dolor sit amet, consetetur sadipscing
                             elitr, sed diam nonumy eirmod tempor invidunt ut
                             labore et dolore magna aliquyam erat, sed diam
                             voluptua. At vero eos et accusam et

tool help
Prints this help screen.
");
        }

        [Test(Description = "Parse should parse all supported data types for option values correctly.")]
        public void Parse_DataTypes_ShouldParseAllSupportedDataTypes()
        {
            var parser = new Parser();

            var setup = parser.Setup;

            var command = setup.Command<DataTypesCommandOptions>().Name("dataTypes");

            command.Option(a => a.Boolean);
            command.Option(a => a.DateTime);
            command.Option(a => a.Decimal);
            command.Option(a => a.Enum);
            command.Option(a => a.Guid);
            command.Option(a => a.Int64);
            command.Option(a => a.String);
            command.Option(a => a.TimeSpan);

            command.Option(a => a.NullableDateTime);
            command.Option(a => a.NullableDecimal);
            command.Option(a => a.NullableEnum);
            command.Option(a => a.NullableGuid);
            command.Option(a => a.NullableInt64);
            command.Option(a => a.NullableTimeSpan);

            command.Option(a => a.DateTimes);
            command.Option(a => a.Decimals);
            command.Option(a => a.Enums);
            command.Option(a => a.Guids);
            command.Option(a => a.Int64s);
            command.Option(a => a.Strings);
            command.Option(a => a.TimeSpans);

            var result = parser.Parse(new String[]
            {
                "dataTypes",
                "--boolean",
                "--dateTime", "12/31/2016 23:59:59",
                "--decimal", "123.45",
                "--enum", "Info",
                "--guid", "8B5CA729-F0F1-4650-A05A-529DC4694569",
                "--int64", "64",
                "--string", "string value",
                "--timeSpan", "1.02:03:04.0050000",
                
                "--nullableDateTime", "12/31/2016 23:59:59",
                "--nullableDecimal", "123.45",
                "--nullableEnum", "Info",
                "--nullableGuid", "8B5CA729-F0F1-4650-A05A-529DC4694569",
                "--nullableInt64", "64",
                "--nullableTimeSpan", "1.02:03:04.0050000",

                "--dateTimes", "01/01/2017 23:59:59", "01/02/2017 23:59:59", "01/03/2017 23:59:59",
                "--decimals", "100.45", "101.45", "102.45",
                "--enums", "Info", "Trace", "Debug",
                "--guids", "4186EE83-E1F4-456E-9FA6-4893E2F34AAD", "EA6F08B3-FE77-4CED-BF67-44948B7483F8", "D6E97844-EB33-4EC7-AC1F-AFEB697D1C6D",
                "--int64s", "64", "65", "66",
                "--strings", "string1", "string2", "string3",
                "--timeSpans", "1.02:03:04.0050000", "2.02:03:04.0050000", "3.02:03:04.0050000"
            });

            result.CommandOptions.Should().BeOfType<DataTypesCommandOptions>();

            var commandOptions = (DataTypesCommandOptions) result.CommandOptions;
            commandOptions.Boolean.Should().BeTrue();
            commandOptions.DateTime.Should().Be(new DateTime(2016, 12, 31, 23, 59, 59));
            commandOptions.Decimal.Should().Be(123.45M);
            commandOptions.Enum.Should().Be(LogLevel.Info);
            commandOptions.Guid.Should().Be(new Guid("8B5CA729-F0F1-4650-A05A-529DC4694569"));
            commandOptions.Int64.Should().Be(64);
            commandOptions.String.Should().Be("string value");
            commandOptions.TimeSpan.Should().Be(new TimeSpan(1, 2, 3, 4, 5));

            commandOptions.NullableDateTime.Should().Be(new DateTime(2016, 12, 31, 23, 59, 59));
            commandOptions.NullableDecimal.Should().Be(123.45M);
            commandOptions.NullableEnum.Should().Be(LogLevel.Info);
            commandOptions.NullableGuid.Should().Be(new Guid("8B5CA729-F0F1-4650-A05A-529DC4694569"));
            commandOptions.NullableInt64.Should().Be(64);
            commandOptions.NullableTimeSpan.Should().Be(new TimeSpan(1, 2, 3, 4, 5));

            commandOptions.DateTimes.Should().BeEquivalentTo(new List<DateTime>() {new DateTime(2017, 01, 01, 23, 59, 59), new DateTime(2017, 01, 02, 23, 59, 59), new DateTime(2017, 01, 03, 23, 59, 59)});
            commandOptions.Decimals.Should().BeEquivalentTo(new List<Decimal>() {100.45M, 101.45M, 102.45M});
            commandOptions.Enums.Should().BeEquivalentTo(new List<LogLevel>() {LogLevel.Info, LogLevel.Trace, LogLevel.Debug});
            commandOptions.Guids.Should().BeEquivalentTo(new List<Guid>() {new Guid("4186EE83-E1F4-456E-9FA6-4893E2F34AAD"), new Guid("EA6F08B3-FE77-4CED-BF67-44948B7483F8"), new Guid("D6E97844-EB33-4EC7-AC1F-AFEB697D1C6D")});
            commandOptions.Int64s.Should().BeEquivalentTo(new List<Int64>() {64, 65, 66});
            commandOptions.Strings.Should().BeEquivalentTo(new List<String>() {"string1", "string2", "string3"});
            commandOptions.TimeSpans.Should().BeEquivalentTo(new List<TimeSpan>() {new TimeSpan(1, 2, 3, 4, 5), new TimeSpan(2, 2, 3, 4, 5), new TimeSpan(3, 2, 3, 4, 5)});
        }

        [Test(Description = "Parse should use the specified default value for each option if the option is not specified in the command line arguments.")]
        public void Parse_Defaults_ShouldUseDefaultValues()
        {
            var parser = new Parser();

            var setup = parser.Setup;

            var command = setup.Command<DataTypesCommandOptions>().Name("dataTypes");

            command.Option(a => a.DateTime).DefaultValue(new DateTime(2016, 12, 31, 23, 59, 59));
            command.Option(a => a.Decimal).DefaultValue(123.45M);
            command.Option(a => a.Enum).DefaultValue(LogLevel.Info);
            command.Option(a => a.Guid).DefaultValue(new Guid("8B5CA729-F0F1-4650-A05A-529DC4694569"));
            command.Option(a => a.Int64).DefaultValue(64);
            command.Option(a => a.String).DefaultValue("string value");
            command.Option(a => a.TimeSpan).DefaultValue(new TimeSpan(1, 2, 3, 4, 5));

            command.Option(a => a.DateTimes).DefaultValue(new List<DateTime>() {new DateTime(2017, 01, 01, 23, 59, 59), new DateTime(2017, 01, 02, 23, 59, 59), new DateTime(2017, 01, 03, 23, 59, 59)});
            command.Option(a => a.Decimals).DefaultValue(new List<Decimal>() {100.45M, 101.45M, 102.45M});
            command.Option(a => a.Enums).DefaultValue(new List<LogLevel>() {LogLevel.Info, LogLevel.Trace, LogLevel.Debug});
            command.Option(a => a.Guids).DefaultValue(new List<Guid>() {new Guid("4186EE83-E1F4-456E-9FA6-4893E2F34AAD"), new Guid("EA6F08B3-FE77-4CED-BF67-44948B7483F8"), new Guid("D6E97844-EB33-4EC7-AC1F-AFEB697D1C6D")});
            command.Option(a => a.Int64s).DefaultValue(new List<Int64>() {64, 65, 66});
            command.Option(a => a.Strings).DefaultValue(new List<String>() {"string1", "string2", "string3"});
            command.Option(a => a.TimeSpans).DefaultValue(new List<TimeSpan>() {new TimeSpan(1, 2, 3, 4, 5), new TimeSpan(2, 2, 3, 4, 5), new TimeSpan(3, 2, 3, 4, 5)});

            var parseResult = parser.Parse(new String[] {"dataTypes"});

            parseResult.CommandOptions.Should().BeOfType<DataTypesCommandOptions>();

            var commandOptions = (DataTypesCommandOptions) parseResult.CommandOptions;

            commandOptions.DateTime.Should().Be(new DateTime(2016, 12, 31, 23, 59, 59));
            commandOptions.Decimal.Should().Be(123.45M);
            commandOptions.Enum.Should().Be(LogLevel.Info);
            commandOptions.Guid.Should().Be(new Guid("8B5CA729-F0F1-4650-A05A-529DC4694569"));
            commandOptions.Int64.Should().Be(64);
            commandOptions.String.Should().Be("string value");
            commandOptions.TimeSpan.Should().Be(new TimeSpan(1, 2, 3, 4, 5));

            commandOptions.DateTimes.Should().BeEquivalentTo(new List<DateTime>() {new DateTime(2017, 01, 01, 23, 59, 59), new DateTime(2017, 01, 02, 23, 59, 59), new DateTime(2017, 01, 03, 23, 59, 59)});
            commandOptions.Decimals.Should().BeEquivalentTo(new List<Decimal>() {100.45M, 101.45M, 102.45M});
            commandOptions.Enums.Should().BeEquivalentTo(new List<LogLevel>() {LogLevel.Info, LogLevel.Trace, LogLevel.Debug});
            commandOptions.Guids.Should().BeEquivalentTo(new List<Guid>() {new Guid("4186EE83-E1F4-456E-9FA6-4893E2F34AAD"), new Guid("EA6F08B3-FE77-4CED-BF67-44948B7483F8"), new Guid("D6E97844-EB33-4EC7-AC1F-AFEB697D1C6D")});
            commandOptions.Int64s.Should().BeEquivalentTo(new List<Int64>() {64, 65, 66});
            commandOptions.Strings.Should().BeEquivalentTo(new List<String>() {"string1", "string2", "string3"});
            commandOptions.TimeSpans.Should().BeEquivalentTo(new List<TimeSpan>() {new TimeSpan(1, 2, 3, 4, 5), new TimeSpan(2, 2, 3, 4, 5), new TimeSpan(3, 2, 3, 4, 5)});
        }

        [Test(Description = "Parse should call the validator of the specified command.")]
        public void Parse_CommandValidator_ShouldCallValidator()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup.HelpTextWriter(null);
            setup.ErrorTextWriter(null);

            var command1 = setup.Command<Command1Options>();
            command1.Name("command1");
            command1.Option(a => a.OptionA);
            command1.Option(a => a.OptionB);

            command1.Validate((context) =>
            {
                if (!String.IsNullOrEmpty(context.CommandOptions.OptionA) && context.CommandOptions.OptionB == null)
                {
                    context.AddError(new InvalidOptionError(context.GetOptionName(a => a.OptionB), "The option '--optionB' must be specified when option '--optionA' is specified."));
                }
            });

            var parseResult = parser.Parse(new String[] {"command1", "--optionA", "optionAValue"});

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<InvalidOptionError>();

            var error = (InvalidOptionError) parseResult.Errors[0];
            error.OptionName.Should().BeEquivalentTo("optionB");
            error.GetErrorMessage().Should().Be("The option --optionB is invalid: The option '--optionB' must be specified when option '--optionA' is specified.");
        }

        [Test(Description = "Parse should call the validator of the default command.")]
        public void Parse_DefaultCommandValidator_ShouldCallValidator()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup.HelpTextWriter(null);
            setup.ErrorTextWriter(null);

            var defaultCommand = setup.DefaultCommand<Command1Options>();
            defaultCommand.Option(a => a.OptionA);
            defaultCommand.Option(a => a.OptionB);

            defaultCommand.Validate((context) =>
            {
                if (!String.IsNullOrEmpty(context.CommandOptions.OptionA) && context.CommandOptions.OptionB == null)
                {
                    context.AddError(new InvalidOptionError(context.GetOptionName(a => a.OptionB), "The option '--optionB' must be specified when option '--optionA' is specified."));
                }
            });

            var parseResult = parser.Parse(new String[] {"--optionA", "optionAValue"});

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<InvalidOptionError>();

            var error = (InvalidOptionError) parseResult.Errors[0];
            error.OptionName.Should().BeEquivalentTo("optionB");
            error.GetErrorMessage().Should().Be("The option --optionB is invalid: The option '--optionB' must be specified when option '--optionA' is specified.");
        }

        [Test(Description = "Parse should ignore unknown options when the IgnoreUnknownOptions is enabled.")]
        public void Parse_IgnoreUnknownOptionsEnabled_ShouldIgnoreUnknownOptions()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup.HelpTextWriter(null);
            setup.ErrorTextWriter(null);

            setup.IgnoreUnknownOptions();

            var command1 = setup.Command<Command1Options>();
            command1.Name("command1");
            command1.Option(a => a.OptionA);
            command1.Option(a => a.OptionB);

            var parseResult = parser.Parse(new String[] {"command1", "--optionA", "optionAValue", "--unknownOption"});
            parseResult.HasErrors.Should().BeFalse();
        }

        [Test(Description = "Parse should return a MoreThanOneCommandError when there is more than once command specified.")]
        public void Parse_MoreThanOneCommand_ShouldReturnError()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup.HelpTextWriter(null);
            setup.ErrorTextWriter(null);

            var command1 = setup.Command<Command1Options>();
            command1.Name("command1");
            command1.Option(a => a.OptionA);

            var parseResult = parser.Parse(new String[] {"command1", "command2"});

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Count.Should().Be(1);
            parseResult.Errors[0].Should().BeOfType<MoreThanOneCommandError>();
            parseResult.Errors[0].GetErrorMessage().Should().Be("More than one command was specified. Please only specify one command.");
        }

        [Test(Description = "Parse should return a MissingCommandError when there is no command specified and the default command has not set up.")]
        public void Parse_MissingCommand_ShouldReturnError()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup.HelpTextWriter(null);
            setup.ErrorTextWriter(null);

            var command1 = setup.Command<Command1Options>();
            command1.Name("command1");
            command1.Option(a => a.OptionA);

            var parseResult = parser.Parse(new String[] {"--optionA"});

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Count.Should().Be(1);
            parseResult.Errors[0].Should().BeOfType<MissingCommandError>();
            parseResult.Errors[0].GetErrorMessage().Should().Be("No command was specified.");
        }

        [Test(Description = "Parse should return a OptionValueMissingError when the value of an option is missing.")]
        public void Parse_OptionValueMissing_ShouldReturnError()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup.HelpTextWriter(null);
            setup.ErrorTextWriter(null);

            var command1 = setup.Command<Command1Options>();
            command1.Name("command1");
            command1.Option(a => a.OptionA);

            var parseResult = parser.Parse(new String[] {"command1", "--optionA"});

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Count.Should().Be(1);
            parseResult.Errors[0].Should().BeOfType<OptionValueMissingError>();
            parseResult.Errors[0].GetErrorMessage().Should().Be("The option --optionA requires a value, but no value was specified.");
        }

        [Test(Description = "Parse should return a DuplicateOptionError when an option was specified more than once.")]
        public void Parse_DuplicateOption_ShouldReturnError()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup.HelpTextWriter(null);
            setup.ErrorTextWriter(null);

            var command1 = setup.Command<Command1Options>();
            command1.Name("command1");
            command1.Option(a => a.OptionA);

            var parseResult = parser.Parse(new String[] {"command1", "--optionA", "optionAValue", "--optionA"});

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Count.Should().Be(1);
            parseResult.Errors[0].Should().BeOfType<DuplicateOptionError>();
            parseResult.Errors[0].GetErrorMessage().Should().Be("The option --optionA is used more than once. Please only use each option once.");
        }

        [Test(Description = "Parse should return a UnknownCommandError when an unknown command was specified.")]
        public void Parse_UnknownCommand_ShouldReturnError()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup.HelpTextWriter(null);
            setup.ErrorTextWriter(null);

            var command1 = setup.Command<Command1Options>();
            command1.Name("command1");
            command1.Option(a => a.OptionA);

            var parseResult = parser.Parse(new String[] {"unknownCommand"});

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Count.Should().Be(1);
            parseResult.Errors[0].Should().BeOfType<UnknownCommandError>();
            parseResult.Errors[0].GetErrorMessage().Should().Be("The command 'unknownCommand' is unknown.");
        }

        [Test(Description = "Parse should return a OptionValueFormatError when the value of an option has an invalid format.")]
        public void Parse_OptionValueHasInvalidFormat_ShouldReturnError()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup.HelpTextWriter(null);
            setup.ErrorTextWriter(null);

            var command1 = setup.Command<DataTypesCommandOptions>();
            command1.Name("command1");
            command1.Option(a => a.Int64);

            var parseResult = parser.Parse(new String[] {"command1", "--int64", "NotANumber"});

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Count.Should().Be(1);
            parseResult.Errors[0].Should().BeOfType<OptionValueInvalidFormatError>();
            parseResult.Errors[0].GetErrorMessage().Should().Be("The value 'NotANumber' of the option --int64 has an invalid format. The expected format is: An integer in the range from -9223372036854775808 to 9223372036854775807.");
        }

        [Test(Description = "Parse should return a UnknownOptionError when an unknown option was specified.")]
        public void Parse_UnknownOption_ShouldReturnError()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup.HelpTextWriter(null);
            setup.ErrorTextWriter(null);

            var command1 = setup.Command<Command1Options>();
            command1.Name("command1");
            command1.Option(a => a.OptionA);

            var parseResult = parser.Parse(new String[] {"command1", "--unknownOption"});

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Count.Should().Be(1);
            parseResult.Errors[0].Should().BeOfType<UnknownOptionError>();
            parseResult.Errors[0].GetErrorMessage().Should().Be("The option --unknownOption is unknown.");
        }
    }
}