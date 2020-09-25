using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.ExceptionServices;
using ParseTheArgs.Errors;

namespace ParseTheArgs
{
    /// <summary>
    /// Represents the result of the parsing of command line arguments.
    /// </summary>
    public class ParseResult
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public ParseResult()
        {
            this.errors = new List<IParseError>();
        }

        /// <summary>
        /// Defines the Object that holds the arguments of the parsed command.
        /// </summary>
        public Object CommandArguments { get; internal set; }

        /// <summary>
        /// Defines the name of the command that was found in the parsed command line arguments.
        /// </summary>
        public String CommandName { get; internal set; }

        /// <summary>
        /// Determines whether the command line was just used to get the help of the program (e.g. no command and/or arguments where passed or the help command was specified).
        /// </summary>
        public Boolean IsHelpCalled { get; internal set; }

        /// <summary>
        /// A list of errors found during parsing.
        /// </summary>
        public ReadOnlyCollection<IParseError> Errors => this.errors.AsReadOnly();

        /// <summary>
        /// Determines if errors where found during parsing.
        /// </summary>
        public Boolean HasErrors => this.errors.Count > 0;

        /// <summary>
        /// Adds a parse error to this instance.
        /// </summary>
        /// <param name="error">The error to add.</param>
        public void AddError(IParseError error)
        {
            this.errors.Add(error);
        }

        /// <summary>
        /// Executes the given <paramref name="commandHandler" /> callback when the command line arguments where parsed to a command of the type <typeparamref name="TCommandArguments" /> or executes the given <paramref name="errorHandler" /> callback when the parsing produced errors.
        /// </summary>
        /// <typeparam name="TCommandArguments">The type of command arguments the command line arguments must have been parsed to to execute the given <paramref name="commandHandler" /> callback.</typeparam>
        /// <param name="commandHandler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommandArguments" /></param>
        /// <param name="errorHandler">A callback that is executed when the parsing produced errors.</param>
        public void Handle<TCommandArguments>(
            Action<TCommandArguments> commandHandler,
            Action<ParseResult> errorHandler = null
        )
        {
            this.Handle(errorHandler, new Delegate[] { commandHandler });
        }

        /// <summary>
        /// Executes one of the given command callbacks when the command line arguments where parsed to a command of a type of one of the given callbacks or executes the given <paramref name="errorHandler" /> callback when the parsing produced errors.
        /// </summary>
        /// <param name="command1Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand1Arguments" /></param>
        /// <param name="command2Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand2Arguments" /></param>
        /// <param name="errorHandler">A callback that is executed when the parsing produced errors.</param>
        public void Handle<TCommand1Arguments, TCommand2Arguments>(
            Action<TCommand1Arguments> command1Handler,
            Action<TCommand2Arguments> command2Handler,
            Action<ParseResult> errorHandler = null
        )
        {
            this.Handle(errorHandler, new Delegate[] { command1Handler, command2Handler });
        }

        /// <summary>
        /// Executes one of the given command callbacks when the command line arguments where parsed to a command of a type of one of the given callbacks or executes the given <paramref name="errorHandler" /> callback when the parsing produced errors.
        /// </summary>
        /// <param name="command1Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand1Arguments" /></param>
        /// <param name="command2Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand2Arguments" /></param>
        /// <param name="command3Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand3Arguments" /></param>
        /// <param name="errorHandler">A callback that is executed when the parsing produced errors.</param>
        public void Handle<TCommand1Arguments, TCommand2Arguments, TCommand3Arguments>(
            Action<TCommand1Arguments> command1Handler,
            Action<TCommand2Arguments> command2Handler,
            Action<TCommand3Arguments> command3Handler,
            Action<ParseResult> errorHandler = null
        )
        {
            this.Handle(errorHandler, new Delegate[] { command1Handler, command2Handler, command3Handler });
        }

