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
        [Test]
        public void TestCommandArguments()
        {
            var result = new ParseResult();
            result.CommandArguments.Should().BeNull();

            var command1Arguments = new Command1Arguments();
            result.CommandArguments = command1Arguments;
            result.CommandArguments.Should().Be(command1Arguments);
        }

        [Test]
        public void TestCommandName()
        {
            var result = new ParseResult();
            result.CommandName.Should().BeNull();

            result.CommandName = "command1";
            result.CommandName.Should().Be("command1");
        }

        [Test]
        public void TestAddError()
        {
            var result = new ParseResult();

            result.HasErrors.Should().BeFalse();
            result.Errors.Should().BeEmpty();

            var argumentMissingError = new ArgumentMissingError(new ArgumentName("argumentA"));
            result.AddError(argumentMissingError);

            var unknownArgumentError = new UnknownArgumentError(new ArgumentName("argumentB"));
            result.AddError(unknownArgumentError);
            
            result.HasErrors.Should().BeTrue();
            
            result.Errors.Should().BeEquivalentTo(argumentMissingError, unknownArgumentError);
        }


        [Test]
        public void TestHandle_WithoutReturnValue()
        {
            var result = new ParseResult();

            var handlersMock = new Mock<IHandlersWithoutReturnValue>();

            var command1Arguments = new Command1Arguments();
            var command2Arguments = new Command2Arguments();
            var command3Arguments = new Command3Arguments();


            result.CommandArguments = null;
            result
                .Handle<Command1Arguments, Command2Arguments>(
                    handlersMock.Object.HandleCommand1,
                    handlersMock.Object.HandleCommand2
                );
            handlersMock.Verify(a => a.HandleCommand1(command1Arguments), Times.Never);
            handlersMock.Verify(a => a.HandleCommand2(command2Arguments), Times.Never);
            handlersMock.Verify(a => a.HandleError(result), Times.Never);
            handlersMock.Reset();


            result.CommandArguments = null;
            result
                .Handle<Command1Arguments, Command2Arguments>(
                    handlersMock.Object.HandleCommand1,
                    handlersMock.Object.HandleCommand2,
                    handlersMock.Object.HandleError
                );
            handlersMock.Verify(a => a.HandleCommand1(command1Arguments), Times.Never);
            handlersMock.Verify(a => a.HandleCommand2(command2Arguments), Times.Never);
            handlersMock.Verify(a => a.HandleError(result), Times.Never);
            handlersMock.Reset();


            result.CommandArguments = command1Arguments;
            result
                .Handle<Command1Arguments, Command2Arguments>(
                    handlersMock.Object.HandleCommand1,
                    handlersMock.Object.HandleCommand2,
                    handlersMock.Object.HandleError
                );
            handlersMock.Verify(a => a.HandleCommand1(command1Arguments), Times.Once);
            handlersMock.Verify(a => a.HandleCommand2(command2Arguments), Times.Never);
            handlersMock.Verify(a => a.HandleError(result), Times.Never);
            handlersMock.Reset();


            result.CommandArguments = command2Arguments;
            result
                .Handle<Command1Arguments, Command2Arguments>(
                    handlersMock.Object.HandleCommand1,
                    handlersMock.Object.HandleCommand2,
                    handlersMock.Object.HandleError
                );
            handlersMock.Verify(a => a.HandleCommand1(command1Arguments), Times.Never);
            handlersMock.Verify(a => a.HandleCommand2(command2Arguments), Times.Once);
            handlersMock.Verify(a => a.HandleError(result), Times.Never);
            handlersMock.Reset();

            result.CommandArguments = command3Arguments;
            result
                .Handle<Command1Arguments, Command2Arguments>(
                    handlersMock.Object.HandleCommand1,
                    handlersMock.Object.HandleCommand2,
                    handlersMock.Object.HandleError
                );
            handlersMock.Verify(a => a.HandleCommand1(command1Arguments), Times.Never);
            handlersMock.Verify(a => a.HandleCommand2(command2Arguments), Times.Never);
            handlersMock.Verify(a => a.HandleError(result), Times.Never);
            handlersMock.Reset();


            result.AddError(new ArgumentMissingError(new ArgumentName("argumentA")));
            result
                .Handle<Command1Arguments, Command2Arguments>(
                    handlersMock.Object.HandleCommand1,
                    handlersMock.Object.HandleCommand2,
                    handlersMock.Object.HandleError
                );
            handlersMock.Verify(a => a.HandleCommand1(command1Arguments), Times.Never);
            handlersMock.Verify(a => a.HandleCommand2(command2Arguments), Times.Never);
            handlersMock.Verify(a => a.HandleError(result), Times.Once);
            handlersMock.Reset();

            
            result
                .Handle<Command1Arguments, Command2Arguments>(
                    handlersMock.Object.HandleCommand1,
                    handlersMock.Object.HandleCommand2
                );
            handlersMock.Verify(a => a.HandleCommand1(command1Arguments), Times.Never);
            handlersMock.Verify(a => a.HandleCommand2(command2Arguments), Times.Never);
            handlersMock.Verify(a => a.HandleError(result), Times.Never);
            handlersMock.Reset();
        }

        [Test]
        public void TestHandle_WithReturnValue()
        {
            var result = new ParseResult();

            var handlersMock = new Mock<IHandlersWithReturnValue>();

            var command1Arguments = new Command1Arguments();
            var command2Arguments = new Command2Arguments();
            var command3Arguments = new Command3Arguments();


            result.CommandArguments = null;
            result
                .Handle<Command1Arguments, Command2Arguments, Int32>(
                    handlersMock.Object.HandleCommand1,
                    handlersMock.Object.HandleCommand2
                )
                .Should()
                .Be(default(Int32));
            handlersMock.Verify(a => a.HandleCommand1(command1Arguments), Times.Never);
            handlersMock.Verify(a => a.HandleCommand2(command2Arguments), Times.Never);
            handlersMock.Verify(a => a.HandleError(result), Times.Never);
            handlersMock.Reset();


            result.CommandArguments = null;
            result
                .Handle<Command1Arguments, Command2Arguments, Int32>(
                    handlersMock.Object.HandleCommand1,
                    handlersMock.Object.HandleCommand2,
                    handlersMock.Object.HandleError
                )
                .Should()
                .Be(default(Int32));
            handlersMock.Verify(a => a.HandleCommand1(command1Arguments), Times.Never);
            handlersMock.Verify(a => a.HandleCommand2(command2Arguments), Times.Never);
            handlersMock.Verify(a => a.HandleError(result), Times.Never);
            handlersMock.Reset();


            handlersMock.Setup(a => a.HandleCommand1(It.Is<Command1Arguments>(arguments => arguments == command1Arguments))).Returns(1);
            result.CommandArguments = command1Arguments;
            result
                .Handle<Command1Arguments, Command2Arguments, Int32>(
                    handlersMock.Object.HandleCommand1,
                    handlersMock.Object.HandleCommand2,
                    handlersMock.Object.HandleError
                )
                .Should()
                .Be(1);
            handlersMock.Verify(a => a.HandleCommand1(command1Arguments), Times.Once);
            handlersMock.Verify(a => a.HandleCommand2(command2Arguments), Times.Never);
            handlersMock.Verify(a => a.HandleError(result), Times.Never);
            handlersMock.Reset();


            handlersMock.Setup(a => a.HandleCommand2(It.Is<Command2Arguments>(arguments => arguments == command2Arguments))).Returns(2);
            result.CommandArguments = command2Arguments;
            result
                .Handle<Command1Arguments, Command2Arguments, Int32>(
                    handlersMock.Object.HandleCommand1,
                    handlersMock.Object.HandleCommand2,
                    handlersMock.Object.HandleError
                )
                .Should()
                .Be(2);
            handlersMock.Verify(a => a.HandleCommand1(command1Arguments), Times.Never);
            handlersMock.Verify(a => a.HandleCommand2(command2Arguments), Times.Once);
            handlersMock.Verify(a => a.HandleError(result), Times.Never);
            handlersMock.Reset();


            result.CommandArguments = command3Arguments;
            result
                .Handle<Command1Arguments, Command2Arguments, Int32>(
                    handlersMock.Object.HandleCommand1,
                    handlersMock.Object.HandleCommand2,
                    handlersMock.Object.HandleError
                )
                .Should()
                .Be(default(Int32));
            handlersMock.Verify(a => a.HandleCommand1(command1Arguments), Times.Never);
            handlersMock.Verify(a => a.HandleCommand2(command2Arguments), Times.Never);
            handlersMock.Verify(a => a.HandleError(result), Times.Never);
            handlersMock.Reset();


            handlersMock.Setup(a => a.HandleError(It.Is<ParseResult>(parseResult => parseResult == result))).Returns(3);
            result.AddError(new ArgumentMissingError(new ArgumentName("argumentA")));
            result
                .Handle<Command1Arguments, Command2Arguments, Int32>(
                    handlersMock.Object.HandleCommand1,
                    handlersMock.Object.HandleCommand2,
                    handlersMock.Object.HandleError
                )
                .Should()
                .Be(3);
            handlersMock.Verify(a => a.HandleCommand1(command1Arguments), Times.Never);
            handlersMock.Verify(a => a.HandleCommand2(command2Arguments), Times.Never);
            handlersMock.Verify(a => a.HandleError(result), Times.Once);
            handlersMock.Reset();

            
            result
                .Handle<Command1Arguments, Command2Arguments, Int32>(
                    handlersMock.Object.HandleCommand1,
                    handlersMock.Object.HandleCommand2
                )
                .Should()
                .Be(default(Int32));
            handlersMock.Verify(a => a.HandleCommand1(command1Arguments), Times.Never);
            handlersMock.Verify(a => a.HandleCommand2(command2Arguments), Times.Never);
            handlersMock.Verify(a => a.HandleError(result), Times.Never);
            handlersMock.Reset();
        }

        [Test]
        public void TestHandle_Exceptions()
        {
            var result = new ParseResult();

            var command1Arguments = new Command1Arguments();


            result.CommandArguments = command1Arguments;
            result
                .Invoking(a =>
                    a.Handle<Command1Arguments>(
                        (arguments) => { throw new Exception("Command1 Exception"); },
                        parseResult => { throw new Exception("Error Exception"); }
                    )
                )
                .Should()
                .Throw<Exception>()
                .WithMessage("Command1 Exception");

            result
                .Invoking(a =>
                    a.Handle<Command1Arguments, Int32>(
                        (arguments) => { throw new Exception("Command1 Exception"); },
                        parseResult => { throw new Exception("Error Exception"); }
                    )
                )
                .Should()
                .Throw<Exception>()
                .WithMessage("Command1 Exception");


            result.AddError(new ArgumentMissingError(new ArgumentName("argumentA")));
            result
                .Invoking(a =>
                    a.Handle<Command1Arguments>(
                        (arguments) => { throw new Exception("Command1 Exception"); },
                        parseResult => { throw new Exception("Error Exception"); }
                    )
                )
                .Should()
                .Throw<Exception>()
                .WithMessage("Error Exception");

            result
                .Invoking(a =>
                    a.Handle<Command1Arguments, Int32>(
                        (arguments) => { throw new Exception("Command1 Exception"); },
                        parseResult => { throw new Exception("Error Exception"); }
                    )
                )
                .Should()
                .Throw<Exception>()
                .WithMessage("Error Exception");
        }
    }

    public interface IHandlersWithoutReturnValue
    {
        void HandleCommand1(Command1Arguments arguments);
        void HandleCommand2(Command2Arguments arguments);
        void HandleError(ParseResult result);
    }

    public interface IHandlersWithReturnValue
    {
        Int32 HandleCommand1(Command1Arguments arguments);
        Int32 HandleCommand2(Command2Arguments arguments);
        Int32 HandleError(ParseResult result);
    }
}
