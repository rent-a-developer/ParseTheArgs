using System;
using System.IO;

namespace ParseTheArgs.Demo
{
    public class FileReplaceCommandOptions
    {
        public FileInfo? InFile { get; set; }
        public FileInfo? OutFile { get; set; }
        public Boolean OverrideOutFile { get; set; }
        
        public String? Pattern { get; set; }
        public String? Replacement { get; set; }

        public Boolean IgnoreCase { get; set; }

        public Boolean UseRegularExpressions { get; set; }

        public Boolean DisplayResult { get; set; }
    }
}