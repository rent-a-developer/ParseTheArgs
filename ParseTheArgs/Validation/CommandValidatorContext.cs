using System;
using System.Linq;
using System.Linq.Expressions;
using ParseTheArgs.Errors;
using ParseTheArgs.Parsers.Commands;

namespace ParseTheArgs.Validation
{
    /// <summary>
    /// Represents the context for a validator for a command.
    /// </summary>
    /// <typeparam name="TCommandOptions">The type in which the values of the options of the command will be stored.</typeparam>
    public class CommandValidatorContext<TCommandOptions>
        where TCommandOptions : class
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
        /// The instance in which the values of the options of the command are stored.
        /// </summary>
        public TCommandOptions? CommandOptions => (TCommandOptions?) this.ParseResult.CommandOptions;

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
        /// Gets the name of the option mapped to the specified property.
        /// </summary>
        /// <param name="optionSelector">An expression to specify the property that is mapped to an option.</param>
        /// <returns>The name of the option that is mapped to the specified property.</returns>
        /// <example>
        /// <code>
        /// public class PrintFileOptions
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
        ///         var command = setup.DefaultCommand&lt;PrintFileOptions&gt;();
        ///         command.Option(a => a.File).Name("file").IsRequired();
        ///         command.Validate(context =>
        ///         {
        ///             if (!File.Exists(context.CommandOptions.File))
        ///             {
        ///                 context.AddError(new InvalidOptionError(context.GetOptionName(a => a.File), "The specified file does not exist."));
        ///             }
        ///         });
        ///     }
        /// }
        /// </code>
        /// </example>
        public String GetOptionName(Expression<Func<TCommandOptions, Object>> optionSelector)
        {
            if (optionSelector == null)
            {
                throw new ArgumentNullException(nameof(optionSelector));
            }

            var propertyInfo = ExpressionHelper.GetPropertyFromPropertyExpression(optionSelector);
            var optionParser = this.commandParser.OptionParsers.FirstOrDefault(a => a.TargetProperty == propertyInfo);

            if (optionParser == null)
            {
                throw new ArgumentException($"The property {propertyInfo.Name} of the type {propertyInfo.DeclaringType.FullName} is not mapped to any option.", nameof(optionSelector));
            }

            return optionParser.OptionName;
        }

        private readonly ICommandParser commandParser;
    }
}