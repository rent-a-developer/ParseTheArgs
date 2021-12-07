using System;
using System.Globalization;
using FluentAssertions;
using NUnit.Framework;
using ParseTheArgs.Parsers.Options;
using ParseTheArgs.Tests.TestData;

namespace ParseTheArgs.Tests.Parsers.Options
{
    [TestFixture]
    public class ValueParserTests
    {
        [Test(Description = "TryParseDateTime should return false when the given value is invalid.")]
        public void TryParseDateTime_InvalidValue_ShouldReturnFalse()
        {
            var parser = new ValueParser();

            parser
                .TryParseDateTime("NotADateTime", null, new CultureInfo("en-US"), DateTimeStyles.None, out var result)
                .Should()
                .BeFalse();

            result.Should().Be(default);
        }

        [Test(Description = "TryParseDateTime should return false when the given value is not in the specified custom format.")]
        public void TryParseDateTime_InvalidValueCustomFormat_ShouldReturnFalse()
        {
            var parser = new ValueParser();

            parser
                .TryParseDateTime("2020-12-31 23:59:59", "ddd dd MMM yyyy h:mm tt", new CultureInfo("en-US"), DateTimeStyles.None, out var result)
                .Should()
                .BeFalse();

            result.Should().Be(default);
        }

        [Test(Description = "TryParseDateTime should return false when the given value is not in the format of the specified custom format provider.")]
        public void TryParseDateTime_InvalidValueCustomFormatProvider_ShouldReturnFalse()
        {
            var parser = new ValueParser();

            parser
                .TryParseDateTime("31.12.2020 23:59:59", null, new CultureInfo("en-CA"), DateTimeStyles.None, out var result)
                .Should()
                .BeFalse();

            result.Should().Be(default);
        }

        [Test(Description = "TryParseDateTime should parse a valid string into a DateTime value.")]
        public void TryParseDateTime_ValidValue_ShouldParseValue()
        {
            var parser = new ValueParser();

            parser
                .TryParseDateTime("2020-12-31 23:59:59", null, new CultureInfo("en-US"), DateTimeStyles.None, out var result)
                .Should()
                .BeTrue();

            result.Should().Be(new DateTime(2020, 12, 31, 23, 59, 59));
        }

        [Test(Description = "TryParseDateTime should parse a valid string accordingly to the specified date time styles into a DateTime value.")]
        public void TryParseDateTime_ValidValueCustomDateTimeStyles_ShouldParseValue()
        {
            var parser = new ValueParser();

            parser
                .TryParseDateTime("23:59:59", null, new CultureInfo("en-US"), DateTimeStyles.NoCurrentDateDefault, out var result)
                .Should()
                .BeTrue();

            result.Should().Be(new DateTime(1, 1, 1, 23, 59, 59));
        }

        [Test(Description = "TryParseDateTime should parse a valid string in the specified custom format into a DateTime value.")]
        public void TryParseDateTime_ValidValueCustomFormat_ShouldParseValue()
        {
            var parser = new ValueParser();

            parser
                .TryParseDateTime("Sun 15 Jun 2008 8:30 AM", "ddd dd MMM yyyy h:mm tt", new CultureInfo("en-US"), DateTimeStyles.None, out var result)
                .Should()
                .BeTrue();

            result.Should().Be(new DateTime(2008, 6, 15, 8, 30, 0));
        }

        [Test(Description = "TryParseDateTime should parse a valid string in the format of the specified custom format provider into a DateTime value.")]
        public void TryParseDateTime_ValidValueCustomFormatProvider_ShouldParseValue()
        {
            var parser = new ValueParser();

            parser
                .TryParseDateTime("31.12.2020 23:59:59", null, new CultureInfo("de-DE"), DateTimeStyles.None, out var result)
                .Should()
                .BeTrue();

            result.Should().Be(new DateTime(2020, 12, 31, 23, 59, 59));
        }

