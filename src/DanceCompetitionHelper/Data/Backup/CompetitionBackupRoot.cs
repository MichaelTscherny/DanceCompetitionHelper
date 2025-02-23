namespace DanceCompetitionHelper.Data.Backup
{
    public class CompetitionBackupRoot
    {
        public CompetitionBackup Competition { get; set; } = default!;
        public IEnumerable<AdjudicatorBackup> Adjudicators { get; set; } = default!;
        public IEnumerable<AdjudicatorPanelBackup> AdjudicatorPanels { get; set; } = default!;
        public IEnumerable<CompetitionClassBackup> CompetitionClasses { get; set; } = default!;
        public IEnumerable<CompetitionVenueBackup> CompetitionVenues { get; set; } = default!;
        public IEnumerable<ConfigurationValueBackup> ConfigurationValues { get; set; } = default!;
        public IEnumerable<ParticipantBackup> Participants { get; set; } = default!;

        // History Stuff

        public IEnumerable<AdjudicatorHistoryBackup> AdjudicatorsHistory { get; set; } = default!;
        public IEnumerable<AdjudicatorPanelHistoryBackup> AdjudicatorPanelHistory { get; set; } = default!;
        public IEnumerable<CompetitionClassHistoryBackup> CompetitionClassesHistory { get; set; } = default!;
        public IEnumerable<ParticipantHistoryBackup> ParticipantHistory { get; set; } = default!;
    }
}
