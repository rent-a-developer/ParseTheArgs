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
        [Test]
        public void TestHasPublicSetter()
        {
            typeof(Item).GetProperty("PropertyWithPublicSetter").HasPublicSetter().Should().BeTrue();
            typeof(Item).GetProperty("PropertyWithPrivateSetter").HasPublicSetter().Should().BeFalse();
            typeof(Item).GetProperty("PropertyWithInternalSetter").HasPublicSetter().Should().BeFalse();
            typeof(Item).GetProperty("PropertyWithProtectedSetter").HasPublicSetter().Should().BeFalse();
            typeof(Item).GetProperty("PropertyWithoutSetter").HasPublicSetter().Should().BeFalse();
        }

        [Test]
        public void TestHasPublicSetter_Exceptions()
        {
            ((PropertyInfo) null).Invoking(a => a.HasPublicSetter()).Should().Throw<ArgumentNullException>();
        }

        class Item
        {
            public String PropertyWithPublicSetter { get; set; }
            public String PropertyWithPrivateSetter { get; private set; }
            public String PropertyWithInternalSetter { get; internal set; }
            public String PropertyWithProtectedSetter { get; protected set; }
            public String PropertyWithoutSetter { get;  }
        }
    }
}