        /// <summary>
        /// Executes one of the given command callbacks when the command line arguments where parsed to a command of a type of one of the given callbacks or executes the given <paramref name="errorHandler" /> callback when the parsing produced errors.
        /// </summary>
        /// <param name="command1Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand1Arguments" /></param>
        /// <param name="command2Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand2Arguments" /></param>
        /// <param name="command3Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand3Arguments" /></param>
        /// <param name="command4Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand4Arguments" /></param>
        /// <param name="errorHandler">A callback that is executed when the parsing produced errors.</param>
        public void Handle<TCommand1Arguments, TCommand2Arguments, TCommand3Arguments, TCommand4Arguments>(
            Action<TCommand1Arguments> command1Handler,
            Action<TCommand2Arguments> command2Handler,
            Action<TCommand3Arguments> command3Handler,
            Action<TCommand4Arguments> command4Handler,
            Action<ParseResult> errorHandler = null
        )
        {
            this.Handle(errorHandler, new Delegate[] { command1Handler, command2Handler, command3Handler, command4Handler });
        }

        /// <summary>
        /// Executes one of the given command callbacks when the command line arguments where parsed to a command of a type of one of the given callbacks or executes the given <paramref name="errorHandler" /> callback when the parsing produced errors.
        /// </summary>
        /// <param name="command1Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand1Arguments" /></param>
        /// <param name="command2Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand2Arguments" /></param>
        /// <param name="command3Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand3Arguments" /></param>
        /// <param name="command4Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand4Arguments" /></param>
        /// <param name="command5Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand5Arguments" /></param>
        /// <param name="errorHandler">A callback that is executed when the parsing produced errors.</param>
        public void Handle<TCommand1Arguments, TCommand2Arguments, TCommand3Arguments, TCommand4Arguments, TCommand5Arguments>(
            Action<TCommand1Arguments> command1Handler,
            Action<TCommand2Arguments> command2Handler,
            Action<TCommand3Arguments> command3Handler,
            Action<TCommand4Arguments> command4Handler,
            Action<TCommand5Arguments> command5Handler,
            Action<ParseResult> errorHandler = null
        )
        {
            this.Handle(errorHandler, new Delegate[] { command1Handler, command2Handler, command3Handler, command4Handler, command5Handler });
        }

        /// <summary>
        /// Executes one of the given command callbacks when the command line arguments where parsed to a command of a type of one of the given callbacks or executes the given <paramref name="errorHandler" /> callback when the parsing produced errors.
        /// </summary>
        /// <param name="command1Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand1Arguments" /></param>
        /// <param name="command2Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand2Arguments" /></param>
        /// <param name="command3Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand3Arguments" /></param>
        /// <param name="command4Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand4Arguments" /></param>
        /// <param name="command5Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand5Arguments" /></param>
        /// <param name="command6Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand6Arguments" /></param>
        /// <param name="errorHandler">A callback that is executed when the parsing produced errors.</param>
        public void Handle<TCommand1Arguments, TCommand2Arguments, TCommand3Arguments, TCommand4Arguments, TCommand5Arguments, TCommand6Arguments>(
            Action<TCommand1Arguments> command1Handler,
            Action<TCommand2Arguments> command2Handler,
            Action<TCommand3Arguments> command3Handler,
            Action<TCommand4Arguments> command4Handler,
            Action<TCommand5Arguments> command5Handler,
            Action<TCommand6Arguments> command6Handler,
            Action<ParseResult> errorHandler = null
        )
        {
            this.Handle(errorHandler, new Delegate[] { command1Handler, command2Handler, command3Handler, command4Handler, command5Handler, command6Handler });
        }

