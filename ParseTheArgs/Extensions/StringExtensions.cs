using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ParseTheArgs.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="string" /> type.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Splits the given text into individual words in the order they appear in the text.
        /// The text is split each time a space or underscore character is found or when the casing of the text changes form lower case to upper case.
        /// Each word is returned in lower case.
        /// </summary>
        /// <param name="text">The text to split into words.</param>
        /// <returns>The words present in the given text.</returns>
        /// <example>
        /// <code>
        /// "commandLineArgument".SplitWords();   // Returns ["command", "line", "argument"].
        /// "CommandLineArgument".SplitWords();   // Returns ["command", "line", "argument"].
        /// "command_line_argument".SplitWords(); // Returns ["command", "line", "argument"].
        /// "command line argument".SplitWords(); // Returns ["command", "line", "argument"].
        /// </code>
        /// </example>
        public static IEnumerable<String> SplitWords(this String text)
        {
            if (String.IsNullOrEmpty(text))
            {
                yield break;
            }

            var currentWordBuilder = new StringBuilder();

            for (var i = 0; i < text.Length; i++)
            {
                var character = text[i];
                if (character == ' ' || character == '_')
                {
                    if (currentWordBuilder.Length > 0)
                    {
                        yield return currentWordBuilder.ToString();
                        currentWordBuilder.Clear();
                    }
                }
                else if (i > 0 && Char.IsLower(text[i - 1]) && Char.IsUpper(character))
                {
                    // Casing has changed from lower case to upper case, so a new word started.
                    if (currentWordBuilder.Length > 0)
                    {
                        yield return currentWordBuilder.ToString();
                    }

                    currentWordBuilder.Clear();
                    currentWordBuilder.Append(Char.ToLower(character));
                }
                else
                {
                    currentWordBuilder.Append(Char.ToLower(character));
                }
            }

            if (currentWordBuilder.Length > 0)
            {
                yield return currentWordBuilder.ToString();
            }
        }

        /// <summary>
        /// Converts the given string into lower camel case (see https://en.wikipedia.org/wiki/Camel_case).
        /// </summary>
        /// <param name="value">The string to convert to lower camel case.</param>
        /// <returns>The given string converted to lower camel case.</returns>
        /// "commandLineArgument".ToLowerCamelCase(); // Returns "commandLineArgument".
        /// "CommandLineArgument".ToLowerCamelCase(); // Returns "commandLineArgument".
        /// "command_line_argument".ToLowerCamelCase(); // Returns "commandLineArgument".
        /// "command line argument".ToLowerCamelCase(); // Returns "commandLineArgument".
        public static String ToCamelCase(this String value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return value;
            }

            var words = value.SplitWords().ToList();

            if (words.Count == 0)
            {
                return "";
            }

            if (words.Count == 1)
            {
                return words[0];
            }

            return words[0] + String.Join("", words.Skip(1).Select(a => a.ToFirstLetterUpperCase()));
        }

        /// <summary>
        /// Gets the given string where the first character is converted to upper case.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <returns>The given string where the first character is converted to upper case.</returns>
        /// <example>
        /// "a".ToFirstLetterUpperCase(); // Returns "A".
        /// "A".ToFirstLetterUpperCase(); // Returns "A".
        /// "ab".ToFirstLetterUpperCase(); // Returns "Ab".
        /// "aB".ToFirstLetterUpperCase(); // Returns "AB".
        /// </example>
        public static String ToFirstLetterUpperCase(this String value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return value;
            }

            if (value.Length == 1)
            {
                return value.ToUpper();
            }

            return Char.ToUpper(value[0]) + value.Substring(1);
        }

        /// <summary>
        /// Wraps the words in the given text at the specified line length boundary.
        /// </summary>
        /// <param name="text">The text to wrap the words in.</param>
        /// <param name="lineLength">The length of the line available for the text.</param>
        /// <returns>The wrapped lines of the given text.</returns>
        public static String[] WordWrap(this String text, Int32 lineLength)
        {
            if (String.IsNullOrEmpty(text))
            {
                return new String[] {text};
            }

            var pattern = @"(?<line>.{1," + lineLength + @"})(?<!\s)(\s+|$)|(?<line>.+?)(\s+|$)";
            var lines = Regex.Matches(text, pattern).Cast<Match>().Select(m => m.Groups["line"].Value);
            return lines.ToArray();
        }
    }
}