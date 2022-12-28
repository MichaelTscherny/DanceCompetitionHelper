using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DanceCompetitionHelper.Database.Diagnostic
{
    public class DbDiagnosticObserver : IObserver<DiagnosticListener>
    {
        private readonly IObserver<KeyValuePair<string, object?>> _keyValueObserver;
        private readonly ILogger<DbDiagnosticObserver> _logger;

        public DbDiagnosticObserver(
            IObserver<KeyValuePair<string, object?>> keyValueObserver,
            ILogger<DbDiagnosticObserver> logger)
        {
            _keyValueObserver = keyValueObserver
                ?? throw new ArgumentNullException(
                    nameof(keyValueObserver));
            _logger = logger
                ?? throw new ArgumentNullException(
                    nameof(logger));
        }

        public void OnCompleted()
        {
            /* Do nothing!*/
        }

        public void OnError(Exception error)
        {
            /* Do nothing!*/
        }

        public void OnNext(DiagnosticListener value)
        {
            if (value.Name == DbLoggerCategory.Name) // "Microsoft.EntityFrameworkCore"
            {
                _logger.LogDebug(
                    "Register key-value observer");

                value.Subscribe(
                    _keyValueObserver);
            }
        }
    }
}
