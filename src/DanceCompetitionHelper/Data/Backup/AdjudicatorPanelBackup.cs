using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Data.Backup
{
    public class AdjudicatorPanelBackup : TableBase
    {
        public Guid AdjudicatorPanelId { get; set; }
        public Guid CompetitionId { get; set; }
        public string Name { get; set; } = default!;
        public string? Comment { get; set; }
    }
}
