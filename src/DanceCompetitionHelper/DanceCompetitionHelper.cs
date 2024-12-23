using DanceCompetitionHelper.Data;
using DanceCompetitionHelper.Database;
using DanceCompetitionHelper.Database.DisplayInfo;
using DanceCompetitionHelper.Database.Enum;
using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Exceptions;
using DanceCompetitionHelper.Extensions;
using DanceCompetitionHelper.Helper;
using DanceCompetitionHelper.OrgImpl;
using DanceCompetitionHelper.OrgImpl.Oetsv;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System.Runtime.CompilerServices;

namespace DanceCompetitionHelper
{
    public class DanceCompetitionHelper : IDanceCompetitionHelper
    {
        private readonly DanceCompetitionHelperDbContext _danceCompHelperDb;
        private readonly ILogger<DanceCompetitionHelperDbContext> _logger;
        private readonly IServiceProvider _serviceProvider;

        public static readonly string DanceCompetitionHelperInfoString = string.Format(
            "DanceCompetitionHelper (c) 2022-{0}",
            DateTime.Now.ToString("yyyy"));

        public IReadOnlyList<ConfigurationValue> MandatoryConfigurationValues { get; } = new List<ConfigurationValue>()
        {
            // GLOBAL
            new ConfigurationValue()
            {
                Key = GlobalConfigurationConstants.MaxCouplesPerHeat,
                Value = "6"
            },
            new ConfigurationValue()
            {
                Key = GlobalConfigurationConstants.MinCooldownTimePerRound,
                Value = "10:00"
            },
            new ConfigurationValue()
            {
                Key = GlobalConfigurationConstants.MinChangeClothesTime,
                Value = "15:00"
            },
            // Oetsv
            new ConfigurationValue()
            {
                Organization = OrganizationEnum.Oetsv,
                Key = OetsvConfigurationConstants.MinTimePerDance,
                Value = "1:30"
            },
        }.AsReadOnly();

        public DanceCompetitionHelper(
            DanceCompetitionHelperDbContext danceCompHelperDb,
            ILogger<DanceCompetitionHelperDbContext> logger,
            IServiceProvider serviceProvider)
        {
            _danceCompHelperDb = danceCompHelperDb
                ?? throw new ArgumentNullException(
                    nameof(danceCompHelperDb));
            _logger = logger
                ?? throw new ArgumentNullException(
                    nameof(logger));
            _serviceProvider = serviceProvider
                ?? throw new ArgumentNullException(
                    nameof(serviceProvider));
        }

        #region Administration stuff

        public DanceCompetitionHelperDbContext GetDbCtx()
        {
            return _danceCompHelperDb;
        }

        public Task AddTestDataAsync(
            CancellationToken cancellationToken)
        {
            _logger.LogDebug(
                "Do '{Method}'",
                nameof(AddTestDataAsync));

            return _danceCompHelperDb.AddTestData(
                cancellationToken);
        }

        public Task MigrateAsync()
        {
            _logger.LogDebug(
                "Do '{Method}'",
                nameof(MigrateAsync));

            return _danceCompHelperDb.MigrateAsync();
        }

        public async Task CheckMandatoryConfigurationAsync(
            CancellationToken cancellationToken)
        {
            _logger.LogDebug(
                "Do '{Method}'",
                nameof(CheckMandatoryConfigurationAsync));

            // THIS "BeginTransactionAsync()" IS ON PURPOSE!..
            // DO not remove!..
            using var dbTrans = await _danceCompHelperDb.BeginTransactionAsync(
                cancellationToken)
                ?? throw new ArgumentNullException(
                    "dbTrans");

            try
            {
                foreach (var mandConfig in MandatoryConfigurationValues)
                {
                    var retConfig = await GetConfigurationAsync(
                        mandConfig,
                        cancellationToken);

                    if (retConfig == null)
                    {
                        _danceCompHelperDb.Configurations.Add(
                            mandConfig);
                    }
                }

                await _danceCompHelperDb.SaveChangesAsync(
                    cancellationToken);
                await dbTrans.CommitAsync(
                    cancellationToken);
            }
            catch (Exception exc)
            {
                _logger.LogError(
                    exc,
                    "{Method} failed: {ErrorMessage}",
                    nameof(CheckMandatoryConfigurationAsync),
                    exc.Message);

                await (dbTrans?.RollbackAsync() ?? Task.CompletedTask);
            }
        }

        public async Task CreateTableHistoryAsync(
            Guid competitionId,
            CancellationToken cancellationToken,
            string comment = "Generated by User")
        {
            var foundComp = await GetCompetitionAsync(
                competitionId,
                cancellationToken);
            /*
            _danceCompHelperDb.Competitions
                .TagWith(
                    nameof(CreateTableHistory) + "(Guid)[0]")
                .FirstOrDefault(
                    x => x.CompetitionId == competitionId);
            */

            if (foundComp == null)
            {
                _logger.LogWarning(
                    "{Competition} with {CompetitionIdName} '{CompetitionId}' unknown. Do not generate any History",
                    nameof(Competition),
                    nameof(Competition.CompetitionId),
                    competitionId);

                return;
            }

            var tableHistCreator = _serviceProvider.GetRequiredService<TableHistoryCreator>();

            tableHistCreator.CreateHistory(
                _danceCompHelperDb,
                foundComp.CompetitionId,
                comment);

        }

        #endregion Administration stuff

        public async Task<Competition?> GetCompetitionAsync(
            Guid? competitionId,
            CancellationToken cancellationToken)
        {
            if (competitionId == null)
            {
                return null;
            }

            return await _danceCompHelperDb.Competitions
                .TagWith(
                    nameof(GetCompetitionAsync) + "(Guid)[0]")
                .FirstOrDefaultAsync(
                    x => x.CompetitionId == competitionId,
                    cancellationToken);
        }

        public async IAsyncEnumerable<Competition> GetCompetitionsAsync(
            [EnumeratorCancellation] CancellationToken cancellationToken,
            bool includeInfos = false)
        {
            var countsOfCompClasses = new Dictionary<Guid, int>();
            var countsOfParticipants = new Dictionary<Guid, int>();

            if (includeInfos)
            {
                countsOfCompClasses = await _danceCompHelperDb.CompetitionClasses
                    .TagWith(
                        nameof(GetCompetitionsAsync) + "(bool?)[0]")
                    .Include(
                        x => x.AdjudicatorPanel)
                    .Where(
                        x => x.Ignore == false)
                    .GroupBy(
                        x => x.CompetitionId,
                        (compId, items) => new
                        {
                            CompetitionId = compId,
                            Count = items.Count(),
                        })
                    .ToDictionaryAsync(
                        x => x.CompetitionId,
                        x => x.Count,
                        cancellationToken);

                countsOfParticipants = await _danceCompHelperDb.Participants
                    .TagWith(
                        nameof(GetCompetitionsAsync) + "(bool?)[1]")
                    .Where(
                        x => x.Ignore == false)
                    .GroupBy(
                        x => x.CompetitionId,
                        (compId, items) => new
                        {
                            CompetitionId = compId,
                            Count = items.Count(),
                        })
                    .ToDictionaryAsync(
                        x => x.CompetitionId,
                        x => x.Count,
                        cancellationToken);
            }

            await foreach (var curComp in _danceCompHelperDb.Competitions
                .TagWith(
                    nameof(GetCompetitionsAsync) + "(bool?)[2]")
                .OrderByDescending(
                    x => x.CompetitionDate)
                .ThenBy(
                    x => x.OrgCompetitionId)
                .AsAsyncEnumerable())
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    yield break;
                }

