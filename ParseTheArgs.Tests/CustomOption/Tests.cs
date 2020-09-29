using System;
using FluentAssertions;
using NUnit.Framework;

namespace ParseTheArgs.Tests.CustomOption
{
    [TestFixture]
    public class Tests
    {
        [Test(Description = "Parse should parse our custom value option correctly.")]
        public void TestParse()
        {
            var parser = new Parser();

            var commandSetup = parser.Setup
                .Command<CustomOptions>()
                .Name("custom");
            
            commandSetup
                .Option(a => a.CustomValue)
                .Name("customValue");

            var result = parser.Parse(new String[] { "custom", "--customValue", "test value" });

            result.CommandOptions.Should().BeOfType<CustomOptions>();

            var commandOptions = (CustomOptions)result.CommandOptions;
            
            commandOptions.CustomValue.Should().BeEquivalentTo(new CustomValue("test value"));
        }
    }
}