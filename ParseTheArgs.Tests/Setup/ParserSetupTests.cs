using System;
using System.IO;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Setup;
using ParseTheArgs.Setup.Commands;
using ParseTheArgs.Tests.TestData;
using static FluentAssertions.FluentActions;

namespace ParseTheArgs.Tests.Setup
{
    [TestFixture]
    public class ParserSetupTests : BaseTestFixture
    {
        [Test(Description = "Banner should assign the given banner text to the parser.")]
        public void Banner_ShouldAssignBannerToParser()
        {
            var parser = A.Fake<Parser>();

            var setup = new ParserSetup(parser);

            setup.Banner("newBanner");

            A.CallToSet(() => parser.Banner).To("newBanner").MustHaveHappened();
        }

        [Test(Description = "Banner should return the same instance of the parser setup.")]
        public void Banner_ShouldReturnParserSetup()
        {
            var parser = A.Fake<Parser>();

            var setup = new ParserSetup(parser);

            setup.Banner("newBanner").Should().Be(setup);
        }

        [Test(Description = "Command should return a new named command setup.")]
        public void Command_ShouldReturnNewNamedCommandSetup()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<Command1Options>>();
            var commandSetup = A.Fake<NamedCommandSetup<Command1Options>>();

            var setup = new ParserSetup(parser);

            A.CallTo(() => parser.GetOrCreateCommandParser<Command1Options>("command1")).Returns(commandParser);
            A.CallTo(() => this.DependencyResolver.Resolve<NamedCommandSetup<Command1Options>>(parser, commandParser)).Returns(commandSetup);

            setup.Command<Command1Options>().Should().Be(commandSetup);
        }

        [Test(Description = "Constructor should throw an exception when the given parser is null.")]
        public void Constructor_ParserIsNull_ShouldThrowException()
        {
            Invoking(() => new ParserSetup(null!))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "DefaultCommand should return a new default command setup.")]
        public void DefaultCommand_ShouldReturnNewDefaultCommandSetup()
        {
            var parser = A.Fake<Parser>();
            var commandParser = A.Fake<CommandParser<Command1Options>>();
            var commandSetup = A.Fake<DefaultCommandSetup<Command1Options>>();

            var setup = new ParserSetup(parser);

            A.CallTo(() => parser.GetOrCreateCommandParser<Command1Options>(null)).Returns(commandParser);
            A.CallTo(() => this.DependencyResolver.Resolve<DefaultCommandSetup<Command1Options>>(parser, commandParser)).Returns(commandSetup);

            setup.DefaultCommand<Command1Options>().Should().Be(commandSetup);
        }

        [Test(Description = "ErrorTextWriter should assign the given error text writer to the parser.")]
        public void ErrorTextWriter_ShouldAssignErrorTextWriterToParser()
        {
            var parser = A.Fake<Parser>();
            var textWriter = A.Fake<TextWriter>();

            var setup = new ParserSetup(parser);

            setup.ErrorTextWriter(textWriter);

            A.CallToSet(() => parser.ErrorTextWriter).To(textWriter).MustHaveHappened();
        }

        [Test(Description = "ErrorTextWriter should return the same instance of the parser setup.")]
        public void ErrorTextWriter_ShouldReturnParserSetup()
        {
            var parser = A.Fake<Parser>();
            var textWriter = A.Fake<TextWriter>();

            var setup = new ParserSetup(parser);

            setup.ErrorTextWriter(textWriter).Should().Be(setup);
        }

        [Test(Description = "HelpTextMaxLineLength should assign the given maximum line length to the parser.")]
        public void HelpTextMaxLineLength_ShouldAssignHelpTextMaxLineLengthToParser()
        {
            var parser = A.Fake<Parser>();

            var setup = new ParserSetup(parser);

            setup.HelpTextMaxLineLength(123);

            A.CallToSet(() => parser.HelpTextMaxLineLength).To(123).MustHaveHappened();
        }

        [Test(Description = "HelpTextMaxLineLength should return the same instance of the parser setup.")]
        public void HelpTextMaxLineLength_ShouldReturnParserSetup()
        {
            var parser = A.Fake<Parser>();

            var setup = new ParserSetup(parser);

            setup.HelpTextMaxLineLength(123).Should().Be(setup);
        }

        [Test(Description = "HelpTextWriter should assign the given error text writer to the parser.")]
        public void HelpTextWriter_ShouldAssignHelpTextWriterToParser()
        {
            var parser = A.Fake<Parser>();
            var textWriter = A.Fake<TextWriter>();

            var setup = new ParserSetup(parser);

            setup.HelpTextWriter(textWriter);

            A.CallToSet(() => parser.HelpTextWriter).To(textWriter).MustHaveHappened();
        }

        [Test(Description = "HelpTextWriter should return the same instance of the parser setup.")]
        public void HelpTextWriter_ShouldReturnParserSetup()
        {
            var parser = A.Fake<Parser>();
            var textWriter = A.Fake<TextWriter>();

            var setup = new ParserSetup(parser);

            setup.HelpTextWriter(textWriter).Should().Be(setup);
        }

        [Test(Description = "IgnoreUnknownOptions should return the same instance of the parser setup.")]
        public void IgnoreUnknownOptions_ShouldReturnParserSetup()
        {
            var parser = A.Fake<Parser>();

            var setup = new ParserSetup(parser);

            setup.IgnoreUnknownOptions().Should().Be(setup);
        }

        [Test(Description = "IgnoreUnknownOptions should set the ignore unknown options flag on the parser.")]
        public void IgnoreUnknownOptions_ShouldSetIgnoreUnknownOptionsFlagOnParser()
        {
            var parser = A.Fake<Parser>();

            var setup = new ParserSetup(parser);

            setup.IgnoreUnknownOptions();

            A.CallToSet(() => parser.IgnoreUnknownOptions).To(true).MustHaveHappened();
        }

        [Test(Description = "ProgramName should assign the given program name to the parser.")]
        public void ProgramName_ShouldAssignProgramNameToParser()
        {
            var parser = A.Fake<Parser>();

            var setup = new ParserSetup(parser);

            setup.ProgramName("newProgramName");

            A.CallToSet(() => parser.ProgramName).To("newProgramName").MustHaveHappened();
        }

        [Test(Description = "ProgramName should return the same instance of the parser setup.")]
        public void ProgramName_ShouldReturnParserSetup()
        {
            var parser = A.Fake<Parser>();

            var setup = new ParserSetup(parser);

            setup.ProgramName("newProgramName").Should().Be(setup);
        }
    }
}