using DanceCompetitionHelper.Database.Config;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace DanceCompetitionHelper.Database.Diagnostic
{
    public class DbKeyValueObserver : IObserver<KeyValuePair<string, object?>>
    {
        private readonly ILogger<DbKeyValueObserver> _logger;
        private readonly IDbConfig _sqLiteDbConfig;

        public DbKeyValueObserver(
            IDbConfig sqLiteDbConfig,
            ILogger<DbKeyValueObserver> logger)
        {
            _sqLiteDbConfig = sqLiteDbConfig
               ?? throw new ArgumentNullException(
                   nameof(sqLiteDbConfig));
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

        public void OnNext(KeyValuePair<string, object?> value)
        {
            if (value.Key == null
                || value.Value == null)
            {
                return;
            }

            /*
            if (value.Key == CoreEventId.ContextInitialized.Name)
            {
                var payload = (ContextInitializedEventData)value.Value;
                _logger.LogTrace(
                    "EF is initializing '{Name}'",
                    payload.Context.GetType().Name);
            }

            if (value.Key == RelationalEventId.ConnectionOpening.Name)
            {
                var payload = (ConnectionEventData)value.Value;
                _logger.LogDebug(
                    "EF is opening a connection to '{ConnectionString}'",
                    payload.Connection.ConnectionString);
            }
            */

            if (RelationalEventId.TransactionStarted.Name == value.Key)
            {
                if (_sqLiteDbConfig.LogAllSqls == false)
                {
                    return;
                }

                var payload = value.Value as TransactionEndEventData;
                if (payload == null)
                {
                    return;
                }

                _logger.LogInformation(
                    payload.ToString());
            }

            if (RelationalEventId.TransactionCommitted.Name == value.Key)
            {
                if (_sqLiteDbConfig.LogAllSqls == false)
                {
                    return;
                }

                var payload = value.Value as TransactionEndEventData;
                if (payload == null)
                {
                    return;
                }

                _logger.LogInformation(
                    payload.ToString());
            }

            if (RelationalEventId.TransactionRolledBack.Name == value.Key)
            {
                if (_sqLiteDbConfig.LogAllSqls == false)
                {
                    return;
                }

                var payload = value.Value as TransactionEndEventData;
                if (payload == null)
                {
                    return;
                }

                _logger.LogInformation(
                    payload.ToString());
            }

            if (RelationalEventId.CommandExecuted.Name == value.Key)
            {
                var payload = value.Value as CommandExecutedEventData;
                if (payload == null)
                {
                    _logger.LogError(
                        "Got invalid Payload: {Payload}",
                        value.Value);
                    return;
                }

                var useLogLevel = LogLevel.None;

                if (_sqLiteDbConfig.LogAllSqls == false)
                {

                    if (payload.Duration >= _sqLiteDbConfig.RuntimeInfo)
                    {
                        useLogLevel = LogLevel.Information;
                    }

                    if (payload.Duration >= _sqLiteDbConfig.RuntimeWarn)
                    {
                        useLogLevel = LogLevel.Warning;
                    }

                    if (payload.Duration >= _sqLiteDbConfig.RuntimeError)
                    {
                        useLogLevel = LogLevel.Error;
                    }
                }
                else
                {
                    useLogLevel = LogLevel.Information;
                }

                if (useLogLevel == LogLevel.None)
                {
                    return;
                }

                var useParams = payload.Command.Parameters.Count <= 0
                    ? "NONE"
                    : "??";

                if (payload.LogParameterValues)
                {
                    var paramHelper = new List<string>();

                    foreach (DbParameter curParam in payload.Command.Parameters)
                    {
                        paramHelper.Add(
                            string.Format(
                                "{0} ({1}): '{2}'",
                                curParam.ParameterName,
                                curParam.DbType,
                                curParam.Value));
                    }

                    useParams = string.Join(
                        "; ",
                        paramHelper);
                }

                _logger.Log(
                    useLogLevel,
                    "Runtime of SQL from '{StartTime}': {Duration} {NL}" +
                    "{CommandText}{NL}" +
                    "Params: {Params}",
                    payload.StartTime,
                    payload.Duration.ToString("g"),
                    Environment.NewLine,
                    payload.Command.CommandText,
                    Environment.NewLine,
                    useParams);
            }
        }
    }
}
