using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace PhysicalFileLogger
{
    public class PhysicalFileLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, PhysicalFileLogger> loggers = new ConcurrentDictionary<string, PhysicalFileLogger>();
        private static readonly Func<string, LogLevel, bool> trueFilter = (s, l) => true;
        private readonly Func<string, LogLevel, bool> filter;
        private PhysicalFileLoggerOptions options;
        private IDisposable reloadToken;
        public PhysicalFileLoggerProvider(Func<string,LogLevel,bool> filter):this(filter,null)
        {

        }
        public PhysicalFileLoggerProvider(IOptionsMonitor<PhysicalFileLoggerOptions> options):this(null,options)
        {

        }
        public PhysicalFileLoggerProvider(Func<string, LogLevel, bool> filter, IOptionsMonitor<PhysicalFileLoggerOptions> options)
        {
            if(this.filter == null)
            {
                this.filter = trueFilter;
            }
            else
            {
                this.filter = filter;
            }
            if(options == null)
            {
                this.options = new PhysicalFileLoggerOptions();
            }
            else
            {
                this.options = options.CurrentValue;
                this.reloadToken = options.OnChange(Reload);
            }
        }

        public void Reload(PhysicalFileLoggerOptions options)
        {
            this.options = options;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return loggers.GetOrAdd(categoryName, CreatePhysicalFileLogger);
        }

        public PhysicalFileLogger CreatePhysicalFileLogger(string categoryName)
        {
            return new PhysicalFileLogger(filter, options, categoryName);
        }

        public void Dispose()
        {
            if(reloadToken != null)
            {
                reloadToken.Dispose();
            }
        }
    }
}
