using System;
using System.Globalization;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;
using ParseTheArgs.Parsers.Options;

namespace ParseTheArgs.Tests.Parsers.Options
{
    [TestFixture]
    public class ValueParserTests
    {
        [SetUp]
        public void SetUp()
        {
            // We fix the current culture to en-US so that parsing of values is done with a known format provider.
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }

        [Test(Description = "TryParseDateTime should parse a valid string into a DateTime value.")]
        public void TryParseDateTime_ValidValue_ShouldParseValue()
        {
            var parser = new ValueParser();

            parser
                .TryParseDateTime("2020-12-31 23:59:59", null, CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime result)
                .Should()
                .BeTrue();

            result.Should().Be(new DateTime(2020, 12, 31, 23, 59, 59));
        }

        [Test(Description = "TryParseDateTime should parse a valid string in the specified custom format into a DateTime value.")]
        public void TryParseDateTime_ValidValueCustomFormat_ShouldParseValue()
        {
            var parser = new ValueParser();

            parser
                .TryParseDateTime("Sun 15 Jun 2008 8:30 AM", "ddd dd MMM yyyy h:mm tt", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime result)
                .Should()
                .BeTrue();

            result.Should().Be(new DateTime(2008, 6, 15, 8, 30, 0));
        }

        [Test(Description = "TryParseDateTime should parse a valid string in the format of the specified custom format provider into a DateTime value.")]
        public void TryParseDateTime_ValidValueCustomFormatProvider_ShouldParseValue()
        {
            var parser = new ValueParser();

            parser
                .TryParseDateTime("31.12.2020 23:59:59", null, new CultureInfo("de-DE"), DateTimeStyles.None, out DateTime result)
                .Should()
                .BeTrue();

            result.Should().Be(new DateTime(2020, 12, 31, 23, 59, 59));
        }

        [Test(Description = "TryParseDateTime should parse a valid string accordingly to the specified date time styles into a DateTime value.")]
        public void TryParseDateTime_ValidValueCustomDateTimeStyles_ShouldParseValue()
        {
            var parser = new ValueParser();

            parser
                .TryParseDateTime("23:59:59", null, CultureInfo.CurrentCulture, DateTimeStyles.NoCurrentDateDefault, out DateTime result)
                .Should()
                .BeTrue();

            result.Should().Be(new DateTime(1, 1, 1, 23, 59, 59));
        }

        [Test(Description = "TryParseDateTime should return false when the given value is invalid.")]
        public void TryParseDateTime_InvalidValue_ShouldReturnFalse()
        {
            var parser = new ValueParser();

            parser
                .TryParseDateTime("NotADateTime", null, CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime result)
                .Should()
                .BeFalse();

            result.Should().Be(default);
        }

        [Test(Description = "TryParseDateTime should return false when the given value is not in the specified custom format.")]
        public void TryParseDateTime_InvalidValueCustomFormat_ShouldReturnFalse()
        {
            var parser = new ValueParser();

            parser
                .TryParseDateTime("2020-12-31 23:59:59", "ddd dd MMM yyyy h:mm tt", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime result)
                .Should()
                .BeFalse();

            result.Should().Be(default);
        }

        [Test(Description = "TryParseDateTime should return false when the given value is not in the format of the specified custom format provider.")]
        public void TryParseDateTime_InvalidValueCustomFormatProvider_ShouldReturnFalse()
        {
            var parser = new ValueParser();

            parser
                .TryParseDateTime("31.12.2020 23:59:59", null, new CultureInfo("en-CA"), DateTimeStyles.None, out DateTime result)
                .Should()
                .BeFalse();

            result.Should().Be(default);
        }

        [Test(Description = "TryParseDateTime throw an exception when the given value is null.")]
        public void TryParseDateTime_ValueIsNull_ShouldThrowException()
        {
            var parser = new ValueParser();

            parser
                .Invoking(a => a.TryParseDateTime(null, null, CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime result))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "TryParseDateTime throw an exception when the given value is an empty string.")]
        public void TryParseDateTime_ValueIsEmptyOrWhitespace_ShouldThrowException()
        {
            var parser = new ValueParser();

            parser
                .Invoking(a => a.TryParseDateTime("", null, CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime result))
                .Should()
                .Throw<ArgumentException>();

            parser
                .Invoking(a => a.TryParseDateTime(" ", null, CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime result))
                .Should()
                .Throw<ArgumentException>();
        }
    }
}
