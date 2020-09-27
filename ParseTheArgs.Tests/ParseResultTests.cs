using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using ParseTheArgs.Errors;
using ParseTheArgs.Tests.TestData;

namespace ParseTheArgs.Tests
{
    [TestFixture]
    public class ParseResultTests
    {
        [Test(Description = "CommandArguments should initially be null.")]
        public void CommandArguments_Initially_ShouldBeNull()
        {
            var result = new ParseResult();
            
            result.CommandArguments.Should().BeNull();
        }

        [Test(Description = "CommandArguments should return the value that was assigned to it.")]
        public void CommandArguments_Assigned_ShouldReturnAssignedValue()
        {
            var result = new ParseResult();
            var command1Arguments = new Command1Arguments();

            result.CommandArguments = command1Arguments;
            result.CommandArguments.Should().Be(command1Arguments);
        }

        [Test(Description = "CommandName should initially be an empty string.")]
        public void CommandName_Initially_ShouldBeEmpty()
        {
            var result = new ParseResult();
            
            result.CommandName.Should().BeEmpty();
        }

        [Test(Description = "CommandName should return the value that was assigned to it.")]
        public void CommandName_Assigned_ShouldReturnAssignedValue()
        {
            var result = new ParseResult();

            result.CommandName = "command1";
            result.CommandName.Should().Be("command1");
        }

        [Test(Description = "Errors should initially be empty.")]
        public void Errors_Initially_ShouldBeEmpty()
        {
            var result = new ParseResult();

            result.Errors.Should().BeEmpty();
        }

        [Test(Description = "HasError should initially be false.")]
        public void HasErrors_Initially_ShouldBeFalse()
        {
            var result = new ParseResult();

            result.HasErrors.Should().BeFalse();
        }

        [Test(Description = "HasErrors should return true when there are errors.")]
        public void HasErrors_ErrorsExist_ShouldBeTrue()
        {
            var result = new ParseResult();

            var argumentMissingError = new ArgumentMissingError(new ArgumentName("argumentA"));
            result.AddError(argumentMissingError);
            
            result.HasErrors.Should().BeTrue();
        }

        [Test(Description = "Errors should return the errors that where added.")]
        public void Errors_ErrorsExist_ShouldReturnErrors()
        {
            var result = new ParseResult();

            var argumentMissingError = new ArgumentMissingError(new ArgumentName("argumentA"));
            result.AddError(argumentMissingError);

            var unknownArgumentError = new UnknownArgumentError(new ArgumentName("argumentB"));
            result.AddError(unknownArgumentError);
            
            result.Errors.Should().BeEquivalentTo(argumentMissingError, unknownArgumentError);
        }

        [Test(Description = "Handle should return 0 when there are no command arguments.")]
        public void Handle_NoCommandArguments_ShouldReturn0()
        {
            var result = new ParseResult();

            result.CommandArguments = null;

            result.Handle().Should().Be(0);
        }

        [Test(Description = "Handle should not call any command handler or the error handler when there are no command arguments.")]
        public void Handle_NoCommandArguments_ShouldNotCallHandlers()
        {
            var result = new ParseResult();
            var handlersMock = new Mock<ICommandHandlers>();

            result.CommandHandler<Command1Arguments>(handlersMock.Object.HandleCommand1);
            result.ErrorHandler(handlersMock.Object.HandleError);
            
            result.CommandArguments = null;

            result.Handle();

            handlersMock.Verify(a => a.HandleCommand1(It.IsAny<Command1Arguments>()), Times.Never);
            handlersMock.Verify(a => a.HandleError(It.IsAny<ParseResult>()), Times.Never);
        }

        [Test(Description = "Handle should return 0 when there are no command handlers.")]
        public void Handle_NoCommandHandlers_ShouldReturn0()
        {
            var result = new ParseResult();
            var command1Arguments = new Command1Arguments();

            result.CommandArguments = command1Arguments;

            result.Handle().Should().Be(0);
        }

        [Test(Description = "Handle should return 0 when there is no matching command handler.")]
        public void Handle_NoMatchingCommandHandler_ShouldReturn0()
        {
            var result = new ParseResult();
            var handlersMock = new Mock<ICommandHandlers>();
            var command1Arguments = new Command1Arguments();

            result.CommandHandler<Command2Arguments>(handlersMock.Object.HandleCommand2);

            result.CommandArguments = command1Arguments;

            result.Handle().Should().Be(0);
        }

        [Test(Description = "Handle should not call non-matching command handlers.")]
        public void Handle_NoMatchingCommandHandler_ShouldNotCallNonMatchingCommandHandlers()
        {
            var result = new ParseResult();
            var handlersMock = new Mock<ICommandHandlers>();
            var command1Arguments = new Command1Arguments();

            result.CommandHandler<Command2Arguments>(handlersMock.Object.HandleCommand2);
            result.CommandHandler<Command3Arguments>(handlersMock.Object.HandleCommand3);

            result.CommandArguments = command1Arguments;

            result.Handle();
            
            handlersMock.Verify(a => a.HandleCommand2(It.IsAny<Command2Arguments>()), Times.Never);
            handlersMock.Verify(a => a.HandleCommand3(It.IsAny<Command3Arguments>()), Times.Never);
        }