        /// <summary>
        /// Executes one of the given command callbacks when the command line arguments where parsed to a command of a type of one of the given callbacks or executes the given <paramref name="errorHandler" /> callback when the parsing produced errors.
        /// </summary>
        /// <param name="command1Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand1Arguments" /></param>
        /// <param name="command2Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand2Arguments" /></param>
        /// <param name="command3Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand3Arguments" /></param>
        /// <param name="command4Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand4Arguments" /></param>
        /// <param name="command5Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand5Arguments" /></param>
        /// <param name="command6Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand6Arguments" /></param>
        /// <param name="command7Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand7Arguments" /></param>
        /// <param name="errorHandler">A callback that is executed when the parsing produced errors.</param>
        public void Handle<TCommand1Arguments, TCommand2Arguments, TCommand3Arguments, TCommand4Arguments, TCommand5Arguments, TCommand6Arguments, TCommand7Arguments>(
            Action<TCommand1Arguments> command1Handler,
            Action<TCommand2Arguments> command2Handler,
            Action<TCommand3Arguments> command3Handler,
            Action<TCommand4Arguments> command4Handler,
            Action<TCommand5Arguments> command5Handler,
            Action<TCommand6Arguments> command6Handler,
            Action<TCommand7Arguments> command7Handler,
            Action<ParseResult> errorHandler = null
        )
        {
            this.Handle(errorHandler, new Delegate[] { command1Handler, command2Handler, command3Handler, command4Handler, command5Handler, command6Handler, command7Handler });
        }

        /// <summary>
        /// Executes one of the given command callbacks when the command line arguments where parsed to a command of a type of one of the given callbacks or executes the given <paramref name="errorHandler" /> callback when the parsing produced errors.
        /// </summary>
        /// <param name="command1Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand1Arguments" /></param>
        /// <param name="command2Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand2Arguments" /></param>
        /// <param name="command3Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand3Arguments" /></param>
        /// <param name="command4Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand4Arguments" /></param>
        /// <param name="command5Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand5Arguments" /></param>
        /// <param name="command6Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand6Arguments" /></param>
        /// <param name="command7Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand7Arguments" /></param>
        /// <param name="command8Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand8Arguments" /></param>
        /// <param name="errorHandler">A callback that is executed when the parsing produced errors.</param>
        public void Handle<TCommand1Arguments, TCommand2Arguments, TCommand3Arguments, TCommand4Arguments, TCommand5Arguments, TCommand6Arguments, TCommand7Arguments, TCommand8Arguments>(
            Action<TCommand1Arguments> command1Handler,
            Action<TCommand2Arguments> command2Handler,
            Action<TCommand3Arguments> command3Handler,
            Action<TCommand4Arguments> command4Handler,
            Action<TCommand5Arguments> command5Handler,
            Action<TCommand6Arguments> command6Handler,
            Action<TCommand7Arguments> command7Handler,
            Action<TCommand8Arguments> command8Handler,
            Action<ParseResult> errorHandler = null
        )
        {
            this.Handle(errorHandler, new Delegate[] { command1Handler, command2Handler, command3Handler, command4Handler, command5Handler, command6Handler, command7Handler, command8Handler });
        }

        /// <summary>
        /// Executes one of the given command callbacks when the command line arguments where parsed to a command of a type of one of the given callbacks or executes the given <paramref name="errorHandler" /> callback when the parsing produced errors.
        /// </summary>
        /// <param name="command1Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand1Arguments" /></param>
        /// <param name="command2Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand2Arguments" /></param>
        /// <param name="command3Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand3Arguments" /></param>
        /// <param name="command4Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand4Arguments" /></param>
        /// <param name="command5Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand5Arguments" /></param>
        /// <param name="command6Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand6Arguments" /></param>
        /// <param name="command7Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand7Arguments" /></param>
        /// <param name="command8Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand8Arguments" /></param>
        /// <param name="command9Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand9Arguments" /></param>
        /// <param name="errorHandler">A callback that is executed when the parsing produced errors.</param>
        public void Handle<TCommand1Arguments, TCommand2Arguments, TCommand3Arguments, TCommand4Arguments, TCommand5Arguments, TCommand6Arguments, TCommand7Arguments, TCommand8Arguments, TCommand9Arguments>(
            Action<TCommand1Arguments> command1Handler,
            Action<TCommand2Arguments> command2Handler,
            Action<TCommand3Arguments> command3Handler,
            Action<TCommand4Arguments> command4Handler,
            Action<TCommand5Arguments> command5Handler,
            Action<TCommand6Arguments> command6Handler,
            Action<TCommand7Arguments> command7Handler,
            Action<TCommand8Arguments> command8Handler,
            Action<TCommand9Arguments> command9Handler,
            Action<ParseResult> errorHandler = null
        )
        {
            this.Handle(errorHandler, new Delegate[] { command1Handler, command2Handler, command3Handler, command4Handler, command5Handler, command6Handler, command7Handler, command8Handler, command9Handler });
        }

