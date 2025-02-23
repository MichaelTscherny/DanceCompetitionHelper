using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Data.Backup
{
    public class AdjudicatorPanelHistoryBackup : TableBase
    {
        public Guid AdjudicatorPanelHistoryId { get; set; }
        public Guid CompetitionId { get; set; }
        public int Version { get; set; }
        public string Name { get; set; } = default!;
        public string? Comment { get; set; }
    }
}
