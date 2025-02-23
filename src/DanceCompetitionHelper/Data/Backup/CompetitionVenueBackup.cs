using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Data.Backup
{
    public class CompetitionVenueBackup : TableBase
    {
        public Guid CompetitionVenueId { get; set; }
        public Guid CompetitionId { get; set; }
        public string Name { get; set; } = default!;
        public int LengthInMeter { get; set; } = default!;
        public int WidthInMeter { get; set; } = default!;
        public string? Comment { get; set; }
    }
}
