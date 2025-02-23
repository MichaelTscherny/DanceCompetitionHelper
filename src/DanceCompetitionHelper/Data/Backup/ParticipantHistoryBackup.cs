using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Data.Backup
{
    public class ParticipantHistoryBackup : TableBase
    {
        public Guid ParticipantHistoryId { get; set; }
        public Guid CompetitionId { get; set; }
        public Guid CompetitionClassHistoryId { get; set; }
        public int CompetitionClassHistoryVersion { get; set; }
        public int Version { get; set; }
        public int StartNumber { get; set; }
        public string NamePartA { get; set; } = default!;
        public string? OrgIdPartA { get; set; }
        public string? NamePartB { get; set; }
        public string? OrgIdPartB { get; set; }
        public string? ClubName { get; set; }
        public string? OrgIdClub { get; set; }
        public double OrgPointsPartA { get; set; }
        public int OrgStartsPartA { get; set; }
        public int? MinStartsForPromotionPartA { get; set; }
        public bool? OrgAlreadyPromotedPartA { get; set; }
        public string? OrgAlreadyPromotedInfoPartA { get; set; }
        public double? OrgPointsPartB { get; set; }
        public int? OrgStartsPartB { get; set; }
        public int? MinStartsForPromotionPartB { get; set; }
        public bool? OrgAlreadyPromotedPartB { get; set; }
        public string? OrgAlreadyPromotedInfoPartB { get; set; }
        public string? Comment { get; set; }
        public bool Ignore { get; set; }
    }
}
