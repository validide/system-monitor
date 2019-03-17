using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using SystemMonitor.Core.Implementations;
using SystemMonitor.Core.Implementations.Monitors;
using SystemMonitor.Core.Implementations.Reporters;
using SystemMonitor.Core.Implementations.Serializers;
using SystemMonitor.UnitTests.Dummies.Core.Contracts.Monitors;
using Xunit;
using CoreHosting = SystemMonitor.Core.Implementations.Hosting;

namespace SystemMonitor.UnitTests.Core.Hosting
{
    public class HostTests
    {
        [Fact]
        public void Host_ContructorExceptions_1()
        {
            Assert.Throws<ArgumentNullException>(() => {
                var monitor = new DummyConstantMonitor();
                var serializer = new TextSerializer<bool>(",");
                var writer = new TextWriterReporter<bool>(Console.Out, serializer);
                var timespan = new TimeSpan(0, 0, 0, 0, 10);
                var host = new CoreHosting.Host(
                    null,
                    new[] {
                    new SystemMonitorPipeline<bool>(monitor, writer)
                    },
                    timespan,
                    NullLoggerFactory.Instance
                );
            });
        }

        [Fact]
        public void Host_ContructorExceptions_2()
        {
            Assert.Throws<ArgumentNullException>(() => {
                var monitor = new DummyConstantMonitor();
                var serializer = new TextSerializer<bool>(",");
                var writer = new TextWriterReporter<bool>(Console.Out, serializer);
                var timespan = new TimeSpan(0, 0, 0, 0, 10);
                var host = new CoreHosting.Host(
                    "Test",
                    null,
                    timespan,
                    NullLoggerFactory.Instance
                );
            });
        }

        [Fact]
        public async Task Host_StartException()
        {
            using (var sw = new StringWriter())
            {
                var monitor = new DummyConstantMonitor();
                var serializer = new TextSerializer<bool>(",");
                var writer = new TextWriterReporter<bool>(sw, serializer);
                var timespan = new TimeSpan(0, 0, 0, 0, 1);
                var logger = new Fakes.FakeLogger();
                var host = new CoreHosting.Host(
                    "ExceptionHost",
                    new[] {
                    new SystemMonitorPipeline<bool>(monitor, writer)
                    },
                    timespan,
                    new Fakes.FakeLoggerFactory(logger)
                );

                var timerMember = host
                    .GetType()
                    .GetField("_timer", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase);

                timerMember.SetValue(host, null);

                await host.StartAsync();

                Assert.NotNull(logger.ProvidedException);

            }
        }

        [Fact]
        public async Task Host_StopException()
        {
            using (var sw = new StringWriter())
            {
                var monitor = new DummyConstantMonitor();
                var serializer = new TextSerializer<bool>(",");
                var writer = new TextWriterReporter<bool>(sw, serializer);
                var timespan = new TimeSpan(0, 0, 0, 0, 1);
                var logger = new Fakes.FakeLogger();
                var host = new CoreHosting.Host(
                    "ExceptionHost",
                    new[] {
                    new SystemMonitorPipeline<bool>(monitor, writer)
                    },
                    timespan,
                    new Fakes.FakeLoggerFactory(logger)
                );

                var timerMember = host
                    .GetType()
                    .GetField("_timer", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase);


                var timer = timerMember.GetValue(host);
                await host.StartAsync();
                timerMember.SetValue(host, null);
                await host.StopAsync();

                Assert.NotNull(logger.ProvidedException);
                timerMember.SetValue(host, timer);
                logger.Reset();
                await host.StopAsync();
                Assert.Null(logger.ProvidedException);
            }
        }



        [Fact]
        public async Task Host_RunTasksException()
        {
            using (var sw = new StringWriter())
            {
                var monitor = new SimpleMonitor<bool>(() => throw new Exception());
                var serializer = new TextSerializer<bool>(",");
                var writer = new TextWriterReporter<bool>(sw, serializer);
                var timespan = new TimeSpan(0, 0, 0, 0, 5);
                var logger = new Fakes.FakeLogger();
                var host = new CoreHosting.Host(
                    "ExceptionHost",
                    new[] {
                    new SystemMonitorPipeline<bool>(monitor, writer)
                    },
                    timespan,
                    new Fakes.FakeLoggerFactory(logger)
                );

                await host.StartAsync();
                Thread.Sleep(7);
                Assert.NotNull(logger.ProvidedException);
                await host.StopAsync();
            }
        }

        [Fact]
        public async Task Host_Basic()
        {
            using (var sw = new StringWriter())
            {
                var monitor = new DummyConstantMonitor();
                var serializer = new TextSerializer<bool>(",");
                var writer = new TextWriterReporter<bool>(sw, serializer);
                var timespan = new TimeSpan(0, 0, 0, 0, 10);
                var iterations = 3.5;
                var host = new CoreHosting.Host(
                    "Test Console Runner",
                    new[] {
                    new SystemMonitorPipeline<bool>(monitor, writer)
                    },
                    timespan,
                    NullLoggerFactory.Instance
                );

                Thread.Sleep(timespan * 1.5);

                Assert.Equal(String.Empty, sw.ToString());
                await host.StartAsync();

                Thread.Sleep(timespan * iterations);
                await host.StopAsync();

                var expectedPart = await serializer.SerializeAsync(await monitor.GetDataAsync());
                var expectedResult = String.Empty;
                for (int i = 0; i < Math.Floor(iterations); i++)
                {
                    expectedResult += expectedPart + Environment.NewLine;
                }


                Assert.Equal(expectedResult, sw.ToString());

                Thread.Sleep(timespan * iterations);

                Assert.Equal(expectedResult, sw.ToString());
            }
        }
    }
}
