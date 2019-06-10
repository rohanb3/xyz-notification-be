using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Xyzies.Notification.Data.Entity;
using Xyzies.Notification.Data.Repository.Behaviour;

namespace Xyzies.Notification.Services.LogProvider
{
    public class LoggerDatabaseProvider : ILoggerProvider
    {
        private ILogRepository _repo;

        public LoggerDatabaseProvider(ILogRepository repo)
        {
            _repo = repo;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new Logger(categoryName, _repo);
        }

        public void Dispose()
        {
        }

        public class Logger : ILogger
        {
            private readonly string _categoryName;
            private readonly ILogRepository _repo;

            public Logger(string categoryName, ILogRepository repo)
            {
                _repo = repo;
                _categoryName = categoryName;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                if (logLevel == LogLevel.Critical || logLevel == LogLevel.Error || logLevel == LogLevel.Warning)
                    RecordMsg(logLevel, eventId, state, exception, formatter);
            }

            private void RecordMsg<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                _repo.Add(new Log
                {
                    LogLevel = logLevel.ToString(),
                    CategoryName = _categoryName,
                    Message = formatter(state, exception),
                    CreateOn = DateTime.Now
                });
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return new NoopDisposable();
            }

            private class NoopDisposable : IDisposable
            {
                public void Dispose()
                {
                }
            }
        }
    }
}
