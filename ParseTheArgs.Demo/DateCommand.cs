using System;
using ParseTheArgs.Errors;
using ParseTheArgs.Setup;
using ParseTheArgs.Validation;
using Console = System.Console;

namespace ParseTheArgs.Demo
{
    public static class DateCommand
    {
        public static Int32 GetDate(DateCommandArguments arguments)
        {
            try
            {
                var date = arguments.Date;
                date = date.Add(arguments.Offset);

                if (arguments.DifferenceToDate != null)
                {
                    var difference = arguments.DifferenceToDate.Value.Subtract(date);
                    Console.WriteLine($"The difference between {date} and {arguments.DifferenceToDate} is:");
                    Console.WriteLine(difference);
                }
                else
                {
                    if (arguments.DisplayInUtc)
                    {
                        date = date.ToUniversalTime();
                    }

                    Console.WriteLine("Date and time:");
                    Console.WriteLine(date);
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unhandled exception:");
                Console.Error.WriteLine(ex.ToString());
                return 1;
            }
        }

        public static void SetupCommand(ParserSetup parserSetup)
        {
            var command = parserSetup
                .Command<DateCommandArguments>()
                .Name("date")
                .Help("Displays a date and time and optionally calculates the difference to another date and time.")
                .ExampleUsage("Toolbox date --utc");

            command
                .Argument(a => a.Date)
                .Name("date")
                .DefaultValue(DateTime.Now)
                .Help("The date and time to display. If not specified the current system date and time are used.");

            command
                .Argument(a => a.Offset)
                .Name("offset")
                .DefaultValue(TimeSpan.Zero)
                .Help("Offsets the date and time by the specified duration before displaying it. Format is HH:MM:SS.");

            command
                .Argument(a => a.DisplayInUtc)
                .Name("utc")
                .Help("Converts the date and time to UTC before displaying it.");

            command
                .Argument(a => a.DifferenceToDate)
                .Name("differenceTo")
                .Help("If specified the difference between the first date (argument --date) and this date is displayed.");

            command.Validate(ValidateArguments);
        }

        private static void ValidateArguments(CommandValidatorContext<DateCommandArguments> context)
        {
            if (context.CommandArguments.DifferenceToDate != null)
            {
                if (context.CommandArguments.DifferenceToDate.Value < context.CommandArguments.Date)
                {
                    context.AddError(
                        new InvalidArgumentError(
                            context.GetArgumentName(a => a.DifferenceToDate),
                            $"The given date must be later than {context.CommandArguments.Date}."
                        )
                    );
                }

                if (context.CommandArguments.DisplayInUtc)
                {
                    context.AddError(
                        new InvalidArgumentError(
                            context.GetArgumentName(a => a.DisplayInUtc),
                            "The option --utc can not be used when the option --differenceTo is used."
                        )
                    );
                }
            }
        }
    }
}