        [Test(Description = "TryParseDateTime throw an exception when the given value is an empty string.")]
        public void TryParseDateTime_ValueIsEmptyOrWhitespace_ShouldThrowException()
        {
            var parser = new ValueParser();

            parser
                .Invoking(a => a.TryParseDateTime("", null, new CultureInfo("en-US"), DateTimeStyles.None, out var result))
                .Should()
                .Throw<ArgumentException>();

            parser
                .Invoking(a => a.TryParseDateTime(" ", null, new CultureInfo("en-US"), DateTimeStyles.None, out var result))
                .Should()
                .Throw<ArgumentException>();
        }

        [Test(Description = "TryParseDateTime throw an exception when the given value is null.")]
        public void TryParseDateTime_ValueIsNull_ShouldThrowException()
        {
            var parser = new ValueParser();

            parser
                .Invoking(a => a.TryParseDateTime(null!, null, new CultureInfo("en-US"), DateTimeStyles.None, out var result))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "TryParseDecimal should return false when the given value is invalid.")]
        public void TryParseDecimal_InvalidValue_ShouldReturnFalse()
        {
            var parser = new ValueParser();

            parser
                .TryParseDecimal("NotANumber", NumberStyles.AllowParentheses, new CultureInfo("en-US"), out var result)
                .Should()
                .BeFalse();

            result.Should().Be(default);
        }

        [Test(Description = "TryParseDecimal should return false when the given value is not in the format of the specified custom format provider.")]
        public void TryParseDecimal_InvalidValueCustomFormatProvider_ShouldReturnFalse()
        {
            var parser = new ValueParser();

            parser
                .TryParseDecimal("1,2.3", NumberStyles.Any, new CultureInfo("de-DE"), out var result)
                .Should()
                .BeFalse();

            result.Should().Be(default);
        }

        [Test(Description = "TryParseDecimal should parse a valid string into a Decimal value.")]
        public void TryParseDecimal_ValidValue_ShouldParseValue()
        {
            var parser = new ValueParser();

            parser
                .TryParseDecimal("1.23", NumberStyles.Any, new CultureInfo("en-US"), out var result)
                .Should()
                .BeTrue();

            result.Should().Be(1.23M);
        }

        [Test(Description = "TryParseDecimal should parse a valid string in the format of the specified custom format provider into a Decimal value.")]
        public void TryParseDecimal_ValidValueCustomFormatProvider_ShouldParseValue()
        {
            var parser = new ValueParser();

            parser
                .TryParseDecimal("1,23", NumberStyles.Any, new CultureInfo("de-DE"), out var result)
                .Should()
                .BeTrue();

            result.Should().Be(1.23M);
        }

        [Test(Description = "TryParseDecimal should parse a valid string accordingly to the specified number styles into a Decimal value.")]
        public void TryParseDecimal_ValidValueCustomNumberStyles_ShouldParseValue()
        {
            var parser = new ValueParser();

            parser
                .TryParseDecimal("(1)", NumberStyles.AllowParentheses, new CultureInfo("en-US"), out var result)
                .Should()
                .BeTrue();

            result.Should().Be(-1M);
        }

        [Test(Description = "TryParseDecimal throw an exception when the given value is an empty string.")]
        public void TryParseDecimal_ValueIsEmptyOrWhitespace_ShouldThrowException()
        {
            var parser = new ValueParser();

            parser
                .Invoking(a => a.TryParseDecimal("", NumberStyles.Any, new CultureInfo("en-US"), out var result))
                .Should()
                .Throw<ArgumentException>();

            parser
                .Invoking(a => a.TryParseDecimal(" ", NumberStyles.Any, new CultureInfo("en-US"), out var result))
                .Should()
                .Throw<ArgumentException>();
        }

