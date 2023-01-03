using DanceCompetitionHelper.Database.Enum;
using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper
{
    public interface IDanceCompetitionHelper : IDisposable
    {
        void Migrate();
        IEnumerable<Competition> GetCompetitions(
            bool includeInfos = false);
        IEnumerable<CompetitionClass> GetCompetitionClasses(
            Guid? competitionId,
            bool includeInfos = false);
        IEnumerable<Participant> GetParticipants(
            Guid? competitionId,
            Guid? competitionClassId,
            bool includeInfos = false);

        Competition? GetCompetition(
            Guid competitionId);

        Guid? FindCompetition(
            Guid? byAnyId);
        Guid? GetCompetition(
            string byName);
        Guid? GetCompetitionClass(
            string byName);
        CompetitionClass? GetCompetitionClass(
            Guid competitionId);

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

        #region CompetitionClass Crud

        void CreateCompetitionClass(
            Guid competitionId,
            string competitionClassName,
            string orgClassId,
            string? discipline,
            string? ageClass,
            string? ageGroup,
            string? className,
            int minStartsForPromotion,
            int minPointsForPromotion,
            bool ignore);

        void EditCompetitionClass(
            Guid competitionClassId,
            string competitionClassName,
            string orgClassId,
            string? discipline,
            string? ageClass,
            string? ageGroup,
            string? className,
            int minStartsForPromotion,
            int minPointsForPromotion,
            bool ignore);

        void RemoveCompetitionClass(
            Guid competitionClassId);

        #endregion //  CompetitionClass Crud

    }
}
