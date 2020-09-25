using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using ParseTheArgs.Extensions;

namespace ParseTheArgs.Tests.Extensions
{
    [TestFixture]
    public class IEnumerableExtensionsTests
    {
        [Test]
        public void TestHasSameElementsThan()
        {
            new List<Int32> { }.HasSameElementsThan(new List<Int32> { }).Should().BeTrue();

            new List<Int32> { 1 }.HasSameElementsThan(new List<Int32> { 1 }).Should().BeTrue();
            new List<Int32> { 1, 2 }.HasSameElementsThan(new List<Int32> { 1, 2 }).Should().BeTrue();

            new List<Int32?> { null }.HasSameElementsThan(new List<Int32?> { null }).Should().BeTrue();
            new List<Int32?> { 1, null }.HasSameElementsThan(new List<Int32?> { 1, null }).Should().BeTrue();

            new List<Int32> { 1 }.HasSameElementsThan(new List<Int32> { }).Should().BeFalse();
            new List<Int32> { }.HasSameElementsThan(new List<Int32> { 1 }).Should().BeFalse();

            new List<Int32> { 1 }.HasSameElementsThan(new List<Int32> { 2 }).Should().BeFalse();
            new List<Int32> { 2 }.HasSameElementsThan(new List<Int32> { 1 }).Should().BeFalse();

            new List<Int32> { 1, 2 }.HasSameElementsThan(new List<Int32> { 1, 2, 3 }).Should().BeFalse();
            new List<Int32> { 1, 2, 3 }.HasSameElementsThan(new List<Int32> { 1, 2 }).Should().BeFalse();

            new List<Int32?> { null }.HasSameElementsThan(new List<Int32?> { 1 }).Should().BeFalse();
            new List<Int32?> { 1 }.HasSameElementsThan(new List<Int32?> { null }).Should().BeFalse();
            new List<Int32?> { 1, null }.HasSameElementsThan(new List<Int32?> { 1, 1 }).Should().BeFalse();
            new List<Int32?> { 1, 1 }.HasSameElementsThan(new List<Int32?> { 1, null }).Should().BeFalse();

            new List<Int32>().Invoking(a => a.HasSameElementsThan(null)).Should().Throw<ArgumentNullException>();

            var listA = new List<Int32> { 1, 2, 3 };
            listA.HasSameElementsThan(listA).Should().BeTrue();

            ((List<Int32>)null).Invoking(a => a.HasSameElementsThan(new List<Int32>())).Should().Throw<ArgumentNullException>();

            new List<Int32>().HasSameElementsThan(new NullEnumerable()).Should().BeFalse();
            new NullEnumerable().HasSameElementsThan(new List<Int32>()).Should().BeFalse();
            new NullEnumerable().HasSameElementsThan(new NullEnumerable()).Should().BeTrue();
        }
    }

    public class NullEnumerable : IEnumerable<Int32>
    {
        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<Int32> GetEnumerator()
        {
            return null;
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}