        [Test(Description = "TryParseDecimal throw an exception when the given value is null.")]
        public void TryParseDecimal_ValueIsNull_ShouldThrowException()
        {
            var parser = new ValueParser();

            parser
                .Invoking(a => a.TryParseDecimal(null!, NumberStyles.Any, new CultureInfo("en-US"), out var result))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "TryParseEnum should return false when the given value is invalid.")]
        public void TryParseEnum_InvalidValue_ShouldReturnFalse()
        {
            var parser = new ValueParser();

            parser
                .TryParseEnum<LogLevel>("NonExistentLogLevel", out var result)
                .Should()
                .BeFalse();

            result.Should().Be(LogLevel.Trace);
        }

        [Test(Description = "TryParseEnum should parse a valid string into an Enum value ignoring casing.")]
        public void TryParseEnum_ValidValue_ShouldIgnoreCasing()
        {
            var parser = new ValueParser();

            parser
                .TryParseEnum<LogLevel>("debug", out var result)
                .Should()
                .BeTrue();

            result.Should().Be(LogLevel.Debug);
        }

        [Test(Description = "TryParseEnum should parse a valid string into an Enum value.")]
        public void TryParseEnum_ValidValue_ShouldParseValue()
        {
            var parser = new ValueParser();

            parser
                .TryParseEnum<LogLevel>("Debug", out var result)
                .Should()
                .BeTrue();

            result.Should().Be(LogLevel.Debug);
        }

        [Test(Description = "TryParseEnum throw an exception when the given value is an empty string.")]
        public void TryParseEnum_ValueIsEmptyOrWhitespace_ShouldThrowException()
        {
            var parser = new ValueParser();

            parser
                .Invoking(a => a.TryParseEnum<LogLevel>("", out var result))
                .Should()
                .Throw<ArgumentException>();

            parser
                .Invoking(a => a.TryParseEnum<LogLevel>(" ", out var result))
                .Should()
                .Throw<ArgumentException>();
        }

        [Test(Description = "TryParseEnum throw an exception when the given value is null.")]
        public void TryParseEnum_ValueIsNull_ShouldThrowException()
        {
            var parser = new ValueParser();

            parser
                .Invoking(a => a.TryParseEnum<LogLevel>(null!, out var result))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "TryParseGuid should return false when the given value is invalid.")]
        public void TryParseGuid_InvalidValue_ShouldReturnFalse()
        {
            var parser = new ValueParser();

            parser
                .TryParseGuid("NotAGuid", null, out var result)
                .Should()
                .BeFalse();

            result.Should().Be(default(Guid));
        }

        [Test(Description = "TryParseGuid should return false when the given value is not in the specified custom format.")]
        public void TryParseGuid_InvalidValueCustomFormat_ShouldReturnFalse()
        {
            var parser = new ValueParser();

            parser
                .TryParseGuid("{0xd00a1b24,0x697b,0x453e,{0x92,0x1c,0xf1,0x0e,0xce,0xbb,0xf2,0xc5}}", "N", out var result)
                .Should()
                .BeFalse();

            result.Should().Be(default(Guid));
        }

        [Test(Description = "TryParseGuid should parse a valid string into a Guid value.")]
        public void TryParseGuid_ValidValue_ShouldParseValue()
        {
            var parser = new ValueParser();

            parser
                .TryParseGuid("d00a1b24-697b-453e-921c-f10ecebbf2c5", null, out var result)
                .Should()
                .BeTrue();

            result.Should().Be(new Guid("d00a1b24-697b-453e-921c-f10ecebbf2c5"));
        }

        [Test(Description = "TryParseGuid should parse a valid string in the specified custom format into a Guid value.")]
        public void TryParseGuid_ValidValueCustomFormat_ShouldParseValue()
        {
            var parser = new ValueParser();

            parser
                .TryParseGuid("{0xd00a1b24,0x697b,0x453e,{0x92,0x1c,0xf1,0x0e,0xce,0xbb,0xf2,0xc5}}", "X", out var result)
                .Should()
                .BeTrue();

            result.Should().Be(new Guid("d00a1b24-697b-453e-921c-f10ecebbf2c5"));
        }

        [Test(Description = "TryParseGuid throw an exception when the given value is an empty string.")]
        public void TryParseGuid_ValueIsEmptyOrWhitespace_ShouldThrowException()
        {
            var parser = new ValueParser();

            parser
                .Invoking(a => a.TryParseGuid("", null, out var result))
                .Should()
                .Throw<ArgumentException>();

            parser
                .Invoking(a => a.TryParseGuid(" ", null, out var result))
                .Should()
                .Throw<ArgumentException>();
        }

        [Test(Description = "TryParseGuid throw an exception when the given value is null.")]
        public void TryParseGuid_ValueIsNull_ShouldThrowException()
        {
            var parser = new ValueParser();

            parser
                .Invoking(a => a.TryParseGuid(null!, null, out var result))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "TryParseInt64 should return false when the given value is invalid.")]
        public void TryParseInt64_InvalidValue_ShouldReturnFalse()
        {
            var parser = new ValueParser();

            parser
                .TryParseInt64("NotANumber", NumberStyles.AllowParentheses, new CultureInfo("en-US"), out var result)
                .Should()
                .BeFalse();

            result.Should().Be(default);
        }

        [Test(Description = "TryParseInt64 should return false when the given value is not in the format of the specified custom format provider.")]
        public void TryParseInt64_InvalidValueCustomFormatProvider_ShouldReturnFalse()
        {
            var parser = new ValueParser();

            parser
                .TryParseInt64("1,234", NumberStyles.Any, new CultureInfo("de-DE"), out var result)
                .Should()
                .BeFalse();

            result.Should().Be(default);
        }

        [Test(Description = "TryParseInt64 should parse a valid string into a Int64 value.")]
        public void TryParseInt64_ValidValue_ShouldParseValue()
        {
            var parser = new ValueParser();

            parser
                .TryParseInt64("123", NumberStyles.Any, new CultureInfo("en-US"), out var result)
                .Should()
                .BeTrue();

            result.Should().Be(123L);
        }

        [Test(Description = "TryParseInt64 should parse a valid string in the format of the specified custom format provider into a Int64 value.")]
        public void TryParseInt64_ValidValueCustomFormatProvider_ShouldParseValue()
        {
            var parser = new ValueParser();

            parser
                .TryParseInt64("1.234", NumberStyles.Any, new CultureInfo("de-DE"), out var result)
                .Should()
                .BeTrue();

            result.Should().Be(1234L);
        }

        [Test(Description = "TryParseInt64 should parse a valid string accordingly to the specified number styles into a Int64 value.")]
        public void TryParseInt64_ValidValueCustomNumberStyles_ShouldParseValue()
        {
            var parser = new ValueParser();

            parser
                .TryParseInt64("(123)", NumberStyles.AllowParentheses, new CultureInfo("en-US"), out var result)
                .Should()
                .BeTrue();

            result.Should().Be(-123L);
        }

        [Test(Description = "TryParseInt64 throw an exception when the given value is an empty string.")]
        public void TryParseInt64_ValueIsEmptyOrWhitespace_ShouldThrowException()
        {
            var parser = new ValueParser();

            parser
                .Invoking(a => a.TryParseInt64("", NumberStyles.Any, new CultureInfo("en-US"), out var result))
                .Should()
                .Throw<ArgumentException>();

            parser
                .Invoking(a => a.TryParseInt64(" ", NumberStyles.Any, new CultureInfo("en-US"), out var result))
                .Should()
                .Throw<ArgumentException>();
        }

        [Test(Description = "TryParseInt64 throw an exception when the given value is null.")]
        public void TryParseInt64_ValueIsNull_ShouldThrowException()
        {
            var parser = new ValueParser();

            parser
                .Invoking(a => a.TryParseInt64(null!, NumberStyles.Any, new CultureInfo("en-US"), out var result))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test(Description = "TryParseTimeSpan should return false when the given value is invalid.")]
        public void TryParseTimeSpan_InvalidValue_ShouldReturnFalse()
        {
            var parser = new ValueParser();

            parser
                .TryParseTimeSpan("NotATimeSpan", null, new CultureInfo("en-US"), TimeSpanStyles.None, out var result)
                .Should()
                .BeFalse();

            result.Should().Be(default);
        }

        [Test(Description = "TryParseTimeSpan should return false when the given value is not in the specified custom format.")]
        public void TryParseTimeSpan_InvalidValueCustomFormat_ShouldReturnFalse()
        {
            var parser = new ValueParser();

            parser
                .TryParseTimeSpan("23:59:59", "G", new CultureInfo("en-US"), TimeSpanStyles.None, out var result)
                .Should()
                .BeFalse();

            result.Should().Be(default);
        }

        [Test(Description = "TryParseTimeSpan should return false when the given value is not in the format of the specified custom format provider.")]
        public void TryParseTimeSpan_InvalidValueCustomFormatProvider_ShouldReturnFalse()
        {
            var parser = new ValueParser();

            parser
                .TryParseTimeSpan("0:18:30:00,0000000", null, new CultureInfo("en-CA"), TimeSpanStyles.None, out var result)
                .Should()
                .BeFalse();

            result.Should().Be(default);
        }

        [Test(Description = "TryParseTimeSpan should parse a valid string into a TimeSpan value.")]
        public void TryParseTimeSpan_ValidValue_ShouldParseValue()
        {
            var parser = new ValueParser();

            parser
                .TryParseTimeSpan("23:59:59", null, new CultureInfo("en-US"), TimeSpanStyles.None, out var result)
                .Should()
                .BeTrue();

            result.Should().Be(new TimeSpan(23, 59, 59));
        }

        [Test(Description = "TryParseTimeSpan should parse a valid string in the specified custom format into a TimeSpan value.")]
        public void TryParseTimeSpan_ValidValueCustomFormat_ShouldParseValue()
        {
            var parser = new ValueParser();

            parser
                .TryParseTimeSpan("0:18:30:00.0000000", "G", new CultureInfo("en-US"), TimeSpanStyles.None, out var result)
                .Should()
                .BeTrue();

            result.Should().Be(new TimeSpan(18, 30, 0));
        }

        [Test(Description = "TryParseTimeSpan should parse a valid string in the format of the specified custom format provider into a TimeSpan value.")]
        public void TryParseTimeSpan_ValidValueCustomFormatProvider_ShouldParseValue()
        {
            var parser = new ValueParser();

            parser
                .TryParseTimeSpan("0:18:30:00,0000000", null, new CultureInfo("de-DE"), TimeSpanStyles.None, out var result)
                .Should()
                .BeTrue();

            result.Should().Be(new TimeSpan(18, 30, 0));
        }

        [Test(Description = "TryParseTimeSpan should parse a valid string accordingly to the specified date time styles into a TimeSpan value.")]
        public void TryParseTimeSpan_ValidValueCustomTimeSpanStyles_ShouldParseValue()
        {
            var parser = new ValueParser();

            parser
                .TryParseTimeSpan("23:59", @"hh\:mm", new CultureInfo("en-US"), TimeSpanStyles.AssumeNegative, out var result)
                .Should()
                .BeTrue();

            result.Should().Be(new TimeSpan(23, 59, 00).Negate());
        }

        [Test(Description = "TryParseTimeSpan throw an exception when the given value is an empty string.")]
        public void TryParseTimeSpan_ValueIsEmptyOrWhitespace_ShouldThrowException()
        {
            var parser = new ValueParser();

            parser
                .Invoking(a => a.TryParseTimeSpan("", null, new CultureInfo("en-US"), TimeSpanStyles.None, out var result))
                .Should()
                .Throw<ArgumentException>();

            parser
                .Invoking(a => a.TryParseTimeSpan(" ", null, new CultureInfo("en-US"), TimeSpanStyles.None, out var result))
                .Should()
                .Throw<ArgumentException>();
        }

        [Test(Description = "TryParseTimeSpan throw an exception when the given value is null.")]
        public void TryParseTimeSpan_ValueIsNull_ShouldThrowException()
        {
            var parser = new ValueParser();

            parser
                .Invoking(a => a.TryParseTimeSpan(null!, null, new CultureInfo("en-US"), TimeSpanStyles.None, out var result))
                .Should()
                .Throw<ArgumentNullException>();
        }
    }
}