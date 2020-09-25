using System;
using System.Collections.Generic;

namespace ParseTheArgs.Tests.TestData
{
    public class DefaultArguments
    {
        public String ArgumentA { get; set; }
        public String ArgumentB { get; set; }
        public Boolean ArgumentC { get; set; }
        public LogLevel ArgumentD { get; set; }
        public Encoding ArgumentE { get; set; }
        public List<LogLevel> ArgumentF { get; set; }
        public List<Encoding> ArgumentG { get; set; }
    }
}