        /// <summary>
        /// Executes one of the given command callbacks when the command line arguments where parsed to a command of a type of one of the given callbacks or executes the given <paramref name="errorHandler" /> callback when the parsing produced errors.
        /// </summary>
        /// <param name="command1Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand1Arguments" /></param>
        /// <param name="command2Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand2Arguments" /></param>
        /// <param name="command3Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand3Arguments" /></param>
        /// <param name="command4Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand4Arguments" /></param>
        /// <param name="command5Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand5Arguments" /></param>
        /// <param name="command6Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand6Arguments" /></param>
        /// <param name="command7Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand7Arguments" /></param>
        /// <param name="command8Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand8Arguments" /></param>
        /// <param name="command9Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand9Arguments" /></param>
        /// <param name="command10Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand10Arguments" /></param>
        /// <param name="errorHandler">A callback that is executed when the parsing produced errors.</param>
        public void Handle<TCommand1Arguments, TCommand2Arguments, TCommand3Arguments, TCommand4Arguments, TCommand5Arguments, TCommand6Arguments, TCommand7Arguments, TCommand8Arguments, TCommand9Arguments, TCommand10Arguments>(
            Action<TCommand1Arguments> command1Handler,
            Action<TCommand2Arguments> command2Handler,
            Action<TCommand3Arguments> command3Handler,
            Action<TCommand4Arguments> command4Handler,
            Action<TCommand5Arguments> command5Handler,
            Action<TCommand6Arguments> command6Handler,
            Action<TCommand7Arguments> command7Handler,
            Action<TCommand8Arguments> command8Handler,
            Action<TCommand9Arguments> command9Handler,
            Action<TCommand10Arguments> command10Handler,
            Action<ParseResult> errorHandler = null
        )
        {
            this.Handle(errorHandler, new Delegate[] { command1Handler, command2Handler, command3Handler, command4Handler, command5Handler, command6Handler, command7Handler, command8Handler, command9Handler, command10Handler });
        }

        /// <summary>
        /// Executes the given <paramref name="commandHandler" /> callback when the command line arguments where parsed to a command of the type <typeparamref name="TCommandArguments" /> or executes the given <paramref name="errorHandler" /> callback when the parsing produced errors.
        /// </summary>
        /// <typeparam name="TCommandArguments">The type of command arguments the command line arguments must have been parsed to to execute the given <paramref name="commandHandler" /> callback.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the given callbacks.</typeparam>
        /// <param name="commandHandler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommandArguments" /></param>
        /// <param name="errorHandler">A callback that is executed when the parsing produced errors.</param>
        /// <returns>The return value of the callback that was executed. If the command line arguments where parsed to a command of another type than <typeparamref name="TCommandArguments" /> and when the parsing produced no errors, the default value of <typeparamref name="TResult" /> is returned.</returns>
        public TResult Handle<TCommandArguments, TResult>(
            Func<TCommandArguments, TResult> commandHandler,
            Func<ParseResult, TResult> errorHandler = null
        )
        {
            return this.Handle(errorHandler, new Delegate[] { commandHandler });
        }

        /// <summary>
        /// Executes one of the given command callbacks when the command line arguments where parsed to a command of a type of one of the given callbacks or executes the given <paramref name="errorHandler" /> callback when the parsing produced errors.
        /// </summary>
        /// <param name="command1Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand1Arguments" /></param>
        /// <param name="command2Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand2Arguments" /></param>
        /// <param name="errorHandler">A callback that is executed when the parsing produced errors.</param>
        /// <returns>The return value of the callback that was executed. If the command line arguments where parsed to a command of another type than one of the given callbacks and when the parsing produced no errors, the default value of <typeparamref name="TResult" /> is returned.</returns>
        public TResult Handle<TCommand1Arguments, TCommand2Arguments, TResult>(
            Func<TCommand1Arguments, TResult> command1Handler,
            Func<TCommand2Arguments, TResult> command2Handler,
            Func<ParseResult, TResult> errorHandler = null
        )
        {
            return this.Handle(errorHandler, new Delegate[] { command1Handler, command2Handler });
        }

