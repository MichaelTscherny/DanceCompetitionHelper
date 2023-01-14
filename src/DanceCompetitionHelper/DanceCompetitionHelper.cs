using DanceCompetitionHelper.Database;
using DanceCompetitionHelper.Database.DisplayInfo;
using DanceCompetitionHelper.Database.Enum;
using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Tables;
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
            Guid competitionId)
        {
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

                        useDisplayInfo.CountMultipleStarters = GetMultipleStarterReuseTransacion(
                            curComp.CompetitionId)
                            .Count();
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
                var multiStartsByCompClass = new Dictionary<Guid, int>();

                if (includeInfos)
                {
                    foreach (var curPart in _danceCompHelperDb.Participants
                        .TagWith(
                            nameof(GetCompetitionClasses) + "(Guid?)[1]")
                        .Where(
                            x => x.CompetitionId == foundComp.CompetitionId)
                        .OrderBy(
                            x => x.CompetitionId)
                        .ThenByDefault())
                    {
                        if (partsByCompClass.TryGetValue(
                            curPart.CompetitionClassId,
                            out var curParticipants) == false)
                        {
                            curParticipants = new List<Participant>();
                            partsByCompClass.Add(
                                curPart.CompetitionClassId,
                                curParticipants);
                        }

                        curParticipants.Add(
                            curPart);
                    }

                    var multipleStarters = GetMultipleStarterReuseTransacion(
                        competitionId.Value)
                        .ToList();

                    foreach (var curMultiStart in multipleStarters
                        .SelectMany(x => x.CompetitionClasses))
                    {
                        var useCompClassid = curMultiStart.CompetitionClassId;
                        if (multiStartsByCompClass.ContainsKey(
                            useCompClassid) == false)
                        {
                            multiStartsByCompClass[useCompClassid] = 1;
                        }
                        else
                        {
                            multiStartsByCompClass[useCompClassid]++;
                        }
                    }
                }

                foreach (var curCompClass in _danceCompHelperDb.CompetitionClasses
                    .TagWith(
                        nameof(GetCompetitionClasses) + "(Guid?)[2]")
                    .Where(
                        x => x.CompetitionId == foundComp.CompetitionId)
                    .OrderBy(
                        x => x.OrgClassId)
                    .ThenBy(
                        x => x.CompetitionClassName))
                {
                    if (includeInfos)
                    {
                        if (curCompClass.DisplayInfo == null)
                        {
                            curCompClass.DisplayInfo = new CompetitionClassDisplayInfo();
                        }

                        var useDisplayInfo = curCompClass.DisplayInfo;
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
                        useDisplayInfo.CountMultipleStarters = curCntMultiStarter;
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
                yield break;
            }

            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
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
                        GetMultipleStarterReuseTransacion(
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
                            useDisplayInfo.MultipleStartInfo.MultipleStarts = true;
                            useDisplayInfo.MultipleStartInfo.MultipleStartsInfo =
                                curClassInfos.GetCompetitionClasseNames();
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
            finally
            {
                dbTrans.Rollback();
            }
        }

        public IEnumerable<MultipleStarter> GetMultipleStarterReuseTransacion(
            Guid competitionId)
        {
            var multipleStarterGouping = _danceCompHelperDb.Participants
                .TagWith(
                    nameof(GetMultipleStarterReuseTransacion) + "(Guid)[0]")
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
                        nameof(GetMultipleStarterReuseTransacion) + "(Guid)[1]")
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
                return GetMultipleStarterReuseTransacion(
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
                        || x.CompetitionClassId == byAnyId)
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
                        || x.CompetitionClassId == byAnyId)
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
            int? orgPointsPartB,
            int? orgStartsPartB,
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
                        OrgPointsPartB = orgPointsPartB,
                        OrgStartsPartB = orgStartsPartB,
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
            int? orgPointsPartB,
            int? orgStartsPartB,
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
                foundParticipant.OrgPointsPartB = orgPointsPartB;
                foundParticipant.OrgStartsPartB = orgStartsPartB;
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
