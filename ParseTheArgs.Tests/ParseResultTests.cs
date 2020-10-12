using System;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using ParseTheArgs.Errors;
using ParseTheArgs.Tests.TestData;

namespace ParseTheArgs.Tests
{
    [TestFixture]
    public class ParseResultTests
    {
        [Test(Description = "AddError should throw an exception when the given error is null.")]
        public void AddError_ErrorIsNull_ShouldThrowException()
        {
            var result = new ParseResult();

            result.Invoking(a => a.AddError(null))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "AddError should add the given error to the parse result.")]
        public void AddError_ShouldAddError()
        {
            var result = new ParseResult();

            var optionMissingError = new OptionMissingError("optionA");
            result.AddError(optionMissingError);

            result.Errors.Should().BeEquivalentTo(optionMissingError);
        }

        [Test(Description = "CommandHandler should throw an exception when the given handler is null.")]
        public void CommandHandler_HandlerIsNull_ShouldThrowException()
        {
            var result = new ParseResult();

            result.Invoking(a => a.CommandHandler<Command1Options>(null))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "CommandName should return the value that was assigned to it.")]
        public void CommandName_Assigned_ShouldReturnAssignedValue()
        {
            var result = new ParseResult();

            result.CommandName = "command1";
            result.CommandName.Should().Be("command1");
        }

        [Test(Description = "CommandName should initially be an empty string.")]
        public void CommandName_Initially_ShouldBeEmpty()
        {
            var result = new ParseResult();

            result.CommandName.Should().BeEmpty();
        }

        [Test(Description = "CommandOptions should return the value that was assigned to it.")]
        public void CommandOptions_Assigned_ShouldReturnAssignedValue()
        {
            var result = new ParseResult();
            var command1Options = new Command1Options();

            result.CommandOptions = command1Options;
            result.CommandOptions.Should().Be(command1Options);
        }

        [Test(Description = "CommandOptions should initially be null.")]
        public void CommandOptions_Initially_ShouldBeNull()
        {
            var result = new ParseResult();

            result.CommandOptions.Should().BeNull();
        }

        [Test(Description = "ErrorHandler should throw an exception when the given handler is null.")]
        public void ErrorHandler_HandlerIsNull_ShouldThrowException()
        {
            var result = new ParseResult();

            result.Invoking(a => a.ErrorHandler(null))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "Errors should return the errors that where added.")]
        public void Errors_ErrorsExist_ShouldReturnErrors()
        {
            var result = new ParseResult();

            var optionMissingError = new OptionMissingError("optionA");
            result.AddError(optionMissingError);

            var unknownOptionError = new UnknownOptionError("optionB");
            result.AddError(unknownOptionError);

            result.Errors.Should().BeEquivalentTo(optionMissingError, unknownOptionError);
        }

        [Test(Description = "Errors should initially be empty.")]
        public void Errors_Initially_ShouldBeEmpty()
        {
            var result = new ParseResult();

            result.Errors.Should().BeEmpty();
        }

        [Test(Description = "Handle should forward the exception thrown by a command handler.")]
        public void Handle_CommandHandlerThrows_ShouldForwardCommandHandlerException()
        {
            var result = new ParseResult();
            var handlers = A.Fake<ICommandHandlers>();
            var command1Options = new Command1Options();

            A.CallTo(() => handlers.HandleCommand1(command1Options)).Throws(new Exception("Command Handler Exception"));
            result.CommandHandler<Command1Options>(handlers.HandleCommand1);

            result.CommandOptions = command1Options;

            result.Invoking(a => a.Handle())
                .Should()
                .Throw<Exception>()
                .WithMessage("Command Handler Exception");
        }

        [Test(Description = "Handle should forward any exception thrown by the error handler.")]
        public void Handle_ErrorHandlerThrows_ShouldForwardErrorHandlerException()
        {
            var result = new ParseResult();
            var handlers = A.Fake<ICommandHandlers>();

            A.CallTo(() => handlers.HandleError(result)).Throws(new Exception("Error Handler Exception"));
            result.ErrorHandler(handlers.HandleError);

            result.AddError(new OptionMissingError("optionA"));

            result.Invoking(a => a.Handle())
                .Should()
                .Throw<Exception>()
                .WithMessage("Error Handler Exception");
        }

        [Test(Description = "Handle should call the error handler when there is an error.")]
        public void Handle_HasErrors_ShouldCallCallErrorHandler()
        {
            var result = new ParseResult();
            var handlers = A.Fake<ICommandHandlers>();

            result.ErrorHandler(handlers.HandleError);

            result.AddError(new OptionMissingError("optionA"));

            result.Handle();

            A.CallTo(() => handlers.HandleError(result)).MustHaveHappenedOnceExactly();
        }

        [Test(Description = "Handle should not call any command handler when there is an error.")]
        public void Handle_HasErrors_ShouldNotCallAnyCommandHandler()
        {
            var result = new ParseResult();
            var handlers = A.Fake<ICommandHandlers>();

            result.CommandHandler<Command1Options>(handlers.HandleCommand1);

            result.AddError(new OptionMissingError("optionA"));

            result.Handle();
            A.CallTo(() => handlers.HandleCommand1(A<Command1Options>.Ignored)).MustNotHaveHappened();
        }

        [Test(Description = "Handle should return the return value of the error handler when there is an error.")]
        public void Handle_HasErrors_ShouldReturnValueOfErrorHandler()
        {
            var result = new ParseResult();
            var handlers = A.Fake<ICommandHandlers>();

            A.CallTo(() => handlers.HandleError(result)).Returns(1);
            result.ErrorHandler(handlers.HandleError);

            result.AddError(new OptionMissingError("optionA"));

            result.Handle().Should().Be(1);
        }

        [Test(Description = "Handle should return 0 when there is an error, but there is no error handler.")]
        public void Handle_HasErrorsAndNoErrorHandler_ShouldReturn0()
        {
            var result = new ParseResult();

            result.AddError(new OptionMissingError("optionA"));

            result.Handle().Should().Be(0);
        }

        [Test(Description = "Handle should call the matching command handler when there is one.")]
        public void Handle_MatchingCommandHandlerExists_ShouldCallMatchingCommandHandler()
        {
            var result = new ParseResult();
            var handlers = A.Fake<ICommandHandlers>();
            var command1Options = new Command1Options();

            result.CommandHandler<Command1Options>(handlers.HandleCommand1);

            result.CommandOptions = command1Options;

            result.Handle();

            A.CallTo(() => handlers.HandleCommand1(command1Options)).MustHaveHappenedOnceExactly();
        }

        [Test(Description = "Handle should return the return value of the matching command handler.")]
        public void Handle_MatchingCommandHandlerExists_ShouldReturnValueOfCommandHandler()
        {
            var result = new ParseResult();
            var handlers = A.Fake<ICommandHandlers>();
            var command1Options = new Command1Options();

            A.CallTo(() => handlers.HandleCommand1(command1Options)).Returns(1);
            result.CommandHandler<Command1Options>(handlers.HandleCommand1);

            result.CommandOptions = command1Options;

            result.Handle().Should().Be(1);
        }

        [Test(Description = "Handle should return 0 when there are no command handlers.")]
        public void Handle_NoCommandHandlers_ShouldReturn0()
        {
            var result = new ParseResult();
            var command1Options = new Command1Options();

            result.CommandOptions = command1Options;

            result.Handle().Should().Be(0);
        }

        [Test(Description = "Handle should not call any command handler or the error handler when there are no command options.")]
        public void Handle_NoCommandOptions_ShouldNotCallHandlers()
        {
            var result = new ParseResult();
            var handlers = A.Fake<ICommandHandlers>();

            result.CommandHandler<Command1Options>(handlers.HandleCommand1);
            result.ErrorHandler(handlers.HandleError);

            result.CommandOptions = null;

            result.Handle();

            A.CallTo(() => handlers.HandleCommand1(A<Command1Options>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => handlers.HandleError(A<ParseResult>.Ignored)).MustNotHaveHappened();
        }

        [Test(Description = "Handle should return 0 when there are no command options.")]
        public void Handle_NoCommandOptions_ShouldReturn0()
        {
            var result = new ParseResult();

            result.CommandOptions = null;

            result.Handle().Should().Be(0);
        }

        [Test(Description = "Handle should not call non-matching command handlers.")]
        public void Handle_NoMatchingCommandHandler_ShouldNotCallNonMatchingCommandHandlers()
        {
            var result = new ParseResult();
            var handlers = A.Fake<ICommandHandlers>();
            var command1Options = new Command1Options();

            result.CommandHandler<Command2Options>(handlers.HandleCommand2);
            result.CommandHandler<Command3Options>(handlers.HandleCommand3);

            result.CommandOptions = command1Options;

            result.Handle();

            A.CallTo(() => handlers.HandleCommand2(A<Command2Options>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => handlers.HandleCommand3(A<Command3Options>.Ignored)).MustNotHaveHappened();
        }

        [Test(Description = "Handle should return 0 when there is no matching command handler.")]
        public void Handle_NoMatchingCommandHandler_ShouldReturn0()
        {
            var result = new ParseResult();
            var handlers = A.Fake<ICommandHandlers>();
            var command1Options = new Command1Options();

            result.CommandHandler<Command2Options>(handlers.HandleCommand2);

            result.CommandOptions = command1Options;

            result.Handle().Should().Be(0);
        }

        [Test(Description = "HasErrors should return true when there are errors.")]
        public void HasErrors_ErrorsExist_ShouldBeTrue()
        {
            var result = new ParseResult();

            var optionMissingError = new OptionMissingError("optionA");
            result.AddError(optionMissingError);

            result.HasErrors.Should().BeTrue();
        }

        [Test(Description = "HasError should initially be false.")]
        public void HasErrors_Initially_ShouldBeFalse()
        {
            var result = new ParseResult();

            result.HasErrors.Should().BeFalse();
        }
    }

    public interface ICommandHandlers
    {
        Int32 HandleCommand1(Command1Options options);
        Int32 HandleCommand2(Command2Options options);
        Int32 HandleCommand3(Command3Options options);
        Int32 HandleError(ParseResult result);
    }
}