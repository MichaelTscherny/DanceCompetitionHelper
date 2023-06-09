namespace DanceCompetitionHelper.Data
{
    public class ImportOrUpdateCompetitionStatus
    {
        public string? OrgCompetitionId { get; set; }
        public Guid? CompetitionId { get; set; }

        public List<string> WorkInfo { get; } = new List<string>();
        public List<string> Errors { get; } = new List<string>();
    }
}