        /// <summary>
        /// Executes one of the given command callbacks when the command line arguments where parsed to a command of a type of one of the given callbacks or executes the given <paramref name="errorHandler" /> callback when the parsing produced errors.
        /// </summary>
        /// <param name="command1Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand1Arguments" /></param>
        /// <param name="command2Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand2Arguments" /></param>
        /// <param name="command3Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand3Arguments" /></param>
        /// <param name="errorHandler">A callback that is executed when the parsing produced errors.</param>
        /// <returns>The return value of the callback that was executed. If the command line arguments where parsed to a command of another type than one of the given callbacks and when the parsing produced no errors, the default value of <typeparamref name="TResult" /> is returned.</returns>
        public TResult Handle<TCommand1Arguments, TCommand2Arguments, TCommand3Arguments, TResult>(
            Func<TCommand1Arguments, TResult> command1Handler,
            Func<TCommand2Arguments, TResult> command2Handler,
            Func<TCommand3Arguments, TResult> command3Handler,
            Func<ParseResult, TResult> errorHandler = null
        )
        {
            return this.Handle(errorHandler, new Delegate[] { command1Handler, command2Handler, command3Handler });
        }

        /// <summary>
        /// Executes one of the given command callbacks when the command line arguments where parsed to a command of a type of one of the given callbacks or executes the given <paramref name="errorHandler" /> callback when the parsing produced errors.
        /// </summary>
        /// <param name="command1Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand1Arguments" /></param>
        /// <param name="command2Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand2Arguments" /></param>
        /// <param name="command3Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand3Arguments" /></param>
        /// <param name="command4Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand4Arguments" /></param>
        /// <param name="errorHandler">A callback that is executed when the parsing produced errors.</param>
        /// <returns>The return value of the callback that was executed. If the command line arguments where parsed to a command of another type than one of the given callbacks and when the parsing produced no errors, the default value of <typeparamref name="TResult" /> is returned.</returns>
        public TResult Handle<TCommand1Arguments, TCommand2Arguments, TCommand3Arguments, TCommand4Arguments, TResult>(
            Func<TCommand1Arguments, TResult> command1Handler,
            Func<TCommand2Arguments, TResult> command2Handler,
            Func<TCommand3Arguments, TResult> command3Handler,
            Func<TCommand4Arguments, TResult> command4Handler,
            Func<ParseResult, TResult> errorHandler = null
        )
        {
            return this.Handle(errorHandler, new Delegate[] { command1Handler, command2Handler, command3Handler, command4Handler });
        }

        /// <summary>
        /// Executes one of the given command callbacks when the command line arguments where parsed to a command of a type of one of the given callbacks or executes the given <paramref name="errorHandler" /> callback when the parsing produced errors.
        /// </summary>
        /// <param name="command1Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand1Arguments" /></param>
        /// <param name="command2Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand2Arguments" /></param>
        /// <param name="command3Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand3Arguments" /></param>
        /// <param name="command4Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand4Arguments" /></param>
        /// <param name="command5Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand5Arguments" /></param>
        /// <param name="errorHandler">A callback that is executed when the parsing produced errors.</param>
        /// <returns>The return value of the callback that was executed. If the command line arguments where parsed to a command of another type than one of the given callbacks and when the parsing produced no errors, the default value of <typeparamref name="TResult" /> is returned.</returns>
        public TResult Handle<TCommand1Arguments, TCommand2Arguments, TCommand3Arguments, TCommand4Arguments, TCommand5Arguments, TResult>(
            Func<TCommand1Arguments, TResult> command1Handler,
            Func<TCommand2Arguments, TResult> command2Handler,
            Func<TCommand3Arguments, TResult> command3Handler,
            Func<TCommand4Arguments, TResult> command4Handler,
            Func<TCommand5Arguments, TResult> command5Handler,
            Func<ParseResult, TResult> errorHandler = null
        )
        {
            return this.Handle(errorHandler, new Delegate[] { command1Handler, command2Handler, command3Handler, command4Handler, command5Handler });
        }

