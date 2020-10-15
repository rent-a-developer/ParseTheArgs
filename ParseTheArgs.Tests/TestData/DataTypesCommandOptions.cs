using System;
using System.Collections.Generic;

namespace ParseTheArgs.Tests.TestData
{
    public class DataTypesCommandOptions
    {
        public Boolean Boolean { get; set; }
        public DateTime DateTime { get; set; }

        public List<DateTime> DateTimes { get; set; }
        public Decimal Decimal { get; set; }
        public List<Decimal> Decimals { get; set; }
        public LogLevel Enum { get; set; }
        public List<LogLevel> Enums { get; set; }
        public Guid Guid { get; set; }
        public List<Guid> Guids { get; set; }
        public Int64 Int64 { get; set; }
        public List<Int64> Int64s { get; set; }

        public Nullable<DateTime> NullableDateTime { get; set; }
        public Nullable<Decimal> NullableDecimal { get; set; }
        public Nullable<LogLevel> NullableEnum { get; set; }
        public Nullable<Guid> NullableGuid { get; set; }
        public Nullable<Int64> NullableInt64 { get; set; }
        public Nullable<TimeSpan> NullableTimeSpan { get; set; }
        public String String { get; set; }
        public List<String> Strings { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public List<TimeSpan> TimeSpans { get; set; }
    }
}