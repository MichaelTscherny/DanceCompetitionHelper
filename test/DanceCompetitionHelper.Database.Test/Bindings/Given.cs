using DanceCompetitionHelper.Database.Config;
using DanceCompetitionHelper.Database.Diagnostic;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Database.Test.Pocos;
using DanceCompetitionHelper.Database.Test.Pocos.DanceCompetitionHelper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System.Diagnostics;
using TechTalk.SpecFlow.Assist;
using TestHelper.Extensions;

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
                            // LogAllSqls = true,
                        });
                    // config.AddSingleton<ILoggerProvider, NUnitLoggerProvider>();
                    config.AddTransient<IDanceCompetitionHelper, DanceCompetitionHelper>();
                    config.AddTransient<IObserver<DiagnosticListener>, DbDiagnosticObserver>();
                    config.AddTransient<IObserver<KeyValuePair<string, object?>>, DbKeyValueObserver>();
                })
                .ConfigureLogging((_, config) =>
                {
                    config.AddConsole();
                    config.SetMinimumLevel(LogLevel.Debug);
                })
                .Build();
        }

        #region Dance Competition Helper Database

        private static long _databaseId = 0;

        public string GetNewDbName()
        {
            return string.Format(
                "{0}_{1}_{2}.sqlite",
                UseNow.ToString("yyyyMMdd_HHmmss"),
                nextDanceCompHelperDb,
                Interlocked.Increment(ref _databaseId));
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
            // for DB-loggigns...
            DiagnosticListener.AllListeners.Subscribe(
                _useHost.Services.GetRequiredService<IObserver<DiagnosticListener>>());
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
                            CompetitionClassName = newCompClass.CompetitionClassName,
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

        [Given(@"following Competition Classes History in ""([^""]*)""")]
        public void GivenFollowingCompetitionClassesHistoryIn(
            string danceCompHelperDb,
            Table table)
        {
            var newCompClassesHistory = table.CreateSet<CompetitionClassHistoryPoco>();

            var useDb = GetDanceCompetitionHelperDbContext(
                danceCompHelperDb);

            using var dbTrans = useDb.BeginTransaction();

            foreach (var newCompClassHist in newCompClassesHistory)
            {
                var useComp = useDb.Competitions.FirstOrDefault(
                    x => x.CompetitionName == newCompClassHist.CompetitionName);

                Assert.That(
                    useComp,
                    Is.Not.Null,
                    "{0} '{1}' not found!",
                    nameof(Competition),
                    newCompClassHist.CompetitionName);

                try
                {
                    useDb.CompetitionClassesHistory.Add(
                        new CompetitionClassHistory()
                        {
                            Competition = useComp,
                            Version = newCompClassHist.Version,
                            OrgClassId = newCompClassHist.OrgClassId,
                            Discipline = newCompClassHist.Discipline,
                            AgeClass = newCompClassHist.AgeClass,
                            AgeGroup = newCompClassHist.AgeGroup,
                            Class = newCompClassHist.Class,

                            MinStartsForPromotion = newCompClassHist.MinStartsForPromotion,
                            MinPointsForPromotion = newCompClassHist.MinPointsForPromotion,
                        });

                    useDb.SaveChanges();
                }
                catch (Exception exc)
                {
                    dbTrans.Rollback();

                    Console.WriteLine(
                        "Error during add of '{0}': {1}",
                        newCompClassHist,
                        exc);
                    throw;
                }
            }

            dbTrans.Commit();
        }

        [Given(@"following Participants in ""([^""]*)""")]
        public void GivenFollowingParticipantsIn(
            string danceCompHelperDb,
            Table table)
        {
            var newParticipants = table.CreateSet<ParticipantPoco>();
            var useDb = GetDanceCompetitionHelperDbContext(
                danceCompHelperDb);

            using var dbTrans = useDb.BeginTransaction();

            foreach (var newPart in newParticipants)
            {
                var useComp = useDb.Competitions.FirstOrDefault(
                    x => x.CompetitionName == newPart.CompetitionName);

                Assert.That(
                    useComp,
                    Is.Not.Null,
                    "{0} '{1}' not found!",
                    nameof(Competition),
                    newPart.CompetitionName);

                var useCompClass = useDb.CompetitionClasses.FirstOrDefault(
                    x => x.CompetitionClassName == newPart.CompetitionClassName);

                Assert.That(
                    useCompClass,
                    Is.Not.Null,
                    "{0} '{1}' not found!",
                    nameof(CompetitionClass),
                    newPart.CompetitionClassName);

                try
                {
                    useDb.Participants.Add(
                        new Participant()
                        {
                            Competition = useComp,
                            CompetitionClass = useCompClass,
                            StartNumber = newPart.StartNumber,
                            NamePartA = newPart.NamePartA,
                            OrgIdPartA = newPart.OrgIdPartA,
                            NamePartB = newPart.NamePartB,
                            OrgIdPartB = newPart.OrgIdPartB,
                            ClubName = newPart.ClubName,
                            OrgIdClub = newPart.OrgIdClub,
                            OrgPointsPartA = newPart.OrgPointsPartA,
                            OrgStartsPartA = newPart.OrgStartsPartA,
                            OrgPointsPartB = newPart.OrgPointsPartB,
                        });

                    useDb.SaveChanges();
                }
                catch (Exception exc)
                {
                    dbTrans.Rollback();

                    Console.WriteLine(
                        "Error during add of '{0}': {1}",
                        newPart,
                        exc);
                    throw;
                }
            }

            dbTrans.Commit();
        }

        [Given(@"following Participants History in ""([^""]*)""")]
        public void GivenFollowingParticipantsHistoryIn(
            string danceCompHelperDb,
            Table table)
        {
            var newParticipantsHistory = table.CreateSet<ParticipantHistoryPoco>();

            var useDb = GetDanceCompetitionHelperDbContext(
                danceCompHelperDb);

            using var dbTrans = useDb.BeginTransaction();

            foreach (var newPartHist in newParticipantsHistory)
            {
                var useComp = useDb.Competitions.FirstOrDefault(
                    x => x.CompetitionName == newPartHist.CompetitionName);

                Assert.That(
                    useComp,
                    Is.Not.Null,
                    "{0} '{1}' not found!",
                    nameof(Competition),
                    newPartHist.CompetitionName);

                var useCompClass = useDb.CompetitionClasses.FirstOrDefault(
                    x => x.CompetitionClassName == newPartHist.CompetitionClassName);

                Assert.That(
                    useCompClass,
                    Is.Not.Null,
                    "{0} '{1}' not found!",
                    nameof(CompetitionClass),
                    newPartHist.CompetitionClassName);

                try
                {
                    useDb.ParticipantsHistory.Add(
                        new ParticipantHistory()
                        {
                            Competition = useComp,
                            CompetitionClass = useCompClass,
                            Version = newPartHist.Version,
                            StartNumber = newPartHist.StartNumber,
                            NamePartA = newPartHist.NamePartA,
                            OrgIdPartA = newPartHist.OrgIdPartA,
                            NamePartB = newPartHist.NamePartB,
                            OrgIdPartB = newPartHist.OrgIdPartB,
                            OrgIdClub = newPartHist.OrgIdClub,
                            OrgPointsPartA = newPartHist.OrgPointsPartA,
                            OrgStartsPartA = newPartHist.OrgStartsPartA,
                            OrgPointsPartB = newPartHist.OrgPointsPartB,
                        });

                    useDb.SaveChanges();
                }
                catch (Exception exc)
                {
                    dbTrans.Rollback();

                    Console.WriteLine(
                        "Error during add of '{0}': {1}",
                        newPartHist,
                        exc);
                    throw;
                }
            }

            dbTrans.Commit();
        }

        #endregion Dance Competition Helper Database

        #region Dance Competition Helper 

        [Given(@"following DanceCompetitionHelper ""([^""]*)""")]
        public void GivenFollowingDanceCompetitionHelper(
            string danceCompHelper)
        {
            GivenFollowingDanceCompDb(
                string.Format(
                    "{0}-db",
                    danceCompHelper));

            var newDanceCompHelper = _useHost.Services
                .GetRequiredService<IDanceCompetitionHelper>();
            newDanceCompHelper.Migrate();

            _scenarioContext.AddToScenarioContext(
                SpecFlowConstants.DanceCompetitionHelper,
                danceCompHelper,
                newDanceCompHelper);

            AddToDispose(
                newDanceCompHelper);
        }

        #endregion Dance Competition Helper 

        [AfterScenario]
        public void AfterScenario()
        {
            DisposeAllCreated();
        }
    }
}