        /// <summary>
        /// Executes one of the given command callbacks when the command line arguments where parsed to a command of a type of one of the given callbacks or executes the given <paramref name="errorHandler" /> callback when the parsing produced errors.
        /// </summary>
        /// <param name="command1Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand1Arguments" /></param>
        /// <param name="command2Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand2Arguments" /></param>
        /// <param name="command3Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand3Arguments" /></param>
        /// <param name="command4Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand4Arguments" /></param>
        /// <param name="command5Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand5Arguments" /></param>
        /// <param name="command6Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand6Arguments" /></param>
        /// <param name="errorHandler">A callback that is executed when the parsing produced errors.</param>
        /// <returns>The return value of the callback that was executed. If the command line arguments where parsed to a command of another type than one of the given callbacks and when the parsing produced no errors, the default value of <typeparamref name="TResult" /> is returned.</returns>
        public TResult Handle<TCommand1Arguments, TCommand2Arguments, TCommand3Arguments, TCommand4Arguments, TCommand5Arguments, TCommand6Arguments, TResult>(
            Func<TCommand1Arguments, TResult> command1Handler,
            Func<TCommand2Arguments, TResult> command2Handler,
            Func<TCommand3Arguments, TResult> command3Handler,
            Func<TCommand4Arguments, TResult> command4Handler,
            Func<TCommand5Arguments, TResult> command5Handler,
            Func<TCommand6Arguments, TResult> command6Handler,
            Func<ParseResult, TResult> errorHandler = null
        )
        {
            return this.Handle(errorHandler, new Delegate[] { command1Handler, command2Handler, command3Handler, command4Handler, command5Handler, command6Handler });
        }

        /// <summary>
        /// Executes one of the given command callbacks when the command line arguments where parsed to a command of a type of one of the given callbacks or executes the given <paramref name="errorHandler" /> callback when the parsing produced errors.
        /// </summary>
        /// <param name="command1Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand1Arguments" /></param>
        /// <param name="command2Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand2Arguments" /></param>
        /// <param name="command3Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand3Arguments" /></param>
        /// <param name="command4Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand4Arguments" /></param>
        /// <param name="command5Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand5Arguments" /></param>
        /// <param name="command6Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand6Arguments" /></param>
        /// <param name="command7Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand7Arguments" /></param>
        /// <param name="errorHandler">A callback that is executed when the parsing produced errors.</param>
        /// <returns>The return value of the callback that was executed. If the command line arguments where parsed to a command of another type than one of the given callbacks and when the parsing produced no errors, the default value of <typeparamref name="TResult" /> is returned.</returns>
        public TResult Handle<TCommand1Arguments, TCommand2Arguments, TCommand3Arguments, TCommand4Arguments, TCommand5Arguments, TCommand6Arguments, TCommand7Arguments, TResult>(
            Func<TCommand1Arguments, TResult> command1Handler,
            Func<TCommand2Arguments, TResult> command2Handler,
            Func<TCommand3Arguments, TResult> command3Handler,
            Func<TCommand4Arguments, TResult> command4Handler,
            Func<TCommand5Arguments, TResult> command5Handler,
            Func<TCommand6Arguments, TResult> command6Handler,
            Func<TCommand7Arguments, TResult> command7Handler,
            Func<ParseResult, TResult> errorHandler = null
        )
        {
            return this.Handle(errorHandler, new Delegate[] { command1Handler, command2Handler, command3Handler, command4Handler, command5Handler, command6Handler, command7Handler });
        }

