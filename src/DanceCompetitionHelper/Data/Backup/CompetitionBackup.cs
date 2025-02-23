using DanceCompetitionHelper.Database;
using DanceCompetitionHelper.Database.Enum;
using DanceCompetitionHelper.Database.Tables;

using System.ComponentModel.DataAnnotations;

namespace DanceCompetitionHelper.Data.Backup
{
    public class CompetitionBackup : TableBase
    {
        public Guid CompetitionId { get; set; }

        public OrganizationEnum Organization { get; set; }

        public string OrgCompetitionId { get; set; } = default!;

        public string CompetitionName { get; set; } = default!;

        public string? CompetitionInfo { get; set; }

        public DateTime CompetitionDate { get; set; }

        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        public string? Comment { get; set; }
    }
}
