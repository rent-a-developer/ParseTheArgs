using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Setup.Options;

namespace ParseTheArgs.Setup.Commands
{
    /// <summary>
    /// Represents the configuration of a command.
    /// </summary>
    /// <typeparam name="TCommandOptions">The type where the values of the options of the command will be stored in.</typeparam>
    public abstract class CommandSetup<TCommandOptions> where TCommandOptions : class, new()
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="parser">The parser the command belongs to.</param>
        /// <param name="commandParserFactory">A function that instantiates the command parser for the command.</param>
        /// <exception cref="ArgumentException"><paramref name="parser"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="commandParserFactory"/> is null.</exception>
        protected CommandSetup(Parser parser, Func<CommandParser<TCommandOptions>> commandParserFactory)
        {
            if (parser == null)
            {
                throw new ArgumentNullException(nameof(parser));
            }

            if (commandParserFactory == null)
            {
                throw new ArgumentNullException(nameof(commandParserFactory));
            }

            this.Parser = parser;
            this.CommandParser = commandParserFactory();
        }

        /// <summary>
        /// The parser for the command.
        /// </summary>
        public CommandParser<TCommandOptions> CommandParser { get; }

        /// <summary>
        /// Sets up a boolean option (a.k.a switch option).
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option value should be stored.</param>
        /// <returns>An instance of <see cref="BooleanOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public BooleanOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, Boolean>> propertyExpression)
        {
            return Dependencies.Resolver.Resolve<BooleanOptionSetup<TCommandOptions>>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an option that accepts a single <see cref="String" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option value should be stored.</param>
        /// <returns>An instance of <see cref="StringOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public StringOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, String>> propertyExpression)
        {
            return Dependencies.Resolver.Resolve<StringOptionSetup<TCommandOptions>>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an option that accepts one or more <see cref="String" /> values.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option values should be stored.</param>
        /// <returns>An instance of <see cref="StringOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public StringListOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, List<String>>> propertyExpression)
        {
            return Dependencies.Resolver.Resolve<StringListOptionSetup<TCommandOptions>>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an option that accepts a single <see cref="DateTime" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option value should be stored.</param>
        /// <returns>An instance of <see cref="DateTimeOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public DateTimeOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, DateTime>> propertyExpression)
        {
            return Dependencies.Resolver.Resolve<DateTimeOptionSetup<TCommandOptions>>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an option that accepts one or more <see cref="DateTime" /> values.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option values should be stored.</param>
        /// <returns>An instance of <see cref="DateTimeOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public DateTimeListOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, List<DateTime>>> propertyExpression)
        {
            return Dependencies.Resolver.Resolve<DateTimeListOptionSetup<TCommandOptions>>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an option that accepts a single <see cref="DateTime" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option value should be stored.</param>
        /// <returns>An instance of <see cref="DateTimeOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public DateTimeOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, DateTime?>> propertyExpression)
        {
            return Dependencies.Resolver.Resolve<DateTimeOptionSetup<TCommandOptions>>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an option that accepts a single <see cref="TimeSpan" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option value should be stored.</param>
        /// <returns>An instance of <see cref="TimeSpanOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public TimeSpanOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, TimeSpan>> propertyExpression)
        {
            return Dependencies.Resolver.Resolve<TimeSpanOptionSetup<TCommandOptions>>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an option that accepts one or more <see cref="TimeSpan" /> values.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option values should be stored.</param>
        /// <returns>An instance of <see cref="TimeSpanListOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public TimeSpanListOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, List<TimeSpan>>> propertyExpression)
        {
            return Dependencies.Resolver.Resolve<TimeSpanListOptionSetup<TCommandOptions>>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an option that accepts a single <see cref="TimeSpan" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option value should be stored.</param>
        /// <returns>An instance of <see cref="TimeSpanOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public TimeSpanOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, TimeSpan?>> propertyExpression)
        {
            return Dependencies.Resolver.Resolve<TimeSpanOptionSetup<TCommandOptions>>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an option that accepts a single <see cref="Int64" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option value should be stored.</param>
        /// <returns>An instance of <see cref="Int64OptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public Int64OptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, Int64>> propertyExpression)
        {
            return Dependencies.Resolver.Resolve<Int64OptionSetup<TCommandOptions>>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an option that accepts one or more <see cref="Int64" /> values.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option values should be stored.</param>
        /// <returns>An instance of <see cref="Int64ListOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public Int64ListOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, List<Int64>>> propertyExpression)
        {
            return Dependencies.Resolver.Resolve<Int64ListOptionSetup<TCommandOptions>>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an option that accepts a single <see cref="Int64" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option value should be stored.</param>
        /// <returns>An instance of <see cref="Int64OptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public Int64OptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, Int64?>> propertyExpression)
        {
            return Dependencies.Resolver.Resolve<Int64OptionSetup<TCommandOptions>>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an option that accepts a single <see cref="Guid" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option value should be stored.</param>
        /// <returns>An instance of <see cref="GuidOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public GuidOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, Guid>> propertyExpression)
        {
            return Dependencies.Resolver.Resolve<GuidOptionSetup<TCommandOptions>>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an option that accepts one or more <see cref="Guid" /> values.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option values should be stored.</param>
        /// <returns>An instance of <see cref="GuidListOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public GuidListOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, List<Guid>>> propertyExpression)
        {
            return Dependencies.Resolver.Resolve<GuidListOptionSetup<TCommandOptions>>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an option that accepts a single <see cref="Guid" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option value should be stored.</param>
        /// <returns>An instance of <see cref="GuidOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public GuidOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, Guid?>> propertyExpression)
        {
            return Dependencies.Resolver.Resolve<GuidOptionSetup<TCommandOptions>>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an option that accepts a single <see cref="Decimal" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option value should be stored.</param>
        /// <returns>An instance of <see cref="DecimalOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public DecimalOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, Decimal>> propertyExpression)
        {
            return Dependencies.Resolver.Resolve<DecimalOptionSetup<TCommandOptions>>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an option that accepts one or more <see cref="Decimal" /> values.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option values should be stored.</param>
        /// <returns>An instance of <see cref="DecimalListOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public DecimalListOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, List<Decimal>>> propertyExpression)
        {
            return Dependencies.Resolver.Resolve<DecimalListOptionSetup<TCommandOptions>>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an option that accepts a single <see cref="Decimal" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option value should be stored.</param>
        /// <returns>An instance of <see cref="DecimalOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public DecimalOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, Decimal?>> propertyExpression)
        {
            return Dependencies.Resolver.Resolve<DecimalOptionSetup<TCommandOptions>>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an option that accepts a single enum member (of the type <typeparamref name="TEnum" />).
        /// The option value is expected to be the name of an enum member of the given type <typeparamref name="TEnum" /> (case-insensitive).
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option value should be stored.</param>
        /// <returns>An instance of <see cref="EnumOptionSetup{TCommandOptions,TEnum}" /> that can be used to configure the option.</returns>
        public EnumOptionSetup<TCommandOptions, TEnum> Option<TEnum>(Expression<Func<TCommandOptions, TEnum>> propertyExpression)
            where TEnum : struct, Enum
        {
            return Dependencies.Resolver.Resolve<EnumOptionSetup<TCommandOptions, TEnum>>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an option that accepts a single enum member (of the type <typeparamref name="TEnum" />).
        /// The option value is expected to be the name of an enum member of the given type <typeparamref name="TEnum" /> (case-insensitive).
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option value should be stored.</param>
        /// <returns>An instance of <see cref="EnumOptionSetup{TCommandOptions,TEnum}" /> that can be used to configure the option.</returns>
        public EnumOptionSetup<TCommandOptions, TEnum> Option<TEnum>(Expression<Func<TCommandOptions, TEnum?>> propertyExpression)
            where TEnum : struct, Enum
        {
            return Dependencies.Resolver.Resolve<EnumOptionSetup<TCommandOptions, TEnum>>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an option that accepts one or more enum members (of the type <typeparamref name="TEnum" />).
        /// The option values are expected to be the names of enum members of the given type <typeparamref name="TEnum" /> (case-insensitive).
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option values should be stored.</param>
        /// <returns>An instance of <see cref="EnumListOptionSetup{TCommandOptions,TEnum}" /> that can be used to configure the option.</returns>
        public EnumListOptionSetup<TCommandOptions, TEnum> Option<TEnum>(Expression<Func<TCommandOptions, List<TEnum>>> propertyExpression)
            where TEnum : struct, Enum
        {
            return Dependencies.Resolver.Resolve<EnumListOptionSetup<TCommandOptions, TEnum>>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Defines the parser the command belongs to.
        /// </summary>
        protected readonly Parser Parser;
    }
}