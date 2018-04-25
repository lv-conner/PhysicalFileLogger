using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using System;
using System.IO;

namespace PhysicalFileLogger
{
    public class PhysicalFileLogger : ILogger
    {
        private static string DefaultCatalogName = "Default+6A924B05-B781-49CB-A436-FE506332A684";
        private readonly Func<string, LogLevel, bool> filter;
        private readonly PhysicalFileLoggerOptions options;
        private readonly string catelogName;
        private readonly string filePath;
        public PhysicalFileLogger(Func<string,LogLevel,bool> filter,PhysicalFileLoggerOptions options,string catelogName)
        {
            if(options == null)
            {
                throw new ArgumentNullException("PhysicalFileLoggerOptions");
            }
            this.filter = filter;
            this.options = options;
            this.catelogName = catelogName;
            if (string.IsNullOrWhiteSpace(this.catelogName))
            {
                catelogName = DefaultCatalogName;
            }
            filePath = InitFileName();
        }

        private string InitFileName()
        {
            EnsureFilePath();
            var fileName = catelogName + options.FileExtension;
            return Path.Combine(options.Path, fileName);
        }
        public void EnsureFilePath()
        {
            if(!Directory.Exists(options.Path))
            {
                Directory.CreateDirectory(options.Path);
            }
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return NullScope.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            if(logLevel == LogLevel.None)
            {
                return false;
            }
            else
            {
                return filter(catelogName, logLevel);
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            File.AppendAllText(filePath, formatter(state, exception));
            File.AppendAllText(filePath, "\n");
        }
    }
}
