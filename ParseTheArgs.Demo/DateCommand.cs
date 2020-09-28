using System;
using ParseTheArgs.Errors;
using ParseTheArgs.Setup;
using ParseTheArgs.Validation;
using Console = System.Console;

namespace ParseTheArgs.Demo
{
    public static class DateCommand
    {
        public static Int32 GetDate(DateCommandOptions options)
        {
            try
            {
                var date = options.Date;
                date = date.Add(options.Offset);

                if (options.DifferenceToDate != null)
                {
                    var difference = options.DifferenceToDate.Value.Subtract(date);
                    Console.WriteLine($"The difference between {date} and {options.DifferenceToDate} is:");
                    Console.WriteLine(difference);
                }
                else
                {
                    if (options.DisplayInUtc)
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
                .Command<DateCommandOptions>()
                .Name("date")
                .Help("Displays a date and time and optionally calculates the difference to another date and time.")
                .ExampleUsage("Toolbox date --utc");

            command
                .Option(a => a.Date)
                .Name("date")
                .DefaultValue(DateTime.Now)
                .Help("The date and time to display. If not specified the current system date and time are used.");

            command
                .Option(a => a.Offset)
                .Name("offset")
                .DefaultValue(TimeSpan.Zero)
                .Help("Offsets the date and time by the specified duration before displaying it. Format is HH:MM:SS.");

            command
                .Option(a => a.DisplayInUtc)
                .Name("utc")
                .Help("Converts the date and time to UTC before displaying it.");

            command
                .Option(a => a.DifferenceToDate)
                .Name("differenceTo")
                .Help("If specified the difference between the first date (option --date) and this date is displayed.");

            command.Validate(Validate);
        }

        private static void Validate(CommandValidatorContext<DateCommandOptions> context)
        {
            if (context.CommandOptions.DifferenceToDate != null)
            {
                if (context.CommandOptions.DifferenceToDate.Value < context.CommandOptions.Date)
                {
                    context.AddError(
                        new InvalidOptionError(
                            context.GetOptionName(a => a.DifferenceToDate),
                            $"The given date must be later than {context.CommandOptions.Date}."
                        )
                    );
                }

                if (context.CommandOptions.DisplayInUtc)
                {
                    context.AddError(
                        new InvalidOptionError(
                            context.GetOptionName(a => a.DisplayInUtc),
                            "The option --utc can not be used when the option --differenceTo is used."
                        )
                    );
                }
            }
        }
    }
}
