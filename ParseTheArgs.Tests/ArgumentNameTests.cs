using System;
using FluentAssertions;
using NUnit.Framework;

namespace ParseTheArgs.Tests
{
    [TestFixture]
    public class ArgumentNameTests
    {
        [Test]
        public void TestConstructors()
        {
            var name = new ArgumentName();
            name.Name.Should().BeEmpty();
            name.ShortName.Should().BeNull();

            name = new ArgumentName("argumentA");
            name.Name.Should().Be("argumentA");
            name.ShortName.Should().BeNull();

            name = new ArgumentName("argumentA", 'a');
            name.Name.Should().Be("argumentA");
            name.ShortName.Should().Be('a');
        }

        [Test]
        public void TestEquals()
        {
            new ArgumentName("argumentA").Equals(new ArgumentName("argumentA")).Should().BeTrue();
            new ArgumentName("argumentA", 'a').Equals(new ArgumentName("argumentA", 'a')).Should().BeTrue();

            var name = new ArgumentName();
            name.Equals(name).Should().BeTrue();

            new ArgumentName("argumentA").Equals(new ArgumentName("argumentB")).Should().BeFalse();
            new ArgumentName("argumentA", 'a').Equals(new ArgumentName("argumentA", 'b')).Should().BeFalse();
            new ArgumentName("argumentA").Equals(Guid.NewGuid()).Should().BeFalse();
            new ArgumentName("argumentA").Equals(null).Should().BeFalse();
            new ArgumentName("argumentA", 'a').Equals(null).Should().BeFalse();
        }

        [Test]
        public void TestEqualsNameOrShortName()
        {
            new ArgumentName("argumentA").EqualsNameOrShortName("argumentA").Should().BeTrue();
            new ArgumentName("argumentA", 'a').EqualsNameOrShortName("argumentA").Should().BeTrue();
            new ArgumentName("argumentA", 'a').EqualsNameOrShortName("a").Should().BeTrue();

            new ArgumentName("argumentA").EqualsNameOrShortName("argumentB").Should().BeFalse();
            new ArgumentName("argumentA", 'a').EqualsNameOrShortName("argumentB").Should().BeFalse();
            new ArgumentName("argumentA", 'a').EqualsNameOrShortName("b").Should().BeFalse();
        }

        [Test]
        public void TestToString()
        {
            new ArgumentName("argumentA").ToString().Should().Be("--argumentA");
            new ArgumentName("argumentA", 'a').ToString().Should().Be("-a (--argumentA)");
        }
    }
}
