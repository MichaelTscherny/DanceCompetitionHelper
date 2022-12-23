using DanceCompetitionHelper.Database.Config;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Database.Test.Pocos;
using DanceCompetitionHelper.Database.Test.Pocos.DanceCompetitionHelper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using TechTalk.SpecFlow.Assist;
using TestHelper.Extensions;
using TestHelper.Logging;

namespace DanceCompetitionHelper.Database.Test.Bindings
{
    [Binding]
    public sealed class Given : BindingBase
    {
        private readonly IHost _useHost;
        private string nextDanceCompHelperDb = "dummy";

        public Given(
            ScenarioContext scenarioContext)
            : base(
                  scenarioContext)
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

        public string GetNewDbName()
        {
            return string.Format(
                "{0}_{1}.sqlite",
                UseNow.ToString("yyyyMMdd_HHmmss"),
                nextDanceCompHelperDb);
        }

        [Given(@"following DanceComp-DB ""([^""]*)""")]
        public void GivenFollowingDanceCompDb(
            string danceCompHelperDb)
        {
            nextDanceCompHelperDb = danceCompHelperDb;
            var newDbFile = GetNewDbName();

            Console.WriteLine(
                "Create Test-DB '{0}' -> {1}",
                danceCompHelperDb,
                newDbFile);

            if (File.Exists(
                newDbFile))
            {
                File.Delete(
                    newDbFile);
            }

            var newDb = _useHost.Services
                .GetRequiredService<DanceCompetitionHelperDbContext>();
            newDb.Migrate();

            var newDbPoco = new DanceCompetitionHelperDbContextPoco(
                newDb,
                newDbFile);
            _scenarioContext.AddToScenarioContext(
                SpecFlowConstants.DanceCompetitionHelperDb,
                danceCompHelperDb,
                newDbPoco);

            AddToDispose(
                newDbPoco);
        }

        [Given(@"following Competitions in ""([^""]*)""")]
        public void GivenFollowingCompetitionsIn(
            string danceCompHelperDb,
            Table table)
        {
            var newComps = table.CreateSet<CompetitionPoco>();
            var useDb = GetDanceCompetitionHelperDbContext(
                danceCompHelperDb);

            using var dbTrans = useDb.BeginTransaction();

            foreach (var newComp in newComps)
            {
                try
                {
                    useDb.Competitions.Add(
                        new Competition()
                        {
                            Organization = newComp.Organization,
                            OrgCompetitionId = newComp.OrgCompetitionId
                                ?? throw new ArgumentNullException(
                                    nameof(newComp.OrgCompetitionId)),
                            CompetitionName = newComp.CompetitionName
                                ?? throw new ArgumentNullException(
                                    nameof(newComp.CompetitionName)),
                            CompetitionInfo = newComp.CompetitionInfo,
                            CompetitionDate = newComp.CompetitionDate ?? UseNow,
                        });

                    useDb.SaveChanges();
                }
                catch (Exception exc)
                {
                    dbTrans.Rollback();

                    Console.WriteLine(
                        "Error during add of '{0}': {1}",
                        newComp,
                        exc);
                    throw;
                }
            }

            dbTrans.Commit();
        }

        [Given(@"following Competition Classes in ""([^""]*)""")]
        public void GivenFollowingCompetitionClassesIn(
            string danceCompHelperDb,
            Table table)
        {
            var newCompClasses = table.CreateSet<CompetitionClassPoco>();

            var useDb = GetDanceCompetitionHelperDbContext(
                danceCompHelperDb);

            using var dbTrans = useDb.BeginTransaction();

            foreach (var newCompClass in newCompClasses)
            {
                var useComp = useDb.Competitions.FirstOrDefault(
                    x => x.CompetitionName == newCompClass.CompetitionName);

                Assert.That(
                    useComp,
                    Is.Not.Null,
                    "{0} '{1}' not found!",
                    nameof(Competition),
                    newCompClass.CompetitionName);

                try
                {
                    useDb.CompetitionClasses.Add(
                        new CompetitionClass()
                        {
                            Competition = useComp,
                            OrgClassId = newCompClass.OrgClassId,
                            Version = newCompClass.Version,
                            Discipline = newCompClass.Discipline,
                            AgeClass = newCompClass.AgeClass,
                            AgeGroup = newCompClass.AgeGroup,
                            Class = newCompClass.Class,

                            MinStartsForPromotion = newCompClass.MinStartsForPromotion,
                            MinPointsForPromotion = newCompClass.MinPointsForPromotion,
                        });

                    useDb.SaveChanges();
                }
                catch (Exception exc)
                {
                    dbTrans.Rollback();

                    Console.WriteLine(
                        "Error during add of '{0}': {1}",
                        newCompClass,
                        exc);
                    throw;
                }
            }

            dbTrans.Commit();
        }


        [AfterScenario]
        public void AfterScenario()
        {
            DisposeAllCreated();
        }
    }
}
