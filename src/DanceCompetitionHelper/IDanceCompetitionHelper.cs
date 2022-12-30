using DanceCompetitionHelper.Database.Enum;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Info;

namespace DanceCompetitionHelper
{
    public interface IDanceCompetitionHelper : IDisposable
    {
        void Migrate();
        IEnumerable<Competition> GetCompetitions(
            bool includeInfos = false);
        IEnumerable<CompetitionClass> GetCompetitionClasses(
            Guid? competitionId);
        IEnumerable<Participant> GetParticipants(
            Guid? competitionId);
        IEnumerable<Participant> GetParticipants(
            Guid? competitionId,
            Guid? competitionClassId);

        Competition? GetCompetition(
            Guid competitionId);
        Guid? GetCompetition(
            string byName);
        Guid? GetCompetitionClass(
            string byName);

        IEnumerable<CompetitionClassInfo> GetCompetitionClassInfos(
            Guid competitionId,
            IEnumerable<Guid>? competitionClassIds);

        IEnumerable<(List<Participant> Participant, List<CompetitionClass> CompetitionClasses)> GetMultipleStarter(
            Guid competitionId);

        #region Competition Crud

        void CreateCompetition(
            string competitionName,
            OrganizationEnum organization,
            string orgCompetitionId,
            string? competitionInfo,
            DateTime competitionDate);

        void EditCompetition(
            Guid competitionId,
            string competitionName,
            OrganizationEnum organization,
            string orgCompetitionId,
            string? competitionInfo,
            DateTime competitionDate);

        void RemoveCompetition(
            Guid competitionId);

        #endregion //  Competition Crud

    }
}
