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
            this.CommandName = String.Empty;
            this.commandHandlers = new Dictionary<Type, Delegate>();
            this.errors = new List<IParseError>();
        }

        /// <summary>
        /// Defines the Object that holds the options of the parsed command.
        /// </summary>
        public virtual Object? CommandOptions { get; internal set; }

        /// <summary>
        /// Defines the name of the command that was found in the parsed command line arguments.
        /// </summary>
        public virtual String CommandName { get; internal set; }

        /// <summary>
        /// Determines whether the command line was just used to get the help of the program (e.g. no command and/or options where passed or the help command was specified).
        /// </summary>
        public virtual Boolean IsHelpCalled { get; internal set; }

        /// <summary>
        /// A list of errors found during parsing.
        /// </summary>
        public virtual ReadOnlyCollection<IParseError> Errors => this.errors.AsReadOnly();

        /// <summary>
        /// Determines if errors where found during parsing.
        /// </summary>
        public virtual Boolean HasErrors => this.errors.Count > 0;

        /// <summary>
        /// Adds a parse error to this instance.
        /// </summary>
        /// <param name="error">The error to add.</param>
        public virtual void AddError(IParseError error)
        {
            if (error == null)
            {
                throw new ArgumentNullException(nameof(error));
            }

            this.errors.Add(error);
        }

        /// <summary>
        /// Sets up a command handler for a specific command.
        /// The handler will be invoked when the <see cref="Handle"/> method is called and <see cref="CommandOptions"/> is of type <typeparamref name="TCommandOptions"/>.
        /// The return value of the specified handler is returned by the <see cref="Handle"/> method when the handler is invoked.
        /// </summary>
        /// <typeparam name="TCommandOptions">The type of the command options the handler handles.</typeparam>
        /// <param name="commandHandler">The handler that handles the command options of type <typeparamref name="TCommandOptions"/>.</param>
        public virtual void CommandHandler<TCommandOptions>(Func<TCommandOptions, Int32> commandHandler)
        {
            if (commandHandler == null)
            {
                throw new ArgumentNullException(nameof(commandHandler));
            }

            this.commandHandlers[typeof(TCommandOptions)] = commandHandler;
        }

        /// <summary>
        /// Sets up an error handler.
        /// The handler will be invoked when <see cref="Handle"/> is called and <see cref="HasErrors"/> is true.
        /// The return value of the specified handler is returned by the <see cref="Handle"/> method when the handler is invoked.
        /// </summary>
        /// <param name="errorHandler">The handler that handles the errors.</param>
        public virtual void ErrorHandler(Func<ParseResult, Int32> errorHandler)
        {
            if (errorHandler == null)
            {
                throw new ArgumentNullException(nameof(errorHandler));
            }

            this.errorHandler = errorHandler;
        }

        /// <summary>
        /// Handles the result of the command line parsing.
        /// The command handler that was set up (via the <see cref="CommandHandler{TCommandOptions}"/> method) for the current command will be invoked.
        /// In case <see cref="HasErrors"/> is true, the error handler that was set up (via the <see cref="ErrorHandler"/> method) will be invoked.
        /// </summary>
        /// <returns>
        /// The return value of the command handler that was set up for the current command.
        /// If no command handler was set up for the current command 0 will be returned.
        /// 
        /// If <see cref="HasErrors"/> is true and an error handler was set up the return value of the error handler will be returned.
        /// If <see cref="HasErrors"/> is true and no error handler was set up 0 will be returned.
        /// </returns>
        public virtual Int32 Handle()
        {
            if (this.HasErrors)
            {
                if (this.errorHandler != null)
                {
                    try
                    {
                        var errorHandlerReturnValue = this.errorHandler.DynamicInvoke(this);
                        if (errorHandlerReturnValue is Int32 errorHandlerInt32ReturnValue)
                        {
                            return errorHandlerInt32ReturnValue;
                        }
                    }
                    catch (TargetInvocationException ex)
                    {
                        ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                    }
                }
            }
            else if (this.CommandOptions != null && this.commandHandlers.TryGetValue(this.CommandOptions.GetType(), out Delegate commandHandler))
            {
                try
                {
                    var commandHandlerReturnValue = commandHandler.DynamicInvoke(this.CommandOptions);
                    if (commandHandlerReturnValue is Int32 commandHandlerInt32ReturnValue)
                    {
                        return commandHandlerInt32ReturnValue;
                    }
                }
                catch (TargetInvocationException ex)
                {
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                }
            }

            return 0;
        }

        private readonly List<IParseError> errors;
        private readonly Dictionary<Type, Delegate> commandHandlers;
        private Delegate? errorHandler;
    }
}