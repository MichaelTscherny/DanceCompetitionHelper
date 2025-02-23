using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Data.Backup
{
    public class AdjudicatorBackup : TableBase
    {
        public Guid AdjudicatorId { get; set; }
        public Guid AdjudicatorPanelId { get; set; }
        public string Abbreviation { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Comment { get; set; }
    }
}
