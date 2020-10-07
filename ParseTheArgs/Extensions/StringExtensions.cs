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
        /// 
        /// The text is split each time:
        /// - A space character is found or
        /// - A underscore character is found or
        /// - When the casing in the text changes form lower to upper case.
        /// 
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
        /// "command1options".SplitWords();       // Returns ["command1", "options"].
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

                var isWordSeparatorCharacter = character == ' ' || character == '_';
                var isCasingChangingFromLoweToUpper = i > 0 && Char.IsUpper(character) && !Char.IsUpper(text[i - 1]);

                if (isWordSeparatorCharacter)
                {
                    // We hit a word boundary, so we return all the characters we have collected so far and clear the builder to start a new word.
                    if (currentWordBuilder.Length > 0)
                    {
                        yield return currentWordBuilder.ToString();
                        currentWordBuilder.Clear();
                    }
                }
                else if (isCasingChangingFromLoweToUpper)
                {
                    // Either the casing has changed from lower to upper case or the end of a number has been found.
                    // Since we consider both cases as a word boundary we return all the characters we have collected so far and clear the builder to start a new word.
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
        /// Converts the given string into camel case (see https://en.wikipedia.org/wiki/Camel_case).
        /// </summary>
        /// <param name="value">The string to convert to lower camel case.</param>
        /// <returns>The given string converted to lower camel case.</returns>
        /// "commandLineArgument".ToCamelCase();   // Returns "commandLineArgument".
        /// "CommandLineArgument".ToCamelCase();   // Returns "commandLineArgument".
        /// "command_line_argument".ToCamelCase(); // Returns "commandLineArgument".
        /// "command line argument".ToCamelCase(); // Returns "commandLineArgument".
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
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (text == String.Empty)
            {
                return new String[] { String.Empty };
            }

            var pattern = @"(?<line>.{1," + lineLength + @"})(?<!\s)(\s+|$)|(?<line>.+?)(\s+|$)";
            var lines = Regex.Matches(text, pattern).Cast<Match>().Select(m => m.Groups["line"].Value);
            return lines.ToArray();
        }
    }
}