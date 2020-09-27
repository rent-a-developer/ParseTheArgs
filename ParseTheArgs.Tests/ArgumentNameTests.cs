using FluentAssertions;
using NUnit.Framework;

namespace ParseTheArgs.Tests
{
    [TestFixture]
    public class ArgumentNameTests
    {
        [Test(Description = "Name should return the value that was assigned to it.")]
        public void Name_Assigned_ShouldReturnAssignedValue()
        {
            var name = new ArgumentName("argumentA");
            name.Name.Should().Be("argumentA");
        }

        [Test(Description = "ShortName should return the value that was assigned to it.")]
        public void ShortName_Assigned_ShouldReturnAssignedValue()
        {
            var name = new ArgumentName("argumentA", 'a');
            name.ShortName.Should().Be('a');
        }

        [Test(Description = "EqualsNameOrShortName should return true when the given other argument name has the same name or short name.")]
        public void EqualsNameOrShortName_NameOrShortNameDoesEqual_ShouldReturnTrue()
        {
            new ArgumentName("argumentA").EqualsNameOrShortName("argumentA").Should().BeTrue();
            new ArgumentName("argumentA", 'a').EqualsNameOrShortName("argumentA").Should().BeTrue();
            new ArgumentName("argumentA", 'a').EqualsNameOrShortName("a").Should().BeTrue();
        }

        [Test(Description = "EqualsNameOrShortName should return false when the given other argument name neither has the same name nor the same short name.")]
        public void EqualsNameOrShortName_NameOrShortNameDoesNotEqual_ShouldReturnTrue()
        {
            new ArgumentName("argumentA").EqualsNameOrShortName("argumentB").Should().BeFalse();
            new ArgumentName("argumentA", 'a').EqualsNameOrShortName("argumentB").Should().BeFalse();
            new ArgumentName("argumentA", 'a').EqualsNameOrShortName("b").Should().BeFalse();
        }

        [Test(Description = "ToString should return the name if only the name is set.")]
        public void ToString_OnlyNameSet_ShouldReturnName()
        {
            new ArgumentName("argumentA").ToString().Should().Be("--argumentA");
        }

        [Test(Description = "ToString should return the name and the short name when both are set.")]
        public void ToString_NameAndShortNameSet_ShouldReturnName()
        {
            new ArgumentName("argumentA", 'a').ToString().Should().Be("-a (--argumentA)");
        }
    }
}
