using DanceCompetitionHelper.Config;
using DanceCompetitionHelper.Database;
using DanceCompetitionHelper.Database.Config;
using DanceCompetitionHelper.Database.Diagnostic;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Database.Test.Pocos;
using DanceCompetitionHelper.Database.Test.Pocos.DanceCompetitionHelper;
using DanceCompetitionHelper.OrgImpl.Oetsv;
using DanceCompetitionHelper.Test.Pocos.DanceCompetitionHelper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using TestHelper.Extensions;

namespace DanceCompetitionHelper.Test.Bindings
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

        public string GetNewDbName(
            bool incrementDatabaseId = true)
        {
            var dbId = incrementDatabaseId
                ? Interlocked.Increment(ref _databaseId)
                : (Interlocked.Read(ref _databaseId) + 1);

            return string.Format(
                "{0}_{1}_{2}.sqlite",
                UseNow.ToString("yyyyMMdd_HHmmss"),
                nextDanceCompHelperDb,
                dbId);
        }

        [Given(@"following DanceComp-DB ""([^""]*)""")]
        public void GivenFollowingDanceCompDb(
            string danceCompHelperDb)
        {
            nextDanceCompHelperDb = danceCompHelperDb;
            var newDbFile = GetNewDbName(
                false);

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

        [Given(@"following Adjudicator Panels in ""([^""]*)""")]
        public void GivenFollowingAdjudicatorPanelsIn(
            string danceCompHelperDb,
            Table table)
        {
            var newAdjPanels = table.CreateSet<AdjudicatorPanelPoco>();
            var useDb = GetDanceCompetitionHelperDbContext(
                danceCompHelperDb);

            using var dbTrans = useDb.BeginTransaction();

            foreach (var newAdjPanel in newAdjPanels)
            {
                try
                {
                    var useComp = GetCompetition(
                        useDb,
                        newAdjPanel.CompetitionName);

                    Assert.That(
                        useComp,
                        Is.Not.Null,
                        "{0} '{1}' not found!",
                        nameof(Competition),
                        newAdjPanel.CompetitionName);


                    useDb.AdjudicatorPanels.Add(
                        new AdjudicatorPanel()
                        {
                            CompetitionId = useComp.CompetitionId,
                            Name = newAdjPanel.Name,
                            Comment = newAdjPanel.Comment,
                        });

                    useDb.SaveChanges();
                }
                catch (Exception exc)
                {
                    dbTrans.Rollback();

                    Console.WriteLine(
                        "Error during add of '{0}': {1}",
                        newAdjPanel,
                        exc);
                    throw;
                }
            }

            dbTrans.Commit();
        }

        [Given(@"following Adjudicators in ""([^""]*)""")]
        public void GivenFollowingAdjudicatorsIn(
            string danceCompHelperDb,
            Table table)
        {
            var newAdjs = table.CreateSet<AdjudicatorPoco>();
            var useDb = GetDanceCompetitionHelperDbContext(
                danceCompHelperDb);

            using var dbTrans = useDb.BeginTransaction();

            foreach (var newAdj in newAdjs)
            {
                try
                {
                    var useComp = GetCompetition(
                    useDb,
                    newAdj.CompetitionName);

                    Assert.That(
                        useComp,
                        Is.Not.Null,
                        "{0} '{1}' not found!",
                        nameof(Competition),
                        newAdj.CompetitionName);

                    var useAdjPanelName = newAdj.AdjudicatorPanelName;
                    var useAdjPanel = GetAdjudicatorPanel(
                        useDb,
                        useComp.CompetitionId,
                        useAdjPanelName);

                    Assert.That(
                        useAdjPanel,
                        Is.Not.Null,
                        "{0} '{1}' not found!",
                        nameof(AdjudicatorPanel),
                        useAdjPanelName);

                    useDb.Adjudicators.Add(
                        new Adjudicator()
                        {
                            AdjudicatorPanelId = useAdjPanel.AdjudicatorPanelId,
                            Abbreviation = newAdj.Abbreviation,
                            Name = newAdj.Name,
                            Comment = newAdj.Comment,
                        });

                    useDb.SaveChanges();
                }
                catch (Exception exc)
                {
                    dbTrans.Rollback();

                    Console.WriteLine(
                        "Error during add of '{0}': {1}",
                        newAdj,
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
                var useComp = GetCompetition(
                    useDb,
                    newCompClass.CompetitionName);

                Assert.That(
                    useComp,
                    Is.Not.Null,
                    "{0} '{1}' not found!",
                    nameof(Competition),
                    newCompClass.CompetitionName);

                var useAdjPanel = GetAdjudicatorPanel(
                    useDb,
                    useComp.CompetitionId,
                    newCompClass.AdjudicatorPanelName);

                Assert.That(
                    useAdjPanel,
                    Is.Not.Null,
                    "{0} '{1}' not found!",
                    nameof(AdjudicatorPanel),
                    newCompClass.AdjudicatorPanelName);

                try
                {
                    useDb.CompetitionClasses.Add(
                        new CompetitionClass()
                        {
                            Competition = useComp,
                            OrgClassId = newCompClass.OrgClassId,
                            CompetitionClassName = newCompClass.CompetitionClassName,
                            AdjudicatorPanel = useAdjPanel,
                            Discipline = newCompClass.Discipline,
                            AgeClass = newCompClass.AgeClass,
                            AgeGroup = newCompClass.AgeGroup,
                            Class = newCompClass.Class,

                            MinStartsForPromotion = newCompClass.MinStartsForPromotion ?? 0,
                            MinPointsForPromotion = newCompClass.MinPointsForPromotion ?? 0,

                            PointsForFirst = newCompClass.PointsForFirst ?? 0.0,
                            ExtraManualStarter = newCompClass.ExtraManualStarter,
                            Comment = newCompClass.Comment,
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
                var useComp = GetCompetition(
                    useDb,
                    newCompClassHist.CompetitionName);

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

                            PointsForFirst = newCompClassHist.PointsForFirst,
                            ExtraManualStarter = newCompClassHist.ExtraManualStarter,
                            Comment = newCompClassHist.Comment,
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
                var useComp = GetCompetition(
                    useDb,
                    newPart.CompetitionName);

                Assert.That(
                    useComp,
                    Is.Not.Null,
                    "{0} '{1}' not found!",
                    nameof(Competition),
                    newPart.CompetitionName);

                var useCompClass = GetCompetitionClass(
                    useDb,
                    useComp.CompetitionId,
                    newPart.CompetitionClassName);

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
                var useComp = GetCompetition(
                    useDb,
                    newPartHist.CompetitionName);

                Assert.That(
                    useComp,
                    Is.Not.Null,
                    "{0} '{1}' not found!",
                    nameof(Competition),
                    newPartHist.CompetitionName);

                // TODO: wrong table!..
                var useCompClassHist = GetCompetitionClass(
                    useDb,
                    useComp.CompetitionId,
                    newPartHist.CompetitionClassName);

                Assert.That(
                    useCompClassHist,
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
                            // CompetitionClassHistory = useCompClassHist,
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

        [Given(@"following data are imported by DanceCompetitionHelper ""([^""]*)""")]
        public void GivenFollowingDataAreImportedByDanceCompetitionHelper(
            string danceCompHelper,
            Table table)
        {
            var compsToImport = table.CreateSet<CompetitionImportPoco>();
            var useDanceCompHelper = GetDanceCompetitionHelper(
                danceCompHelper);

            var rootPath = AssemblyExtensions.GetAssemblyPath() ?? string.Empty;

            foreach (var toImport in compsToImport)
            {
                useDanceCompHelper.ImportOrUpdateCompetition(
                    toImport.Organization,
                    toImport.OrgCompetitionId,
                    Database.Enum.ImportTypeEnum.Excel,
                    new[] {
                        Path.Combine(
                            rootPath,
                            toImport.CompetitionFile ?? string.Empty),
                        Path.Combine(
                            rootPath,
                            toImport.ParticipantsFile ?? string.Empty)
                    });
            }
        }

        #endregion Dance Competition Helper 

        [AfterScenario]
        public void AfterScenario()
        {
            DisposeAllCreated();
        }
    }
}
