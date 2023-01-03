using DanceCompetitionHelper.Database.Config;
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
                logFactory.CreateLogger<DanceCompetitionHelperDbContext>());
        }
    }
}
