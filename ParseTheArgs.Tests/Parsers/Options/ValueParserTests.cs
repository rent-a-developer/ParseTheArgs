using System;
using System.Globalization;
using FluentAssertions;
using NUnit.Framework;
using ParseTheArgs.Parsers.Options;

namespace ParseTheArgs.Tests.Parsers.Options
{
    [TestFixture]
    public class ValueParserTests
    {
        [Test(Description = "TryParseDateTime should parse a valid string into a DateTime value.")]
        public void TryParseDateTime_ValidValue_ShouldParseValue()
        {
            var parser = new ValueParser();

            parser
                .TryParseDateTime("2020-12-31 23:59:59", null, new CultureInfo("en-US"), DateTimeStyles.None, out DateTime result)
                .Should()
                .BeTrue();

            result.Should().Be(new DateTime(2020, 12, 31, 23, 59, 59));
        }

        [Test(Description = "TryParseDateTime should parse a valid string in the specified custom format into a DateTime value.")]
        public void TryParseDateTime_ValidValueCustomFormat_ShouldParseValue()
        {
            var parser = new ValueParser();

            parser
                .TryParseDateTime("Sun 15 Jun 2008 8:30 AM", "ddd dd MMM yyyy h:mm tt", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime result)
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
                .TryParseDateTime("23:59:59", null, new CultureInfo("en-US"), DateTimeStyles.NoCurrentDateDefault, out DateTime result)
                .Should()
                .BeTrue();

            result.Should().Be(new DateTime(1, 1, 1, 23, 59, 59));
        }

        [Test(Description = "TryParseDateTime should return false when the given value is invalid.")]
        public void TryParseDateTime_InvalidValue_ShouldReturnFalse()
        {
            var parser = new ValueParser();

            parser
                .TryParseDateTime("NotADateTime", null, new CultureInfo("en-US"), DateTimeStyles.None, out DateTime result)
                .Should()
                .BeFalse();

            result.Should().Be(default);
        }

        [Test(Description = "TryParseDateTime should return false when the given value is not in the specified custom format.")]
        public void TryParseDateTime_InvalidValueCustomFormat_ShouldReturnFalse()
        {
            var parser = new ValueParser();

            parser
                .TryParseDateTime("2020-12-31 23:59:59", "ddd dd MMM yyyy h:mm tt", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime result)
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
                .Invoking(a => a.TryParseDateTime(null, null, new CultureInfo("en-US"), DateTimeStyles.None, out DateTime result))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "TryParseDateTime throw an exception when the given value is an empty string.")]
        public void TryParseDateTime_ValueIsEmptyOrWhitespace_ShouldThrowException()
        {
            var parser = new ValueParser();

            parser
                .Invoking(a => a.TryParseDateTime("", null, new CultureInfo("en-US"), DateTimeStyles.None, out DateTime result))
                .Should()
                .Throw<ArgumentException>();

            parser
                .Invoking(a => a.TryParseDateTime(" ", null, new CultureInfo("en-US"), DateTimeStyles.None, out DateTime result))
                .Should()
                .Throw<ArgumentException>();
        }

        [Test(Description = "TryParseDecimal should parse a valid string into a Decimal value.")]
        public void TryParseDecimal_ValidValue_ShouldParseValue()
        {
            var parser = new ValueParser();

            parser
                .TryParseDecimal("1.23", NumberStyles.Any, new CultureInfo("en-US"), out Decimal result)
                .Should()
                .BeTrue();

            result.Should().Be(1.23M);
        }

        [Test(Description = "TryParseDecimal should parse a valid string in the format of the specified custom format provider into a Decimal value.")]
        public void TryParseDecimal_ValidValueCustomFormatProvider_ShouldParseValue()
        {
            var parser = new ValueParser();

            parser
                .TryParseDecimal("1,23", NumberStyles.Any, new CultureInfo("de-DE"), out Decimal result)
                .Should()
                .BeTrue();

            result.Should().Be(1.23M);
        }

        [Test(Description = "TryParseDecimal should parse a valid string accordingly to the specified number styles into a Decimal value.")]
        public void TryParseDecimal_ValidValueCustomNumberStyles_ShouldParseValue()
        {
            var parser = new ValueParser();

            parser
                .TryParseDecimal("(1)", NumberStyles.AllowParentheses, new CultureInfo("en-US"), out Decimal result)
                .Should()
                .BeTrue();

            result.Should().Be(-1M);
        }

        [Test(Description = "TryParseDecimal should return false when the given value is invalid.")]
        public void TryParseDecimal_InvalidValue_ShouldReturnFalse()
        {
            var parser = new ValueParser();

            parser
                .TryParseDecimal("NotANumber", NumberStyles.AllowParentheses, new CultureInfo("en-US"), out Decimal result)
                .Should()
                .BeFalse();

            result.Should().Be(default);
        }

        [Test(Description = "TryParseDecimal should return false when the given value is not in the format of the specified custom format provider.")]
        public void TryParseDecimal_InvalidValueCustomFormatProvider_ShouldReturnFalse()
        {
            var parser = new ValueParser();

            parser
                .TryParseDecimal("1,2.3", NumberStyles.Any, new CultureInfo("de-DE"), out Decimal result)
                .Should()
                .BeFalse();

            result.Should().Be(default);
        }

        [Test(Description = "TryParseDecimal throw an exception when the given value is null.")]
        public void TryParseDecimal_ValueIsNull_ShouldThrowException()
        {
            var parser = new ValueParser();

            parser
                .Invoking(a => a.TryParseDecimal(null, NumberStyles.Any, new CultureInfo("en-US"), out Decimal result))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "TryParseDecimal throw an exception when the given value is an empty string.")]
        public void TryParseDecimal_ValueIsEmptyOrWhitespace_ShouldThrowException()
        {
            var parser = new ValueParser();

            parser
                .Invoking(a => a.TryParseDecimal("", NumberStyles.Any, new CultureInfo("en-US"), out Decimal result))
                .Should()
                .Throw<ArgumentException>();

            parser
                .Invoking(a => a.TryParseDecimal(" ", NumberStyles.Any, new CultureInfo("en-US"), out Decimal result))
                .Should()
                .Throw<ArgumentException>();
        }
    }
}
