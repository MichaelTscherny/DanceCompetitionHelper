using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace TestHelper.Logging
{
    public class NUnitLogger : ILogger, IDisposable
    {
        public void Dispose()
        {
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            var message = formatter(state, exception);

            var logString = string.Format(
                "{0} [{1}]: {2}",
                logLevel,
                eventId,
                message);

            Debug.WriteLine(logString);
            Console.WriteLine(logString);
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull
            => this;
    }
}
