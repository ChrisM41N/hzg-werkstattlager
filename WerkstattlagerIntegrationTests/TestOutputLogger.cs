#pragma warning disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using Xunit.Abstractions;

namespace WerkstattlagerIntegration;

public class TestOutputLoggerConfiguration
{
    public LogLevel LogLevel { get; set; } = LogLevel.Information;
    public int EventId { get; set; } = 0;
    public ITestOutputHelper TestOutputHelper { get; set; }
}

public class TestOutputLogger : ILogger
{
    private readonly string _name;
    private readonly TestOutputLoggerConfiguration _configuration;

    public TestOutputLogger(string name, TestOutputLoggerConfiguration configuration)
    {
        _name = name;
        _configuration = configuration;
    }

    public IDisposable? BeginScope<TState>(TState state) => null;
    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (
            !IsEnabled(logLevel) ||
            _configuration.EventId != 0 && _configuration.EventId != eventId.Id
            ) { return; }

        _configuration.TestOutputHelper.WriteLine($"{eventId} - {eventId.Id} - {_name} - {formatter(state, exception)}");
    }
}

public class TestOutputLoggerProvider : ILoggerProvider
{
    private readonly TestOutputLoggerConfiguration _configuration;
    private readonly ConcurrentDictionary<string, TestOutputLogger> _loggers = new();
    public TestOutputLoggerProvider(TestOutputLoggerConfiguration configuration)
    { _configuration = configuration; }

    public ILogger CreateLogger(string categoryName)
    { return _loggers.GetOrAdd(categoryName, name => new TestOutputLogger(name, _configuration)); }

    public void Dispose()
    { _loggers.Clear(); }
}

public static class TestOutputLoggerExtensions
{
    public static ILoggerFactory AddTestOutputLogger(this ILoggerFactory loggerFactory, TestOutputLoggerConfiguration configuration)
    {
        loggerFactory.AddProvider(new TestOutputLoggerProvider(configuration));
        return loggerFactory;
    }

    public static ILoggerFactory AddTestOutputLogger(this ILoggerFactory loggerFactory)
    { return loggerFactory.AddTestOutputLogger(new TestOutputLoggerConfiguration()); }

    public static ILoggerFactory AddTestOutputLogger(this ILoggerFactory loggerFactory, Action<TestOutputLoggerConfiguration> configure)
    {
        var configuration = new TestOutputLoggerConfiguration();
        configure(configuration);
        return loggerFactory.AddTestOutputLogger(configuration);
    }

    public static ILoggingBuilder AddTestOutputLogger(this ILoggingBuilder loggingBuilder, TestOutputLoggerConfiguration configuration)
    { return loggingBuilder.AddProvider(new TestOutputLoggerProvider(configuration)); }

    public static ILoggingBuilder AddTestOutputLogger(this ILoggingBuilder loggingBuilder)
    { return loggingBuilder.AddTestOutputLogger(new TestOutputLoggerConfiguration()); }

    public static ILoggingBuilder AddTestOutputLogger(this ILoggingBuilder loggingBuilder, Action<TestOutputLoggerConfiguration> configure)
    {
        var configuration = new TestOutputLoggerConfiguration();
        configure(configuration);
        return loggingBuilder.AddTestOutputLogger(configuration);
    }
}
