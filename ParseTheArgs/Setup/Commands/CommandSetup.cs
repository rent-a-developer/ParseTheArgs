using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Parsers.Options;
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
        /// <param name="commandParser">The command parser for the command.</param>
        /// <exception cref="ArgumentException"><paramref name="parser" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="commandParser" /> is null.</exception>
        protected CommandSetup(Parser parser, CommandParser<TCommandOptions> commandParser)
        {
            if (parser == null)
            {
                throw new ArgumentNullException(nameof(parser));
            }

            if (commandParser == null)
            {
                throw new ArgumentNullException(nameof(commandParser));
            }

            this.Parser = parser;
            this.CommandParser = commandParser;
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
            var targetProperty = ExpressionHelper.GetPropertyFromPropertyExpression(propertyExpression);
            var optionParser = this.CommandParser.GetOrCreateOptionParser<BooleanOptionParser>(targetProperty);

            return Dependencies.Resolver.Resolve<BooleanOptionSetup<TCommandOptions>>(this.CommandParser, optionParser);
        }

        /// <summary>
        /// Sets up an option that accepts a single <see cref="String" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option value should be stored.</param>
        /// <returns>An instance of <see cref="StringOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public StringOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, String>> propertyExpression)
        {
            var targetProperty = ExpressionHelper.GetPropertyFromPropertyExpression(propertyExpression);
            var optionParser = this.CommandParser.GetOrCreateOptionParser<StringOptionParser>(targetProperty);

            return Dependencies.Resolver.Resolve<StringOptionSetup<TCommandOptions>>(this.CommandParser, optionParser);
        }

        /// <summary>
        /// Sets up an option that accepts one or more <see cref="String" /> values.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option values should be stored.</param>
        /// <returns>An instance of <see cref="StringOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public StringListOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, List<String>>> propertyExpression)
        {
            var targetProperty = ExpressionHelper.GetPropertyFromPropertyExpression(propertyExpression);
            var optionParser = this.CommandParser.GetOrCreateOptionParser<StringListOptionParser>(targetProperty);

            return Dependencies.Resolver.Resolve<StringListOptionSetup<TCommandOptions>>(this.CommandParser, optionParser);
        }

        /// <summary>
        /// Sets up an option that accepts a single <see cref="DateTime" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option value should be stored.</param>
        /// <returns>An instance of <see cref="DateTimeOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public DateTimeOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, DateTime>> propertyExpression)
        {
            var targetProperty = ExpressionHelper.GetPropertyFromPropertyExpression(propertyExpression);
            var optionParser = this.CommandParser.GetOrCreateOptionParser<DateTimeOptionParser>(targetProperty);

            return Dependencies.Resolver.Resolve<DateTimeOptionSetup<TCommandOptions>>(this.CommandParser, optionParser);
        }

        /// <summary>
        /// Sets up an option that accepts one or more <see cref="DateTime" /> values.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option values should be stored.</param>
        /// <returns>An instance of <see cref="DateTimeOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public DateTimeListOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, List<DateTime>>> propertyExpression)
        {
            var targetProperty = ExpressionHelper.GetPropertyFromPropertyExpression(propertyExpression);
            var optionParser = this.CommandParser.GetOrCreateOptionParser<DateTimeListOptionParser>(targetProperty);

            return Dependencies.Resolver.Resolve<DateTimeListOptionSetup<TCommandOptions>>(this.CommandParser, optionParser);
        }

        /// <summary>
        /// Sets up an option that accepts a single <see cref="DateTime" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option value should be stored.</param>
        /// <returns>An instance of <see cref="DateTimeOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public DateTimeOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, DateTime?>> propertyExpression)
        {
            var targetProperty = ExpressionHelper.GetPropertyFromPropertyExpression(propertyExpression);
            var optionParser = this.CommandParser.GetOrCreateOptionParser<DateTimeOptionParser>(targetProperty);

            return Dependencies.Resolver.Resolve<DateTimeOptionSetup<TCommandOptions>>(this.CommandParser, optionParser);
        }

        /// <summary>
        /// Sets up an option that accepts a single <see cref="TimeSpan" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option value should be stored.</param>
        /// <returns>An instance of <see cref="TimeSpanOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public TimeSpanOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, TimeSpan>> propertyExpression)
        {
            var targetProperty = ExpressionHelper.GetPropertyFromPropertyExpression(propertyExpression);
            var optionParser = this.CommandParser.GetOrCreateOptionParser<TimeSpanOptionParser>(targetProperty);

            return Dependencies.Resolver.Resolve<TimeSpanOptionSetup<TCommandOptions>>(this.CommandParser, optionParser);
        }

        /// <summary>
        /// Sets up an option that accepts one or more <see cref="TimeSpan" /> values.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option values should be stored.</param>
        /// <returns>An instance of <see cref="TimeSpanListOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public TimeSpanListOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, List<TimeSpan>>> propertyExpression)
        {
            var targetProperty = ExpressionHelper.GetPropertyFromPropertyExpression(propertyExpression);
            var optionParser = this.CommandParser.GetOrCreateOptionParser<TimeSpanListOptionParser>(targetProperty);

            return Dependencies.Resolver.Resolve<TimeSpanListOptionSetup<TCommandOptions>>(this.CommandParser, optionParser);
        }

        /// <summary>
        /// Sets up an option that accepts a single <see cref="TimeSpan" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option value should be stored.</param>
        /// <returns>An instance of <see cref="TimeSpanOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public TimeSpanOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, TimeSpan?>> propertyExpression)
        {
            var targetProperty = ExpressionHelper.GetPropertyFromPropertyExpression(propertyExpression);
            var optionParser = this.CommandParser.GetOrCreateOptionParser<TimeSpanOptionParser>(targetProperty);

            return Dependencies.Resolver.Resolve<TimeSpanOptionSetup<TCommandOptions>>(this.CommandParser, optionParser);
        }

        /// <summary>
        /// Sets up an option that accepts a single <see cref="Int64" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option value should be stored.</param>
        /// <returns>An instance of <see cref="Int64OptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public Int64OptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, Int64>> propertyExpression)
        {
            var targetProperty = ExpressionHelper.GetPropertyFromPropertyExpression(propertyExpression);
            var optionParser = this.CommandParser.GetOrCreateOptionParser<Int64OptionParser>(targetProperty);

            return Dependencies.Resolver.Resolve<Int64OptionSetup<TCommandOptions>>(this.CommandParser, optionParser);
        }

        /// <summary>
        /// Sets up an option that accepts one or more <see cref="Int64" /> values.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option values should be stored.</param>
        /// <returns>An instance of <see cref="Int64ListOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public Int64ListOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, List<Int64>>> propertyExpression)
        {
            var targetProperty = ExpressionHelper.GetPropertyFromPropertyExpression(propertyExpression);
            var optionParser = this.CommandParser.GetOrCreateOptionParser<Int64ListOptionParser>(targetProperty);

            return Dependencies.Resolver.Resolve<Int64ListOptionSetup<TCommandOptions>>(this.CommandParser, optionParser);
        }

        /// <summary>
        /// Sets up an option that accepts a single <see cref="Int64" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option value should be stored.</param>
        /// <returns>An instance of <see cref="Int64OptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public Int64OptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, Int64?>> propertyExpression)
        {
            var targetProperty = ExpressionHelper.GetPropertyFromPropertyExpression(propertyExpression);
            var optionParser = this.CommandParser.GetOrCreateOptionParser<Int64OptionParser>(targetProperty);

            return Dependencies.Resolver.Resolve<Int64OptionSetup<TCommandOptions>>(this.CommandParser, optionParser);
        }

        /// <summary>
        /// Sets up an option that accepts a single <see cref="Guid" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option value should be stored.</param>
        /// <returns>An instance of <see cref="GuidOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public GuidOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, Guid>> propertyExpression)
        {
            var targetProperty = ExpressionHelper.GetPropertyFromPropertyExpression(propertyExpression);
            var optionParser = this.CommandParser.GetOrCreateOptionParser<GuidOptionParser>(targetProperty);

            return Dependencies.Resolver.Resolve<GuidOptionSetup<TCommandOptions>>(this.CommandParser, optionParser);
        }

        /// <summary>
        /// Sets up an option that accepts one or more <see cref="Guid" /> values.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option values should be stored.</param>
        /// <returns>An instance of <see cref="GuidListOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public GuidListOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, List<Guid>>> propertyExpression)
        {
            var targetProperty = ExpressionHelper.GetPropertyFromPropertyExpression(propertyExpression);
            var optionParser = this.CommandParser.GetOrCreateOptionParser<GuidListOptionParser>(targetProperty);

            return Dependencies.Resolver.Resolve<GuidListOptionSetup<TCommandOptions>>(this.CommandParser, optionParser);
        }

        /// <summary>
        /// Sets up an option that accepts a single <see cref="Guid" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option value should be stored.</param>
        /// <returns>An instance of <see cref="GuidOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public GuidOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, Guid?>> propertyExpression)
        {
            var targetProperty = ExpressionHelper.GetPropertyFromPropertyExpression(propertyExpression);
            var optionParser = this.CommandParser.GetOrCreateOptionParser<GuidOptionParser>(targetProperty);

            return Dependencies.Resolver.Resolve<GuidOptionSetup<TCommandOptions>>(this.CommandParser, optionParser);
        }

        /// <summary>
        /// Sets up an option that accepts a single <see cref="Decimal" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option value should be stored.</param>
        /// <returns>An instance of <see cref="DecimalOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public DecimalOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, Decimal>> propertyExpression)
        {
            var targetProperty = ExpressionHelper.GetPropertyFromPropertyExpression(propertyExpression);
            var optionParser = this.CommandParser.GetOrCreateOptionParser<DecimalOptionParser>(targetProperty);

            return Dependencies.Resolver.Resolve<DecimalOptionSetup<TCommandOptions>>(this.CommandParser, optionParser);
        }

        /// <summary>
        /// Sets up an option that accepts one or more <see cref="Decimal" /> values.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option values should be stored.</param>
        /// <returns>An instance of <see cref="DecimalListOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public DecimalListOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, List<Decimal>>> propertyExpression)
        {
            var targetProperty = ExpressionHelper.GetPropertyFromPropertyExpression(propertyExpression);
            var optionParser = this.CommandParser.GetOrCreateOptionParser<DecimalListOptionParser>(targetProperty);

            return Dependencies.Resolver.Resolve<DecimalListOptionSetup<TCommandOptions>>(this.CommandParser, optionParser);
        }

        /// <summary>
        /// Sets up an option that accepts a single <see cref="Decimal" /> value.
        /// </summary>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option value should be stored.</param>
        /// <returns>An instance of <see cref="DecimalOptionSetup{TCommandOptions}" /> that can be used to configure the option.</returns>
        public DecimalOptionSetup<TCommandOptions> Option(Expression<Func<TCommandOptions, Decimal?>> propertyExpression)
        {
            var targetProperty = ExpressionHelper.GetPropertyFromPropertyExpression(propertyExpression);
            var optionParser = this.CommandParser.GetOrCreateOptionParser<DecimalOptionParser>(targetProperty);

            return Dependencies.Resolver.Resolve<DecimalOptionSetup<TCommandOptions>>(this.CommandParser, optionParser);
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
            var targetProperty = ExpressionHelper.GetPropertyFromPropertyExpression(propertyExpression);
            var optionParser = this.CommandParser.GetOrCreateOptionParser<EnumOptionParser<TEnum>>(targetProperty);

            return Dependencies.Resolver.Resolve<EnumOptionSetup<TCommandOptions, TEnum>>(this.CommandParser, optionParser);
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
            var targetProperty = ExpressionHelper.GetPropertyFromPropertyExpression(propertyExpression);
            var optionParser = this.CommandParser.GetOrCreateOptionParser<EnumOptionParser<TEnum>>(targetProperty);

            return Dependencies.Resolver.Resolve<EnumOptionSetup<TCommandOptions, TEnum>>(this.CommandParser, optionParser);
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
            var targetProperty = ExpressionHelper.GetPropertyFromPropertyExpression(propertyExpression);
            var optionParser = this.CommandParser.GetOrCreateOptionParser<EnumListOptionParser<TEnum>>(targetProperty);

            return Dependencies.Resolver.Resolve<EnumListOptionSetup<TCommandOptions, TEnum>>(this.CommandParser, optionParser);
        }

        /// <summary>
        /// Defines the parser the command belongs to.
        /// </summary>
        protected readonly Parser Parser;
    }
}