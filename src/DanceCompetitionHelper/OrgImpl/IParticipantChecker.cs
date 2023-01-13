using DanceCompetitionHelper.Database.DisplayInfo;
using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.OrgImpl
{
    public interface IParticipantChecker
    {
        void SetCompetitionClasses(
            IEnumerable<CompetitionClass> competitionClasses);
        void SetMultipleStarter(
            IEnumerable<MultipleStarter> multipleStarters);

        (bool PromotionA, bool? PromotionB) CheckParticipantPromotion(
            Participant participant);
    }
}