        /// <summary>
        /// Executes one of the given command callbacks when the command line arguments where parsed to a command of a type of one of the given callbacks or executes the given <paramref name="errorHandler" /> callback when the parsing produced errors.
        /// </summary>
        /// <param name="command1Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand1Arguments" /></param>
        /// <param name="command2Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand2Arguments" /></param>
        /// <param name="command3Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand3Arguments" /></param>
        /// <param name="command4Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand4Arguments" /></param>
        /// <param name="command5Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand5Arguments" /></param>
        /// <param name="command6Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand6Arguments" /></param>
        /// <param name="command7Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand7Arguments" /></param>
        /// <param name="command8Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand8Arguments" /></param>
        /// <param name="errorHandler">A callback that is executed when the parsing produced errors.</param>
        /// <returns>The return value of the callback that was executed. If the command line arguments where parsed to a command of another type than one of the given callbacks and when the parsing produced no errors, the default value of <typeparamref name="TResult" /> is returned.</returns>
        public TResult Handle<TCommand1Arguments, TCommand2Arguments, TCommand3Arguments, TCommand4Arguments, TCommand5Arguments, TCommand6Arguments, TCommand7Arguments, TCommand8Arguments, TResult>(
            Func<TCommand1Arguments, TResult> command1Handler,
            Func<TCommand2Arguments, TResult> command2Handler,
            Func<TCommand3Arguments, TResult> command3Handler,
            Func<TCommand4Arguments, TResult> command4Handler,
            Func<TCommand5Arguments, TResult> command5Handler,
            Func<TCommand6Arguments, TResult> command6Handler,
            Func<TCommand7Arguments, TResult> command7Handler,
            Func<TCommand8Arguments, TResult> command8Handler,
            Func<ParseResult, TResult> errorHandler = null
        )
        {
            return this.Handle(errorHandler, new Delegate[] { command1Handler, command2Handler, command3Handler, command4Handler, command5Handler, command6Handler, command7Handler, command8Handler });
        }

        /// <summary>
        /// Executes one of the given command callbacks when the command line arguments where parsed to a command of a type of one of the given callbacks or executes the given <paramref name="errorHandler" /> callback when the parsing produced errors.
        /// </summary>
        /// <param name="command1Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand1Arguments" /></param>
        /// <param name="command2Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand2Arguments" /></param>
        /// <param name="command3Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand3Arguments" /></param>
        /// <param name="command4Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand4Arguments" /></param>
        /// <param name="command5Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand5Arguments" /></param>
        /// <param name="command6Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand6Arguments" /></param>
        /// <param name="command7Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand7Arguments" /></param>
        /// <param name="command8Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand8Arguments" /></param>
        /// <param name="command9Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand9Arguments" /></param>
        /// <param name="errorHandler">A callback that is executed when the parsing produced errors.</param>
        /// <returns>The return value of the callback that was executed. If the command line arguments where parsed to a command of another type than one of the given callbacks and when the parsing produced no errors, the default value of <typeparamref name="TResult" /> is returned.</returns>
        public TResult Handle<TCommand1Arguments, TCommand2Arguments, TCommand3Arguments, TCommand4Arguments, TCommand5Arguments, TCommand6Arguments, TCommand7Arguments, TCommand8Arguments, TCommand9Arguments, TResult>(
            Func<TCommand1Arguments, TResult> command1Handler,
            Func<TCommand2Arguments, TResult> command2Handler,
            Func<TCommand3Arguments, TResult> command3Handler,
            Func<TCommand4Arguments, TResult> command4Handler,
            Func<TCommand5Arguments, TResult> command5Handler,
            Func<TCommand6Arguments, TResult> command6Handler,
            Func<TCommand7Arguments, TResult> command7Handler,
            Func<TCommand8Arguments, TResult> command8Handler,
            Func<TCommand9Arguments, TResult> command9Handler,
            Func<ParseResult, TResult> errorHandler = null
        )
        {
            return this.Handle(errorHandler, new Delegate[] { command1Handler, command2Handler, command3Handler, command4Handler, command5Handler, command6Handler, command7Handler, command8Handler, command9Handler });
        }

