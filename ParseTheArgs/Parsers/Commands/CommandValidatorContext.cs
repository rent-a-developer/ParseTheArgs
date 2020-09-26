using System;
using System.Linq;
using System.Linq.Expressions;
using ParseTheArgs.Errors;

namespace ParseTheArgs.Parsers.Commands
{
    /// <summary>
    /// Represents the context for a validator for a command.
    /// </summary>
    /// <typeparam name="TCommandArguments">The type in which the values of the arguments of the command will be stored.</typeparam>
    public class CommandValidatorContext<TCommandArguments>
        where TCommandArguments : class
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="commandParser">The parser of the command.</param>
        /// <param name="parseResult">The (preliminary) result of the command line arguments parsing.</param>
        public CommandValidatorContext(ICommandParser commandParser, ParseResult parseResult)
        {
            this.commandParser = commandParser;
            this.ParseResult = parseResult;
        }

        /// <summary>
        /// The instance in which the values of the arguments of the command are stored.
        /// </summary>
        public TCommandArguments? CommandArguments => (TCommandArguments?) this.ParseResult.CommandArguments;

        /// <summary>
        /// The (preliminary) result of the command line arguments parsing.
        /// </summary>
        public ParseResult ParseResult { get; }

        /// <summary>
        /// Adds an error to the parse result.
        /// </summary>
        /// <param name="error">The error to add to the parse result.</param>
        public void AddError(IParseError error)
        {
            this.ParseResult.AddError(error);
        }

        /// <summary>
        /// Gets the name of the argument mapped to the specified property.
        /// </summary>
        /// <param name="argumentSelector">An expression to specify the property that is mapped to an argument.</param>
        /// <returns>An instance of <see cref="ArgumentName"/> representing the name of the argument.</returns>
        /// <example>
        /// <code>
        /// public class PrintFileArguments
        /// {
        ///     public String File { get; set; }
        /// }
        ///
        /// public class Program
        /// {
        ///     public static void Main(String[] args)
        ///     {
        ///         var parser = new Parser();
        ///         var setup = parser.Setup;
        ///
        ///         var command = setup.DefaultCommand&lt;PrintFileArguments&gt;();
        ///         command.Argument(a => a.File).Name("file").IsRequired();
        ///         command.Validate(context =>
        ///         {
        ///             if (!File.Exists(context.CommandArguments.File))
        ///             {
        ///                 context.AddError(new InvalidArgumentError(context.GetArgumentName(a => a.File), "The specified file does not exist."));
        ///             }
        ///         });
        ///     }
        /// }
        /// </code>
        /// </example>
        public ArgumentName GetArgumentName(Expression<Func<TCommandArguments, Object>> argumentSelector)
        {
            if (argumentSelector == null)
            {
                throw new ArgumentNullException(nameof(argumentSelector));
            }

            var propertyInfo = ExpressionHelper.GetPropertyFromPropertyExpression(argumentSelector);
            var argumentParser = this.commandParser.ArgumentParsers.FirstOrDefault(a => a.TargetProperty == propertyInfo);

            if (argumentParser == null)
            {
                throw new ArgumentException($"The property {propertyInfo.Name} of the type {propertyInfo.DeclaringType.FullName} is not mapped to any argument.", nameof(argumentSelector));
            }

            return argumentParser.ArgumentName;
        }

        private readonly ICommandParser commandParser;
    }
}