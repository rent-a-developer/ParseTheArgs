using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using ParseTheArgs.Tokens;
using static FluentAssertions.FluentActions;

namespace ParseTheArgs.Tests.Tokens
{
    [TestFixture]
    public class CommandLineArgumentsTokenizerTests
    {
        [Test(Description = "Tokenize should return no tokens when there are no arguments.")]
        public void Tokenize_NoArguments_ShouldReturnNoTokens()
        {
            CommandLineArgumentsTokenizer.Tokenize(new String[] {}).Should().BeEquivalentTo(new List<Token>());
        }

        [Test(Description = "Tokenize should return tokens that represent each present argument.")]
        public void Tokenize_Arguments_ShouldReturnTokensRepresentingTheGivenArguments()
        {
            CommandLineArgumentsTokenizer.Tokenize(new String[] {"command"}).Should().BeEquivalentTo(new CommandToken("command"));
            CommandLineArgumentsTokenizer.Tokenize(new String[] {"command1", "command2"}).Should().BeEquivalentTo(new CommandToken("command1"), new CommandToken("command2"));

            CommandLineArgumentsTokenizer.Tokenize(new String[] {"-a"}).Should().BeEquivalentTo(new OptionToken("a"));
            CommandLineArgumentsTokenizer.Tokenize(new String[] { "--option" }).Should().BeEquivalentTo(new OptionToken("option"));

            CommandLineArgumentsTokenizer.Tokenize(new String[] {"-a", "-b"}).Should().BeEquivalentTo(new OptionToken("a"), new OptionToken("b"));
            CommandLineArgumentsTokenizer.Tokenize(new String[] {"--optionA", "--optionB"}).Should().BeEquivalentTo(new OptionToken("optionA"), new OptionToken("optionB"));

            CommandLineArgumentsTokenizer.Tokenize(new String[] {"--option", "value"}).Should().BeEquivalentTo(new OptionToken("option") { OptionValues = { "value" } });
            CommandLineArgumentsTokenizer.Tokenize(new String[] {"--option", "value1", "value2"}).Should().BeEquivalentTo(new OptionToken("option") { OptionValues = { "value1", "value2" } });
            CommandLineArgumentsTokenizer.Tokenize(new String[] {"--optionA", "valueA", "--optionB", "valueB"}).Should().BeEquivalentTo(new OptionToken("optionA") { OptionValues = { "valueA" } }, new OptionToken("optionB") { OptionValues = { "valueB" } });

            CommandLineArgumentsTokenizer.Tokenize(new String[] { "command", "-a" }).Should().BeEquivalentTo(new CommandToken("command"), new OptionToken("a"));
            CommandLineArgumentsTokenizer.Tokenize(new String[] { "command", "--option" }).Should().BeEquivalentTo(new CommandToken("command"), new OptionToken("option"));
        }

        [Test(Description = "Tokenize should throw an exception when the given arguments array is null.")]
        public void Tokenize_Null_ShouldThrowException()
        {
            Invoking(() => CommandLineArgumentsTokenizer.Tokenize(null).ToList()).Should().Throw<ArgumentNullException>();
        }
    }
}
