using System;
using System.Collections.Generic;
using System.Text;

namespace PhysicalFileLogger
{
    public class PhysicalFileLoggerOptions
    {
        public PhysicalFileLoggerOptions()
        {
            Path = "Log/";
            FileExtension = ".txt";
        }
        public string Path { get; set; }
        public string FileExtension { get; set; }
    }
}
