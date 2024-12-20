using AutoMapper;
using DanceCompetitionHelper.Database;
using DanceCompetitionHelper.Database.Enum;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Database.Test;
using DanceCompetitionHelper.Database.Test.Pocos;
using DanceCompetitionHelper.Database.Test.Pocos.DanceCompetitionHelper;
using DanceCompetitionHelper.OrgImpl.Oetsv;
using DanceCompetitionHelper.Test.Pocos.DanceCompetitionHelper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            _useHost = TestConfiguration.CreateDefaultTestHost(
                GetNewDbName());
        }

        public IMapper GetMapper()
        {
            return _useHost.Services.GetRequiredService<IMapper>();
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
            newDb.MigrateAsync();

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
        public async Task GivenFollowingCompetitionsIn(
            string danceCompHelperDb,
            DataTable table)
        {
            var newComps = table.CreateSet<CompetitionPoco>();
            var useDb = GetDanceCompetitionHelperDbContext(
                danceCompHelperDb);
            var mapper = GetMapper();

            using var dbTrans = await useDb.BeginTransactionAsync(
                CancellationToken.None)
                ?? throw new ArgumentNullException(
                    "dbTrans");

            foreach (var newComp in newComps)
            {
                try
                {
                    useDb.Competitions.Add(
                        mapper.Map<Competition>(
                            newComp
                                .AssertCreate()));

                    await useDb.SaveChangesAsync();
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

            await dbTrans.CommitAsync();
        }

        [Given(@"following Adjudicator Panels in ""([^""]*)""")]
        public async Task GivenFollowingAdjudicatorPanelsIn(
            string danceCompHelperDb,
            DataTable table)
        {
            var newAdjPanels = table.CreateSet<AdjudicatorPanelPoco>();
            var useDb = GetDanceCompetitionHelperDbContext(
                danceCompHelperDb);
            var mapper = GetMapper();

            using var dbTrans = await useDb.BeginTransactionAsync(
                CancellationToken.None)
                ?? throw new ArgumentNullException(
                    "dbTrans");

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
                        $"{nameof(Competition)} '{newAdjPanel.CompetitionName}' not found!");

                    var newAdjPanelEntity = mapper.Map<AdjudicatorPanel>(
                        newAdjPanel
                            .AssertCreate());
                    newAdjPanelEntity.CompetitionId = useComp.CompetitionId;

                    useDb.AdjudicatorPanels.Add(
                        newAdjPanelEntity);

                    await useDb.SaveChangesAsync();
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

            await dbTrans.CommitAsync();
        }

        [Given(@"following Adjudicators in ""([^""]*)""")]
        public async Task GivenFollowingAdjudicatorsIn(
            string danceCompHelperDb,
            DataTable table)
        {
            var newAdjs = table.CreateSet<AdjudicatorPoco>();
            var useDb = GetDanceCompetitionHelperDbContext(
                danceCompHelperDb);

            using var dbTrans = await useDb.BeginTransactionAsync(
                CancellationToken.None)
                ?? throw new ArgumentNullException(
                    "dbTrans");
            var mapper = GetMapper();

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
                        $"{nameof(Competition)} '{newAdj.CompetitionName}' not found!");

                    var useAdjPanelName = newAdj.AdjudicatorPanelName;
                    var useAdjPanel = GetAdjudicatorPanel(
                        useDb,
                        useComp.CompetitionId,
                        useAdjPanelName);

                    Assert.That(
                        useAdjPanel,
                        Is.Not.Null,
                        $"{nameof(AdjudicatorPanel)} '{useAdjPanelName}' not found!");

                    var newAdjEntity = mapper.Map<Adjudicator>(
                        newAdj
                            .AssertCreate());
                    newAdjEntity.AdjudicatorPanelId = useAdjPanel.AdjudicatorPanelId;

                    useDb.Adjudicators.Add(
                        newAdjEntity);

                    useDb.SaveChanges();
                }
                catch (Exception exc)
                {
                    await dbTrans.RollbackAsync();

                    Console.WriteLine(
                        "Error during add of '{0}': {1}",
                        newAdj,
                        exc);
                    throw;
                }
            }

            await dbTrans.CommitAsync();
        }

        [Given(@"following Competition Classes in ""([^""]*)""")]
        public async Task GivenFollowingCompetitionClassesIn(
            string danceCompHelperDb,
            DataTable table)
        {
            // CAUTION: special stuff... otherwise the creation will fail...
            var newCompClasses = SortForCreation(
                table
                    .CreateSet<CompetitionClassPoco>());

            var useDb = GetDanceCompetitionHelperDbContext(
                danceCompHelperDb);
            var mapper = GetMapper();

            using var dbTrans = await useDb.BeginTransactionAsync(
                CancellationToken.None)
                ?? throw new ArgumentNullException(
                    "dbTrans");

            foreach (var newCompClass in newCompClasses)
            {
                var useComp = GetCompetition(
                    useDb,
                    newCompClass.CompetitionName);

                Assert.That(
                    useComp,
                    Is.Not.Null,
                    $"{nameof(Competition)} '{newCompClass.CompetitionName}' not found!");

                var useAdjPanel = GetAdjudicatorPanel(
                    useDb,
                    useComp.CompetitionId,
                    newCompClass.AdjudicatorPanelName);

                Assert.That(
                    useAdjPanel,
                    Is.Not.Null,
                    $"{nameof(AdjudicatorPanel)} '{newCompClass.AdjudicatorPanelName}' not found!");

                var useFollowUpCopmpClass = GetCompetitionClass(
                    useDb,
                    useComp.CompetitionId,
                    newCompClass.FollowUpCompetitionClassName);

                if (string.IsNullOrEmpty(
                    newCompClass.FollowUpCompetitionClassName) == false)
                {
                    Assert.That(
                        useFollowUpCopmpClass,
                        Is.Not.Null,
                        $"Follow Up {nameof(CompetitionClass)} '{newCompClass.FollowUpCompetitionClassName}' not found!");
                }

                try
                {
                    var newComClassEntity = mapper.Map<CompetitionClass>(
                        newCompClass
                            .AssertCreate());

                    newComClassEntity.Competition = useComp;
                    newComClassEntity.FollowUpCompetitionClass = useFollowUpCopmpClass;
                    newComClassEntity.AdjudicatorPanel = useAdjPanel;

                    useDb.CompetitionClasses.Add(
                        newComClassEntity);

                    await useDb.SaveChangesAsync();
                }
                catch (Exception exc)
                {
                    await dbTrans.RollbackAsync();

                    Console.WriteLine(
                        "Error during add of '{0}': {1}",
                        newCompClass,
                        exc);
                    throw;
                }
            }

            await dbTrans.CommitAsync();
        }

        [Given(@"following Competition Classes History in ""([^""]*)""")]
        public async Task GivenFollowingCompetitionClassesHistoryIn(
            string danceCompHelperDb,
            DataTable table)
        {
            var newCompClassesHistory = table.CreateSet<CompetitionClassHistoryPoco>();

            var useDb = GetDanceCompetitionHelperDbContext(
                danceCompHelperDb);
            var mapper = GetMapper();

            using var dbTrans = await useDb.BeginTransactionAsync(
                CancellationToken.None)
                ?? throw new ArgumentNullException(
                    "dbTrans");

            foreach (var newCompClassHist in newCompClassesHistory)
            {
                var useComp = GetCompetition(
                    useDb,
                    newCompClassHist.CompetitionName);

                Assert.That(
                    useComp,
                    Is.Not.Null,
                    $"{nameof(Competition)} '{newCompClassHist.CompetitionName}' not found!");

                try
                {
                    var newCompClassHistEntity = mapper.Map<CompetitionClassHistory>(
                        newCompClassHist
                            .AssertCreate());
                    newCompClassHistEntity.Competition = useComp;

                    useDb.CompetitionClassesHistory.Add(
                        newCompClassHistEntity);

                    await useDb.SaveChangesAsync();
                }
                catch (Exception exc)
                {
                    await dbTrans.RollbackAsync();

                    Console.WriteLine(
                        "Error during add of '{0}': {1}",
                        newCompClassHist,
                        exc);
                    throw;
                }
            }

            await dbTrans.CommitAsync();
        }

        [Given(@"following Competition Venues in ""([^""]*)""")]
        public async Task GivenFollowingCompetitionVenuesIn(
            string danceCompHelperDb,
            DataTable table)
        {
            var newCompVenues = table.CreateSet<CompetitionVenuePoco>();
            var useDb = GetDanceCompetitionHelperDbContext(
                danceCompHelperDb);
            var mapper = GetMapper();

            using var dbTrans = await useDb.BeginTransactionAsync(
                CancellationToken.None)
                ?? throw new ArgumentNullException(
                    "dbTrans");

            foreach (var newCompVenue in newCompVenues)
            {
                try
                {
                    var useComp = GetCompetition(
                        useDb,
                        newCompVenue.CompetitionName);

                    Assert.That(
                        useComp,
                        Is.Not.Null,
                        $"{nameof(Competition)} '{newCompVenue.CompetitionName}' not found!");

                    var newCompVenueEntity = mapper.Map<CompetitionVenue>(
                        newCompVenue
                            .AssertCreate());
                    newCompVenueEntity.CompetitionId = useComp.CompetitionId;

                    useDb.CompetitionVenues.Add(
                        newCompVenueEntity);

                    await useDb.SaveChangesAsync();
                }
                catch (Exception exc)
                {
                    await dbTrans.RollbackAsync();

                    Console.WriteLine(
                        "Error during add of '{0}': {1}",
                        newCompVenue,
                        exc);
                    throw;
                }
            }

            await dbTrans.CommitAsync();
        }

        [Given(@"following Participants in ""([^""]*)""")]
        public async Task GivenFollowingParticipantsIn(
            string danceCompHelperDb,
            DataTable table)
        {
            var newParticipants = table.CreateSet<ParticipantPoco>();
            var useDb = GetDanceCompetitionHelperDbContext(
                danceCompHelperDb);
            var mapper = GetMapper();

            using var dbTrans = await useDb.BeginTransactionAsync(
                CancellationToken.None)
                ?? throw new ArgumentNullException(
                    "dbTrans");

            foreach (var newPart in newParticipants)
            {
                var useComp = GetCompetition(
                    useDb,
                    newPart.CompetitionName);

                Assert.That(
                    useComp,
                    Is.Not.Null,
                    $"{nameof(Competition)} '{newPart.CompetitionName}' not found!");

                var useCompClass = GetCompetitionClass(
                    useDb,
                    useComp.CompetitionId,
                    newPart.CompetitionClassName);

                Assert.That(
                    useCompClass,
                    Is.Not.Null,
                    $"{nameof(CompetitionClass)} '{newPart.CompetitionClassName}' not found!");

                try
                {
                    var newPartEntity = mapper.Map<Participant>(
                        newPart
                            .AssertCreate());

                    newPartEntity.Competition = useComp;
                    newPartEntity.CompetitionClass = useCompClass;

                    useDb.Participants.Add(
                        newPartEntity);

                    await useDb.SaveChangesAsync();
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

            await dbTrans.CommitAsync();
        }

        [Given(@"following Participants History in ""([^""]*)""")]
        public async Task GivenFollowingParticipantsHistoryIn(
            string danceCompHelperDb,
            DataTable table)
        {
            var newParticipantsHistory = table.CreateSet<ParticipantHistoryPoco>();

            var useDb = GetDanceCompetitionHelperDbContext(
                danceCompHelperDb);
            var mapper = GetMapper();

            using var dbTrans = await useDb.BeginTransactionAsync(
                CancellationToken.None)
                ?? throw new ArgumentNullException(
                    "dbTrans");

            foreach (var newPartHist in newParticipantsHistory)
            {
                var useComp = GetCompetition(
                    useDb,
                    newPartHist.CompetitionName);

                Assert.That(
                    useComp,
                    Is.Not.Null,
                    $"{nameof(Competition)} '{newPartHist.CompetitionName}' not found!");

                var useCompClassHist = GetCompetitionClassHistory(
                    useDb,
                    useComp.CompetitionId,
                    newPartHist.CompetitionClassName);

                Assert.That(
                    useCompClassHist,
                    Is.Not.Null,
                    $"{nameof(CompetitionClassHistory)} '{newPartHist.CompetitionClassName}' not found!");

                try
                {
                    var newPartHistEntity = mapper.Map<ParticipantHistory>(
                        newPartHist
                            .AssertCreate());

                    newPartHistEntity.Competition = useComp;
                    newPartHistEntity.CompetitionClassHistory = useCompClassHist;

                    useDb.ParticipantsHistory.Add(
                        newPartHistEntity);

                    await useDb.SaveChangesAsync();
                }
                catch (Exception exc)
                {
                    await dbTrans.RollbackAsync();

                    Console.WriteLine(
                        "Error during add of '{0}': {1}",
                        newPartHist,
                        exc);
                    throw;
                }
            }

            await dbTrans.CommitAsync();
        }

        [Given(@"following Configuration Values in ""([^""]*)""")]
        public async Task GivenFollowingConfigurationExistsIn(
            string danceCompHelperDb,
            DataTable table)
        {
            // CAUTION: special stuff... otherwise the creation will fail...
            var newConfigValues = table
                .CreateSet<ConfigurationValuePoco>();

            var useDb = GetDanceCompetitionHelperDbContext(
                danceCompHelperDb);
            var mapper = GetMapper();

            using var dbTrans = await useDb.BeginTransactionAsync(
                CancellationToken.None)
                ?? throw new ArgumentNullException(
                    "dbTrans");

            foreach (var newCfg in newConfigValues)
            {
                try
                {
                    OrganizationEnum? useOrganization = newCfg.Organization;
                    Guid? useCompId = null;
                    Guid? useCompClassId = null;
                    Guid? useCompVenueId = null;

                    if (string.IsNullOrEmpty(
                        newCfg.CompetitionName) == false)
                    {
                        var useComp = GetCompetition(
                            useDb,
                            newCfg.CompetitionName);

                        Assert.That(
                            useComp,
                            Is.Not.Null,
                            $"{nameof(Competition)} '{newCfg.CompetitionName}' not found!");

                        Assert.That(
                            useComp.Organization,
                            Is.EqualTo(
                                newCfg.Organization),
                            $"{nameof(Competition)} '{newCfg.CompetitionName}' {nameof(useComp.Organization)} missmatch!");

                        useCompId = useComp.CompetitionId;
                    }

                    if (string.IsNullOrEmpty(
                        newCfg.CompetitionClassName) == false)
                    {
                        var useCompClass = GetCompetitionClass(
                            useDb,
                            useCompId ?? Guid.Empty,
                            newCfg.CompetitionClassName);

                        Assert.That(
                            useCompClass,
                            Is.Not.Null,
                            $"{nameof(CompetitionClass)} '{newCfg.CompetitionClassName}' not found!");
                        Assert.That(
                            useCompClass.CompetitionId,
                            Is.EqualTo(
                                useCompId),
                            $"{nameof(CompetitionClass)} '{newCfg.CompetitionClassName}' ID missmatch!");

                        useCompClassId = useCompClass.CompetitionClassId;
                    }

                    if (string.IsNullOrEmpty(
                        newCfg.CompetitionVenueName) == false)
                    {
                        var useCompVenue = GetCompetitionVenue(
                            useDb,
                            useCompId ?? Guid.Empty,
                            newCfg.CompetitionVenueName);

                        Assert.That(
                            useCompVenue,
                            Is.Not.Null,
                            $"{nameof(CompetitionVenue)} '{newCfg.CompetitionVenueName}' not found!");
                        Assert.That(
                            useCompVenue.CompetitionId,
                            Is.EqualTo(
                                useCompId),
                            $"{nameof(CompetitionVenue)} '{newCfg.CompetitionVenueName}' ID missmatch!");

                        useCompVenueId = useCompVenue.CompetitionVenueId;
                    }

                    newCfg.SanityCheck();

                    var newCfgEntity = mapper.Map<ConfigurationValue>(
                        newCfg
                            .AssertCreate());

                    newCfgEntity.Organization = useOrganization;
                    newCfgEntity.CompetitionId = useCompId;
                    newCfgEntity.CompetitionClassId = useCompClassId;
                    newCfgEntity.CompetitionVenueId = useCompVenueId;

                    useDb.Configurations.Add(
                        newCfgEntity);

                    await useDb.SaveChangesAsync();
                }
                catch (Exception exc)
                {
                    await dbTrans.RollbackAsync();

                    Console.WriteLine(
                        "Error during add of '{0}': {1}",
                        newCfg,
                        exc);
                    throw;
                }
            }

            await dbTrans.CommitAsync();
        }


        #endregion Dance Competition Helper Database

        #region Dance Competition Helper 

        [Given(@"following DanceCompetitionHelper ""([^""]*)""")]
        public async Task GivenFollowingDanceCompetitionHelper(
            string danceCompHelper)
        {
            GivenFollowingDanceCompDb(
                string.Format(
                    "{0}-db",
                    danceCompHelper));

            var newDanceCompHelper = _useHost.Services
                .GetRequiredService<IDanceCompetitionHelper>();
            await newDanceCompHelper.MigrateAsync();
            await newDanceCompHelper.CheckMandatoryConfigurationAsync(
                CancellationToken.None);

            _scenarioContext.AddToScenarioContext(
                SpecFlowConstants.DanceCompetitionHelper,
                danceCompHelper,
                newDanceCompHelper);

            AddToDispose(
                newDanceCompHelper);
        }

        [Given(@"following data are imported by DanceCompetitionHelper ""([^""]*)""")]
        public async Task GivenFollowingDataAreImportedByDanceCompetitionHelper(
            string danceCompHelper,
            DataTable table)
        {
            var compsToImport = table.CreateSet<CompetitionImportPoco>();
            var useDanceCompHelper = GetDanceCompetitionHelper(
                danceCompHelper);

            var rootPath = AssemblyExtensions.GetAssemblyPath() ?? string.Empty;

            foreach (var toImport in compsToImport)
            {
                var useParams = new Dictionary<string, string>();

                if (toImport.FindFollowUpClasses)
                {
                    useParams.Add(
                        nameof(OetsvCompetitionImporter.FindFollowUpClasses),
                        "true");
                }

                await useDanceCompHelper.ImportOrUpdateCompetitionAsync(
                    toImport.Organization,
                    toImport.OrgCompetitionId,
                    ImportTypeEnum.Excel,
                    CancellationToken.None,
                    new[] {
                        Path.Combine(
                            rootPath,
                            toImport.CompetitionFile ?? string.Empty),
                        Path.Combine(
                            rootPath,
                            toImport.ParticipantsFile ?? string.Empty)
                    },
                    useParams);
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
