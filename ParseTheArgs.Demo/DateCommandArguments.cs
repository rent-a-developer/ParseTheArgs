using System;

namespace ParseTheArgs.Demo
{
    public class DateCommandArguments
    {
        public DateTime Date { get; set; }
        public Nullable<DateTime> DifferenceToDate { get; set; }
        public TimeSpan Offset { get; set; }
        public Boolean DisplayInUtc { get; set; }
    }
}
