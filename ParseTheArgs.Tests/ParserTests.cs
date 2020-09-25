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
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }

        [Test]
        public void TestParse_Help()
        {
            var parser = new Parser();

            var helpTextWriterMock = new Mock<TextWriter>();

            var setup = parser.Setup;

            setup
                .ProgramName("tool")
                .Banner("Banner Text");

            setup
                .Command<Command1Arguments>()
                .Name("command1");

            setup
                .HelpTextWriter(helpTextWriterMock.Object);

            var parseResult1 = parser.Parse(new String[] {});
            parseResult1.IsHelpCalled.Should().BeTrue();

            var parseResult2 = parser.Parse(new String[] {"help"});
            parseResult2.IsHelpCalled.Should().BeTrue();

            helpTextWriterMock.Verify(a => a.Write(@"Banner Text

tool <command> [arguments]

Commands:
command1	

tool help
Prints this help screen.

tool help <command>
Prints the help screen for the specified command.
"), Times.Exactly(2));

            parser.Parse(new String[] { "help", "command1" });

            helpTextWriterMock.Verify(a => a.Write(@"Banner Text

tool command1 

Arguments:
"), Times.Exactly(1));
        }

        [Test(Description = "Tests fix for https://github.com/rent-a-developer/ParseTheArgs/issues/1")]
        public void TestParse_Help_ArgumentWithoutHelp()
        {
            var parser = new Parser();

            parser.Setup.HelpTextWriter(new StringWriter());

            var defaultCommand = parser.Setup
                .DefaultCommand<Command1Arguments>();

            defaultCommand
                .Argument(a => a.ArgumentA)
                .Name("argumentA");

            parser.Invoking(a => a.Parse(new String[] { })).Should().NotThrow();
        }

        [Test]
        public void TestParse_Error()
        {
            var parser = new Parser();

            var mock = new Mock<TextWriter>();

            var setup = parser.Setup;

            setup
                .ProgramName("tool")
                .Banner("Banner Text");

            setup
                .Command<Command1Arguments>()
                .Name("command1");

            setup
                .ErrorTextWriter(mock.Object);

            parser.Parse(new String[] {"command1", "--unknownArgument"});

            mock.Verify(a => a.Write(@"Banner Text

Invalid or missing argument(s):
- The argument --unknownArgument is unknown.

Try the following command to get help:
tool help command1
"));
        }

        [Test]
        public void TestGetCommandHelpText()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup
                .ProgramName("tool")
                .Banner("Banner Text");

            var command1 = setup.Command<Command1Arguments>();
            command1.Name("command1");
            command1.Help("Command1 help.");
            command1.ExampleUsage("Command1 Example Usage.");
            command1.Argument(a => a.ArgumentA).Help("ArgumentA help.").ShortName('a').IsRequired();
            command1.Argument(a => a.ArgumentB).Help("ArgumentB help.").ShortName('b');
            command1.Argument(a => a.ArgumentC).Help("ArgumentC help.").IsRequired();

            parser
                .GetCommandHelpText("command1")
                .Should()
                .Be(@"Banner Text

tool command1 [-a|--argumentA value] [-b|--argumentB value] [--argumentC value value ...]

Command1 help.

Arguments:
-a|--argumentA [value]        (Required) ArgumentA help.
-b|--argumentB [value]        (Optional) ArgumentB help.
--argumentC [value value ...] (Required) ArgumentC help.

Example usage:
Command1 Example Usage.
");

            parser
                .GetCommandHelpText("command1", false)
                .Should()
                .Be(@"tool command1 [-a|--argumentA value] [-b|--argumentB value] [--argumentC value value ...]

Command1 help.

Arguments:
-a|--argumentA [value]        (Required) ArgumentA help.
-b|--argumentB [value]        (Optional) ArgumentB help.
--argumentC [value value ...] (Required) ArgumentC help.

Example usage:
Command1 Example Usage.
");

            parser
                .GetCommandHelpText("unknownCommand", false)
                .Should()
                .Be(@"The command 'unknownCommand' is unknown.
Try the following command to get a list of valid commands:
tool help
");
        }

        [Test]
        public void TestGetErrorsText()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup
                .ProgramName("tool")
                .Banner("Banner Text");

            parser
                .GetErrorsText(new ParseResult())
                .Should()
                .BeEmpty();

            var parseResult = new ParseResult();
            parseResult.AddError(new ArgumentMissingError(new ArgumentName("argumentA", 'a')));
            parseResult.AddError(new ArgumentMultipleValuesError(new ArgumentName("argumentA", 'a')));

            parser
                .GetErrorsText(parseResult)
                .Should()
                .Be(@"Banner Text

Invalid or missing argument(s):
- The argument -a (--argumentA) is missing.
- Multiple values are given for the argument -a (--argumentA), but the argument does not support multiple values.

Try the following command to get help:
tool help
");
        }

        [Test]
        public void TestGetHelpText()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup.ProgramName("tool").Banner("Banner Text");

            parser
                .GetHelpText()
                .Should()
                .Be(@"Banner Text

tool help
Prints this help screen.
");

            var defaultCommand = setup.DefaultCommand<DefaultArguments>();
            defaultCommand.Help("DefaultCommand Help.");
            defaultCommand.ExampleUsage("DefaultCommand Example Usage");
            defaultCommand.Argument(a => a.ArgumentA).Help("ArgumentA help.");
            defaultCommand.Argument(a => a.ArgumentB).Help("ArgumentB help.");
            defaultCommand.Argument(a => a.ArgumentC).Help("ArgumentC help.");

            defaultCommand
                .Argument(a => a.ArgumentD)
                .Help("ArgumentD help.");

            defaultCommand
                .Argument(a => a.ArgumentE)
                .Help("ArgumentE help.")
                .OptionHelp(Encoding.ASCII, "ASCII help.")
                .OptionHelp(Encoding.UTF8, "UTF8 help.")
                .OptionHelp(Encoding.UTF16, "UTF16 help.");

            defaultCommand
                .Argument(a => a.ArgumentF)
                .Help("ArgumentF help.");

            defaultCommand
                .Argument(a => a.ArgumentG)
                .Help("ArgumentG help.")
                .OptionHelp(Encoding.ASCII, "ASCII help.")
                .OptionHelp(Encoding.UTF8, "UTF8 help.")
                .OptionHelp(Encoding.UTF16, "UTF16 help.");

            var command1 = setup.Command<Command1Arguments>();
            command1.Name("command1");
            command1.Help("Command1 help.");

            var command2 = setup.Command<Command2Arguments>();
            command2.Name("command2");
            command2.Help("Command2 help.");

            parser
                .GetHelpText()
                .Should()
                .Be(@"Banner Text

tool [--argumentA value] [--argumentB value] [--argumentC] [--argumentD value] [--argumentE value] [--argumentF value value ...] [--argumentG value value ...]

DefaultCommand Help.

Arguments:
--argumentA [value]           (Optional) ArgumentA help.
--argumentB [value]           (Optional) ArgumentB help.
--argumentC                   (Optional) ArgumentC help.
--argumentD [value]           (Optional) ArgumentD help. Possible values: Trace, Info, Debug, Error.
--argumentE [value]           (Optional) ArgumentE help. Possible values: ASCII, UTF8, UTF16.
                                         ASCII: ASCII help.
                                         UTF8: UTF8 help.
                                         UTF16: UTF16 help.
--argumentF [value value ...] (Optional) ArgumentF help. Possible values: Trace, Info, Debug, Error.
--argumentG [value value ...] (Optional) ArgumentG help. Possible values: ASCII, UTF8, UTF16.
                                         ASCII: ASCII help.
                                         UTF8: UTF8 help.
                                         UTF16: UTF16 help.

Example usage:
DefaultCommand Example Usage

tool <command> [arguments]

Commands:
command1	Command1 help.
command2	Command2 help.

tool help
Prints this help screen.

tool help <command>
Prints the help screen for the specified command.
");

            parser
                .GetHelpText(false)
                .Should()
                .Be(@"tool [--argumentA value] [--argumentB value] [--argumentC] [--argumentD value] [--argumentE value] [--argumentF value value ...] [--argumentG value value ...]

DefaultCommand Help.

Arguments:
--argumentA [value]           (Optional) ArgumentA help.
--argumentB [value]           (Optional) ArgumentB help.
--argumentC                   (Optional) ArgumentC help.
--argumentD [value]           (Optional) ArgumentD help. Possible values: Trace, Info, Debug, Error.
--argumentE [value]           (Optional) ArgumentE help. Possible values: ASCII, UTF8, UTF16.
                                         ASCII: ASCII help.
                                         UTF8: UTF8 help.
                                         UTF16: UTF16 help.
--argumentF [value value ...] (Optional) ArgumentF help. Possible values: Trace, Info, Debug, Error.
--argumentG [value value ...] (Optional) ArgumentG help. Possible values: ASCII, UTF8, UTF16.
                                         ASCII: ASCII help.
                                         UTF8: UTF8 help.
                                         UTF16: UTF16 help.

Example usage:
DefaultCommand Example Usage

tool <command> [arguments]

Commands:
command1	Command1 help.
command2	Command2 help.

tool help
Prints this help screen.

tool help <command>
Prints the help screen for the specified command.
");
        }

        [Test]
        public void TestGetHelpText_MaxLineLength()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup.ProgramName("tool");

            setup.HelpTextMaxLineLength(80);

            var defaultCommand = setup.DefaultCommand<DefaultArguments>();
            defaultCommand.Argument(a => a.ArgumentA).Help("Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et");

            parser
                .GetHelpText()
                .Should()
                .Be(@"tool [--argumentA value]

Arguments:
--argumentA [value] (Optional) Lorem ipsum dolor sit amet, consetetur sadipscing
                               elitr, sed diam nonumy eirmod tempor invidunt ut
                               labore et dolore magna aliquyam erat, sed diam
                               voluptua. At vero eos et accusam et

tool help
Prints this help screen.
");
        }

        [Test]
        public void TestParse_DataTypes()
        {
            var parser = new Parser();

            var setup = parser.Setup;

            var command = setup.Command<DataTypesCommandArguments>().Name("dataTypes");

            command.Argument(a => a.Boolean);
            command.Argument(a => a.DateTime);
            command.Argument(a => a.Decimal);
            command.Argument(a => a.Enum);
            command.Argument(a => a.Guid);
            command.Argument(a => a.Int64);
            command.Argument(a => a.String);
            command.Argument(a => a.TimeSpan);

            command.Argument(a => a.NullableDateTime);
            command.Argument(a => a.NullableDecimal);
            command.Argument(a => a.NullableEnum);
            command.Argument(a => a.NullableGuid);
            command.Argument(a => a.NullableInt64);
            command.Argument(a => a.NullableTimeSpan);

            command.Argument(a => a.DateTimes);
            command.Argument(a => a.Decimals);
            command.Argument(a => a.Enums);
            command.Argument(a => a.Guids);
            command.Argument(a => a.Int64s);
            command.Argument(a => a.Strings);
            command.Argument(a => a.TimeSpans);

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

            result.CommandArguments.Should().BeOfType<DataTypesCommandArguments>();

            var commandArguments = (DataTypesCommandArguments) result.CommandArguments;
            commandArguments.Boolean.Should().BeTrue();
            commandArguments.DateTime.Should().Be(new DateTime(2016, 12, 31, 23, 59, 59));
            commandArguments.Decimal.Should().Be(123.45M);
            commandArguments.Enum.Should().Be(LogLevel.Info);
            commandArguments.Guid.Should().Be(new Guid("8B5CA729-F0F1-4650-A05A-529DC4694569"));
            commandArguments.Int64.Should().Be(64);
            commandArguments.String.Should().Be("string value");
            commandArguments.TimeSpan.Should().Be(new TimeSpan(1, 2, 3, 4, 5));

            commandArguments.NullableDateTime.Should().Be(new DateTime(2016, 12, 31, 23, 59, 59));
            commandArguments.NullableDecimal.Should().Be(123.45M);
            commandArguments.NullableEnum.Should().Be(LogLevel.Info);
            commandArguments.NullableGuid.Should().Be(new Guid("8B5CA729-F0F1-4650-A05A-529DC4694569"));
            commandArguments.NullableInt64.Should().Be(64);
            commandArguments.NullableTimeSpan.Should().Be(new TimeSpan(1, 2, 3, 4, 5));

            commandArguments.DateTimes.Should().BeEquivalentTo(new List<DateTime>() {new DateTime(2017, 01, 01, 23, 59, 59), new DateTime(2017, 01, 02, 23, 59, 59), new DateTime(2017, 01, 03, 23, 59, 59)});
            commandArguments.Decimals.Should().BeEquivalentTo(new List<Decimal>() {100.45M, 101.45M, 102.45M});
            commandArguments.Enums.Should().BeEquivalentTo(new List<LogLevel>() {LogLevel.Info, LogLevel.Trace, LogLevel.Debug});
            commandArguments.Guids.Should().BeEquivalentTo(new List<Guid>() {new Guid("4186EE83-E1F4-456E-9FA6-4893E2F34AAD"), new Guid("EA6F08B3-FE77-4CED-BF67-44948B7483F8"), new Guid("D6E97844-EB33-4EC7-AC1F-AFEB697D1C6D")});
            commandArguments.Int64s.Should().BeEquivalentTo(new List<Int64>() {64, 65, 66});
            commandArguments.Strings.Should().BeEquivalentTo(new List<String>() {"string1", "string2", "string3"});
            commandArguments.TimeSpans.Should().BeEquivalentTo(new List<TimeSpan>() {new TimeSpan(1, 2, 3, 4, 5), new TimeSpan(2, 2, 3, 4, 5), new TimeSpan(3, 2, 3, 4, 5)});
        }

        [Test]
        public void TestParse_Defaults()
        {
            var parser = new Parser();

            var setup = parser.Setup;

            var command = setup.Command<DataTypesCommandArguments>().Name("dataTypes");

            command.Argument(a => a.DateTime).DefaultValue(new DateTime(2016, 12, 31, 23, 59, 59));
            command.Argument(a => a.Decimal).DefaultValue(123.45M);
            command.Argument(a => a.Enum).DefaultValue(LogLevel.Info);
            command.Argument(a => a.Guid).DefaultValue(new Guid("8B5CA729-F0F1-4650-A05A-529DC4694569"));
            command.Argument(a => a.Int64).DefaultValue(64);
            command.Argument(a => a.String).DefaultValue("string value");
            command.Argument(a => a.TimeSpan).DefaultValue(new TimeSpan(1, 2, 3, 4, 5));

            command.Argument(a => a.DateTimes).DefaultValue(new List<DateTime>() {new DateTime(2017, 01, 01, 23, 59, 59), new DateTime(2017, 01, 02, 23, 59, 59), new DateTime(2017, 01, 03, 23, 59, 59)});
            command.Argument(a => a.Decimals).DefaultValue(new List<Decimal>() {100.45M, 101.45M, 102.45M});
            command.Argument(a => a.Enums).DefaultValue(new List<LogLevel>() {LogLevel.Info, LogLevel.Trace, LogLevel.Debug});
            command.Argument(a => a.Guids).DefaultValue(new List<Guid>() {new Guid("4186EE83-E1F4-456E-9FA6-4893E2F34AAD"), new Guid("EA6F08B3-FE77-4CED-BF67-44948B7483F8"), new Guid("D6E97844-EB33-4EC7-AC1F-AFEB697D1C6D")});
            command.Argument(a => a.Int64s).DefaultValue(new List<Int64>() {64, 65, 66});
            command.Argument(a => a.Strings).DefaultValue(new List<String>() {"string1", "string2", "string3"});
            command.Argument(a => a.TimeSpans).DefaultValue(new List<TimeSpan>() {new TimeSpan(1, 2, 3, 4, 5), new TimeSpan(2, 2, 3, 4, 5), new TimeSpan(3, 2, 3, 4, 5)});

            var parseResult = parser.Parse(new String[] {"dataTypes"});

            parseResult.CommandArguments.Should().BeOfType<DataTypesCommandArguments>();

            var commandArguments = (DataTypesCommandArguments) parseResult.CommandArguments;

            commandArguments.DateTime.Should().Be(new DateTime(2016, 12, 31, 23, 59, 59));
            commandArguments.Decimal.Should().Be(123.45M);
            commandArguments.Enum.Should().Be(LogLevel.Info);
            commandArguments.Guid.Should().Be(new Guid("8B5CA729-F0F1-4650-A05A-529DC4694569"));
            commandArguments.Int64.Should().Be(64);
            commandArguments.String.Should().Be("string value");
            commandArguments.TimeSpan.Should().Be(new TimeSpan(1, 2, 3, 4, 5));

            commandArguments.DateTimes.Should().BeEquivalentTo(new List<DateTime>() {new DateTime(2017, 01, 01, 23, 59, 59), new DateTime(2017, 01, 02, 23, 59, 59), new DateTime(2017, 01, 03, 23, 59, 59)});
            commandArguments.Decimals.Should().BeEquivalentTo(new List<Decimal>() {100.45M, 101.45M, 102.45M});
            commandArguments.Enums.Should().BeEquivalentTo(new List<LogLevel>() {LogLevel.Info, LogLevel.Trace, LogLevel.Debug});
            commandArguments.Guids.Should().BeEquivalentTo(new List<Guid>() {new Guid("4186EE83-E1F4-456E-9FA6-4893E2F34AAD"), new Guid("EA6F08B3-FE77-4CED-BF67-44948B7483F8"), new Guid("D6E97844-EB33-4EC7-AC1F-AFEB697D1C6D")});
            commandArguments.Int64s.Should().BeEquivalentTo(new List<Int64>() {64, 65, 66});
            commandArguments.Strings.Should().BeEquivalentTo(new List<String>() {"string1", "string2", "string3"});
            commandArguments.TimeSpans.Should().BeEquivalentTo(new List<TimeSpan>() {new TimeSpan(1, 2, 3, 4, 5), new TimeSpan(2, 2, 3, 4, 5), new TimeSpan(3, 2, 3, 4, 5)});
        }

        [Test]
        public void TestParse_ValidateCommand()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup.HelpTextWriter(null);
            setup.ErrorTextWriter(null);

            setup.ProgramName("fileTool");

            var command1 = setup.Command<Command1Arguments>();
            command1.Name("command1");
            command1.Argument(a => a.ArgumentA);
            command1.Argument(a => a.ArgumentB);

            command1.Validate((context) =>
            {
                if (!String.IsNullOrEmpty(context.CommandArguments.ArgumentA) && context.CommandArguments.ArgumentB == null)
                {
                    context.AddError(new InvalidArgumentError(context.GetArgumentName(a => a.ArgumentB), "The argument '--argumentB' must be specified when argument '--argumentA' is specified."));
                }
            });

            var parseResult = parser.Parse(new String[] {"command1", "--argumentA", "argumentAValue"});

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<InvalidArgumentError>();

            var error = (InvalidArgumentError) parseResult.Errors[0];
            error.ArgumentName.Should().Be(new ArgumentName("argumentB"));
            error.GetErrorMessage().Should().Be("The argument --argumentB is invalid: The argument '--argumentB' must be specified when argument '--argumentA' is specified.");
        }

        [Test]
        public void TestParse_ValidateDefaultCommand()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup.HelpTextWriter(null);
            setup.ErrorTextWriter(null);

            setup.ProgramName("fileTool");

            var command1 = setup.DefaultCommand<Command1Arguments>();
            command1.Argument(a => a.ArgumentA);
            command1.Argument(a => a.ArgumentB);

            command1.Validate((context) =>
            {
                if (!String.IsNullOrEmpty(context.CommandArguments.ArgumentA) && context.CommandArguments.ArgumentB == null)
                {
                    context.AddError(new InvalidArgumentError(context.GetArgumentName(a => a.ArgumentB), "The argument '--argumentB' must be specified when argument '--argumentA' is specified."));
                }
            });

            var parseResult = parser.Parse(new String[] {"--argumentA", "argumentAValue"});

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<InvalidArgumentError>();

            var error = (InvalidArgumentError) parseResult.Errors[0];
            error.ArgumentName.Should().Be(new ArgumentName("argumentB"));
            error.GetErrorMessage().Should().Be("The argument --argumentB is invalid: The argument '--argumentB' must be specified when argument '--argumentA' is specified.");
        }

        [Test]
        public void TestParse_IgnoreUnknownArguments()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup.HelpTextWriter(null);
            setup.ErrorTextWriter(null);

            setup.IgnoreUnknownArguments();

            var command1 = setup.Command<Command1Arguments>();
            command1.Name("command1");
            command1.Argument(a => a.ArgumentA);
            command1.Argument(a => a.ArgumentB);

            var parseResult = parser.Parse(new String[] {"command1", "--argumentA", "argumentAValue", "--unknownArgument"});
            parseResult.HasErrors.Should().BeFalse();
        }

        [Test]
        public void TestParse_MoreThanOneCommandError()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup.HelpTextWriter(null);
            setup.ErrorTextWriter(null);

            var command1 = setup.Command<Command1Arguments>();
            command1.Name("command1");
            command1.Argument(a => a.ArgumentA);

            var parseResult = parser.Parse(new String[] {"command1", "command2"});

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Count.Should().Be(1);
            parseResult.Errors[0].Should().BeOfType<MoreThanOneCommandError>();
            parseResult.Errors[0].GetErrorMessage().Should().Be("More than one command specified. Please only specify one command.");
        }

        [Test]
        public void TestParse_MissingCommandError()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup.HelpTextWriter(null);
            setup.ErrorTextWriter(null);

            var command1 = setup.Command<Command1Arguments>();
            command1.Name("command1");
            command1.Argument(a => a.ArgumentA);

            var parseResult = parser.Parse(new String[] {"--argumentA"});

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Count.Should().Be(1);
            parseResult.Errors[0].Should().BeOfType<MissingCommandError>();
            parseResult.Errors[0].GetErrorMessage().Should().Be("No command was specified.");
        }

        [Test]
        public void TestParse_ArgumentValueMissingError()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup.HelpTextWriter(null);
            setup.ErrorTextWriter(null);

            var command1 = setup.Command<Command1Arguments>();
            command1.Name("command1");
            command1.Argument(a => a.ArgumentA);

            var parseResult = parser.Parse(new String[] {"command1", "--argumentA"});

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Count.Should().Be(1);
            parseResult.Errors[0].Should().BeOfType<ArgumentValueMissingError>();
            parseResult.Errors[0].GetErrorMessage().Should().Be("The value for the argument --argumentA is missing.");
        }

        [Test]
        public void TestParse_DuplicateArgumentError()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup.HelpTextWriter(null);
            setup.ErrorTextWriter(null);

            var command1 = setup.Command<Command1Arguments>();
            command1.Name("command1");
            command1.Argument(a => a.ArgumentA);

            var parseResult = parser.Parse(new String[] {"command1", "--argumentA", "argumentAValue", "--argumentA"});

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Count.Should().Be(1);
            parseResult.Errors[0].Should().BeOfType<DuplicateArgumentError>();
            parseResult.Errors[0].GetErrorMessage().Should().Be("The argument --argumentA is used more than once. Please only use each argument once.");
        }

        [Test]
        public void TestParse_UnknownCommandError()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup.HelpTextWriter(null);
            setup.ErrorTextWriter(null);

            var command1 = setup.Command<Command1Arguments>();
            command1.Name("command1");
            command1.Argument(a => a.ArgumentA);

            var parseResult = parser.Parse(new String[] {"unknownCommand"});

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Count.Should().Be(1);
            parseResult.Errors[0].Should().BeOfType<UnknownCommandError>();
            parseResult.Errors[0].GetErrorMessage().Should().Be("The command 'unknownCommand' is unknown.");
        }

        [Test]
        public void TestParse_ArgumentValueFormatError()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup.HelpTextWriter(null);
            setup.ErrorTextWriter(null);

            var command1 = setup.Command<DataTypesCommandArguments>();
            command1.Name("command1");
            command1.Argument(a => a.Int64);

            var parseResult = parser.Parse(new String[] {"command1", "--int64", "NotANumber"});

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Count.Should().Be(1);
            parseResult.Errors[0].Should().BeOfType<ArgumentValueFormatError>();
            parseResult.Errors[0].GetErrorMessage().Should().Be("The value 'NotANumber' of the argument --int64 has an invalid format. The expected format is: An integer in the range from -9223372036854775808 to 9223372036854775807.");
        }

        [Test]
        public void TestParse_UnknownArgumentError()
        {
            var parser = new Parser();

            var setup = parser.Setup;
            setup.HelpTextWriter(null);
            setup.ErrorTextWriter(null);

            var command1 = setup.Command<Command1Arguments>();
            command1.Name("command1");
            command1.Argument(a => a.ArgumentA);

            var parseResult = parser.Parse(new String[] {"command1", "--unknownArgument"});

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Count.Should().Be(1);
            parseResult.Errors[0].Should().BeOfType<UnknownArgumentError>();
            parseResult.Errors[0].GetErrorMessage().Should().Be("The argument --unknownArgument is unknown.");
        }
    }
}