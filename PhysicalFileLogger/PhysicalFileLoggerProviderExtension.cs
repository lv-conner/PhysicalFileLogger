using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace PhysicalFileLogger.Extension
{
    public static class PhysicalFileLoggerProviderExtension
    {
        public static ILoggingBuilder AddPhyFileLogger(this ILoggingBuilder builder)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, PhysicalFileLoggerProvider>());
            builder.Services.Configure<PhysicalFileLoggerOptions>(options => { });
            return builder;
        }
        public static ILoggingBuilder AddPhyFileLogger(this ILoggingBuilder builder,Action<PhysicalFileLoggerOptions> configure)
        {
            builder.AddPhyFileLogger();
            builder.Services.Configure<PhysicalFileLoggerOptions>(configure);
            return builder;
        }
        public static ILoggingBuilder AddPhyFileLogger(this ILoggingBuilder builder,IConfiguration configuration)
        {
            builder.AddPhyFileLogger();
            builder.Services.Configure<PhysicalFileLoggerOptions>(configuration);
            return builder;
        }
    }
}
