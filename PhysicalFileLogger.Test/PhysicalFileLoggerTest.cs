using System;
using Xunit;
using PhysicalFileLogger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using PhysicalFileLogger.Extension;
using System.IO;
using System.Linq;

namespace PhysicalFileLogger.Test
{
    public class PhysicalFileLoggerTest
    {
        [Fact]
        public void LoggerCache()
        {
            var factory = init();
            var logger = factory.CreateLogger("program");
            logger.LogInformation("hello");
            var logger1 = factory.CreateLogger("program");
            Assert.True(object.ReferenceEquals(logger, logger1));

            var logger2 = factory.CreateLogger<PhysicalFileLoggerTest>();
            var logger3 = factory.CreateLogger<PhysicalFileLoggerTest>();
            Assert.NotSame(logger2, logger3);

            var logger4 = factory.CreateLogger(typeof(PhysicalFileLoggerTest));
            var logger5 = factory.CreateLogger(typeof(PhysicalFileLoggerTest));
            Assert.Same(logger4, logger5);
        }
        [Fact]
        public void LogTest()
        {
            var factory = init();
            var logger = factory.CreateLogger("program");
            string message = "hello";
            logger.LogInformation(message);
            var infos =  File.ReadLines(Directory.GetCurrentDirectory() + "/log/program.txt");
            Assert.True(infos.Last() == message);
        }
        [Fact]
        public void SpecifiedPathByOptions()
        {
            var path = "D:/log/";
            var extension = ".data";
            var factory = new ServiceCollection()
                .AddOptions()
                .AddLogging(builder =>
                {
                    builder.AddPhyFileLogger();
                })
                .Configure<PhysicalFileLoggerOptions>(options =>
                {
                    options.Path = path;
                    options.FileExtension = extension;
                })
                .BuildServiceProvider()
                .GetService<ILoggerFactory>();
            var catelogName = "optionsPath";
            var logger = factory.CreateLogger(catelogName);
            var message = "options path is" + path;
            logger.LogInformation(message);
            var logs = File.ReadAllLines(Path.Combine(path, catelogName + extension));
            Assert.True(logs.Last() == message);
        }

        public ILoggerFactory init()
        {
            return new ServiceCollection()
            .AddOptions()
            .AddLogging(builder =>
            {
                builder.AddPhyFileLogger();
            })
            .BuildServiceProvider().GetService<ILoggerFactory>();
        }

    }
}
