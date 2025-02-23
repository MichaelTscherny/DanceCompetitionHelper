using DanceCompetitionHelper.Database.Enum;
using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Data.Backup
{
    public class ConfigurationValueBackup : TableBase
    {
        public Guid ConfigurationValueId { get; set; }
        public OrganizationEnum? Organization { get; set; }
        public Guid? CompetitionId { get; set; }
        public Guid? CompetitionClassId { get; set; }
        public CompetitionClass? CompetitionClass { get; set; }
        public Guid? CompetitionVenueId { get; set; }
        public string Key { get; set; } = default!;
        public string? Value { get; set; }
        public string? Comment { get; set; }
    }
}
