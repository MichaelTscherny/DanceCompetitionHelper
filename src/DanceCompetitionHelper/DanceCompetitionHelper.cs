using DanceCompetitionHelper.Database;
using DanceCompetitionHelper.Database.Tables;
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
                "Do '{0}'",
                nameof(Migrate));
        }

        public IEnumerable<Competition> GetCompetitions()
        {
            return _danceCompHelperDb.Competitions;
        }

        public IEnumerable<CompetitionClass> GetCompetitionClasses(
            string competitionName)
        {
            var foundComp = _danceCompHelperDb.Competitions.FirstOrDefault(
                x => x.CompetitionName == competitionName);

            if (foundComp == null)
            {
                return Enumerable.Empty<CompetitionClass>();
            }

            return _danceCompHelperDb.CompetitionClasses
                .Where(x => x.Competition == foundComp);
        }

        public IEnumerable<Participant> GetParticipants(
            string competitionName)
        {
            var foundComp = _danceCompHelperDb.Competitions.FirstOrDefault(
                x => x.CompetitionName == competitionName);

            if (foundComp == null)
            {
                return Enumerable.Empty<Participant>();
            }

            return _danceCompHelperDb.Participants
                .Where(x => x.Competition == foundComp);
        }

        protected virtual void Dispose(bool disposing)
        {
            _logger.LogDebug(
                "Do '{0}'",
                nameof(Dispose));

            _danceCompHelperDb.Dispose();
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
