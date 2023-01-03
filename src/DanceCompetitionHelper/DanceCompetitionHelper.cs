using DanceCompetitionHelper.Config;
using DanceCompetitionHelper.Database;
using DanceCompetitionHelper.Database.DisplayInfo;
using DanceCompetitionHelper.Database.Enum;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Info;
using DanceCompetitionHelper.OrgImpl;
using DanceCompetitionHelper.OrgImpl.Oetsv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DanceCompetitionHelper
{
    public class DanceCompetitionHelper : IDanceCompetitionHelper
    {
        private readonly DanceCompetitionHelperDbContext _danceCompHelperDb;
        private readonly ILogger<DanceCompetitionHelperDbContext> _logger;

        public DanceCompetitionHelper(
            DanceCompetitionHelperDbContext danceCompHelperDb,
            ILogger<DanceCompetitionHelperDbContext> logger)
        {
            _danceCompHelperDb = danceCompHelperDb
                ?? throw new ArgumentNullException(
                    nameof(danceCompHelperDb));

            _logger = logger
                ?? throw new ArgumentNullException(
                    nameof(logger));
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
                        nameof(GetCompetition) + "(Guid)")
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
                            nameof(GetCompetitions) + "(bool?)")
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
                            nameof(GetCompetitions) + "(bool?)")
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
                        nameof(GetCompetitions) + "(bool?)")
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
                        nameof(GetCompetitionClasses) + "(Guid?)")
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
                                nameof(GetCompetitionClasses) + "(Guid?)")
                            .Where(
                                x => x.CompetitionId == foundComp.CompetitionId)
                            .OrderBy(
                                x => x.NamePartA)
                            .ThenBy(
                                x => x.OrgIdPartA)
                            .ThenBy(
                                x => x.NamePartB)
                            .ThenBy(
                                x => x.OrgIdPartB)
                            .ThenBy(
                                x => x.OrgIdClub)
                            .ThenBy(
                                x => x.StartNumber))
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
                        nameof(GetCompetitionClasses) + "(Guid?)")
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
            Guid? competitionId)
        {
            if (competitionId == null)
            {
                return Enumerable.Empty<Participant>();
            }

            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                var foundComp = _danceCompHelperDb.Competitions
                    .TagWith(
                        nameof(GetParticipants) + "(Guid?)")
                    .FirstOrDefault(
                        x => x.CompetitionId == competitionId);

                if (foundComp == null)
                {
                    return Enumerable.Empty<Participant>();
                }

                return _danceCompHelperDb.Participants
                    .TagWith(
                        nameof(GetParticipants) + "(Guid?)")
                    .Where(
                        x => x.Competition == foundComp
                        && x.Ignore == false);
            }
            finally
            {
                dbTrans.Rollback();
            }
        }

        public IEnumerable<Participant> GetParticipants(
            Guid? competitionId,
            Guid? competitionClassId)
        {
            if (competitionId == null
                || competitionClassId == null)
            {
                return Enumerable.Empty<Participant>();
            }

            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                var foundComp = _danceCompHelperDb.Competitions
                    .TagWith(
                        nameof(GetParticipants) + "(Guid?, Guid?)")
                    .FirstOrDefault(
                        x => x.CompetitionId == competitionId);
                var foundCompClass = _danceCompHelperDb.CompetitionClasses
                    .TagWith(
                        nameof(GetParticipants) + "(Guid?, Guid?)")
                    .FirstOrDefault(
                        x => x.CompetitionId == competitionId
                        && x.CompetitionClassId == competitionClassId
                        && x.Ignore == false);

                if (foundComp == null
                    || foundCompClass == null)
                {
                    return Enumerable.Empty<Participant>();
                }

                return _danceCompHelperDb.Participants
                    .TagWith(
                        nameof(GetParticipants) + "(Guid?, Guid?)")
                    .Where(
                        x => x.Competition == foundComp
                        && x.CompetitionClass == foundCompClass
                        && x.Ignore == false);
            }
            finally
            {
                dbTrans.Rollback();
            }
        }

        /*
        public IEnumerable<CompetitionClassInfo> GetCompetitionClassInfos(
            Guid competitionId,
            IEnumerable<Guid>? competitionClassIds)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                var foundComp = _danceCompHelperDb.Competitions
                    .TagWith(
                        nameof(GetCompetitionClassInfos) + "(Guid, IEnumerable<Guid>?)")
                    .FirstOrDefault(
                        x => x.CompetitionId == competitionId);

                if (foundComp == null)
                {
                    yield break;
                }

                var useCompClasses = _danceCompHelperDb.CompetitionClasses
                    .TagWith(
                        nameof(GetCompetitionClassInfos) + "(Guid, IEnumerable<Guid>?)")
                    .Where(
                        x => x.Competition == foundComp);

                if (competitionClassIds != null)
                {
                    var useCompClassIds = new HashSet<Guid>(competitionClassIds);

                    if (useCompClassIds.Count >= 1)
                    {
                        useCompClasses = useCompClasses
                            .TagWith(
                                nameof(GetCompetitionClassInfos) + "(Guid, IEnumerable<Guid>?)")
                            .Where(
                                x => useCompClassIds.Contains(x.CompetitionClassId));
                    }
                }

                var participtansByClassid = new Dictionary<Guid, List<Participant>>();
                // var multipleStartsByPart = new Dictionary<Guid, >
                var allPart = new List<Participant>();

                foreach (var curPart in _danceCompHelperDb.Participants
                    .TagWith(
                        nameof(GetCompetitionClassInfos) + "(Guid, IEnumerable<Guid>?)")
                    .Where(
                        x => x.Competition == foundComp
                        && x.Ignore == false))
                {
                    allPart.Add(
                        curPart);

                    if (participtansByClassid.TryGetValue(
                        curPart.CompetitionClassId,
                        out var curParts) == false)
                    {
                        curParts = new List<Participant>();
                        participtansByClassid[curPart.CompetitionClassId] = curParts;
                    }

                    curParts.Add(curPart);
                }

                /*
                var allPart = _danceCompHelperDb.Participants.Where(
                    x => x.Competition == foundComp
                    && x.Ignore == false)
                    .ToList();
                *

                foreach (var curCompClass in useCompClasses
                    .TagWith(
                        nameof(GetCompetitionClassInfos) + "(Guid, IEnumerable<Guid>?)")
                    .OrderBy(
                        x => x.OrgClassId))
                {
                    var extraPart = new ExtraParticipants();
                    var compSettings = new CompetitionClassSettings();

                    var retCompClassInfo = new CompetitionClassInfo(
                        this,
                        curCompClass.Competition,
                        curCompClass,
                        GetExtraParticipants(
                            curCompClass,
                            participtansByClassid),
                        GetCompetitionClassSettings(
                            curCompClass));

                    yield return retCompClassInfo;
                }
            }
            finally
            {
                dbTrans.Rollback();
            }
        }
        */

        public IEnumerable<(List<Participant> Participant, List<CompetitionClass> CompetitionClasses)> GetMultipleStarterReuseTransacion(
            Guid competitionId)
        {
            var multipleStarterGouping = _danceCompHelperDb.Participants
                .TagWith(
                    nameof(GetMultipleStarterReuseTransacion) + "(Guid)")
                .Where(
                    x => x.CompetitionId == competitionId)
                .GroupBy(
                    x => new
                    {
                        x.NamePartA,
                        x.OrgIdPartA,
                        x.NamePartB,
                        x.OrgIdPartB,
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
                        nameof(GetMultipleStarterReuseTransacion) + "(Guid)")
                    .Where(
                        x => x.NamePartA == curMultiStart.NamePartA
                        && x.OrgIdPartA == curMultiStart.OrgIdPartA
                        && x.NamePartB == curMultiStart.NamePartB
                        && x.OrgIdPartB == curMultiStart.OrgIdPartB
                        && x.OrgIdClub == curMultiStart.OrgIdClub)
                    .ToList();
                var allComps = allPartInfo
                    .Where(
                        x => x.CompetitionClass != null)
                    .Select(
                        x => x.CompetitionClass)
                    .Distinct()
                    .ToList();

                yield return (allPartInfo,
                    allComps);
            }
        }


        public IEnumerable<(List<Participant> Participant, List<CompetitionClass> CompetitionClasses)> GetMultipleStarter(
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

        public ExtraParticipants GetExtraParticipants(
            CompetitionClass competitionClass,
            Dictionary<Guid, List<Participant>> participantsByCompClass)
        {
            var partChecker = GetParticipantChecker(
                competitionClass.Competition);

            _logger.LogTrace(
                "{Method}() done",
                nameof(GetExtraParticipants));

            return new ExtraParticipants();
        }

        public CompetitionClassSettings GetCompetitionClassSettings(
            CompetitionClass competitionClass)
        {
            var partChecker = GetParticipantChecker(
                competitionClass.Competition);

            _logger.LogTrace(
                "{Method}() done",
                nameof(GetCompetitionClassSettings));

            return new CompetitionClassSettings();
        }

        #region Get helper

        public IParticipantChecker GetParticipantChecker(
            Competition competition)
        {
            switch (competition.Organization)
            {
                case OrganizationEnum.Oetsv:
                    return new OetsvParticipantChecker();

                default:
                    throw new NotImplementedException(
                        string.Format(
                            "{Method}: '{Organization}' not yet implemented!",
                            nameof(GetParticipantChecker),
                            competition.Organization));
            }
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
                        nameof(GetCompetition) + "(string)")
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
                        nameof(FindCompetition) + "(Guid)")
                    .FirstOrDefault(
                        x => x.CompetitionId == byAnyId
                        || x.CompetitionClassId == byAnyId)
                    ?.CompetitionId;

                if (foundCompId == null)
                {
                    foundCompId = _danceCompHelperDb.Competitions
                        .TagWith(
                            nameof(FindCompetition) + "(Guid)")
                        .FirstOrDefault(
                            x => x.CompetitionId == byAnyId)
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

        public Guid? GetCompetitionClass(string byName)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                return _danceCompHelperDb.CompetitionClasses
                    .TagWith(
                        nameof(GetCompetitionClass) + "(string)")
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
                        nameof(GetCompetitionClass) + "(Guid)")
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

        #endregion // Conversions/Lookups

        #region Competition Crud

        public void CreateCompetition(
            string competitionName,
            OrganizationEnum organization,
            string orgCompetitionId,
            string? competitionInfo,
            DateTime competitionDate)
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
            DateTime competitionDate)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                var foundComp = _danceCompHelperDb.Competitions
                    .TagWith(
                        nameof(EditCompetition) + "(Guid, ...)")
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
                        nameof(RemoveCompetition) + "(Guid)")
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
            bool ignore)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                var foundCompId = _danceCompHelperDb.Competitions
                    .TagWith(
                        nameof(CreateCompetitionClass) + "(Guid,...)")
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
            bool ignore)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                var foundCompClass = _danceCompHelperDb.CompetitionClasses
                    .TagWith(
                        nameof(EditCompetitionClass) + "(Guid, ...)")
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
                        nameof(RemoveCompetitionClass) + "(Guid)")
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
