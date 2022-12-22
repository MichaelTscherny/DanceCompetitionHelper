using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace DanceCompetitionHelper.Database.Test.Tests.UnitTests
{
    [TestFixture]
    public class DanceCompetitionHelperDbContextTests
    {
        private static long _dbCounter = 0;

        private readonly List<string> _createdDbs = new List<string>();

        public string GetNewDbName()
        {
            var newDb = string.Format(
                "{0}_{1}.sqlite",
                DateTime.Now.ToString("yyyyMMdd_HHmmss"),
                Interlocked.Increment(ref _dbCounter));

            Console.WriteLine(
                "NewDb: {0}",
                newDb);

            _createdDbs.Add(
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

        [OneTimeTearDown]
        public void CleanUpDbs()
        {
            foreach (var curDb in _createdDbs)
            {
                TryDeleteDb(
                    curDb);
            }
        }

        public void TryDeleteDb(
            string dbName)
        {
            if (File.Exists(dbName) == false)
            {
                return;
            }

            try
            {
                File.Delete(dbName);

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
            var useDb = GetNewDbName();

            using var dbCtx = new DanceCompetitionHelperDbContext(
                useDb);

            dbCtx.Database.Migrate();

            using var dbTrans = dbCtx.Database.BeginTransaction();

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
                _createdDbs.Remove(useDb);
                dbTrans.Rollback();

                throw;
            }
        }
    }
}
