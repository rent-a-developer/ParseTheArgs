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
        public static Int32 ReplaceFile(FileReplaceCommandArguments arguments)
        {
            try
            {
                var text = File.ReadAllText(arguments.InFile.FullName);
                String replacedText;

                if (arguments.UseRegularExpressions)
                {
                    var regexOptions = arguments.IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None;
                    var regex = new Regex(arguments.Pattern, regexOptions);
                    replacedText = regex.Replace(text, arguments.Replacement, Int32.MaxValue);
                }
                else
                {
                    if (arguments.IgnoreCase)
                    {
                        replacedText = Regex.Replace(text, Regex.Escape(arguments.Pattern), arguments.Replacement, RegexOptions.IgnoreCase);
                    }
                    else
                    {
                        replacedText = text.Replace(arguments.Pattern, arguments.Replacement);
                    }
                }

                File.WriteAllText(arguments.OutFile.FullName, replacedText);

                if (arguments.DisplayResult)
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
                .Command<FileReplaceCommandArguments>()
                .Name("fileReplace")
                .Help("Reads text from a file, replaces a text with another text and saves the result to a new file.")
                .ExampleUsage("Toolbox fileReplace --in In.txt --out Out.txt --pattern cat --replaceWith dog");

            command
                .Argument(a => a.InFile)
                .Name("in")
                .Help("The file to read the text from.")
                .IsRequired();

            command
                .Argument(a => a.OutFile)
                .Name("out")
                .Help("The file to write the result to.")
                .IsRequired();

            command
                .Argument(a => a.Pattern)
                .Name("pattern")
                .Help("The text to replace.")
                .IsRequired();

            command
                .Argument(a => a.Replacement)
                .Name("replaceWith")
                .Help("The replacement text.")
                .IsRequired();

            command
                .Argument(a => a.IgnoreCase)
                .Name("ignoreCase")
                .Help("Ignores casing when searching for the specified text (--pattern).");

            command
                .Argument(a => a.OverrideOutFile)
                .Name("override")
                .Help("Overrides the output file if it already exists. If this option is not enabled and the output file already exists an error will be shown.");

            command
                .Argument(a => a.UseRegularExpressions)
                .Name("regex")
                .Help("Treats the given pattern (--pattern) as a regular expression.");

            command
                .Argument(a => a.DisplayResult)
                .Name("display")
                .Help("Displays the result on the console.");

            command.Validate(ValidateArguments);
        }

        private static void ValidateArguments(CommandValidatorContext<FileReplaceCommandArguments> context)
        {
            if (!context.CommandArguments.InFile.Exists)
            {
                context.AddError(
                    new InvalidArgumentError(
                        context.GetArgumentName(a => a.InFile),
                        "The specified file does not exist."
                    )
                );
            }

            if (context.CommandArguments.OutFile.Exists && !context.CommandArguments.OverrideOutFile)
            {
                context.AddError(
                    new InvalidArgumentError(
                        context.GetArgumentName(a => a.OutFile),
                        "The specified file already exists. Use the option --override to override the file."
                    )
                );
            }
        }
    }
}
