using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper
{
    public interface IDanceCompetitionHelper : IDisposable
    {
        void Migrate();
        IEnumerable<Competition> GetCompetitions();
        IEnumerable<CompetitionClass> GetCompetitionClasses(
            string competitionName);
        IEnumerable<Participant> GetParticipants(
            string competitionName);
    }
}
