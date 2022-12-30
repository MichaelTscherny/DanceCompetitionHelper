using DanceCompetitionHelper.Database.Config;
using DanceCompetitionHelper.Database.Diagnostic;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace DanceCompetitionHelper.Database
{
    public class DanceCompetitionHelperDbContextFactory : IDesignTimeDbContextFactory<DanceCompetitionHelperDbContext>
    {
        public DanceCompetitionHelperDbContext CreateDbContext(string[] args)
        {
            var useSqLiteDbFile = "dummyFile.sqlite";
            if (args != null
                && args.Length >= 1)
            {
                useSqLiteDbFile = args[0];
            }

            var logFactory = LoggerFactory.Create(
                config =>
                {
                    config.AddConsole();
                });

            return new DanceCompetitionHelperDbContext(
                new SqLiteDbConfig()
                {
                    SqLiteDbFile = useSqLiteDbFile,
                },
                new DbDiagnosticObserver(
                    new DbKeyValueObserver(
                        new SqLiteDbConfig(),
                        logFactory.CreateLogger<DbKeyValueObserver>()),
                    logFactory.CreateLogger<DbDiagnosticObserver>()),
                logFactory.CreateLogger<DanceCompetitionHelperDbContext>());
        }
    }
}
