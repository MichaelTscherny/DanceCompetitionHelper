using DanceCompetitionHelper.Config;
using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Info
{
    public class CompetitionClassInfo
    {
        private readonly DanceCompetitionHelper _danceCompetitionHelper;

        public Competition Competition { get; }
        public CompetitionClass CompetitionClass { get; }
        public CompetitionClassSettings CompetitionClassSettings { get; }
        public List<Participant> Participants { get; } = new List<Participant>();
        public ExtraParticipants ExtraParticipants { get; }

        public int CountParticipants => Participants.Count();
        public int CountExtraParticipants => ExtraParticipants.AllExtraParticipants;
        public int NeededRounds => (int)Math.Ceiling(
            (decimal)(CountParticipants + ExtraParticipants.AllExtraParticipants)
            / (decimal)CompetitionClassSettings.MaxParticipantsAtOnce);

        public CompetitionClassInfo(
            DanceCompetitionHelper danceCompetitionHelper,
            Competition competition,
            CompetitionClass competitionClass,
            ExtraParticipants? extraParticipants,
            CompetitionClassSettings competitionClassSettings)
        {
            _danceCompetitionHelper = danceCompetitionHelper
                ?? throw new ArgumentNullException(
                    nameof(danceCompetitionHelper));
            Competition = competition
                ?? throw new ArgumentNullException(
                    nameof(competition));
            CompetitionClass = competitionClass
                ?? throw new ArgumentNullException(
                    nameof(competitionClass));

            ExtraParticipants = extraParticipants
                ?? new ExtraParticipants();

            CompetitionClassSettings = competitionClassSettings
                ?? throw new ArgumentNullException(
                    nameof(competitionClassSettings));

            Reload();
        }

        public void Reload()
        {
            Participants.Clear();
            Participants.AddRange(
                _danceCompetitionHelper.GetParticipants(
                    Competition.CompetitionId));
        }
    }
}
