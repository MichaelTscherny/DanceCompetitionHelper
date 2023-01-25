using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Info;

namespace DanceCompetitionHelper.Database.DisplayInfo
{
    public class CompetitionClassDisplayInfo
    {
        public List<Participant> Participants { get; } = new List<Participant>();
        public ExtraParticipants ExtraParticipants { get; } = new ExtraParticipants();

        public int CountParticipants => Participants.Count();
        public int CountExtraParticipants => ExtraParticipants.AllExtraParticipants;
        public int NeededRounds => (int)Math.Ceiling(
            (decimal)(CountParticipants + ExtraParticipants.AllExtraParticipants)
            / 7m /* (decimal)CompetitionClassSettings.MaxParticipantsAtOnce*/);

        public int CountMultipleStarters { get; set; }
        public string? CountMultipleStartersInfo { get; set; }
    }
}
