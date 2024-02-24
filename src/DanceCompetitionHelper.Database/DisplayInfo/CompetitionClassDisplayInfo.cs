using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Info;

namespace DanceCompetitionHelper.Database.DisplayInfo
{
    public class CompetitionClassDisplayInfo
    {
        public List<Participant> Participants { get; } = new List<Participant>();
        public ExtraParticipants ExtraParticipants { get; } = new ExtraParticipants();

        public int MaxCouplesPerHeat { get; set; } = 1;

        public int CountParticipants => Participants.Count();
        public int CountExtraParticipants => ExtraParticipants.AllExtraParticipants;
        public int NeededRounds => (int)Math.Ceiling(
            (CountParticipants + CountExtraParticipants)
            / (decimal)MaxCouplesPerHeat);

        public int CountMultipleStarters { get; set; }
        public string? CountMultipleStartersInfo { get; set; }
    }
}
