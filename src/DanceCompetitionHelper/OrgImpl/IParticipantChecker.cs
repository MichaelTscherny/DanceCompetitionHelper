using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.OrgImpl
{
    public interface IParticipantChecker
    {
        void CheckParticipantPromotion(
            Participant participant);
    }
}
