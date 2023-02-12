using DanceCompetitionHelper.Database;
using DanceCompetitionHelper.Database.DisplayInfo;
using DanceCompetitionHelper.Database.Enum;
using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Extensions;
using DanceCompetitionHelper.OrgImpl;
using DanceCompetitionHelper.OrgImpl.Oetsv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DanceCompetitionHelper
{
    public class DanceCompetitionHelper : IDanceCompetitionHelper
    {
        private readonly DanceCompetitionHelperDbContext _danceCompHelperDb;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<DanceCompetitionHelperDbContext> _logger;

        public DanceCompetitionHelper(
            DanceCompetitionHelperDbContext danceCompHelperDb,
            ILoggerFactory loggerFactory,
            ILogger<DanceCompetitionHelperDbContext> logger)
        {
            _danceCompHelperDb = danceCompHelperDb
                ?? throw new ArgumentNullException(
                    nameof(danceCompHelperDb));

            _logger = logger
                ?? throw new ArgumentNullException(
                    nameof(logger));
            _loggerFactory = loggerFactory
                ?? throw new ArgumentNullException(
                    nameof(loggerFactory));
        }

        public void AddTestData()
        {
            _logger.LogDebug(
                "Do '{Method}'",
                nameof(AddTestData));

            _danceCompHelperDb.AddTestData();
        }

        public void Migrate()
        {
            _logger.LogDebug(
                "Do '{Method}'",
                nameof(Migrate));

            _danceCompHelperDb.Migrate();
        }

        public Competition? GetCompetition(
            Guid? competitionId)
        {
            if (competitionId == null)
            {
                return null;
            }

            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                return _danceCompHelperDb.Competitions
                    .TagWith(
                        nameof(GetCompetition) + "(Guid)[0]")
                    .FirstOrDefault(
                        x => x.CompetitionId == competitionId);
            }
            finally
            {
                dbTrans.Rollback();
            }
        }

        public IEnumerable<Competition> GetCompetitions(
            bool includeInfos = false)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            var countsOfCompClasses = new Dictionary<Guid, int>();
            var countsOfParticipants = new Dictionary<Guid, int>();

            try
            {
                if (includeInfos)
                {
                    countsOfCompClasses = _danceCompHelperDb.CompetitionClasses
                        .TagWith(
                            nameof(GetCompetitions) + "(bool?)[0]")
                        .Include(
                            x => x.AdjudicatorPanel)
                        .GroupBy(
                            x => x.CompetitionId,
                            (compId, items) => new
                            {
                                CompetitionId = compId,
                                Count = items.Count(),
                            })
                        .ToDictionary(
                            x => x.CompetitionId,
                            x => x.Count);

                    countsOfParticipants = _danceCompHelperDb.Participants
                        .TagWith(
                            nameof(GetCompetitions) + "(bool?)[1]")
                        .GroupBy(
                            x => x.CompetitionId,
                            (compId, items) => new
                            {
                                CompetitionId = compId,
                                Count = items.Count(),
                            })
                        .ToDictionary(
                            x => x.CompetitionId,
                            x => x.Count);
                }

                foreach (var curComp in _danceCompHelperDb.Competitions
                    .TagWith(
                        nameof(GetCompetitions) + "(bool?)[2]")
                    .OrderByDescending(
                        x => x.CompetitionDate)
                    .ThenBy(
                        x => x.OrgCompetitionId))
                {
                    if (includeInfos)
                    {
                        if (curComp.DisplayInfo == null)
                        {
                            curComp.DisplayInfo = new CompetitionDisplayInfo();
                        }

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

                        useDisplayInfo.CountMultipleStarters = GetMultipleStarterReuseTransaction(
                            curComp.CompetitionId)
                            .Count();
                        useDisplayInfo.CountAdjudicatorPanels = _danceCompHelperDb.AdjudicatorPanels
                            .TagWith(
                                nameof(GetCompetitions) + " " + nameof(Adjudicator) + ".Count")
                            .Count(
                                x => x.CompetitionId == curComp.CompetitionId);
                    }

                    yield return curComp;
                }
            }
            finally
            {
                dbTrans.Rollback();
            }
        }

        public IEnumerable<CompetitionClass> GetCompetitionClasses(
            Guid? competitionId,
            bool includeInfos = false)
        {
            if (competitionId == null)
            {
                yield break;
            }

            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                var foundComp = _danceCompHelperDb.Competitions
                    .TagWith(
                        nameof(GetCompetitionClasses) + "(Guid?)[0]")
                    .FirstOrDefault(
                        x => x.CompetitionId == competitionId);

                if (foundComp == null)
                {
                    yield break;
                }

                var partsByCompClass = new Dictionary<Guid, List<Participant>>();
                var multiStartsByCompClass = new Dictionary<Guid, List<Participant>>();

                ICompetitonClassChecker? compClassChecker = GetCompetitonClassChecker(
                    foundComp);
                var higherClassificationsByLowerCompClassId = new Dictionary<Guid, List<CompetitionClass>>();

                var possibleWinnerFromCompClass = new Dictionary<Guid, List<CompetitionClass>>();
                var foundPromotionFromCompClass = new Dictionary<Guid, List<Participant>>();

                var allCompetitionClasses = _danceCompHelperDb.CompetitionClasses
                    .TagWith(
                        nameof(GetCompetitionClasses) + "(Guid?)[2]")
                    .Include(
                        x => x.AdjudicatorPanel)
                    .Where(
                        x => x.CompetitionId == foundComp.CompetitionId)
                    .OrderBy(
                        x => x.OrgClassId)
                    .ThenBy(
                        x => x.CompetitionClassName)
                    .ToList();

                // collect "higher classes"...
                foreach (var curCompClass in allCompetitionClasses)
                {
                    var higherClasses = (compClassChecker?.GetHigherClassifications(
                            curCompClass,
                            allCompetitionClasses)
                            ?? Enumerable.Empty<CompetitionClass>())
                        .ToList();

                    higherClassificationsByLowerCompClassId[curCompClass.CompetitionClassId]
                        = higherClasses;

                    foreach (var curHighClass in higherClasses)
                    {
                        possibleWinnerFromCompClass.AddToBucket(
                            curHighClass.CompetitionClassId,
                            curCompClass);
                    }
                }

                if (includeInfos)
                {
                    foreach (var curPart in GetParticipantsReuseTransaction(
                            foundComp.CompetitionId,
                            null,
                            true)
                        .AsQueryable()
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
                                    foreach (var curHighClass in foundHigherClasses)
                                    {
                                        foundPromotionFromCompClass.AddToBucket(
                                            curHighClass.CompetitionClassId,
                                            curPart);
                                    }
                                }
                            }
                        }
                    }

                    var multipleStarters = GetMultipleStarterReuseTransaction(
                        competitionId.Value)
                        .ToList();

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

                foreach (var curCompClass in allCompetitionClasses)
                {
                    if (includeInfos)
                    {
                        if (curCompClass.DisplayInfo == null)
                        {
                            curCompClass.DisplayInfo = new CompetitionClassDisplayInfo();
                        }

                        var useDisplayInfo = curCompClass.DisplayInfo;
                        var useExtraPart = useDisplayInfo.ExtraParticipants;
                        var useCompClassId = curCompClass.CompetitionClassId;

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

                        if (possibleWinnerFromCompClass.TryGetValue(
                            useCompClassId,
                            out var possibleWinners))
                        {
                            useExtraPart.ByWinning += possibleWinners.Count;
                            useExtraPart.ByWinningInfo += possibleWinners.GetCompetitionClasseNames();
                        }

                        if (foundPromotionFromCompClass.TryGetValue(
                            useCompClassId,
                            out var possiblePromotions))
                        {
                            useExtraPart.ByPromotion += possiblePromotions.Count;
                            useExtraPart.ByPromotionInfo += possiblePromotions.GetStartNumber();
                        }
                    }

                    yield return curCompClass;
                }
            }
            finally
            {
                dbTrans.Rollback();
            }
        }

        public IEnumerable<Participant> GetParticipants(
            Guid? competitionId,
            Guid? competitionClassId,
            bool includeInfos = false)
        {
            if (competitionId == null)
            {
                return Enumerable.Empty<Participant>();
            }

            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                return GetParticipantsReuseTransaction(
                    competitionId,
                    competitionClassId,
                    includeInfos);
            }
            finally
            {
                dbTrans.Rollback();
            }
        }

        public IEnumerable<Participant> GetParticipantsReuseTransaction(
            Guid? competitionId,
            Guid? competitionClassId,
            bool includeInfos = false)
        {
            if (competitionId == null)
            {
                yield break;
            }

            var foundComp = _danceCompHelperDb.Competitions
                .TagWith(
                    nameof(GetParticipants) + "(Guid?, Guid?, bool)[0]")
                .FirstOrDefault(
                    x => x.CompetitionId == competitionId);
            var foundCompClass = _danceCompHelperDb.CompetitionClasses
                .TagWith(
                    nameof(GetParticipants) + "(Guid?, Guid?, bool)[1]")
                .FirstOrDefault(
                    x => x.CompetitionId == competitionId
                    && x.CompetitionClassId == competitionClassId
                    && x.Ignore == false);

            if (foundComp == null)
            {
                yield break;
            }

            var multiStarter = new List<MultipleStarter>();
            var multiStarterByParticipantsId = new Dictionary<Guid, List<CompetitionClass>>();
            var allCompClasses = new List<CompetitionClass>();
            IParticipantChecker? participantChecker = null;

            if (includeInfos)
            {
                allCompClasses.AddRange(
                    _danceCompHelperDb.CompetitionClasses
                        .TagWith(
                            nameof(GetParticipants) + "(Guid?, Guid?, bool)[1]")
                        .Where(
                            x => x.CompetitionId == competitionId));

                multiStarter.AddRange(
                    GetMultipleStarterReuseTransaction(
                        foundComp.CompetitionId));

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
                    nameof(GetParticipants) + "(Guid?, Guid?, bool)[2]")
                .Include(
                    x => x.CompetitionClass)
                .Where(
                    x => x.CompetitionId == foundComp.CompetitionId
                    && x.Ignore == false);

            if (foundCompClass != null)
            {
                qryParticipants = qryParticipants.Where(
                    x => x.CompetitionClassId == foundCompClass.CompetitionClassId);
            }

            foreach (var curPart in qryParticipants
                .OrderBy(
                    x => x.CompetitionClass.OrgClassId)
                .ThenBy(
                    x => x.CompetitionClass.CompetitionClassName)
                .ThenByDefault())
            {
                if (includeInfos)
                {
                    if (curPart.DisplayInfo == null)
                    {
                        curPart.DisplayInfo = new ParticipantDisplayInfo();
                    }

                    var useDisplayInfo = curPart.DisplayInfo;

                    if (useDisplayInfo.MultipleStartInfo == null)
                    {
                        useDisplayInfo.MultipleStartInfo = new CheckMultipleStartInfo();
                    }

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

        public IEnumerable<AdjudicatorPanel> GetAdjudicatorPanels(
            Guid? competitionId,
            bool includeInfos = false)
        {
            if (competitionId == null)
            {
                return Enumerable.Empty<AdjudicatorPanel>();
            }

            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                return GetAdjudicatorPanelsReuseTransaction(
                    competitionId,
                    includeInfos);
            }
            finally
            {
                dbTrans.Rollback();
            }
        }

        public IEnumerable<AdjudicatorPanel> GetAdjudicatorPanelsReuseTransaction(
            Guid? competitionId,
            bool includeInfos = false)
        {
            foreach (var curAdjPanel in _danceCompHelperDb.AdjudicatorPanels
                .TagWith(
                    nameof(GetAdjudicatorPanelsReuseTransaction) + "(Guid?, bool)[0]")
                .Where(
                    x => x.CompetitionId == competitionId))
            {
                if (includeInfos)
                {
                    curAdjPanel.DisplayInfo = new AdjudicatorPanelDisplayInfos();
                    var useDisplayInfo = curAdjPanel.DisplayInfo;

                    useDisplayInfo.CountAdjudicators = _danceCompHelperDb.Adjudicators
                        .TagWith(
                            nameof(GetAdjudicatorPanelsReuseTransaction) + "(Guid?, bool)[1]")
                        .Count(
                            x => x.AdjudicatorPanelId == curAdjPanel.AdjudicatorPanelId);
                }

                yield return curAdjPanel;
            }
        }

        public IEnumerable<Adjudicator> GetAdjudicators(
            Guid? competitionId,
            Guid? adjudicatorPanelId,
            bool includeInfos = false)
        {
            if (competitionId == null)
            {
                return Enumerable.Empty<Adjudicator>();
            }

            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                return GetAdjudicatorsReuseTransaction(
                    competitionId,
                    adjudicatorPanelId,
                    includeInfos);
            }
            finally
            {
                dbTrans.Rollback();
            }
        }

        public IEnumerable<Adjudicator> GetAdjudicatorsReuseTransaction(
            Guid? competitionId,
            Guid? adjudicatorPanelId,
            bool includeInfos = false)
        {
            var foundComp = _danceCompHelperDb.Competitions
                .TagWith(
                    nameof(GetAdjudicatorsReuseTransaction) + "(Guid?, Guid?)[0]")
                .FirstOrDefault(
                    x => x.CompetitionId == competitionId);

            if (foundComp == null)
            {
                yield break;
            }

            var useAdjPanels = new List<Guid>();

            if (adjudicatorPanelId == null)
            {
                useAdjPanels.AddRange(
                    _danceCompHelperDb.AdjudicatorPanels
                        .TagWith(
                            nameof(GetAdjudicatorsReuseTransaction) + "(Guid?, Guid?)[1]")
                        .Where(
                            x => x.CompetitionId == foundComp.CompetitionId)
                        .OrderBy(
                            x => x.Name)
                        .Select(
                            x => x.AdjudicatorPanelId));
            }
            else
            {
                var foundAdjPanel = _danceCompHelperDb.AdjudicatorPanels
                    .TagWith(
                        nameof(GetAdjudicatorsReuseTransaction) + "(Guid?, Guid?)[3]")
                    .FirstOrDefault(
                        x => x.CompetitionId == foundComp.CompetitionId);

                if (foundAdjPanel == null)
                {
                    yield break;
                }

                useAdjPanels.Add(
                    foundAdjPanel.AdjudicatorPanelId);
            }

            foreach (var curAdjPanelId in useAdjPanels)
            {
                foreach (var curRetAdj in _danceCompHelperDb.Adjudicators
                    .TagWith(
                        nameof(GetAdjudicatorsReuseTransaction) + "(Guid?, Guid?)[4]")
                    .Include(
                        x => x.AdjudicatorPanel)
                    .Where(
                        x => x.AdjudicatorPanelId == curAdjPanelId)
                    .OrderBy(
                        x => x.Abbreviation))
                {
                    yield return curRetAdj;
                }
            }
        }

        public IEnumerable<MultipleStarter> GetMultipleStarterReuseTransaction(
            Guid competitionId)
        {
            var multipleStarterGouping = _danceCompHelperDb.Participants
                .TagWith(
                    nameof(GetMultipleStarterReuseTransaction) + "(Guid)[0]")
                .Where(
                    x => x.CompetitionId == competitionId)
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

            foreach (var curMultiStart in multipleStarterGouping)
            {
                var allPartInfo = _danceCompHelperDb.Participants
                    .TagWith(
                        nameof(GetMultipleStarterReuseTransaction) + "(Guid)[1]")
                    .Include(
                        x => x.CompetitionClass)
                    .Where(
                        x => x.NamePartA == curMultiStart.NamePartA
                        && x.OrgIdPartA == curMultiStart.OrgIdPartA
                        && x.NamePartB == curMultiStart.NamePartB
                        && x.OrgIdPartB == curMultiStart.OrgIdPartB
                        && x.ClubName == curMultiStart.ClubName
                        && x.OrgIdClub == curMultiStart.OrgIdClub)
                    .ToList();

                yield return new MultipleStarter(
                    allPartInfo);
            }
        }

        public IEnumerable<MultipleStarter> GetMultipleStarter(
            Guid competitionId)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                return GetMultipleStarterReuseTransaction(
                    competitionId);
            }
            finally
            {
                _logger.LogTrace(
                    "{Method}() done",
                    nameof(GetMultipleStarter));

                dbTrans.Rollback();
            }
        }

        #region Get helper

        public IParticipantChecker? GetParticipantChecker(
            Competition competition)
        {
            switch (competition.Organization)
            {
                case OrganizationEnum.Oetsv:
                    return new OetsvParticipantChecker(
                        _loggerFactory.CreateLogger<OetsvParticipantChecker>());

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

        #endregion // Get helper

        #region Conversions/Lookups

        public Guid? GetCompetition(string byName)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                return _danceCompHelperDb.Competitions
                    .TagWith(
                        nameof(GetCompetition) + "(string)[0]")
                    .FirstOrDefault(
                        x => x.CompetitionName == byName)
                    ?.CompetitionId;
            }
            finally
            {
                _logger.LogTrace(
                    "{Method}() done",
                    nameof(GetCompetition));

                dbTrans.Rollback();
            }
        }

        public Guid? FindCompetition(
            Guid? byAnyId)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                var foundCompId = _danceCompHelperDb.CompetitionClasses
                    .TagWith(
                        nameof(FindCompetition) + "(Guid)[0]")
                    .FirstOrDefault(
                        x => x.CompetitionId == byAnyId
                        || x.CompetitionClassId == byAnyId
                        || x.AdjudicatorPanelId == byAnyId)
                    ?.CompetitionId;

                if (foundCompId == null)
                {
                    foundCompId = _danceCompHelperDb.Competitions
                        .TagWith(
                            nameof(FindCompetition) + "(Guid)[1]")
                        .FirstOrDefault(
                            x => x.CompetitionId == byAnyId)
                        ?.CompetitionId;
                }

                if (foundCompId == null)
                {
                    foundCompId = _danceCompHelperDb.Participants
                        .TagWith(
                            nameof(FindCompetition) + "(Guid)[2]")
                        .FirstOrDefault(
                            x => x.ParticipantId == byAnyId)
                        ?.CompetitionId;
                }

                if (foundCompId == null)
                {
                    var foundAdj = _danceCompHelperDb.Adjudicators
                        .TagWith(
                            nameof(FindCompetition) + "(Guid)[4]")
                        .FirstOrDefault(
                            x => x.AdjudicatorId == byAnyId);

                    if (foundAdj != null)
                    {
                        foundCompId = _danceCompHelperDb.AdjudicatorPanels
                            .TagWith(
                                nameof(FindCompetition) + "(Guid)[5]")
                            .FirstOrDefault(
                                x => x.AdjudicatorPanelId == foundAdj.AdjudicatorPanelId)
                            ?.CompetitionId;
                    }
                }

                if (foundCompId == null)
                {
                    foundCompId = _danceCompHelperDb.AdjudicatorPanels
                        .TagWith(
                            nameof(FindCompetition) + "(Guid)[6]")
                        .FirstOrDefault(
                            x => x.AdjudicatorPanelId == byAnyId)
                        ?.CompetitionId;
                }

                return foundCompId;
            }
            finally
            {
                _logger.LogTrace(
                    "{Method}() done",
                    nameof(FindCompetition));

                dbTrans.Rollback();
            }
        }

        public Guid? FindCompetitionClass(
            Guid? byAnyId)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                var foundCompClassId = _danceCompHelperDb.CompetitionClasses
                    .TagWith(
                        nameof(FindCompetitionClass) + "(Guid)[0]")
                    .FirstOrDefault(
                        x => x.CompetitionId == byAnyId
                        || x.CompetitionClassId == byAnyId
                        || x.AdjudicatorPanelId == byAnyId)
                    ?.CompetitionClassId;

                if (foundCompClassId == null)
                {
                    foundCompClassId = _danceCompHelperDb.Participants
                        .TagWith(
                            nameof(FindCompetitionClass) + "(Guid)[1]")
                        .FirstOrDefault(
                            x => x.ParticipantId == byAnyId)
                        ?.CompetitionClassId;
                }

                return foundCompClassId;
            }
            finally
            {
                _logger.LogTrace(
                    "{Method}() done",
                    nameof(FindCompetitionClass));

                dbTrans.Rollback();
            }
        }

        public Guid? GetCompetitionClass(string byName)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                return _danceCompHelperDb.CompetitionClasses
                    .TagWith(
                        nameof(GetCompetitionClass) + "(string)[0]")
                    .FirstOrDefault(
                        x => x.CompetitionClassName == byName
                        && x.Ignore == false)
                    ?.CompetitionClassId;
            }
            finally
            {
                _logger.LogTrace(
                    "{Method}() done",
                    nameof(GetCompetitionClass));

                dbTrans.Rollback();
            }
        }

        public CompetitionClass? GetCompetitionClass(
            Guid competitionId)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                return _danceCompHelperDb.CompetitionClasses
                    .TagWith(
                        nameof(GetCompetitionClass) + "(Guid)[0]")
                    .FirstOrDefault(
                        x => x.CompetitionClassId == competitionId);
            }
            finally
            {
                _logger.LogTrace(
                    "{Method}() done",
                    nameof(GetCompetitionClass));

                dbTrans.Rollback();
            }
        }

        public Participant? GetParticipant(
            Guid participantId)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                return _danceCompHelperDb.Participants
                    .TagWith(
                        nameof(GetParticipant) + "(Guid)[0]")
                    .FirstOrDefault(
                        x => x.ParticipantId == participantId);
            }
            finally
            {
                _logger.LogTrace(
                    "{Method}() done",
                    nameof(GetParticipant));

                dbTrans.Rollback();
            }
        }

        public AdjudicatorPanel? GetAdjudicatorPanel(
            Guid adjudicatorPanelId)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                return _danceCompHelperDb.AdjudicatorPanels
                    .TagWith(
                        nameof(GetAdjudicatorPanel) + "(Guid)[0]")
                    .FirstOrDefault(
                        x => x.AdjudicatorPanelId == adjudicatorPanelId);
            }
            finally
            {
                _logger.LogTrace(
                    "{Method}() done",
                    nameof(GetAdjudicatorPanel));

                dbTrans.Rollback();
            }
        }

        public Adjudicator? GetAdjudicator(
            Guid adjudicatorId)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                return _danceCompHelperDb.Adjudicators
                    .TagWith(
                        nameof(GetAdjudicator) + "(Guid)[0]")
                    .Include(
                         x => x.AdjudicatorPanel)
                    .FirstOrDefault(
                        x => x.AdjudicatorId == adjudicatorId);
            }
            finally
            {
                _logger.LogTrace(
                    "{Method}() done",
                    nameof(GetAdjudicator));

                dbTrans.Rollback();
            }
        }

        #endregion // Conversions/Lookups

        #region Competition Crud

        public void CreateCompetition(
            string competitionName,
            OrganizationEnum organization,
            string orgCompetitionId,
            string? competitionInfo,
            DateTime competitionDate,
            string? comment)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                _danceCompHelperDb.Competitions.Add(
                    new Competition()
                    {
                        CompetitionName = competitionName,
                        Organization = organization,
                        OrgCompetitionId = orgCompetitionId,
                        CompetitionInfo = competitionInfo,
                        CompetitionDate = competitionDate,
                        Comment = comment,
                    });

                _danceCompHelperDb.SaveChanges();
                dbTrans.Commit();
            }
            catch (Exception exc)
            {
                _logger.LogError(
                    exc,
                    "Error during {Method}: {Message}",
                    nameof(CreateCompetition),
                    exc.Message);

                dbTrans.Rollback();

                throw;
            }
            finally
            {
                _logger.LogTrace(
                    "{Method}() done",
                    nameof(CreateCompetition));
            }
        }

        public void EditCompetition(
            Guid competitionId,
            string competitionName,
            OrganizationEnum organization,
            string orgCompetitionId,
            string? competitionInfo,
            DateTime competitionDate,
            string? comment)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                var foundComp = _danceCompHelperDb.Competitions
                    .TagWith(
                        nameof(EditCompetition) + "(Guid, ...)[0]")
                    .FirstOrDefault(
                        x => x.CompetitionId == competitionId);

                if (foundComp == null)
                {
                    throw new ArgumentException(
                        string.Format(
                            "{0} with id '{1}' not found!",
                            nameof(Competition),
                            competitionId));
                }

                foundComp.CompetitionName = competitionName;
                foundComp.Organization = organization;
                foundComp.OrgCompetitionId = orgCompetitionId;
                foundComp.CompetitionInfo = competitionInfo;
                foundComp.CompetitionDate = competitionDate;
                foundComp.Comment = comment;

                _danceCompHelperDb.SaveChanges();
                dbTrans.Commit();
            }
            catch (Exception exc)
            {
                _logger.LogError(
                    exc,
                    "Error during {Method}: {Message}",
                    nameof(EditCompetition),
                    exc.Message);

                dbTrans.Rollback();

                throw;
            }
            finally
            {
                _logger.LogTrace(
                    "{Method}() done",
                    nameof(EditCompetition));
            }
        }

        public void RemoveCompetition(
            Guid competitionId)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                var foundComp = _danceCompHelperDb.Competitions
                    .TagWith(
                        nameof(RemoveCompetition) + "(Guid)[0]")
                    .FirstOrDefault(
                        x => x.CompetitionId == competitionId);

                if (foundComp == null)
                {
                    _logger.LogWarning(
                        "{Competition} with id '{CompetitionId}' not found!",
                        nameof(Competition),
                        competitionId);
                    return;
                }

                _danceCompHelperDb.Competitions.Remove(
                    foundComp);

                _danceCompHelperDb.SaveChanges();
                dbTrans.Commit();
            }
            catch (Exception exc)
            {
                _logger.LogError(
                    exc,
                    "Error during {Method}: {Message}",
                    nameof(RemoveCompetition),
                    exc.Message);

                dbTrans.Rollback();

                throw;
            }
            finally
            {
                _logger.LogTrace(
                    "{Method}() done",
                    nameof(RemoveCompetition));
            }
        }

        #endregion //  Competition Crud

        #region AdjudicatorPanel Crud

        public void CreateAdjudicatorPanel(
            Guid competitionId,
            string name,
            string? comment)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                var foundComp = _danceCompHelperDb.Competitions
                    .TagWith(
                        nameof(CreateAdjudicatorPanel) + "(Guid, string, string?)[0]")
                    .FirstOrDefault(
                        x => x.CompetitionId == competitionId);

                if (foundComp == null)
                {
                    throw new ArgumentException(
                        string.Format(
                            "{0} with id '{1}' not found!",
                            nameof(Competition),
                            competitionId));
                }

                _danceCompHelperDb.AdjudicatorPanels.Add(
                    new AdjudicatorPanel()
                    {
                        Competition = foundComp,
                        Name = name,
                        Comment = comment,
                    });

                _danceCompHelperDb.SaveChanges();
                dbTrans.Commit();
            }
            catch (Exception exc)
            {
                _logger.LogError(
                    exc,
                    "Error during {Method}: {Message}",
                    nameof(CreateAdjudicatorPanel),
                    exc.Message);

                dbTrans.Rollback();

                throw;
            }
            finally
            {
                _logger.LogTrace(
                    "{Method}() done",
                    nameof(CreateAdjudicatorPanel));
            }
        }

        public void EditAdjudicatorPanel(
            Guid adjudicatorPanelId,
            Guid competitionId,
            string name,
            string? comment)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                var foundAdjPanel = _danceCompHelperDb.AdjudicatorPanels
                    .TagWith(
                        nameof(EditAdjudicatorPanel) + "(Guid, ...)[0]")
                    .FirstOrDefault(
                        x => x.AdjudicatorPanelId == adjudicatorPanelId);

                if (foundAdjPanel == null)
                {
                    throw new ArgumentException(
                        string.Format(
                            "{0} with id '{1}' not found!",
                            nameof(AdjudicatorPanel),
                            adjudicatorPanelId));
                }

                foundAdjPanel.CompetitionId = competitionId;
                foundAdjPanel.Name = name;
                foundAdjPanel.Comment = comment;

                _danceCompHelperDb.SaveChanges();
                dbTrans.Commit();
            }
            catch (Exception exc)
            {
                _logger.LogError(
                    exc,
                    "Error during {Method}: {Message}",
                    nameof(EditAdjudicatorPanel),
                    exc.Message);

                dbTrans.Rollback();

                throw;
            }
            finally
            {
                _logger.LogTrace(
                    "{Method}() done",
                    nameof(EditAdjudicatorPanel));
            }
        }

        public void RemoveAdjudicatorPanel(
            Guid adjudicatorPanelId)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                var foundAdjPanel = _danceCompHelperDb.AdjudicatorPanels
                    .TagWith(
                        nameof(RemoveAdjudicatorPanel) + "(Guid)")
                    .FirstOrDefault(
                        x => x.AdjudicatorPanelId == adjudicatorPanelId);

                if (foundAdjPanel == null)
                {
                    _logger.LogWarning(
                        "{AdjudicatorPanel} with id '{AdjudicatorPanelId}' not found!",
                        nameof(AdjudicatorPanel),
                        adjudicatorPanelId);
                    return;
                }

                _danceCompHelperDb.AdjudicatorPanels.Remove(
                    foundAdjPanel);

                _danceCompHelperDb.SaveChanges();
                dbTrans.Commit();
            }
            catch (Exception exc)
            {
                _logger.LogError(
                    exc,
                    "Error during {Method}: {Message}",
                    nameof(RemoveAdjudicatorPanel),
                    exc.Message);

                dbTrans.Rollback();

                throw;
            }
            finally
            {
                _logger.LogTrace(
                    "{Method}() done",
                    nameof(RemoveAdjudicatorPanel));
            }
        }

        #endregion //  AdjudicatorPanel Crud

        #region Adjudicator Crud

        public void CreateAdjudicator(
            Guid adjudicatorPanelId,
            string abbreviation,
            string name,
            string? comment)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                _danceCompHelperDb.Adjudicators.Add(
                    new Adjudicator()
                    {
                        AdjudicatorPanelId = adjudicatorPanelId,
                        Abbreviation = abbreviation,
                        Name = name,
                        Comment = comment,
                    });

                _danceCompHelperDb.SaveChanges();
                dbTrans.Commit();
            }
            catch (Exception exc)
            {
                _logger.LogError(
                    exc,
                    "Error during {Method}: {Message}",
                    nameof(CreateAdjudicator),
                    exc.Message);

                dbTrans.Rollback();

                throw;
            }
            finally
            {
                _logger.LogTrace(
                    "{Method}() done",
                    nameof(CreateAdjudicator));
            }
        }

        public void EditAdjudicator(
            Guid adjudicatorId,
            Guid adjudicatorPanelId,
            string abbreviation,
            string name,
            string? comment)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                var foundAdj = _danceCompHelperDb.Adjudicators
                    .TagWith(
                        nameof(EditAdjudicator) + "(Guid, ...)[0]")
                    .FirstOrDefault(
                        x => x.AdjudicatorId == adjudicatorId);

                if (foundAdj == null)
                {
                    throw new ArgumentException(
                        string.Format(
                            "{0} with id '{1}' not found!",
                            nameof(Adjudicator),
                            adjudicatorId));
                }

                foundAdj.AdjudicatorPanelId = adjudicatorPanelId;
                foundAdj.Abbreviation = abbreviation;
                foundAdj.Name = name;
                foundAdj.Comment = comment;

                _danceCompHelperDb.SaveChanges();
                dbTrans.Commit();
            }
            catch (Exception exc)
            {
                _logger.LogError(
                    exc,
                    "Error during {Method}: {Message}",
                    nameof(EditAdjudicator),
                    exc.Message);

                dbTrans.Rollback();

                throw;
            }
            finally
            {
                _logger.LogTrace(
                    "{Method}() done",
                    nameof(EditAdjudicator));
            }
        }

        public void RemoveAdjudicator(
            Guid adjudicatorId)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                var foundAdj = _danceCompHelperDb.Adjudicators
                    .TagWith(
                        nameof(RemoveAdjudicator) + "(Guid)")
                    .FirstOrDefault(
                        x => x.AdjudicatorId == adjudicatorId);

                if (foundAdj == null)
                {
                    _logger.LogWarning(
                        "{Adjudicator} with id '{AdjudicatorId}' not found!",
                        nameof(Adjudicator),
                        adjudicatorId);
                    return;
                }

                _danceCompHelperDb.Adjudicators.Remove(
                    foundAdj);

                _danceCompHelperDb.SaveChanges();
                dbTrans.Commit();
            }
            catch (Exception exc)
            {
                _logger.LogError(
                    exc,
                    "Error during {Method}: {Message}",
                    nameof(RemoveAdjudicator),
                    exc.Message);

                dbTrans.Rollback();

                throw;
            }
            finally
            {
                _logger.LogTrace(
                    "{Method}() done",
                    nameof(RemoveAdjudicator));
            }
        }

        #endregion //  Adjudicator Crud

        #region CompetitionClass Crud

        public void CreateCompetitionClass(
            Guid competitionId,
            string competitionClassName,
            string orgClassId,
            string? discipline,
            string? ageClass,
            string? ageGroup,
            string? className,
            int minStartsForPromotion,
            int minPointsForPromotion,
            int pointsForFirst,
            int extraManualStarter,
            string? comment,
            bool ignore)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                var foundCompId = _danceCompHelperDb.Competitions
                    .TagWith(
                        nameof(CreateCompetitionClass) + "(Guid,...)[0]")
                    .FirstOrDefault(
                        x => x.CompetitionId == competitionId);

                if (foundCompId == null)
                {
                    throw new ArgumentException(
                        string.Format(
                            "{0} with id '{1}' not found!",
                            nameof(Competition),
                            competitionId));
                }

                _danceCompHelperDb.CompetitionClasses.Add(
                    new CompetitionClass()
                    {
                        Competition = foundCompId,
                        OrgClassId = orgClassId,
                        CompetitionClassName = competitionClassName,
                        Discipline = discipline,
                        AgeClass = ageClass,
                        AgeGroup = ageGroup,
                        Class = className,
                        MinStartsForPromotion = minStartsForPromotion,
                        MinPointsForPromotion = minPointsForPromotion,
                        PointsForFirst = pointsForFirst,
                        ExtraManualStarter = extraManualStarter,
                        Comment = comment,
                        Ignore = ignore,
                    });

                _danceCompHelperDb.SaveChanges();
                dbTrans.Commit();
            }
            catch (Exception exc)
            {
                _logger.LogError(
                    exc,
                    "Error during {Method}: {Message}",
                    nameof(CreateCompetitionClass),
                    exc.Message);

                dbTrans.Rollback();

                throw;
            }
            finally
            {
                _logger.LogTrace(
                    "{Method}() done",
                    nameof(CreateCompetitionClass));
            }
        }

        public void EditCompetitionClass(
            Guid competitionClassId,
            string competitionClassName,
            string orgClassId,
            string? discipline,
            string? ageClass,
            string? ageGroup,
            string? className,
            int minStartsForPromotion,
            int minPointsForPromotion,
            int pointsForFirst,
            int extraManualStarter,
            string? comment,
            bool ignore)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                var foundCompClass = _danceCompHelperDb.CompetitionClasses
                    .TagWith(
                        nameof(EditCompetitionClass) + "(Guid, ...)[0]")
                    .FirstOrDefault(
                        x => x.CompetitionClassId == competitionClassId);

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

                _danceCompHelperDb.SaveChanges();
                dbTrans.Commit();
            }
            catch (Exception exc)
            {
                _logger.LogError(
                    exc,
                    "Error during {Method}: {Message}",
                    nameof(EditCompetitionClass),
                    exc.Message);

                dbTrans.Rollback();

                throw;
            }
            finally
            {
                _logger.LogTrace(
                    "{Method}() done",
                    nameof(EditCompetitionClass));
            }
        }

        public void RemoveCompetitionClass(
            Guid competitionClassId)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                var foundCompClass = _danceCompHelperDb.CompetitionClasses
                    .TagWith(
                        nameof(RemoveCompetitionClass) + "(Guid)[0]")
                    .FirstOrDefault(
                        x => x.CompetitionClassId == competitionClassId);

                if (foundCompClass == null)
                {
                    _logger.LogWarning(
                        "{CompetitionClass} with id '{CompetitionClassId}' not found!",
                        nameof(CompetitionClass),
                        competitionClassId);
                    return;
                }

                _danceCompHelperDb.CompetitionClasses.Remove(
                    foundCompClass);

                _danceCompHelperDb.SaveChanges();
                dbTrans.Commit();
            }
            catch (Exception exc)
            {
                _logger.LogError(
                    exc,
                    "Error during {Method}: {Message}",
                    nameof(RemoveCompetitionClass),
                    exc.Message);

                dbTrans.Rollback();

                throw;
            }
            finally
            {
                _logger.LogTrace(
                    "{Method}() done",
                    nameof(RemoveCompetitionClass));
            }
        }

        #endregion //  CompetitionClass Crud

        #region Participant Crud

        public void CreateParticipant(
            Guid competitionId,
            Guid competitionClassId,
            int startNumber,
            string namePartA,
            string? orgIdPartA,
            string? namePartB,
            string? orgIdPartB,
            string? clubName,
            string? orgIdClub,
            int orgPointsPartA,
            int orgStartsPartA,
            int? minStartsForPromotionPartA,
            int? orgPointsPartB,
            int? orgStartsPartB,
            int? minStartsForPromotionPartB,
            string? comment,
            bool ignore)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                var foundCompId = _danceCompHelperDb.Competitions
                    .TagWith(
                        nameof(CreateParticipant) + "(Guid,...)[0]")
                    .FirstOrDefault(
                        x => x.CompetitionId == competitionId);

                if (foundCompId == null)
                {
                    throw new ArgumentException(
                        string.Format(
                            "{0} with id '{1}' not found!",
                            nameof(Competition),
                            competitionId));
                }

                var foundCompClassId = _danceCompHelperDb.CompetitionClasses
                    .TagWith(
                        nameof(CreateCompetitionClass) + "(Guid,...)[1]")
                    .FirstOrDefault(
                        x => x.CompetitionClassId == competitionClassId);

                if (foundCompClassId == null)
                {
                    throw new ArgumentException(
                        string.Format(
                            "{0} with id '{1}' not found!",
                            nameof(CompetitionClass),
                            competitionClassId));
                }

                _danceCompHelperDb.Participants.Add(
                    new Participant()
                    {
                        CompetitionId = competitionId,
                        CompetitionClassId = competitionClassId,
                        StartNumber = startNumber,
                        NamePartA = namePartA,
                        OrgIdPartA = orgIdPartA,
                        NamePartB = namePartB,
                        OrgIdPartB = orgIdPartB,
                        ClubName = clubName,
                        OrgIdClub = orgIdClub,
                        OrgPointsPartA = orgPointsPartA,
                        OrgStartsPartA = orgStartsPartA,
                        MinStartsForPromotionPartA = minStartsForPromotionPartA,
                        OrgPointsPartB = orgPointsPartB,
                        OrgStartsPartB = orgStartsPartB,
                        MinStartsForPromotionPartB = minStartsForPromotionPartB,
                        Comment = comment,
                        Ignore = ignore,
                    });

                _danceCompHelperDb.SaveChanges();
                dbTrans.Commit();
            }
            catch (Exception exc)
            {
                _logger.LogError(
                    exc,
                    "Error during {Method}: {Message}",
                    nameof(CreateParticipant),
                    exc.Message);

                dbTrans.Rollback();

                throw;
            }
            finally
            {
                _logger.LogTrace(
                    "{Method}() done",
                    nameof(CreateParticipant));
            }
        }

        public void EditParticipant(
            Guid participantId,
            Guid competitionClassId,
            int startNumber,
            string namePartA,
            string? orgIdPartA,
            string? namePartB,
            string? orgIdPartB,
            string? clubName,
            string? orgIdClub,
            int orgPointsPartA,
            int orgStartsPartA,
            int? minStartsForPromotionPartA,
            int? orgPointsPartB,
            int? orgStartsPartB,
            int? minStartsForPromotionPartB,
            string? comment,
            bool ignore)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                var foundCompClassId = _danceCompHelperDb.CompetitionClasses
                    .TagWith(
                        nameof(EditParticipant) + "(Guid,...)[0]")
                    .FirstOrDefault(
                        x => x.CompetitionClassId == competitionClassId);

                if (foundCompClassId == null)
                {
                    throw new ArgumentException(
                        string.Format(
                            "{0} with id '{1}' not found!",
                            nameof(CompetitionClass),
                            competitionClassId));
                }

                var foundParticipant = _danceCompHelperDb.Participants
                    .TagWith(
                        nameof(EditParticipant) + "(Guid,...)[1]")
                    .FirstOrDefault(
                        x => x.ParticipantId == participantId);

                if (foundParticipant == null)
                {
                    throw new ArgumentException(
                        string.Format(
                            "{0} with id '{1}' not found!",
                            nameof(Participant),
                            participantId));
                }

                foundParticipant.CompetitionClassId = competitionClassId;
                foundParticipant.StartNumber = startNumber;
                foundParticipant.NamePartA = namePartA;
                foundParticipant.OrgIdPartA = orgIdPartA;
                foundParticipant.NamePartB = namePartB;
                foundParticipant.OrgIdPartB = orgIdPartB;
                foundParticipant.ClubName = clubName;
                foundParticipant.OrgIdClub = orgIdClub;
                foundParticipant.OrgPointsPartA = orgPointsPartA;
                foundParticipant.OrgStartsPartA = orgStartsPartA;
                foundParticipant.MinStartsForPromotionPartA = minStartsForPromotionPartA;
                foundParticipant.OrgPointsPartB = orgPointsPartB;
                foundParticipant.OrgStartsPartB = orgStartsPartB;
                foundParticipant.MinStartsForPromotionPartB = minStartsForPromotionPartB;
                foundParticipant.Comment = comment;
                foundParticipant.Ignore = ignore;

                _danceCompHelperDb.SaveChanges();
                dbTrans.Commit();
            }
            catch (Exception exc)
            {
                _logger.LogError(
                    exc,
                    "Error during {Method}: {Message}",
                    nameof(EditParticipant),
                    exc.Message);

                dbTrans.Rollback();

                throw;
            }
            finally
            {
                _logger.LogTrace(
                    "{Method}() done",
                    nameof(EditParticipant));
            }
        }

        public void RemoveParticipant(
            Guid participantId)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                var foundParticipant = _danceCompHelperDb.Participants
                    .TagWith(
                        nameof(RemoveParticipant) + "(Guid)[0]")
                    .FirstOrDefault(
                        x => x.ParticipantId == participantId);

                if (foundParticipant == null)
                {
                    _logger.LogWarning(
                        "{Participant} with id '{ParticipantId}' not found!",
                        nameof(Participant),
                        participantId);
                    return;
                }

                _danceCompHelperDb.Participants.Remove(
                    foundParticipant);

                _danceCompHelperDb.SaveChanges();
                dbTrans.Commit();
            }
            catch (Exception exc)
            {
                _logger.LogError(
                    exc,
                    "Error during {Method}: {Message}",
                    nameof(RemoveParticipant),
                    exc.Message);

                dbTrans.Rollback();

                throw;
            }
            finally
            {
                _logger.LogTrace(
                    "{Method}() done",
                    nameof(RemoveParticipant));
            }
        }

        #endregion // Participant Crud

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

        #endregion // IDisposable
    }
}
