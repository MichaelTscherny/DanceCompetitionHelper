using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Database.DisplayInfo
{
    public class ParticipantDisplayInfo
    {
        public List<CompetitionClass> MultipleStarts { get; set; } = new List<CompetitionClass>();
        public bool PossiblePromotion { get; set; }
    }
}