        /// <summary>
        /// Executes one of the given command callbacks when the command line arguments where parsed to a command of a type of one of the given callbacks or executes the given <paramref name="errorHandler" /> callback when the parsing produced errors.
        /// </summary>
        /// <param name="command1Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand1Arguments" /></param>
        /// <param name="command2Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand2Arguments" /></param>
        /// <param name="command3Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand3Arguments" /></param>
        /// <param name="command4Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand4Arguments" /></param>
        /// <param name="command5Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand5Arguments" /></param>
        /// <param name="command6Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand6Arguments" /></param>
        /// <param name="command7Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand7Arguments" /></param>
        /// <param name="command8Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand8Arguments" /></param>
        /// <param name="command9Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand9Arguments" /></param>
        /// <param name="command10Handler">A callback that is executed when the command line arguments where parsed to a command of the type <typeparamref name="TCommand10Arguments" /></param>
        /// <param name="errorHandler">A callback that is executed when the parsing produced errors.</param>
        /// <returns>The return value of the callback that was executed. If the command line arguments where parsed to a command of another type than one of the given callbacks and when the parsing produced no errors, the default value of <typeparamref name="TResult" /> is returned.</returns>
        public TResult Handle<TCommand1Arguments, TCommand2Arguments, TCommand3Arguments, TCommand4Arguments, TCommand5Arguments, TCommand6Arguments, TCommand7Arguments, TCommand8Arguments, TCommand9Arguments, TCommand10Arguments, TResult>(
            Func<TCommand1Arguments, TResult> command1Handler,
            Func<TCommand2Arguments, TResult> command2Handler,
            Func<TCommand3Arguments, TResult> command3Handler,
            Func<TCommand4Arguments, TResult> command4Handler,
            Func<TCommand5Arguments, TResult> command5Handler,
            Func<TCommand6Arguments, TResult> command6Handler,
            Func<TCommand7Arguments, TResult> command7Handler,
            Func<TCommand8Arguments, TResult> command8Handler,
            Func<TCommand9Arguments, TResult> command9Handler,
            Func<TCommand10Arguments, TResult> command10Handler,
            Func<ParseResult, TResult> errorHandler = null
        )
        {
            return this.Handle(errorHandler, new Delegate[] { command1Handler, command2Handler, command3Handler, command4Handler, command5Handler, command6Handler, command7Handler, command8Handler, command9Handler, command10Handler });
        }

        private TResult Handle<TResult>(Func<ParseResult, TResult> errorHandler, Delegate[] commandHandlers)
        {
            if (this.HasErrors)
            {
                if (errorHandler != null)
                {
                    return errorHandler(this);
                }

                return default(TResult);
            }

            if (this.CommandArguments != null)
            {
                foreach (var commandHandler in commandHandlers)
                {
                    var commandArgumentsType = commandHandler.GetType().GetGenericArguments()[0];

                    if (this.CommandArguments.GetType() != commandArgumentsType)
                    {
                        continue;
                    }

                    try
                    {
                        return (TResult)commandHandler.DynamicInvoke(this.CommandArguments);
                    }
                    catch (TargetInvocationException ex)
                    {
                        ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                    }
                }
            }

            return default(TResult);
        }

        private void Handle(Action<ParseResult> errorHandler, Delegate[] commandHandlers)
        {
            if (this.HasErrors)
            {
                errorHandler?.Invoke(this);
                return;
            }

            if (this.CommandArguments != null)
            {
                foreach (var commandHandler in commandHandlers)
                {
                    var commandArgumentsType = commandHandler.GetType().GetGenericArguments()[0];

                    if (this.CommandArguments.GetType() != commandArgumentsType)
                    {
                        continue;
                    }

                    try
                    {
                        commandHandler.DynamicInvoke(this.CommandArguments);
                        break;
                    }
                    catch (TargetInvocationException ex)
                    {
                        ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                    }
                }
            }
        }

        private readonly List<IParseError> errors;
    }
}