using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Info;

namespace DanceCompetitionHelper
{
    public interface IDanceCompetitionHelper : IDisposable
    {
        void Migrate();
        IEnumerable<Competition> GetCompetitions();
        IEnumerable<CompetitionClass> GetCompetitionClasses(
            Guid? competitionId);
        IEnumerable<Participant> GetParticipants(
            Guid? competitionId);
        IEnumerable<Participant> GetParticipants(
            Guid? competitionId,
            Guid? competitionClassId);

        Guid? GetCompetition(
            string byName);
        Guid? GetCompetitionClass(
            string byName);

        IEnumerable<CompetitionClassInfo> GetCompetitionClassInfos(
            Guid competitionId,
            IEnumerable<Guid>? competitionClassIds);

        IEnumerable<(List<Participant> Participant, List<CompetitionClass> CompetitionClasses)> GetMultipleStarter(
            Guid competitionId);
    }
}
