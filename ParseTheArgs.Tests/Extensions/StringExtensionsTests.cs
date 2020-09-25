using System;
using FluentAssertions;
using NUnit.Framework;
using ParseTheArgs.Extensions;

namespace ParseTheArgs.Tests.Extensions
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void TestSplitWords()
        {
            "commandLineArgument".SplitWords().Should().BeEquivalentTo("command", "line", "argument");
            "CommandLineArgument".SplitWords().Should().BeEquivalentTo("command", "line", "argument");
            "command_line_argument".SplitWords().Should().BeEquivalentTo("command", "line", "argument");
            "command line argument".SplitWords().Should().BeEquivalentTo("command", "line", "argument");
            "".SplitWords().Should().BeEmpty();
            ((String) null).SplitWords().Should().BeEmpty();
        }

        [Test]
        public void TestToCamelCase()
        {
            "commandLineArgument".ToCamelCase().Should().Be("commandLineArgument");
            "CommandLineArgument".ToCamelCase().Should().Be("commandLineArgument");
            "command_line_argument".ToCamelCase().Should().Be("commandLineArgument");
            "command line argument".ToCamelCase().Should().Be("commandLineArgument");
            ((String) null).ToCamelCase().Should().Be(null);
            "".ToCamelCase().Should().Be("");
            " ".ToCamelCase().Should().Be("");
        }

        [Test]
        public void TestToFirstLetterUpperCase()
        {
            "A".ToFirstLetterUpperCase().Should().Be("A");
            "a".ToFirstLetterUpperCase().Should().Be("A");
            "ab".ToFirstLetterUpperCase().Should().Be("Ab");
            "aB".ToFirstLetterUpperCase().Should().Be("AB");
            ((String) null).ToFirstLetterUpperCase().Should().Be(null);
            "".ToFirstLetterUpperCase().Should().Be("");
            " ".ToFirstLetterUpperCase().Should().Be(" ");
        }

        [Test]
        public void TestWordWrap()
        {
            "Lorem ipsum dolor sit amet, consetetur. sadipscing elitr, sed diam.".WordWrap(20).Should().BeEquivalentTo(new String[] { "Lorem ipsum dolor", "sit amet,", "consetetur.", "sadipscing elitr,", "sed diam." });
            ((String) null).WordWrap(20).Should().BeEquivalentTo(new String[] { null });
            "".WordWrap(20).Should().BeEquivalentTo(new String[] { "" });
        }
    }
}
