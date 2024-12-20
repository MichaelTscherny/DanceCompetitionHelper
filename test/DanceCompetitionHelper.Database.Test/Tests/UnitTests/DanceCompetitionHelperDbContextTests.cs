using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace DanceCompetitionHelper.Database.Test.Tests.UnitTests
{
    [TestFixture]
    public class DanceCompetitionHelperDbContextTests
    {
        private readonly IHost _useHost;

        public DanceCompetitionHelperDbContextTests()
        {
            _useHost = TestConfiguration.CreateDefaultTestHost(
                GetNewDbName());
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
        public static void CleanUpOldDbs()
        {
            foreach (var oldDb in Directory.EnumerateFiles(
                ".",
                "*.sqlite"))
            {
                TryDeleteDb(
                    oldDb);
            }
        }

        public static void TryDeleteDb(
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

        public DanceCompetitionHelperDbContext GetDanceCompetitionHelperDbContext()
        {
            var dbCtx = _useHost.Services
                .GetRequiredService<DanceCompetitionHelperDbContext>();
            // for DB-loggigns...
            DiagnosticListener.AllListeners.Subscribe(
                _useHost.Services.GetRequiredService<IObserver<DiagnosticListener>>());
            dbCtx.MigrateAsync();

            return dbCtx;
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _useHost?.Dispose();
        }

        [Test]
        public async Task SimpleCreate()
        {
            using var dbCtx = GetDanceCompetitionHelperDbContext();
            using var dbTrans = await dbCtx.BeginTransactionAsync(
                CancellationToken.None)
                ?? throw new ArgumentNullException(
                    "dbTrans");

            try
            {

                dbCtx.Competitions.Add(
                    new Tables.Competition()
                    {
                        OrgCompetitionId = "1507",
                        CompetitionName = "Test-Comp",
                        CompetitionInfo = "Just an info",
                    });

                await dbCtx.SaveChangesAsync();
                await dbTrans.CommitAsync();

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
                await dbTrans.RollbackAsync();

                throw;
            }
        }
    }
}