                if (includeInfos)
                {
                    // CAUTION: EF is caching, if we dont want wrong values
                    // we need to recreate this!..
                    curComp.DisplayInfo = new CompetitionDisplayInfo();

                    var useDisplayInfo = curComp.DisplayInfo;

                    if (countsOfCompClasses.TryGetValue(
                        curComp.CompetitionId,
                        out var countCompClasses))
                    {
                        useDisplayInfo.CountCompetitionClasses = countCompClasses;
                    }

                    if (countsOfParticipants.TryGetValue(
                        curComp.CompetitionId,
                        out var countParticipants))
                    {
                        useDisplayInfo.CountParticipants = countParticipants;
                    }

                    useDisplayInfo.CountMultipleStarters = await GetMultipleStarterAsync(
                        curComp.CompetitionId,
                        cancellationToken)
                        .CountAsync();
                    useDisplayInfo.CountAdjudicatorPanels = await _danceCompHelperDb.AdjudicatorPanels
                        .TagWith(
                            nameof(GetCompetitionsAsync) + " " + nameof(Adjudicator) + ".Count")
                        .CountAsync(
                            x => x.CompetitionId == curComp.CompetitionId,
                            cancellationToken);

                    var helpAllItems = await _danceCompHelperDb.TableVersionInfos
                        .TagWith(
                            nameof(GetCompetitionsAsync) + " " + nameof(TableVersionInfo) + ".Count")
                        .Where(
                            x => x.CompetitionId == curComp.CompetitionId)
                        .Select(
                            x => x.CurrentVersion)
                        .ToListAsync(
                            cancellationToken);

                    if (helpAllItems.Count >= 1)
                    {
                        useDisplayInfo.CountVersions = helpAllItems.Max();
                    }
                }

