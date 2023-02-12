using DanceCompetitionHelper.Database.DisplayInfo;
using DanceCompetitionHelper.Database.Enum;
using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper
{
    public interface IDanceCompetitionHelper : IDisposable
    {
        void AddTestData();
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
        IEnumerable<AdjudicatorPanel> GetAdjudicatorPanels(
            Guid? competitionId,
            bool includeInfos = false);
        IEnumerable<Adjudicator> GetAdjudicators(
            Guid? competitionId,
            Guid? adjudicatorPanelId,
            bool includeInfos = false);

        Competition? GetCompetition(
            Guid? competitionId);

        Guid? FindCompetition(
            Guid? byAnyId);
        Guid? FindCompetitionClass(
            Guid? byAnyId);
        Guid? GetCompetition(
            string byName);
        Guid? GetCompetitionClass(
            string byName);
        CompetitionClass? GetCompetitionClass(
            Guid competitionId);

        Participant? GetParticipant(
            Guid participantId);

        AdjudicatorPanel? GetAdjudicatorPanel(
            Guid adjudicatorPanelId);

        Adjudicator? GetAdjudicator(
            Guid adjudicatorId);

        IEnumerable<MultipleStarter> GetMultipleStarter(
            Guid competitionId);

        #region Competition Crud

        void CreateCompetition(
            string competitionName,
            OrganizationEnum organization,
            string orgCompetitionId,
            string? competitionInfo,
            DateTime competitionDate,
            string? comment);

        void EditCompetition(
            Guid competitionId,
            string competitionName,
            OrganizationEnum organization,
            string orgCompetitionId,
            string? competitionInfo,
            DateTime competitionDate,
            string? comment);

        void RemoveCompetition(
            Guid competitionId);

        #endregion //  Competition Crud

        #region AdjudicatorPanel Crud

        void CreateAdjudicatorPanel(
            Guid competitionId,
            string name,
            string? comment);

        void EditAdjudicatorPanel(
            Guid adjudicatorPanelId,
            Guid competitionId,
            string name,
            string? comment);

        void RemoveAdjudicatorPanel(
            Guid adjudicatorPanelId);

        #endregion //  AdjudicatorPanel Crud

        #region Adjudicator Crud

        void CreateAdjudicator(
            Guid adjudicatorPanelId,
            string abbreviation,
            string name,
            string? comment);

        void EditAdjudicator(
            Guid adjudicatorId,
            Guid adjudicatorPanelId,
            string abbreviation,
            string name,
            string? comment);

        void RemoveAdjudicator(
            Guid adjudicatorId);

        #endregion //  Adjudicator Crud

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
            int pointsForFirts,
            int extraManualStarter,
            string? comment,
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
            int pointsForFirst,
            int extraManualStarter,
            string? comment,
            bool ignore);

        void RemoveCompetitionClass(
            Guid competitionClassId);

        #endregion //  CompetitionClass Crud 

        #region Participant Crud

        void CreateParticipant(
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
            bool ignore);

        void EditParticipant(
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
            bool ignore);

        void RemoveParticipant(
            Guid participantId);

        #endregion //  CompetitionClass Crud

    }
}
