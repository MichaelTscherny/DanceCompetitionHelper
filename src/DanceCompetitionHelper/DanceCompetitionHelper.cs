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

                        if (countsOfCompClasses.TryGetValue(
                            curComp.CompetitionId,
                            out var countCompClasses))
                        {
                            curComp.DisplayInfo.CountCompetitionClasses = countCompClasses;
                        }

                        if (countsOfParticipants.TryGetValue(
                            curComp.CompetitionId,
                            out var countParticipants))
                        {
                            curComp.DisplayInfo.CountParticipants = countParticipants;
                        }
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
            Guid? competitionId)
        {
            if (competitionId == null)
            {
                return Enumerable.Empty<CompetitionClass>();
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
                    return Enumerable.Empty<CompetitionClass>();
                }

                return _danceCompHelperDb.CompetitionClasses
                    .TagWith(
                        nameof(GetCompetitionClasses) + "(Guid?)")
                    .Include(x => x.Competition)
                    .Where(
                        x => x.CompetitionId == foundComp.CompetitionId);
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
                */

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

        public IEnumerable<(List<Participant> Participant, List<CompetitionClass> CompetitionClasses)> GetMultipleStarter(
            Guid competitionId)
        {
            using var dbTrans = _danceCompHelperDb.BeginTransaction();

            try
            {
                var foundComp = _danceCompHelperDb.Competitions
                    .TagWith(
                        nameof(GetMultipleStarter) + "(Guid)")
                    .FirstOrDefault(
                        x => x.CompetitionId == competitionId);

                if (foundComp == null)
                {
                    yield break;
                }

                var multipleStarterGouping = _danceCompHelperDb.Participants
                    .TagWith(
                        nameof(GetMultipleStarter) + "(Guid)")
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
                            nameof(GetMultipleStarter) + "(Guid)")
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