                yield return curComp;
            }
        }

        public async IAsyncEnumerable<CompetitionClass> GetCompetitionClassesAsync(
            Guid? competitionId,
            [EnumeratorCancellation] CancellationToken cancellationToken,
            bool includeInfos = false,
            bool showAll = false)
        {
            if (competitionId == null)
            {
                yield break;
            }

            var foundComp = await GetCompetitionAsync(
                competitionId,
                cancellationToken);

            if (foundComp == null)
            {
                yield break;
            }

            var partsByCompClass = new Dictionary<Guid, List<Participant>>();
            var multiStartsByCompClass = new Dictionary<Guid, List<Participant>>();

            ICompetitonClassChecker? compClassChecker = GetCompetitonClassChecker(
                foundComp);
            var higherClassificationsByLowerCompClassId = new Dictionary<Guid, CompetitionClass>();
            var foundPromotionFromCompClass = new Dictionary<Guid, List<Participant>>();

            var getAllCompClasses = _danceCompHelperDb.CompetitionClasses
                .TagWith(
                    nameof(GetCompetitionClassesAsync) + "(Guid?)[2]")
                .Include(
                    x => x.AdjudicatorPanel)
                .Where(
                    x => x.CompetitionId == foundComp.CompetitionId)
                .OrderBy(
                    x => x.OrgClassId)
                .ThenBy(
                    x => x.CompetitionClassName);

            var allCompetitionClasses = await (showAll
                ? getAllCompClasses
                : getAllCompClasses
                    .Where(
                        x => x.Ignore == false))
                .ToListAsync(
                    cancellationToken);

            // cleanup "PreviousCompetitionClass"
            foreach (var curCompClass in allCompetitionClasses)
            {
                curCompClass.PreviousCompetitionClass = null;
            }

            // collect "higher classes"...
            foreach (var curCompClass in allCompetitionClasses)
            {
                var useFollowUpComp = curCompClass.FollowUpCompetitionClass;
                if (useFollowUpComp == null
                    // must not be "me"
                    || curCompClass == useFollowUpComp)
                {
                    continue;
                }

                useFollowUpComp.PreviousCompetitionClass = curCompClass;

                higherClassificationsByLowerCompClassId[curCompClass.CompetitionClassId]
                    = useFollowUpComp;
            }

            if (includeInfos)
            {
                await foreach (var curPart in GetParticipantsAsync(
                    foundComp.CompetitionId,
                    null,
                    cancellationToken,
                    true)
                    .OrderBy(
                        x => x.CompetitionId)
                    .ThenByDefault())
                {
                    partsByCompClass.AddToBucket(
                        curPart.CompetitionClassId,
                        curPart);

                    var usePromotionInfo = curPart.DisplayInfo?.PromotionInfo;
                    if (usePromotionInfo != null)
                    {
                        if (usePromotionInfo.PossiblePromotionA
                            || (usePromotionInfo.PossiblePromotionB ?? false))
                        {
                            if (higherClassificationsByLowerCompClassId.TryGetValue(
                                curPart.CompetitionClassId,
                                out var foundHigherClasses))
                            {
                                foundPromotionFromCompClass.AddToBucket(
                                    foundHigherClasses.CompetitionClassId,
                                    curPart);
                            }
                        }
                    }
                }

                var multipleStarters = await GetMultipleStarterAsync(
                    competitionId.Value,
                    cancellationToken)
                    .ToListAsync(
                        cancellationToken);

                foreach (var curMultiStart in multipleStarters)
                {
                    foreach (var multiCompClasss in curMultiStart.CompetitionClasses)
                    {
                        foreach (var multiPart in curMultiStart.Participants
                            .Where(
                                x => x.CompetitionClassId == multiCompClasss.CompetitionClassId))
                        {
                            multiStartsByCompClass.AddToBucket(
                                multiCompClasss.CompetitionClassId,
                                multiPart,
                                true);
                        }
                    }
                }
            }

            // fill "displayInfo"...
            foreach (var curCompClass in allCompetitionClasses)
            {
                if (includeInfos)
                {
                    // CAUTION: EF is caching, if we dont want wrong values
                    // we need to re-create this!..
                    curCompClass.DisplayInfo = new CompetitionClassDisplayInfo();

                    var useDisplayInfo = curCompClass.DisplayInfo;
                    var useExtraPart = useDisplayInfo.ExtraParticipants;
                    var useCompClassId = curCompClass.CompetitionClassId;

                    useDisplayInfo.MaxCouplesPerHeat = Convert.ToInt16(
                        (await GetConfigurationAsync(
                            GlobalConfigurationConstants.MaxCouplesPerHeat,
                            curCompClass,
                            cancellationToken))
                        ?.ValueParser
                        ?.AsDecimal()
                        ?? 1);

                    if (partsByCompClass.TryGetValue(
                        useCompClassId,
                        out var participants))
                    {
                        curCompClass.DisplayInfo.Participants.AddRange(
                            participants);
                    }

                    multiStartsByCompClass.TryGetValue(
                        useCompClassId,
                        out var curCntMultiStarter);
                    useDisplayInfo.CountMultipleStarters = curCntMultiStarter?.Count ?? 0;
                    useDisplayInfo.CountMultipleStartersInfo = curCntMultiStarter.GetStartNumber();
                }
            }

            // fill "Extra Part"...
            foreach (var curCompClass in allCompetitionClasses)
            {
                var useDisplayInfo = curCompClass.DisplayInfo;

                if (useDisplayInfo != null
                    && includeInfos)
                {
                    var useExtraPart = useDisplayInfo.ExtraParticipants;
                    var useCompClassId = curCompClass.CompetitionClassId;
                    var validClasses = new List<CompetitionClass>();

                    var usePrevCompClass = curCompClass.PreviousCompetitionClass;
                    if (usePrevCompClass != null
                        // must no be me...
                        && curCompClass != usePrevCompClass
                        /* TODO: filter her?..
                        && usePrevCompClass.Ignore == false
                        */)
                    {
                        // --- BY WINNING ---
                        // TODO: filter her?.. ?? only if we got a "running comp"...
                        if (usePrevCompClass.DisplayInfo?.Participants.Count >= 1)
                        {
                            validClasses.Add(
                                usePrevCompClass);
                        }

                        useExtraPart.ByWinning += validClasses.Count;
                        useExtraPart.ByWinningInfo += validClasses.GetCompetitionClasseNames();

                        // --- BY PROMOTION ---
                        if (foundPromotionFromCompClass.TryGetValue(
                            useCompClassId,
                            out var possiblePromotions))
                        {
                            useExtraPart.ByPromotion += possiblePromotions.Count;
                            useExtraPart.ByPromotionInfo += possiblePromotions.GetStartNumber();
                        }
                    }
                }

                yield return curCompClass;
            }
        }

        public async IAsyncEnumerable<CompetitionVenue> GetCompetitionVenuesAsync(
            Guid? competitionId,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            if (competitionId == null)
            {
                yield break;
            }


            var foundComp = GetCompetitionAsync(
                competitionId,
                cancellationToken);

            if (foundComp == null)
            {
                yield break;
            }

            // we do only basic info..
            await foreach (var curVenue in _danceCompHelperDb.CompetitionVenues
                .TagWith(
                    nameof(GetCompetitionVenuesAsync))
                .Where(
                    x => x.CompetitionId == competitionId)
                .OrderBy(
                    x => x.Name)
                .ToAsyncEnumerable())
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    yield break;
                }

                yield return curVenue;
            }
        }

        public async IAsyncEnumerable<Participant> GetParticipantsAsync(
            Guid? competitionId,
            Guid? competitionClassId,
            [EnumeratorCancellation] CancellationToken cancellationToken,
            bool includeInfos = false,
            bool showAll = false)
        {
            if (competitionId == null)
            {
                yield break;
            }

            var foundComp = await GetCompetitionAsync(
                competitionId,
                cancellationToken);

            if (foundComp == null)
            {
                yield break;
            }

            // we do only basic info..
            var foundCompClass = await _danceCompHelperDb.CompetitionClasses
                .TagWith(
                    nameof(GetParticipantsAsync) + "(Guid?, Guid?, bool)[1]")
                .FirstOrDefaultAsync(
                    x => x.CompetitionId == competitionId
                    && x.CompetitionClassId == competitionClassId);

            var multiStarter = new List<MultipleStarter>();
            var multiStarterByParticipantsId = new Dictionary<Guid, List<CompetitionClass>>();
            var allCompClasses = new List<CompetitionClass>();
            IParticipantChecker? participantChecker = null;

            if (includeInfos)
            {
                allCompClasses.AddRange(
                    // we only need basic infos here...
                    _danceCompHelperDb.CompetitionClasses
                        .TagWith(
                            nameof(GetParticipantsAsync) + "(Guid?, Guid?, bool)[1]")
                        .Where(
                            x => x.CompetitionId == competitionId));

                multiStarter.AddRange(
                    await GetMultipleStarterAsync(
                        foundComp.CompetitionId,
                        cancellationToken)
                    .ToListAsync(
                        cancellationToken));

                multiStarterByParticipantsId =
                    multiStarter.SelectMany(
                        x => x.GetCompetitionClassesOfParticipants())
                    .ToDictionary(
                        x => x.ParticipantId,
                        x => x.CompetitionClass);

                participantChecker = GetParticipantChecker(
                    foundComp);

                if (participantChecker != null)
                {
                    participantChecker.SetCompetitionClasses(
                        allCompClasses);
                    participantChecker.SetMultipleStarter(
                        multiStarter);
                }
            }

            var qryParticipants = _danceCompHelperDb.Participants
                .TagWith(
                    nameof(GetParticipantsAsync) + "(Guid?, Guid?, bool)[2]")
                .Include(
                    x => x.CompetitionClass)
                .Where(
                    // "Ignore" is checked below!
                    x => x.CompetitionId == foundComp.CompetitionId);

            if (showAll == false)
            {
                qryParticipants = qryParticipants.Where(
                    x => x.Ignore == false);
            }

            if (foundCompClass != null)
            {
                qryParticipants = qryParticipants.Where(
                    x => x.CompetitionClassId == foundCompClass.CompetitionClassId);
            }

            await foreach (var curPart in qryParticipants
                .OrderBy(
                    x => x.CompetitionClass.OrgClassId)
                .ThenBy(
                    x => x.CompetitionClass.CompetitionClassName)
                .ThenByDefault()
                .AsAsyncEnumerable())
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    yield break;
                }

                if (includeInfos)
                {
                    // CAUTION: EF is caching, if we dont want wrong values
                    // we need to recreate this!..
                    curPart.DisplayInfo = new ParticipantDisplayInfo();

                    var useDisplayInfo = curPart.DisplayInfo;
                    useDisplayInfo.MultipleStartInfo = new CheckMultipleStartInfo();

                    if (multiStarterByParticipantsId.TryGetValue(
                        curPart.ParticipantId,
                        out var curClassInfos))
                    {
                        var useMultiStartInfo = useDisplayInfo.MultipleStartInfo;
                        useMultiStartInfo.MultipleStarts = true;
                        useMultiStartInfo.MultipleStartsInfo =
                            curClassInfos.GetCompetitionClasseNames();
                        useMultiStartInfo.IncludedCompetitionClasses.AddRange(
                            curClassInfos);
                    }

                    if (participantChecker != null)
                    {
                        useDisplayInfo.PromotionInfo = participantChecker.CheckParticipantPromotion(
                            curPart);
                    }
                }

                yield return curPart;
            }
        }

        public async IAsyncEnumerable<AdjudicatorPanel> GetAdjudicatorPanelsAsync(
            Guid? competitionId,
            [EnumeratorCancellation] CancellationToken cancellationToken,
            bool includeInfos = false)
        {
            if (competitionId == null)
            {
                yield break;
            }

            var foundComp = await GetCompetitionAsync(
                competitionId,
                cancellationToken);

            if (foundComp == null)
            {
                yield break;
            }

            await foreach (var curAdjPanel in _danceCompHelperDb.AdjudicatorPanels
                .TagWith(
                    nameof(GetAdjudicatorPanelsAsync) + "(Guid?, bool)[0]")
                .Where(
                    x => x.CompetitionId == foundComp.CompetitionId)
                .AsAsyncEnumerable())
            {
                if (includeInfos)
                {
                    curAdjPanel.DisplayInfo = new AdjudicatorPanelDisplayInfos();
                    var useDisplayInfo = curAdjPanel.DisplayInfo;

                    useDisplayInfo.CountAdjudicators = await _danceCompHelperDb.Adjudicators
                        .TagWith(
                            nameof(GetAdjudicatorPanelsAsync) + "(Guid?, bool)[1]")
                        .CountAsync(
                            x => x.AdjudicatorPanelId == curAdjPanel.AdjudicatorPanelId);
                }

                yield return curAdjPanel;
            }
        }

        public async IAsyncEnumerable<Adjudicator> GetAdjudicatorsAsync(
            Guid? competitionId,
            Guid? adjudicatorPanelId,
            [EnumeratorCancellation] CancellationToken cancellationToken,
            bool includeInfos = false)
        {
            if (competitionId == null)
            {
                yield break;
            }

            var foundComp = await GetCompetitionAsync(
                competitionId,
                cancellationToken);

            if (foundComp == null)
            {
                yield break;
            }

            var useAdjPanels = new List<Guid>();

            if (adjudicatorPanelId == null)
            {
                useAdjPanels.AddRange(
                    await _danceCompHelperDb.AdjudicatorPanels
                        .TagWith(
                            nameof(GetAdjudicatorsAsync) + "(Guid?, Guid?)[1]")
                        .Where(
                            x => x.CompetitionId == foundComp.CompetitionId)
                        .OrderBy(
                            x => x.Name)
                        .Select(
                            x => x.AdjudicatorPanelId)
                        .ToListAsync(
                            cancellationToken));
            }
            else
            {
                var foundAdjPanel = await _danceCompHelperDb.AdjudicatorPanels
                    .TagWith(
                        nameof(GetAdjudicatorsAsync) + "(Guid?, Guid?)[3]")
                    .FirstOrDefaultAsync(
                        x => x.CompetitionId == foundComp.CompetitionId,
                        cancellationToken);

                if (foundAdjPanel == null)
                {
                    yield break;
                }

                useAdjPanels.Add(
                    foundAdjPanel.AdjudicatorPanelId);
            }

            foreach (var curAdjPanelId in useAdjPanels.Distinct())
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    yield break;
                }

                await foreach (var curRetAdj in _danceCompHelperDb.Adjudicators
                    .TagWith(
                        nameof(GetAdjudicatorsAsync) + "(Guid?, Guid?)[4]")
                    .Include(
                        x => x.AdjudicatorPanel)
                    .Where(
                        x => x.AdjudicatorPanelId == curAdjPanelId)
                    .OrderBy(
                        x => x.Abbreviation)
                    .AsAsyncEnumerable())
                {
                    yield return curRetAdj;
                }
            }
        }

        public async IAsyncEnumerable<MultipleStarter> GetMultipleStarterAsync(
            Guid competitionId,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var foundComp = await GetCompetitionAsync(
                competitionId,
                cancellationToken);

            if (foundComp == null)
            {
                yield break;
            }

            var multipleStarterGouping = _danceCompHelperDb.Participants
                .TagWith(
                    nameof(GetMultipleStarterAsync) + "(Guid)[0]")
                .Where(
                    x => x.CompetitionId == foundComp.CompetitionId
                    && x.Ignore == false)
                .GroupBy(
                    x => new
                    {
                        x.NamePartA,
                        x.OrgIdPartA,
                        x.NamePartB,
                        x.OrgIdPartB,
                        x.ClubName,
                        x.OrgIdClub,
                    })
                .Where(
                    x => x.Count() >= 2)
                .Select(
                    x => x.Key);

            await foreach (var curMultiStart in multipleStarterGouping
                .AsAsyncEnumerable())
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    yield break;
                }

                var allPartInfo = await _danceCompHelperDb.Participants
                    .TagWith(
                        nameof(GetMultipleStarterAsync) + "(Guid)[1]")
                    .Include(
                        x => x.CompetitionClass)
                    .Where(
                        x => x.CompetitionId == foundComp.CompetitionId
                        && x.NamePartA == curMultiStart.NamePartA
                        && x.OrgIdPartA == curMultiStart.OrgIdPartA
                        && x.NamePartB == curMultiStart.NamePartB
                        && x.OrgIdPartB == curMultiStart.OrgIdPartB
                        && x.ClubName == curMultiStart.ClubName
                        && x.OrgIdClub == curMultiStart.OrgIdClub
                        && x.Ignore == false)
                    .ToListAsync(
                        cancellationToken);

                yield return new MultipleStarter(
                    allPartInfo);
            }
        }

        #region Get helper

        public IParticipantChecker? GetParticipantChecker(
            Competition competition)
        {
            switch (competition.Organization)
            {
                case OrganizationEnum.Oetsv:
                    return _serviceProvider.GetRequiredService<OetsvParticipantChecker>();

                default:
                    _logger.LogError(
                        "{Method}: '{Organization}' not yet implemented!",
                            nameof(GetParticipantChecker),
                            competition.Organization);
                    break;
            }

            return null;
        }

        public ICompetitonClassChecker? GetCompetitonClassChecker(
            Competition competition)
        {
            switch (competition.Organization)
            {
                case OrganizationEnum.Oetsv:
                    return new OetsvCompetitonClassChecker();

                default:
                    _logger.LogError(
                        "{Method}: '{Organization}' not yet implemented!",
                            nameof(GetCompetitonClassChecker),
                            competition.Organization);
                    break;
            }

            return null;
        }

        #endregion Get helper

        #region Conversions/Lookups

        public Task<Competition?> GetCompetitionAsync(
            string byName,
            CancellationToken cancellationToken)
        {
            return _danceCompHelperDb.Competitions
                .TagWith(
                    nameof(GetCompetitionAsync) + "(string)[0]")
                .FirstOrDefaultAsync(
                    x => x.CompetitionName == byName,
                    cancellationToken);
        }

        public async Task<Competition?> FindCompetitionAsync(
            Guid? byAnyId,
            CancellationToken cancellationToken)
        {
            if (byAnyId.HasValue == false)
            {
                return null;
            }

            var foundCompId = (await _danceCompHelperDb.CompetitionClasses
                .TagWith(
                    nameof(FindCompetitionAsync) + "(Guid)[0]")
                .FirstOrDefaultAsync(
                    x => x.CompetitionId == byAnyId
                    || x.CompetitionClassId == byAnyId
                    || x.AdjudicatorPanelId == byAnyId,
                    cancellationToken))
                ?.CompetitionId;

            if (foundCompId == null)
            {
                foundCompId = (await _danceCompHelperDb.Competitions
                    .TagWith(
                        nameof(FindCompetitionAsync) + "(Guid)[1]")
                    .FirstOrDefaultAsync(
                        x => x.CompetitionId == byAnyId,
                        cancellationToken))
                    ?.CompetitionId;
            }

            if (foundCompId == null)
            {
                foundCompId = (await _danceCompHelperDb.Participants
                    .TagWith(
                        nameof(FindCompetitionAsync) + "(Guid)[2]")
                    .FirstOrDefaultAsync(
                        x => x.ParticipantId == byAnyId,
                        cancellationToken))
                    ?.CompetitionId;
            }

            if (foundCompId == null)
            {
                var foundAdj = await _danceCompHelperDb.Adjudicators
                    .TagWith(
                        nameof(FindCompetitionAsync) + "(Guid)[4]")
                    .FirstOrDefaultAsync(
                        x => x.AdjudicatorId == byAnyId,
                        cancellationToken);

                if (foundAdj != null)
                {
                    foundCompId = (await _danceCompHelperDb.AdjudicatorPanels
                        .TagWith(
                            nameof(FindCompetitionAsync) + "(Guid)[5]")
                        .FirstOrDefaultAsync(
                            x => x.AdjudicatorPanelId == foundAdj.AdjudicatorPanelId,
                            cancellationToken))
                        ?.CompetitionId;
                }
            }

            if (foundCompId == null)
            {
                foundCompId = (await _danceCompHelperDb.AdjudicatorPanels
                    .TagWith(
                        nameof(FindCompetitionAsync) + "(Guid)[6]")
                    .FirstOrDefaultAsync(
                        x => x.AdjudicatorPanelId == byAnyId,
                        cancellationToken))
                    ?.CompetitionId;
            }

            if (foundCompId == null)
            {
                foundCompId = (await _danceCompHelperDb.CompetitionVenues
                    .TagWith(
                        nameof(FindCompetitionAsync) + "(Guid)[7]")
                    .FirstOrDefaultAsync(
                        x => x.CompetitionVenueId == byAnyId,
                        cancellationToken))
                    ?.CompetitionId;
            }

            if (foundCompId != null)
            {
                return await _danceCompHelperDb.Competitions
                    .TagWith(
                        nameof(FindCompetitionAsync) + "(Guid)[8]")
                    .FirstOrDefaultAsync(
                        x => x.CompetitionId == foundCompId,
                        cancellationToken);
            }

            return null;
        }

        public async Task<CompetitionClass?> FindCompetitionClassAsync(
            Guid? byAnyId,
            CancellationToken cancellationToken)
        {
            if (byAnyId.HasValue == false)
            {
                return null;
            }

            var foundCompClass = await _danceCompHelperDb.CompetitionClasses
                .TagWith(
                    nameof(FindCompetitionClassAsync) + "(Guid)[0]")
                .FirstOrDefaultAsync(
                    x => x.CompetitionId == byAnyId
                    || x.CompetitionClassId == byAnyId
                    || x.AdjudicatorPanelId == byAnyId,
                    cancellationToken);

            if (foundCompClass == null)
            {
                var foundCompClassId = (await _danceCompHelperDb.Participants
                    .TagWith(
                        nameof(FindCompetitionClassAsync) + "(Guid)[1]")
                    .FirstOrDefaultAsync(
                        x => x.ParticipantId == byAnyId,
                        cancellationToken))
                    ?.CompetitionClassId;

                foundCompClass = await _danceCompHelperDb.CompetitionClasses
                    .TagWith(
                        nameof(FindCompetitionClassAsync) + "(Guid)[2]")
                    .FirstOrDefaultAsync(
                        x => x.CompetitionClassId == foundCompClassId,
                        cancellationToken);
            }

            return foundCompClass;
        }

        public Task<CompetitionClass?> GetCompetitionClassAsync(
            string byName,
            CancellationToken cancellationToken)
        {
            return _danceCompHelperDb.CompetitionClasses
                .TagWith(
                    nameof(GetCompetitionClassAsync) + "(string)[0]")
                .FirstOrDefaultAsync(
                    x => x.CompetitionClassName == byName,
                    /* TODO: really need ignore?.. *
                    && x.Ignore == false,
                    */
                    cancellationToken);
        }

        public Task<CompetitionClass?> GetCompetitionClassAsync(
            Guid competitionClassId,
            CancellationToken cancellationToken)
        {
            return _danceCompHelperDb.CompetitionClasses
                .TagWith(
                    nameof(GetCompetitionClassAsync) + "(Guid)[0]")
                .FirstOrDefaultAsync(
                    x => x.CompetitionClassId == competitionClassId,
                    cancellationToken);
        }

        public Task<CompetitionVenue?> GetCompetitionVenueAsync(
            Guid? competitionVenueId,
            CancellationToken cancellationToken,
            bool includeCompetition = true)
        {
            var qry = _danceCompHelperDb.CompetitionVenues
                .TagWith(
                    nameof(GetCompetitionVenueAsync) + "(Guid)[0]");

            if (includeCompetition)
            {
                qry = qry.Include(
                    x => x.Competition);
            }

            return qry.FirstOrDefaultAsync(
                x => x.CompetitionVenueId == competitionVenueId,
                cancellationToken);
        }

        public Task<Participant?> GetParticipantAsync(
            Guid participantId,
            CancellationToken cancellationToken)
        {
            return _danceCompHelperDb.Participants
                .TagWith(
                    nameof(GetParticipantAsync) + "(Guid)[0]")
                .FirstOrDefaultAsync(
                    x => x.ParticipantId == participantId,
                    cancellationToken);
        }

        public Task<AdjudicatorPanel?> GetAdjudicatorPanelAsync(
            Guid adjudicatorPanelId,
            CancellationToken cancellationToken,
            bool includeCompetition = false)
        {
            var qry = _danceCompHelperDb.AdjudicatorPanels
                .TagWith(
                    nameof(GetAdjudicatorPanelAsync) + "(Guid)[0]");

            if (includeCompetition)
            {
                qry = qry.Include(
                    x => x.Competition);
            }

            return qry.FirstOrDefaultAsync(
                x => x.AdjudicatorPanelId == adjudicatorPanelId,
                cancellationToken);
        }

        public Task<Adjudicator?> GetAdjudicatorAsync(
            Guid adjudicatorId,
            CancellationToken cancellationToken)
        {
            return _danceCompHelperDb.Adjudicators
                .TagWith(
                    nameof(GetAdjudicatorAsync) + "(Guid)[0]")
                .Include(
                     x => x.AdjudicatorPanel)
                .FirstOrDefaultAsync(
                    x => x.AdjudicatorId == adjudicatorId,
                    cancellationToken);
        }

        #endregion Conversions/Lookups

        #region Competition Crud

        // TODO: rework do "AddCompetition"
        public Task CreateCompetitionAsync(
            Competition newCompetition,
            CancellationToken cancellationToken)
        {
            _danceCompHelperDb.Competitions.Add(
                newCompetition);

            return Task.CompletedTask;
        }

        public Task RemoveCompetitionAsync(
            Competition removeCompetition,
            CancellationToken cancellationToken)
        {
            if (removeCompetition != null)
            {
                // dependent items...
                // because cascade delete does not work...
                _danceCompHelperDb.Configurations.RemoveRange(
                    _danceCompHelperDb.Configurations.Where(
                        x => x.Organization == removeCompetition.Organization
                        && x.CompetitionId == removeCompetition.CompetitionId));

                _danceCompHelperDb.Competitions.Remove(
                    removeCompetition);
            }

            return Task.CompletedTask;
        }

        #endregion Competition Crud

        #region AdjudicatorPanel Crud

        public Task CreateAdjudicatorPanelAsync(
            AdjudicatorPanel newAdjudicatorPanel,
            CancellationToken cancellationToken)
        {
            _danceCompHelperDb.AdjudicatorPanels.Add(
                newAdjudicatorPanel);

            return Task.CompletedTask;
        }

        public Task RemoveAdjudicatorPanelAsync(
            AdjudicatorPanel removeAdjudicatorPanel,
            CancellationToken cancellationToken)
        {
            _danceCompHelperDb.AdjudicatorPanels.Remove(
                removeAdjudicatorPanel);

            return Task.CompletedTask;
        }

        #endregion AdjudicatorPanel Crud

        #region Adjudicator Crud

        public Task CreateAdjudicatorAsync(
            Adjudicator newAdjudicator,
            CancellationToken cancellationToken)
        {
            _danceCompHelperDb.Adjudicators.Add(
                newAdjudicator);

            return Task.CompletedTask;
        }

        public Task RemoveAdjudicatorAsync(
            Adjudicator removeAdjudicator,
            CancellationToken cancellationToken)
        {
            _danceCompHelperDb.Adjudicators.Remove(
                removeAdjudicator);

            return Task.CompletedTask;
        }

        #endregion Adjudicator Crud

        #region CompetitionClass Crud

        // TODO: rework to "AddCompetitionClassAsync"
        public async Task CreateCompetitionClassAsync(
            CompetitionClass createCompetitionClass,
            CancellationToken cancellationToken)
        {
            try
            {
                var foundCompId = await GetCompetitionAsync(
                    createCompetitionClass.CompetitionId,
                    cancellationToken)
                    ?? throw new ArgumentException(
                        string.Format(
                            "{0} with id '{1}' not found!",
                            nameof(Competition),
                            createCompetitionClass.CompetitionId));

                _danceCompHelperDb.CompetitionClasses.Add(
                    createCompetitionClass);
            }
            catch (Exception exc)
            {
                _logger.LogError(
                    exc,
                    "Error during {Method}: {Message}",
                    nameof(CreateCompetitionClassAsync),
                    exc.Message);

                throw;
            }
            finally
            {
                _logger.LogTrace(
                    "{Method}() done",
                    nameof(CreateCompetitionClassAsync));
            }
        }

        [Obsolete("do not use", true)]
        public async Task EditCompetitionClassAsync(
            Guid competitionClassId,
            string competitionClassName,
            Guid? followUpCompetitionClassId,
            Guid adjudicatorPanelId,
            string orgClassId,
            string? discipline,
            string? ageClass,
            string? ageGroup,
            string? className,
            int minStartsForPromotion,
            double minPointsForPromotion,
            double pointsForFirst,
            int extraManualStarter,
            string? comment,
            bool ignore,
            string? competitionColor,
            CancellationToken cancellationToken)
        {
            var foundCompClass = await GetCompetitionClassAsync(
                competitionClassId,
                cancellationToken);

            if (foundCompClass == null)
            {
                throw new ArgumentException(
                    string.Format(
                        "{0} with id '{1}' not found!",
                        nameof(CompetitionClass),
                        competitionClassId));
            }

            foundCompClass.OrgClassId = orgClassId;
            foundCompClass.CompetitionClassName = competitionClassName;
            foundCompClass.FollowUpCompetitionClassId = followUpCompetitionClassId == Guid.Empty
                ? null
                : followUpCompetitionClassId;
            foundCompClass.AdjudicatorPanelId = adjudicatorPanelId;
            foundCompClass.Discipline = discipline;
            foundCompClass.AgeClass = ageClass;
            foundCompClass.AgeGroup = ageGroup;
            foundCompClass.Class = className;
            foundCompClass.MinStartsForPromotion = minStartsForPromotion;
            foundCompClass.MinPointsForPromotion = minPointsForPromotion;
            foundCompClass.PointsForFirst = pointsForFirst;
            foundCompClass.ExtraManualStarter = extraManualStarter;
            foundCompClass.Comment = comment;
            foundCompClass.Ignore = ignore;
            foundCompClass.CompetitionColor = competitionColor;

            // TODO: needed?..
            await _danceCompHelperDb.SaveChangesAsync(
                cancellationToken);
        }

        public Task RemoveCompetitionClassAsync(
            CompetitionClass removeCompetitionClass,
            CancellationToken cancellationToken)
        {
            _danceCompHelperDb.CompetitionClasses.Remove(
                removeCompetitionClass);

            return Task.CompletedTask;
        }

        #endregion CompetitionClass Crud

        #region CompetitionVanue Crud

        public Task CreateCompetitionVenueAsync(
            CompetitionVenue newCompetitionVenue,
            CancellationToken cancellationToken)
        {
            _danceCompHelperDb.CompetitionVenues.Add(
                newCompetitionVenue);

            return Task.CompletedTask;
        }

        public Task RemoveCompetitionVenueAsync(
            CompetitionVenue removeCompetitionVenue,
            CancellationToken cancellationToken)
        {
            _danceCompHelperDb.CompetitionVenues.Remove(
                removeCompetitionVenue);

            return Task.CompletedTask;
        }

        #endregion CompetitionVanue Crud

        #region Participant Crud

        public async Task CreateParticipantAsync(
            Participant createParticipant,
            CancellationToken cancellationToken)
        {
            var foundCompId = await GetCompetitionAsync(
                createParticipant.CompetitionId,
                cancellationToken);

            if (foundCompId == null)
            {
                throw new ArgumentException(
                    string.Format(
                        "{0} with id '{1}' not found!",
                        nameof(Competition),
                        createParticipant.CompetitionId));
            }

            var foundCompClassId = await GetCompetitionClassAsync(
                createParticipant.CompetitionClassId,
                cancellationToken);

            if (foundCompClassId == null)
            {
                throw new ArgumentException(
                    string.Format(
                        "{0} with id '{1}' not found!",
                        nameof(CompetitionClass),
                        createParticipant.CompetitionClassId));
            }

            _danceCompHelperDb.Participants.Add(
                createParticipant);
        }

        public Task RemoveParticipantAsync(
            Participant removeParticipant,
            CancellationToken cancellationToken)
        {
            _danceCompHelperDb.Participants.Remove(
                removeParticipant);

            return Task.CompletedTask;
        }

        #endregion Participant Crud

        #region Configuration

        public async Task<(IEnumerable<ConfigurationValue>? ConfigurationValues,
            Competition? Competition,
            IEnumerable<Competition>? Competitions,
            IEnumerable<CompetitionClass>? CompetitionClasses,
            IEnumerable<CompetitionVenue>? CompetitionVenues)>
            GetConfigurationsAsync(
                Guid? competitionId,
                CancellationToken cancellationToken)
        {
            IAsyncEnumerable<ConfigurationValue> retConfValues = AsyncEnumerable.Empty<ConfigurationValue>();
            Competition? retComp;
            IAsyncEnumerable<Competition> retComps = AsyncEnumerable.Empty<Competition>();
            IAsyncEnumerable<CompetitionClass> retCompClasses = AsyncEnumerable.Empty<CompetitionClass>();
            IAsyncEnumerable<CompetitionVenue> retCompVenues = AsyncEnumerable.Empty<CompetitionVenue>();

            retComp = await GetCompetitionAsync(
                competitionId,
                cancellationToken);

            retConfValues = _danceCompHelperDb.Configurations
                .TagWith(
                    nameof(GetConfigurationsAsync) + "[0]")
                .ToAsyncEnumerable();
            retComps = _danceCompHelperDb.Competitions
                .TagWith(
                    nameof(GetConfigurationsAsync) + "[1]")
                .ToAsyncEnumerable();
            retCompClasses = _danceCompHelperDb.CompetitionClasses
                .TagWith(
                    nameof(GetConfigurationsAsync) + "[2]")
                .ToAsyncEnumerable();
            retCompVenues = _danceCompHelperDb.CompetitionVenues
                .TagWith(
                    nameof(GetConfigurationsAsync) + "[3]")
                .ToAsyncEnumerable();

            if (retComp != null)
            {
                var orgIds = new HashSet<OrganizationEnum?>()
                    {
                        null,
                        OrganizationEnum.Any,
                        retComp.Organization
                    };
                var compIds = new HashSet<Guid?>()
                    {
                        null,
                        retComp.CompetitionId,
                    };

                retConfValues = retConfValues
                    .Where(
                        x => orgIds.Contains(x.Organization)
                        && compIds.Contains(x.CompetitionId));

                retComps = retComps
                    .Where(
                        x => x.CompetitionId == retComp.CompetitionId);

                retCompClasses = retCompClasses
                    .Where(
                        x => x.CompetitionId == retComp.CompetitionId);

                retCompVenues = retCompVenues
                    .Where(
                        x => x.CompetitionId == retComp.CompetitionId);
            }

            // sort...
            retConfValues = retConfValues
                .OrderBy(
                    x => x.Key);
            retComps = retComps
                .OrderBy(
                    x => x.CompetitionName);
            retCompClasses = retCompClasses
                .OrderBy(
                    x => x.OrgClassId)
                .ThenBy(
                    x => x.CompetitionClassName);
            retCompVenues = retCompVenues
                .OrderBy(
                    x => x.Name);

            var retConfValuesTask = retConfValues.ToListAsync(cancellationToken).AsTask();
            var retCompsTask = retComps.ToListAsync(cancellationToken).AsTask();
            var retCompClassesTask = retCompClasses.ToListAsync(cancellationToken).AsTask();
            var retCompVenuesTask = retCompVenues.ToListAsync(cancellationToken).AsTask();

            await Task.WhenAll(
                retConfValuesTask,
                retCompsTask,
                retCompClassesTask,
                retCompVenuesTask);

            return (
                await retConfValuesTask,
                retComp,
                await retCompsTask,
                await retCompClassesTask,
                await retCompVenuesTask);
        }

        public Task<ConfigurationValue?> GetConfigurationAsync(
            string key,
            CancellationToken cancellationToken)
        {
            return GetConfigurationAsync(
                new ConfigurationValue()
                {
                    Key = key
                },
                cancellationToken);
        }

        public async Task<ConfigurationValue?> GetConfigurationAsync(
            string key,
            OrganizationEnum organization,
            CancellationToken cancellationToken)
        {
            return await GetConfigurationAsync(
                new ConfigurationValue()
                {
                    Organization = organization,
                    Key = key,
                },
                cancellationToken)
                // fallback up...
                ?? await GetConfigurationAsync(
                    key,
                    cancellationToken);
        }

        public async Task<ConfigurationValue?> GetConfigurationAsync(
            string key,
            Competition? competition,
            CancellationToken cancellationToken)
        {
            if (competition == null)
            {
                return null;
            }

            return await GetConfigurationAsync(
                new ConfigurationValue()
                {
                    Organization = competition.Organization,
                    CompetitionId = competition.CompetitionId,
                    Key = key,
                },
                cancellationToken)
                // fallback up...
                ?? await GetConfigurationAsync(
                    key,
                    competition.Organization,
                    cancellationToken);
        }

        public async Task<ConfigurationValue?> GetConfigurationAsync(
            string key,
            CompetitionClass? competitionClass,
            CancellationToken cancellationToken)
        {
            if (competitionClass == null)
            {
                return null;
            }

            var useCompetition = competitionClass.Competition;

            return await GetConfigurationAsync(
                new ConfigurationValue()
                {
                    Organization = useCompetition.Organization,
                    CompetitionId = useCompetition.CompetitionId,
                    CompetitionClassId = competitionClass.CompetitionClassId,
                    Key = key,
                },
                cancellationToken)
                // fallback up...
                ?? await GetConfigurationAsync(
                    key,
                    useCompetition,
                    cancellationToken);
        }

        [Obsolete("really needed except from tests?..")]
        // TODO: really needed except from tests?..
        public async Task<ConfigurationValue?> GetConfigurationAsync(
            string key,
            Competition? competition,
            CompetitionVenue? competitionVenue,
            CancellationToken cancellationToken)
        {
            if (competition == null
                && competitionVenue == null)
            {
                return null;
            }

            if (competitionVenue == null)
            {
                return null;
            }

            var useCompetition = competitionVenue.Competition;

            return await GetConfigurationAsync(
                new ConfigurationValue()
                {
                    Organization = useCompetition.Organization,
                    CompetitionId = useCompetition.CompetitionId,
                    CompetitionClassId = null,
                    CompetitionVenueId = competitionVenue.CompetitionVenueId,
                    Key = key,
                },
                cancellationToken)
                // fallback up...
                ?? await GetConfigurationAsync(
                    key,
                    competition,
                    cancellationToken)
                ?? await GetConfigurationAsync(
                    key,
                    competitionVenue.Competition,
                    cancellationToken);
        }

        public async Task<ConfigurationValue?> GetConfigurationAsync(
            string key,
            CompetitionClass? competitionClass,
            CompetitionVenue? competitionVenue,
            CancellationToken cancellationToken)
        {
            if (competitionClass == null
                && competitionVenue == null)
            {
                return null;
            }

            if (competitionVenue == null)
            {
                return null;
            }

            var useCompetition = competitionVenue.Competition;
            var usecompetitionClassId = competitionClass?.CompetitionClassId ?? Guid.Empty;

            return await GetConfigurationAsync(
                new ConfigurationValue()
                {
                    Organization = useCompetition.Organization,
                    CompetitionId = useCompetition.CompetitionId,
                    CompetitionClassId = usecompetitionClassId,
                    CompetitionVenueId = competitionVenue.CompetitionVenueId,
                    Key = key,
                },
                cancellationToken)
                ?? await GetConfigurationAsync(
                    new ConfigurationValue()
                    {
                        Organization = useCompetition.Organization,
                        CompetitionId = useCompetition.CompetitionId,
                        CompetitionClassId = null,
                        CompetitionVenueId = competitionVenue.CompetitionVenueId,
                        Key = key,
                    },
                    cancellationToken)
                // fallback up...
                ?? await GetConfigurationAsync(
                    key,
                    competitionClass,
                    cancellationToken)
                ?? await GetConfigurationAsync(
                    key,
                    competitionVenue.Competition,
                    cancellationToken);
        }

        public async Task<ConfigurationValue?> GetConfigurationAsync(
            ConfigurationValue? cfgValue,
            CancellationToken cancellationToken)
        {
            if (cfgValue == null)
            {
                return null;
            }

            return await _danceCompHelperDb.Configurations
                .TagWith(
                    nameof(GetConfigurationAsync) + "(ConfigurationValue)")
                .FirstOrDefaultAsync(
                    x => x.Organization == cfgValue.Organization
                    && x.CompetitionId == cfgValue.CompetitionId
                    && x.CompetitionClassId == cfgValue.CompetitionClassId
                    && x.CompetitionVenueId == cfgValue.CompetitionVenueId
                    && x.Key == cfgValue.Key,
                    cancellationToken);
        }

        public async Task CreateConfigurationAsync(
            ConfigurationValue crateConfigurationValue,
            CancellationToken cancellationToken)
        {
            if (crateConfigurationValue.Organization == OrganizationEnum.Any)
            {
                crateConfigurationValue.Organization = null;
            }

            try
            {
                Competition? chkComp = null;
                if (crateConfigurationValue.CompetitionId != null)
                {
                    chkComp = await GetCompetitionAsync(
                        crateConfigurationValue.CompetitionId,
                        cancellationToken);

                    if (chkComp != null
                        && chkComp.Organization != crateConfigurationValue.Organization)
                    {
                        throw new ArgumentOutOfRangeException(
                            nameof(crateConfigurationValue.CompetitionId),
                            string.Format(
                                "{0} ({1}) does not match passed {2} ({3})",
                                nameof(crateConfigurationValue.Organization),
                                crateConfigurationValue.Organization,
                                nameof(Competition),
                                chkComp.Organization));
                    }
                }

                CompetitionClass? chkCompClass = null;
                if (crateConfigurationValue.CompetitionClassId != null)
                {
                    chkCompClass = await GetCompetitionClassAsync(
                        crateConfigurationValue.CompetitionClassId.Value,
                        cancellationToken);

                    if (chkCompClass == null
                        || chkCompClass.CompetitionId != chkComp?.CompetitionId)
                    {
                        throw new ArgumentOutOfRangeException(
                            nameof(crateConfigurationValue.CompetitionClassId),
                            string.Format(
                                "{0} ({1}) does not match passed {2} ({3})",
                                nameof(CompetitionClass),
                                crateConfigurationValue.CompetitionClassId,
                                nameof(Competition),
                                crateConfigurationValue.CompetitionId));
                    }
                }

                _danceCompHelperDb.Configurations.Add(
                    crateConfigurationValue);
            }
            catch (Exception exc)
            {
                _logger.LogError(
                    exc,
                    "Error during {Method}: {Message}",
                    nameof(CreateConfigurationAsync),
                    exc.Message);

                throw;
            }
            finally
            {
                _logger.LogTrace(
                    "{Method}() done",
                    nameof(CreateConfigurationAsync));
            }
        }

        public Task RemoveConfigurationAsync(
            ConfigurationValue removeConfigurationValue,
            CancellationToken cancellationToken)
        {
            _danceCompHelperDb.Configurations.Remove(
                removeConfigurationValue);

            return Task.CompletedTask;
        }

        #endregion Configuration

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            _logger.LogDebug(
                "Do '{Method}'",
                nameof(Dispose));

            _danceCompHelperDb.Dispose();
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable


        #region Importer

        public async Task<ImportOrUpdateCompetitionStatus> ImportOrUpdateCompetitionAsync(
            OrganizationEnum organization,
            string? orgCompetitionId,
            ImportTypeEnum importType,
            CancellationToken cancellationToken,
            IEnumerable<string>? filePaths,
            Dictionary<string, string>? parameters = null)
        {
            var retWorkStatus = new ImportOrUpdateCompetitionStatus();

            try
            {
                switch (organization)
                {
                    case OrganizationEnum.Oetsv:
                        if (int.TryParse(
                            orgCompetitionId,
                            out var useCompetitionId) == false)
                        {
                            throw new FormatException(
                                string.Format(
                                    "'{0}' is not a valid {1}-Competition-ID",
                                    orgCompetitionId,
                                    OrganizationEnum.Oetsv));
                        }

                        var oetsvImporter = _serviceProvider.GetRequiredService<OetsvCompetitionImporter>();
                        oetsvImporter.DanceCompetitionHelper = this;
                        oetsvImporter.FindFollowUpClasses = parameters?.ContainsKey(
                            nameof(OetsvCompetitionImporter.FindFollowUpClasses)) ?? false;
                        oetsvImporter.UpdateData = parameters?.ContainsKey(
                            nameof(OetsvCompetitionImporter.UpdateData)) ?? false;

                        Func<List<string>> importFunc;

                        switch (importType)
                        {
                            case ImportTypeEnum.Url:
                                importFunc = () =>
                                {
                                    return oetsvImporter.ImportOrUpdateByUrl(
                                        orgCompetitionId,
                                        oetsvImporter.UpdateData
                                            ? oetsvImporter.GetUpdateUriForOrgId(
                                                useCompetitionId)
                                            : null,
                                        oetsvImporter.GetCompetitioUriForOrgId(
                                            useCompetitionId),
                                        null,
                                        oetsvImporter.GetParticipantsUriForOrgId(
                                            useCompetitionId));
                                };
                                break;

                            case ImportTypeEnum.Excel:
                                var useFiles = filePaths?.ToList() ?? new List<string>();
                                if (useFiles.Count < 2)
                                {
                                    throw new ArgumentNullException(
                                        nameof(filePaths),
                                        string.Format(
                                            "'{0}' must contain at least 2 files!",
                                            nameof(filePaths)));
                                }

                                importFunc = () =>
                                {
                                    return oetsvImporter.ImportOrUpdateByFile(
                                        orgCompetitionId,
                                        useFiles[0],
                                        null,
                                        useFiles[1]);
                                };
                                break;

                            default:
                                throw new NotImplementedException(
                                    string.Format(
                                        "{0} '{1}' not yet implemented!",
                                        nameof(ImportTypeEnum),
                                        importType));
                        }

                        using (var dbTrans = await _danceCompHelperDb.BeginTransactionAsync(
                            cancellationToken)
                            ?? throw new ArgumentNullException(
                                "dbTrans"))
                        {
                            try
                            {
                                var checkComp = _danceCompHelperDb.Competitions
                                    .TagWith(
                                        nameof(ImportOrUpdateCompetitionAsync) + "[01]")
                                    .FirstOrDefault(
                                        x => x.OrgCompetitionId == orgCompetitionId);

                                if (checkComp != null)
                                {
                                    await CreateTableHistoryAsync(
                                        checkComp.CompetitionId,
                                        cancellationToken);
                                }

                                retWorkStatus.WorkInfo.AddRange(
                                    importFunc());

                                await _danceCompHelperDb.SaveChangesAsync(
                                    cancellationToken);
                                await dbTrans.CommitAsync(
                                    cancellationToken);

                                // read back infos
                                checkComp = await _danceCompHelperDb.Competitions
                                    .TagWith(
                                        nameof(ImportOrUpdateCompetitionAsync) + "[02]")
                                    .FirstOrDefaultAsync(
                                        x => x.OrgCompetitionId == orgCompetitionId,
                                        cancellationToken);

                                if (checkComp != null)
                                {
                                    retWorkStatus.OrgCompetitionId = checkComp.OrgCompetitionId;
                                    retWorkStatus.CompetitionId = checkComp.CompetitionId;
                                }
                            }
                            catch (Exception exc)
                            {
                                _logger.LogError(
                                    exc,
                                    "Error during {Organization} import: {ErrorMessage}",
                                    organization,
                                    exc.Message);

                                retWorkStatus.Errors.Add(
                                    exc.Message);
                                retWorkStatus.WorkInfo.Add(
                                    exc.Message);

                                await (dbTrans?.RollbackAsync() ?? Task.CompletedTask);
                            }
                        }
                        break;
                }
            }
            catch (Exception exc)
            {
                _logger.LogError(
                    exc,
                    "{Method} error: {ErrorMessage}",
                    nameof(ImportOrUpdateCompetitionAsync),
                    exc.Message);

                retWorkStatus.Errors.Add(
                    exc.Message);
                retWorkStatus.WorkInfo.Add(
                    exc.Message);
            }

            return retWorkStatus;
        }

        #endregion Importer

        #region Transaction Helper

        public async Task<T?> RunInReadonlyTransaction<T>(
            Func<IDanceCompetitionHelper, DanceCompetitionHelperDbContext, IDbContextTransaction, CancellationToken, Task<T>> func,
            CancellationToken cancellationToken = default,
            bool rethrowException = true,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (func == null)
            {
                throw new ArgumentNullException(
                    nameof(func));
            }

            using var dbTrans = await _danceCompHelperDb.BeginTransactionAsync(
                cancellationToken)
                ?? throw new ArgumentNullException(
                    "dbTrans");

            try
            {
                return await func(
                    this,
                    _danceCompHelperDb,
                    dbTrans,
                    cancellationToken);
            }
            catch (Exception exc)
            {
                _logger.LogError(
                    exc,
                    "Error during {MemberName}(): {Message}",
                    memberName,
                    exc.Message);

                if (rethrowException)
                {
                    throw;
                }

                return default;
            }
            finally
            {
                await dbTrans.RollbackAsync();
            }
        }

        public async Task<T?> RunInTransactionWithSaveChangesAndCommit<T>(
            Func<IDanceCompetitionHelper, DanceCompetitionHelperDbContext, IDbContextTransaction, CancellationToken, Task<object?>> func,
            Func<object?, CancellationToken, Task<T>> onSuccess,
            Func<object?, CancellationToken, Task<T>>? onNoData,
            Func<Exception, object?, CancellationToken, Task<T>>? onException,
            CancellationToken cancellationToken = default,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (func == null)
            {
                throw new ArgumentNullException(
                    nameof(func));
            }

            using var dbTrans = await _danceCompHelperDb.BeginTransactionAsync(
                cancellationToken)
                ?? throw new ArgumentNullException(
                    "dbTrans");

            object? routeObjects = null;

            try
            {
                routeObjects = await func(
                    this,
                    _danceCompHelperDb,
                    dbTrans,
                    cancellationToken);

                await _danceCompHelperDb.SaveChangesAsync(
                    cancellationToken);
                await dbTrans.CommitAsync(
                    cancellationToken);

                return await onSuccess(
                    routeObjects,
                    cancellationToken);
            }
            catch (NoDataFoundException noDataExc)
            {
                _logger.LogWarning(
                    noDataExc,
                    "No Data during {MemberName}(): {Message}",
                    memberName,
                    noDataExc.Message);

                await dbTrans.RollbackAsync();

                if (onNoData != null)
                {
                    return await onNoData(
                        routeObjects ?? noDataExc.RouteObjects,
                        cancellationToken);
                }

                return default;

            }
            catch (Exception exc)
            {
                _logger.LogError(
                    exc,
                    "Error during {MemberName}(): {Message}",
                    memberName,
                    exc.Message);

                await dbTrans.RollbackAsync();

                if (onException != null)
                {
                    return await onException(
                        exc,
                        routeObjects,
                        cancellationToken);
                }

                return default;
            }
        }

        #endregion Transaction Helper

    }
}
