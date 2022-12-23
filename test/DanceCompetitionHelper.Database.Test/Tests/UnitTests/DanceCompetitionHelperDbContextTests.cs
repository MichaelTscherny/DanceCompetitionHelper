using DanceCompetitionHelper.Database.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using TestHelper.Logging;

namespace DanceCompetitionHelper.Database.Test.Tests.UnitTests
{
    [TestFixture]
    public class DanceCompetitionHelperDbContextTests
    {
        private readonly IHost _useHost;

        public DanceCompetitionHelperDbContextTests()
        {
            _useHost = Host.CreateDefaultBuilder()
                .ConfigureServices((_, config) =>
                {
                    config.AddDbContext<DanceCompetitionHelperDbContext>();
                    config.AddTransient<IDbConfig>(
                        (srvProv) => new SqLiteDbConfig()
                        {
                            SqLiteDbFile = GetNewDbName(),
                        });
                    config.AddSingleton<ILoggerProvider, NUnitLoggerProvider>();
                })
                .ConfigureLogging((_, config) =>
                {
                    config.SetMinimumLevel(LogLevel.Debug);
                })
                .Build();
        }

        private static long _dbCounter = 0;

        public string GetNewDbName()
        {
            var newDb = string.Format(
                "{0}_{1}.sqlite",
                DateTime.Now.ToString("yyyyMMdd_HHmmss"),
                Interlocked.Increment(ref _dbCounter));

            Console.WriteLine(
                "NewDb: {0}",
                newDb);

            return newDb;
        }

        [OneTimeSetUp]
        public void CleanUpOldDbs()
        {
            foreach (var oldDb in Directory.EnumerateFiles(
                ".",
                "*.sqlite"))
            {
                TryDeleteDb(
                    oldDb);
            }
        }

        public void TryDeleteDb(
            string dbName)
        {
            if (File.Exists(
                dbName) == false)
            {
                return;
            }

            try
            {
                File.Delete(
                    dbName);

                Console.WriteLine(
                    "Deleted '{0}'",
                    dbName);
            }
            catch (Exception exc)
            {
                Console.WriteLine(
                    "Unable to deleted '{0}': {1}",
                    dbName,
                    exc.Message);
            }
        }

        [Test]
        public void SimpleCreate()
        {
            using var dbCtx = _useHost.Services
                .GetRequiredService<DanceCompetitionHelperDbContext>();

            dbCtx.Migrate();

            using var dbTrans = dbCtx.BeginTransaction();

            try
            {

                dbCtx.Competitions.Add(
                    new Tables.Competition()
                    {
                        OrgCompetitionId = "1507",
                        CompetitionName = "Test-Comp",
                        CompetitionInfo = "Just an info",
                    });

                dbCtx.SaveChanges();
                dbTrans.Commit();

                foreach (var curComp in dbCtx.Competitions)
                {
                    Console.WriteLine(
                        "{0} ({1}): {2} ({3})",
                        curComp.CompetitionId,
                        curComp.OrgCompetitionId,
                        curComp.CompetitionName,
                        curComp.CompetitionInfo);
                }
            }
            catch
            {
                dbTrans.Rollback();

                throw;
            }
        }
    }
}
