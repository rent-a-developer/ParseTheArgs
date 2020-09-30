using System;
using FluentAssertions;
using NUnit.Framework;
using ParseTheArgs.Extensions;

namespace ParseTheArgs.Tests.Extensions
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test(Description = "SplitWords should return the individual words of a camel case string.")]
        public void SplitWords_CamelCase_ShouldReturnWords()
        {
            "commandLineArgument".SplitWords().Should().BeEquivalentTo("command", "line", "argument");
        }

        [Test(Description = "SplitWords should return an empty string when an empty string is given.")]
        public void SplitWords_EmptyString_ShouldReturnEmptyString()
        {
            "".SplitWords().Should().BeEmpty();
        }

        [Test(Description = "SplitWords should return an empty sequence when null is given.")]
        public void SplitWords_Null_ShouldReturnEmptyString()
        {
            ((String) null).SplitWords().Should().BeEmpty();
        }

        [Test(Description = "SplitWords should return the individual words of a pascal case string.")]
        public void SplitWords_PascalCase_ShouldReturnWords()
        {
            "CommandLineArgument".SplitWords().Should().BeEquivalentTo("command", "line", "argument");
        }

        [Test(Description = "SplitWords should return the individual words of a string where the words are separated by spaces.")]
        public void SplitWords_Space_ShouldReturnWords()
        {
            "command line argument".SplitWords().Should().BeEquivalentTo("command", "line", "argument");
        }

        [Test(Description = "SplitWords should return the individual words of a string where the words are separated by underscores.")]
        public void SplitWords_Underscore_ShouldReturnWords()
        {
            "command_line_argument".SplitWords().Should().BeEquivalentTo("command", "line", "argument");
        }

        [Test(Description = "ToCamelCase should return an empty string when an empty string is given.")]
        public void ToCamelCase_EmptyString_ShouldReturnEmptyString()
        {
            String.Empty.ToCamelCase().Should().BeEmpty();
        }

        [Test(Description = "ToCamelCase should return null when null is given.")]
        public void ToCamelCase_Null_ShouldReturnNull()
        {
            ((String) null).ToCamelCase().Should().BeNull();
        }

        [Test(Description = "ToCamelCase should return an empty string when only spaces are given.")]
        public void ToCamelCase_OnlySpaces_ShouldReturnEmptyString()
        {
            "   ".ToCamelCase().Should().BeEmpty();
        }

        [Test(Description = "ToCamelCase should return the given string converted according to the camel case convention.")]
        public void ToCamelCase_String_ShouldReturnConvertedToCamelCase()
        {
            "commandLineArgument".ToCamelCase().Should().Be("commandLineArgument");
            "CommandLineArgument".ToCamelCase().Should().Be("commandLineArgument");
            "command_line_argument".ToCamelCase().Should().Be("commandLineArgument");
            "command line argument".ToCamelCase().Should().Be("commandLineArgument");
        }

        [Test(Description = "ToFirstLetterUpperCase should return an empty string when an empty string is given.")]
        public void ToFirstLetterUpperCase_EmptyString_ShouldReturnEmptyString()
        {
            "".ToFirstLetterUpperCase().Should().Be("");
        }

        [Test(Description = "ToFirstLetterUpperCase should return null when null is given.")]
        public void ToFirstLetterUpperCase_Null_ShouldReturnNull()
        {
            ((String) null).ToFirstLetterUpperCase().Should().Be(null);
        }

        [Test(Description = "ToFirstLetterUpperCase should return spaces when spaces are given.")]
        public void ToFirstLetterUpperCase_Spaces_ShouldReturnSpaces()
        {
            " ".ToFirstLetterUpperCase().Should().Be(" ");
            "  ".ToFirstLetterUpperCase().Should().Be("  ");
        }

        [Test(Description = "ToFirstLetterUpperCase should return the given text where the first letter is converted to upper case.")]
        public void ToFirstLetterUpperCase_Text_ShouldReturnFirstLetterInUpperCase()
        {
            "A".ToFirstLetterUpperCase().Should().Be("A");
            "a".ToFirstLetterUpperCase().Should().Be("A");
            "ab".ToFirstLetterUpperCase().Should().Be("Ab");
            "aB".ToFirstLetterUpperCase().Should().Be("AB");
        }

        [Test(Description = "WordWrap should return a sequence with a single empty string value when an empty string is given.")]
        public void WordWrap_EmptyString_ShouldReturnEmptyString()
        {
            "".WordWrap(20).Should().BeEquivalentTo(new String[] {""});
        }

        [Test(Description = "WordWrap should throw an exception when null is given.")]
        public void WordWrap_Null_ShouldThrowException()
        {
            ((String) null).Invoking(a => a.WordWrap(20))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "WordWrap should word wrap the given text and return the resulting lines.")]
        public void WordWrap_Text_ShouldWordWrapAndReturnLines()
        {
            "Lorem ipsum dolor sit amet, consetetur. sadipscing elitr, sed diam.".WordWrap(20).Should().BeEquivalentTo(new String[] {"Lorem ipsum dolor", "sit amet,", "consetetur.", "sadipscing elitr,", "sed diam."});
        }
    }
}