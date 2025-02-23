using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Data.Backup
{
    public class AdjudicatorHistoryBackup : TableBase
    {
        public Guid AdjudicatorHistoryId { get; set; }
        public Guid AdjudicatorPanelHistoryId { get; set; }
        public int AdjudicatorPanelHistoryVersion { get; set; }
        public int Version { get; set; }
        public string Abbreviation { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Comment { get; set; }
    }
}