        [Test(Description = "Handle should return 0 when there is an error, but there is no error handler.")]
        public void Handle_HasErrorsAndNoErrorHandler_ShouldReturn0()
        {
            var result = new ParseResult();

            result.AddError(new ArgumentMissingError(new ArgumentName("argumentA")));

            result.Handle().Should().Be(0);
        }

        [Test(Description = "Handle should not call any command handler when there is an error.")]
        public void Handle_HasErrors_ShouldNotCallAnyCommandHandler()
        {
            var result = new ParseResult();
            var handlersMock = new Mock<ICommandHandlers>();

            result.CommandHandler<Command1Arguments>(handlersMock.Object.HandleCommand1);
            
            result.AddError(new ArgumentMissingError(new ArgumentName("argumentA")));

            result.Handle();
            handlersMock.Verify(a => a.HandleCommand1(It.IsAny<Command1Arguments>()), Times.Never);
        }

        [Test(Description = "Handle should call the error handler when there is an error.")]
        public void Handle_HasErrors_ShouldCallCallErrorHandler()
        {
            var result = new ParseResult();
            var handlersMock = new Mock<ICommandHandlers>();

            result.ErrorHandler(handlersMock.Object.HandleError);

            result.AddError(new ArgumentMissingError(new ArgumentName("argumentA")));

            result.Handle();

            handlersMock.Verify(a => a.HandleError(result), Times.Once);
        }

        [Test(Description = "Handle should return the return value of the error handler when there is an error.")]
        public void Handle_HasErrors_ShouldReturnValueOfErrorHandler()
        {
            var result = new ParseResult();
            var handlersMock = new Mock<ICommandHandlers>();

            handlersMock.Setup(a => a.HandleError(It.Is<ParseResult>(parseResult => parseResult == result))).Returns(1);
            result.ErrorHandler(handlersMock.Object.HandleError);

            result.AddError(new ArgumentMissingError(new ArgumentName("argumentA")));

            result.Handle().Should().Be(1);
        }

        [Test(Description = "Handle should call the matching command handler when there is one.")]
        public void Handle_MatchingCommandHandlerExists_ShouldCallMatchingCommandHandler()
        {
            var result = new ParseResult();
            var handlersMock = new Mock<ICommandHandlers>();
            var command1Arguments = new Command1Arguments();

            result.CommandHandler<Command1Arguments>(handlersMock.Object.HandleCommand1);

            result.CommandArguments = command1Arguments;

            result.Handle();

            handlersMock.Verify(a => a.HandleCommand1(command1Arguments), Times.Once);
        }

        [Test(Description = "Handle should return the return value of the matching command handler.")]
        public void Handle_MatchingCommandHandlerExists_ShouldReturnValueOfCommandHandler()
        {
            var result = new ParseResult();
            var handlersMock = new Mock<ICommandHandlers>();
            var command1Arguments = new Command1Arguments();

            handlersMock.Setup(a => a.HandleCommand1(It.Is<Command1Arguments>(arguments => arguments == command1Arguments))).Returns(1);
            result.CommandHandler<Command1Arguments>(handlersMock.Object.HandleCommand1);

            result.CommandArguments = command1Arguments;

            result.Handle().Should().Be(1);
        }

        [Test(Description = "Handle should forward the exception thrown by a command handler.")]
        public void Handle_CommandHandlerThrows_ShouldForwardCommandHandlerException()
        {
            var result = new ParseResult();
            var handlersMock = new Mock<ICommandHandlers>();
            var command1Arguments = new Command1Arguments();

            handlersMock.Setup(a => a.HandleCommand1(It.IsAny<Command1Arguments>())).Throws(new Exception("Command Handler Exception"));
            result.CommandHandler<Command1Arguments>(handlersMock.Object.HandleCommand1);

            result.CommandArguments = command1Arguments;

            result.Invoking(a => a.Handle())
                .Should()
                .Throw<Exception>()
                .WithMessage("Command Handler Exception");
        }

        [Test(Description = "Handle should forward any exception thrown by the error handler.")]
        public void Handle_ErrorHandlerThrows_ShouldForwardErrorHandlerException()
        {
            var result = new ParseResult();
            var handlersMock = new Mock<ICommandHandlers>();

            handlersMock.Setup(a => a.HandleError(It.IsAny<ParseResult>())).Throws(new Exception("Error Handler Exception"));
            result.ErrorHandler(handlersMock.Object.HandleError);

            result.AddError(new ArgumentMissingError(new ArgumentName("argumentA")));

            result.Invoking(a => a.Handle())
                .Should()
                .Throw<Exception>()
                .WithMessage("Error Handler Exception");
        }
    }

    public interface ICommandHandlers
    {
        Int32 HandleCommand1(Command1Arguments arguments);
        Int32 HandleCommand2(Command2Arguments arguments);
        Int32 HandleCommand3(Command3Arguments arguments);
        Int32 HandleError(ParseResult result);
    }
}
