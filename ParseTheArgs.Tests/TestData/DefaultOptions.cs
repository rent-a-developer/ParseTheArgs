using System;
using System.Collections.Generic;

namespace ParseTheArgs.Tests.TestData
{
    public class DefaultOptions
    {
        public String OptionA { get; set; }
        public String OptionB { get; set; }
        public Boolean OptionC { get; set; }
        public LogLevel OptionD { get; set; }
        public Encoding OptionE { get; set; }
        public List<LogLevel> OptionF { get; set; }
        public List<Encoding> OptionG { get; set; }
    }
}