using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Data.Backup
{
    public class CompetitionClassHistoryBackup : TableBase
    {
        public Guid CompetitionClassHistoryId { get; set; }
        public string OrgClassId { get; set; } = default!;
        public Guid CompetitionId { get; set; }
        public Guid? FollowUpCompetitionClassHistoryId { get; set; }
        public Guid AdjudicatorPanelHistoryId { get; set; }
        public int AdjudicatorPanelHistoryVersion { get; set; }
        public int Version { get; set; }
        public string CompetitionClassName { get; set; } = default!;
        public string? Discipline { get; set; }
        public string? AgeClass { get; set; }
        public string? AgeGroup { get; set; }
        public string? Class { get; set; }
        public int MinStartsForPromotion { get; set; }
        public double MinPointsForPromotion { get; set; }
        public double PointsForFirst { get; set; }
        public int ExtraManualStarter { get; set; }
        public string? Comment { get; set; }
        public string? CompetitionColor { get; set; }
        public bool Ignore { get; set; }
    }
}
