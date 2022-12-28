using DanceCompetitionHelper.Config;
using DanceCompetitionHelper.Database;
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
            _danceCompHelperDb.Migrate();
            _logger.LogDebug(
                "Do '{Method}'",
                nameof(Migrate));
        }

        public IEnumerable<Competition> GetCompetitions()
        {
            return _danceCompHelperDb.Competitions;
        }

        public IEnumerable<CompetitionClass> GetCompetitionClasses(
            Guid? competitionId)
        {
            if (competitionId == null)
            {
                return Enumerable.Empty<CompetitionClass>();
            }

            var foundComp = _danceCompHelperDb.Competitions.FirstOrDefault(
                x => x.CompetitionId == competitionId);

            if (foundComp == null)
            {
                return Enumerable.Empty<CompetitionClass>();
            }

            return _danceCompHelperDb.CompetitionClasses
                .Where(x => x.Competition == foundComp);
        }

        public IEnumerable<Participant> GetParticipants(
            Guid? competitionId)
        {
            if (competitionId == null)
            {
                return Enumerable.Empty<Participant>();
            }

            var foundComp = _danceCompHelperDb.Competitions.FirstOrDefault(
                x => x.CompetitionId == competitionId);

            if (foundComp == null)
            {
                return Enumerable.Empty<Participant>();
            }

            return _danceCompHelperDb.Participants
                .Where(
                    x => x.Competition == foundComp
                    && x.Ignore == false);
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

            var foundComp = _danceCompHelperDb.Competitions.FirstOrDefault(
                x => x.CompetitionId == competitionId);
            var foundCompClass = _danceCompHelperDb.CompetitionClasses.FirstOrDefault(
                x => x.CompetitionId == competitionId
                && x.CompetitionClassId == competitionClassId
                && x.Ignore == false);

            if (foundComp == null
                || foundCompClass == null)
            {
                return Enumerable.Empty<Participant>();
            }

            return _danceCompHelperDb.Participants
                .Where(
                    x => x.Competition == foundComp
                    && x.CompetitionClass == foundCompClass
                    && x.Ignore == false);
        }

        public IEnumerable<CompetitionClassInfo> GetCompetitionClassInfos(
            Guid competitionId,
            IEnumerable<Guid>? competitionClassIds)
        {
            var foundComp = _danceCompHelperDb.Competitions.FirstOrDefault(
                x => x.CompetitionId == competitionId);

            if (foundComp == null)
            {
                yield break;
            }

            var useCompClasses = _danceCompHelperDb.CompetitionClasses
                .Where(x => x.Competition == foundComp);

            if (competitionClassIds != null)
            {
                var useCompClassIds = new HashSet<Guid>(competitionClassIds);

                if (useCompClassIds.Count >= 1)
                {
                    useCompClasses = useCompClasses.Where(
                        x => useCompClassIds.Contains(x.CompetitionClassId));
                }
            }

            var participtansByClassid = new Dictionary<Guid, List<Participant>>();
            // var multipleStartsByPart = new Dictionary<Guid, >
            var allPart = new List<Participant>();

            foreach (var curPart in _danceCompHelperDb.Participants.Where(
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

            foreach (var curCompClass in useCompClasses.OrderBy(x => x.OrgClassId))
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

        public IEnumerable<(List<Participant> Participant, List<CompetitionClass> CompetitionClasses)> GetMultipleStarter(
            Guid competitionId)
        {
            var foundComp = _danceCompHelperDb.Competitions.FirstOrDefault(
                x => x.CompetitionId == competitionId);

            if (foundComp == null)
            {
                yield break;
            }

            var multipleStarterGouping = _danceCompHelperDb.Participants
                .TagWith(nameof(GetMultipleStarter))
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

            _logger.LogTrace(
                "{Method}() done",
                nameof(GetMultipleStarter));
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
            return _danceCompHelperDb.Competitions
                .FirstOrDefault(
                    x => x.CompetitionName == byName)
                ?.CompetitionId;
        }

        public Guid? GetCompetitionClass(string byName)
        {
            return _danceCompHelperDb.CompetitionClasses
                .FirstOrDefault(
                    x => x.CompetitionClassName == byName
                    && x.Ignore == false)
                ?.CompetitionClassId;
        }

        #endregion // Conversions/Lookups

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
