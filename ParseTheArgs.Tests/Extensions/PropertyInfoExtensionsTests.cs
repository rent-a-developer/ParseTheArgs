using System;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using ParseTheArgs.Extensions;

namespace ParseTheArgs.Tests.Extensions
{
    [TestFixture]
    public class PropertyInfoExtensionsTests
    {
        [Test(Description = "HasPublicSetter should throw an exception when the given property info is null.")]
        public void HasPublicSetter_PropertyInfoIsNull_ShouldThrowException()
        {
            ((PropertyInfo) null!).Invoking(a => a.HasPublicSetter()).Should().Throw<ArgumentNullException>();
        }

        [Test(Description = "HasPublicSetter should return false when a public setter is not present.")]
        public void HasPublicSetter_PublicSetterAbsent_ShouldReturnFalse()
        {
            typeof(Item).GetProperty("PropertyWithPrivateSetter").HasPublicSetter().Should().BeFalse();
            typeof(Item).GetProperty("PropertyWithInternalSetter").HasPublicSetter().Should().BeFalse();
            typeof(Item).GetProperty("PropertyWithProtectedSetter").HasPublicSetter().Should().BeFalse();
            typeof(Item).GetProperty("PropertyWithoutSetter").HasPublicSetter().Should().BeFalse();
        }

        [Test(Description = "HasPublicSetter should return true when a public setter is present.")]
        public void HasPublicSetter_PublicSetterPresent_ShouldReturnTrue()
        {
            typeof(Item).GetProperty("PropertyWithPublicSetter").HasPublicSetter().Should().BeTrue();
        }

        class Item
        {
            public String? PropertyWithInternalSetter { get; internal set; }
            public String? PropertyWithoutSetter { get; }
            public String? PropertyWithPrivateSetter { get; private set; }
            public String? PropertyWithProtectedSetter { get; protected set; }
            public String? PropertyWithPublicSetter { get; set; }
        }
    }
}