using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Setup.Arguments;

namespace ParseTheArgs.Setup.Commands
{
    /// <summary>
    /// Represents the configuration of a command.
    /// </summary>
    /// <typeparam name="TCommandArguments">The type where the values of the arguments of the command will be stored in.</typeparam>
    public abstract class CommandSetup<TCommandArguments> where TCommandArguments : class, new()
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="parser">The parser the command belongs to.</param>
        /// <param name="commandParserFactory">A function that instantiates the command parser for the command.</param>
        protected CommandSetup(Parser parser, Func<Parser, CommandParser<TCommandArguments>> commandParserFactory)
        {
            this.Parser = parser;
            this.CommandParser = commandParserFactory(parser);
        }

        /// <summary>
        /// The parser for the command.
        /// </summary>
        public CommandParser<TCommandArguments> CommandParser { get; }

        /// <summary>
        /// Sets up a boolean argument (a.k.a switch argument).
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandArguments" /> type in which the argument value should be stored.</param>
        /// <returns>An instance of <see cref="BooleanArgumentSetup{TCommandArguments}" /> that can be used to configure the argument.</returns>
        public BooleanArgumentSetup<TCommandArguments> Argument(Expression<Func<TCommandArguments, Boolean>> propertyExpression)
        {
            return new BooleanArgumentSetup<TCommandArguments>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an argument that accepts a single <see cref="String" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandArguments" /> type in which the argument value should be stored.</param>
        /// <returns>An instance of <see cref="StringArgumentSetup{TCommandArguments}" /> that can be used to configure the argument.</returns>
        public StringArgumentSetup<TCommandArguments> Argument(Expression<Func<TCommandArguments, String>> propertyExpression)
        {
            return new StringArgumentSetup<TCommandArguments>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an argument that accepts one or more <see cref="String" /> values.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandArguments" /> type in which the argument values should be stored.</param>
        /// <returns>An instance of <see cref="StringArgumentSetup{TCommandArguments}" /> that can be used to configure the argument.</returns>
        public StringListArgumentSetup<TCommandArguments> Argument(Expression<Func<TCommandArguments, List<String>>> propertyExpression)
        {
            return new StringListArgumentSetup<TCommandArguments>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an argument that accepts a single <see cref="DateTime" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandArguments" /> type in which the argument value should be stored.</param>
        /// <returns>An instance of <see cref="DateTimeArgumentSetup{TCommandArguments}" /> that can be used to configure the argument.</returns>
        public DateTimeArgumentSetup<TCommandArguments> Argument(Expression<Func<TCommandArguments, DateTime>> propertyExpression)
        {
            return new DateTimeArgumentSetup<TCommandArguments>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an argument that accepts one or more <see cref="DateTime" /> values.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandArguments" /> type in which the argument values should be stored.</param>
        /// <returns>An instance of <see cref="DateTimeArgumentSetup{TCommandArguments}" /> that can be used to configure the argument.</returns>
        public DateTimeListArgumentSetup<TCommandArguments> Argument(Expression<Func<TCommandArguments, List<DateTime>>> propertyExpression)
        {
            return new DateTimeListArgumentSetup<TCommandArguments>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an argument that accepts a single <see cref="DateTime" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandArguments" /> type in which the argument value should be stored.</param>
        /// <returns>An instance of <see cref="DateTimeArgumentSetup{TCommandArguments}" /> that can be used to configure the argument.</returns>
        public DateTimeArgumentSetup<TCommandArguments> Argument(Expression<Func<TCommandArguments, DateTime?>> propertyExpression)
        {
            return new DateTimeArgumentSetup<TCommandArguments>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an argument that accepts a single <see cref="TimeSpan" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandArguments" /> type in which the argument value should be stored.</param>
        /// <returns>An instance of <see cref="TimeSpanArgumentSetup{TCommandArguments}" /> that can be used to configure the argument.</returns>
        public TimeSpanArgumentSetup<TCommandArguments> Argument(Expression<Func<TCommandArguments, TimeSpan>> propertyExpression)
        {
            return new TimeSpanArgumentSetup<TCommandArguments>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an argument that accepts one or more <see cref="TimeSpan" /> values.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandArguments" /> type in which the argument values should be stored.</param>
        /// <returns>An instance of <see cref="TimeSpanListArgumentSetup{TCommandArguments}" /> that can be used to configure the argument.</returns>
        public TimeSpanListArgumentSetup<TCommandArguments> Argument(Expression<Func<TCommandArguments, List<TimeSpan>>> propertyExpression)
        {
            return new TimeSpanListArgumentSetup<TCommandArguments>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an argument that accepts a single <see cref="TimeSpan" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandArguments" /> type in which the argument value should be stored.</param>
        /// <returns>An instance of <see cref="TimeSpanArgumentSetup{TCommandArguments}" /> that can be used to configure the argument.</returns>
        public TimeSpanArgumentSetup<TCommandArguments> Argument(Expression<Func<TCommandArguments, TimeSpan?>> propertyExpression)
        {
            return new TimeSpanArgumentSetup<TCommandArguments>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an argument that accepts a single <see cref="Int64" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandArguments" /> type in which the argument value should be stored.</param>
        /// <returns>An instance of <see cref="Int64ArgumentSetup{TCommandArguments}" /> that can be used to configure the argument.</returns>
        public Int64ArgumentSetup<TCommandArguments> Argument(Expression<Func<TCommandArguments, Int64>> propertyExpression)
        {
            return new Int64ArgumentSetup<TCommandArguments>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an argument that accepts one or more <see cref="Int64" /> values.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandArguments" /> type in which the argument values should be stored.</param>
        /// <returns>An instance of <see cref="Int64ListArgumentSetup{TCommandArguments}" /> that can be used to configure the argument.</returns>
        public Int64ListArgumentSetup<TCommandArguments> Argument(Expression<Func<TCommandArguments, List<Int64>>> propertyExpression)
        {
            return new Int64ListArgumentSetup<TCommandArguments>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an argument that accepts a single <see cref="Int64" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandArguments" /> type in which the argument value should be stored.</param>
        /// <returns>An instance of <see cref="Int64ArgumentSetup{TCommandArguments}" /> that can be used to configure the argument.</returns>
        public Int64ArgumentSetup<TCommandArguments> Argument(Expression<Func<TCommandArguments, Int64?>> propertyExpression)
        {
            return new Int64ArgumentSetup<TCommandArguments>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an argument that accepts a single <see cref="Guid" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandArguments" /> type in which the argument value should be stored.</param>
        /// <returns>An instance of <see cref="GuidArgumentSetup{TCommandArguments}" /> that can be used to configure the argument.</returns>
        public GuidArgumentSetup<TCommandArguments> Argument(Expression<Func<TCommandArguments, Guid>> propertyExpression)
        {
            return new GuidArgumentSetup<TCommandArguments>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an argument that accepts one or more <see cref="Guid" /> values.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandArguments" /> type in which the argument values should be stored.</param>
        /// <returns>An instance of <see cref="GuidListArgumentSetup{TCommandArguments}" /> that can be used to configure the argument.</returns>
        public GuidListArgumentSetup<TCommandArguments> Argument(Expression<Func<TCommandArguments, List<Guid>>> propertyExpression)
        {
            return new GuidListArgumentSetup<TCommandArguments>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an argument that accepts a single <see cref="Guid" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandArguments" /> type in which the argument value should be stored.</param>
        /// <returns>An instance of <see cref="GuidArgumentSetup{TCommandArguments}" /> that can be used to configure the argument.</returns>
        public GuidArgumentSetup<TCommandArguments> Argument(Expression<Func<TCommandArguments, Guid?>> propertyExpression)
        {
            return new GuidArgumentSetup<TCommandArguments>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an argument that accepts a single <see cref="Decimal" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandArguments" /> type in which the argument value should be stored.</param>
        /// <returns>An instance of <see cref="DecimalArgumentSetup{TCommandArguments}" /> that can be used to configure the argument.</returns>
        public DecimalArgumentSetup<TCommandArguments> Argument(Expression<Func<TCommandArguments, Decimal>> propertyExpression)
        {
            return new DecimalArgumentSetup<TCommandArguments>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an argument that accepts one or more <see cref="Decimal" /> values.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandArguments" /> type in which the argument values should be stored.</param>
        /// <returns>An instance of <see cref="DecimalListArgumentSetup{TCommandArguments}" /> that can be used to configure the argument.</returns>
        public DecimalListArgumentSetup<TCommandArguments> Argument(Expression<Func<TCommandArguments, List<Decimal>>> propertyExpression)
        {
            return new DecimalListArgumentSetup<TCommandArguments>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an argument that accepts a single <see cref="Decimal" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandArguments" /> type in which the argument value should be stored.</param>
        /// <returns>An instance of <see cref="DecimalArgumentSetup{TCommandArguments}" /> that can be used to configure the argument.</returns>
        public DecimalArgumentSetup<TCommandArguments> Argument(Expression<Func<TCommandArguments, Decimal?>> propertyExpression)
        {
            return new DecimalArgumentSetup<TCommandArguments>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an argument that accepts a single enum member (of the type <typeparamref name="TEnum" />).
        /// The argument value is expected to be the name of an enum member of the given type <typeparamref name="TEnum" /> (case-insensitive).
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandArguments" /> type in which the argument value should be stored.</param>
        /// <returns>An instance of <see cref="EnumArgumentSetup{TCommandArguments,TEnum}" /> that can be used to configure the argument.</returns>
        public EnumArgumentSetup<TCommandArguments, TEnum> Argument<TEnum>(Expression<Func<TCommandArguments, TEnum>> propertyExpression)
            where TEnum : struct, Enum
        {
            return new EnumArgumentSetup<TCommandArguments, TEnum>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an argument that accepts a single enum member (of the type <typeparamref name="TEnum" />).
        /// The argument value is expected to be the name of an enum member of the given type <typeparamref name="TEnum" /> (case-insensitive).
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandArguments" /> type in which the argument value should be stored.</param>
        /// <returns>An instance of <see cref="EnumArgumentSetup{TCommandArguments,TEnum}" /> that can be used to configure the argument.</returns>
        public EnumArgumentSetup<TCommandArguments, TEnum> Argument<TEnum>(Expression<Func<TCommandArguments, TEnum?>> propertyExpression)
            where TEnum : struct, Enum
        {
            return new EnumArgumentSetup<TCommandArguments, TEnum>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Sets up an argument that accepts one or more enum members (of the type <typeparamref name="TEnum" />).
        /// The argument values are expected to be the names of enum members of the given type <typeparamref name="TEnum" /> (case-insensitive).
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandArguments" /> type in which the argument values should be stored.</param>
        /// <returns>An instance of <see cref="EnumListArgumentSetup{TCommandArguments,TEnum}" /> that can be used to configure the argument.</returns>
        public EnumListArgumentSetup<TCommandArguments, TEnum> Argument<TEnum>(Expression<Func<TCommandArguments, List<TEnum>>> propertyExpression)
            where TEnum : struct, Enum
        {
            return new EnumListArgumentSetup<TCommandArguments, TEnum>(this.CommandParser, propertyExpression);
        }

        /// <summary>
        /// Defines the parser the command belongs to.
        /// </summary>
        protected readonly Parser Parser;
    }
}