using DanceCompetitionHelper.Config;
using DanceCompetitionHelper.Database.Config;
using DanceCompetitionHelper.Database.Diagnostic;
using DanceCompetitionHelper.OrgImpl.Oetsv;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DanceCompetitionHelper.Database.Test
{
    public class TestConfiguration
    {
        public static IHost CreateDefaultTestHost(
            string sqLiteDbFile) => Host.CreateDefaultBuilder()
                .ConfigureServices((_, config) =>
                {
                    config.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

                    config.AddDbContext<DanceCompetitionHelperDbContext>();
                    config.AddTransient<IDbConfig>(
                        (srvProv) => new SqLiteDbConfig()
                        {
                            SqLiteDbFile = sqLiteDbFile,
                            // LogAllSqls = true,
                        });
                    config.AddTransient(
                        (srvProv) => new ImporterSettings()
                        {
                            // LogAllSqls = true,
                        });
                    // config.AddSingleton<ILoggerProvider, NUnitLoggerProvider>();
                    config.AddTransient<IDanceCompetitionHelper, DanceCompetitionHelper>();
                    config.AddTransient<IObserver<DiagnosticListener>, DbDiagnosticObserver>();
                    config.AddTransient<IObserver<KeyValuePair<string, object?>>, DbKeyValueObserver>();

                    // OeTSV Stuff...
                    config.AddTransient<OetsvCompetitionImporter>();
                    config.AddTransient<OetsvParticipantChecker>();

                    //
                    var fakeHttpClientFactory = new Mock<IHttpClientFactory>();
                    config.AddSingleton<IHttpClientFactory>(fakeHttpClientFactory.Object);
                })
                .ConfigureLogging((_, config) =>
                {
                    config.AddConsole();
                    config.SetMinimumLevel(LogLevel.Debug);
                })
                .Build();
    }
}
