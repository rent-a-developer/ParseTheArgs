using System;
using System.IO;
using System.Text.RegularExpressions;
using ParseTheArgs.Errors;
using ParseTheArgs.Setup;
using ParseTheArgs.Validation;

namespace ParseTheArgs.Demo
{
    public static class FileReplaceCommand
    {
        public static Int32 ReplaceFile(FileReplaceCommandOptions options)
        {
            try
            {
                var text = File.ReadAllText(options.InFile.FullName);
                String replacedText;

                if (options.UseRegularExpressions)
                {
                    var regexOptions = options.IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None;
                    var regex = new Regex(options.Pattern, regexOptions);
                    replacedText = regex.Replace(text, options.Replacement, Int32.MaxValue);
                }
                else
                {
                    if (options.IgnoreCase)
                    {
                        replacedText = Regex.Replace(text, Regex.Escape(options.Pattern), options.Replacement, RegexOptions.IgnoreCase);
                    }
                    else
                    {
                        replacedText = text.Replace(options.Pattern, options.Replacement);
                    }
                }

                File.WriteAllText(options.OutFile.FullName, replacedText);

                if (options.DisplayResult)
                {
                    Console.WriteLine(replacedText);
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
                .Command<FileReplaceCommandOptions>()
                .Name("fileReplace")
                .Help("Reads text from a file, replaces a text with another text and saves the result to a new file.")
                .ExampleUsage("Toolbox fileReplace --in In.txt --out Out.txt --pattern cat --replaceWith dog");

            command
                .Option(a => a.InFile)
                .Name("in")
                .Help("The file to read the text from.")
                .IsRequired();

            command
                .Option(a => a.OutFile)
                .Name("out")
                .Help("The file to write the result to.")
                .IsRequired();

            command
                .Option(a => a.Pattern)
                .Name("pattern")
                .Help("The text to replace.")
                .IsRequired();

            command
                .Option(a => a.Replacement)
                .Name("replaceWith")
                .Help("The replacement text.")
                .IsRequired();

            command
                .Option(a => a.IgnoreCase)
                .Name("ignoreCase")
                .Help("Ignores casing when searching for the specified text (--pattern).");

            command
                .Option(a => a.OverrideOutFile)
                .Name("override")
                .Help("Overrides the output file if it already exists. If this option is not enabled and the output file already exists an error will be shown.");

            command
                .Option(a => a.UseRegularExpressions)
                .Name("regex")
                .Help("Treats the given pattern (--pattern) as a regular expression.");

            command
                .Option(a => a.DisplayResult)
                .Name("display")
                .Help("Displays the result on the console.");

            command.Validate(Validate);
        }

        private static void Validate(CommandValidatorContext<FileReplaceCommandOptions> context)
        {
            if (!context.CommandOptions.InFile.Exists)
            {
                context.AddError(
                    new InvalidOptionError(
                        context.GetOptionName(a => a.InFile),
                        "The specified file does not exist."
                    )
                );
            }

            if (context.CommandOptions.OutFile.Exists && !context.CommandOptions.OverrideOutFile)
            {
                context.AddError(
                    new InvalidOptionError(
                        context.GetOptionName(a => a.OutFile),
                        "The specified file already exists. Use the option --override to override the file."
                    )
                );
            }
        }
    }
}
