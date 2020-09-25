using System;
using FluentAssertions;
using NUnit.Framework;

namespace ParseTheArgs.Tests.CustomArgument
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void TestParse()
        {
            var parser = new Parser();

            var commandSetup = parser.Setup.Command<CustomArguments>().Name("custom");
            commandSetup.Argument(a => a.CustomValue).Name("customValue");

            var result = parser.Parse(new String[] { "custom", "--customValue", "test value" });

            result.CommandArguments.Should().BeOfType<CustomArguments>();

            var commandArguments = (CustomArguments)result.CommandArguments;
            commandArguments.CustomValue.Should().Be(new CustomValue("test value"));
        }
    }
}