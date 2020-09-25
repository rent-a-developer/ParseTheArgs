using System;
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
        [Test]
        public void TestTokenize()
        {
            CommandLineArgumentsTokenizer.Tokenize(new String[] {"command"}).Should().BeEquivalentTo(new CommandToken("command"));
            CommandLineArgumentsTokenizer.Tokenize(new String[] {"command1", "command2"}).Should().BeEquivalentTo(new CommandToken("command1"), new CommandToken("command2"));

            CommandLineArgumentsTokenizer.Tokenize(new String[] {"-a"}).Should().BeEquivalentTo(new ArgumentToken("a"));
            CommandLineArgumentsTokenizer.Tokenize(new String[] { "--argument" }).Should().BeEquivalentTo(new ArgumentToken("argument"));

            CommandLineArgumentsTokenizer.Tokenize(new String[] {"-a", "-b"}).Should().BeEquivalentTo(new ArgumentToken("a"), new ArgumentToken("b"));
            CommandLineArgumentsTokenizer.Tokenize(new String[] {"--argumentA", "--argumentB"}).Should().BeEquivalentTo(new ArgumentToken("argumentA"), new ArgumentToken("argumentB"));

            CommandLineArgumentsTokenizer.Tokenize(new String[] {"--argument", "value"}).Should().BeEquivalentTo(new ArgumentToken("argument") { ArgumentValues = { "value" } });
            CommandLineArgumentsTokenizer.Tokenize(new String[] {"--argument", "value1", "value2"}).Should().BeEquivalentTo(new ArgumentToken("argument") { ArgumentValues = { "value1", "value2" } });
            CommandLineArgumentsTokenizer.Tokenize(new String[] {"--argumentA", "valueA", "--argumentB", "valueB"}).Should().BeEquivalentTo(new ArgumentToken("argumentA") { ArgumentValues = { "valueA" } }, new ArgumentToken("argumentB") { ArgumentValues = { "valueB" } });

            CommandLineArgumentsTokenizer.Tokenize(new String[] { "command", "-a" }).Should().BeEquivalentTo(new CommandToken("command"), new ArgumentToken("a"));
            CommandLineArgumentsTokenizer.Tokenize(new String[] { "command", "--argument" }).Should().BeEquivalentTo(new CommandToken("command"), new ArgumentToken("argument"));
        }

        [Test]
        public void TestTokenize_Exceptions()
        {
            Invoking(() => CommandLineArgumentsTokenizer.Tokenize(null).ToList()).Should().Throw<ArgumentNullException>();
        }
    }
}
