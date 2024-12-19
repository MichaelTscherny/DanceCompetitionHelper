using DanceCompetitionHelper.Database.DisplayInfo;
using DanceCompetitionHelper.Database.Enum;
using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanceCompetitionHelper.Database.Tables
{
    [Comment("A Competition 'root'")]
    [Index(nameof(Organization), nameof(OrgCompetitionId), IsUnique = true)]
    [PrimaryKey(nameof(CompetitionId))]
    public class Competition : TableBase, IDefaultTrim
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CompetitionId { get; set; }

        [Required]
        public OrganizationEnum Organization { get; set; }

        [Required]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthOrgId)]
        [Comment("'Internal' Org-Id of " + nameof(Competition))]
        public string OrgCompetitionId { get; set; } = default!;

        [Required]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsLarge)]
        public string CompetitionName { get; set; } = default!;

        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsLarge)]
        public string? CompetitionInfo { get; set; }

        [Required]
        public DateTime CompetitionDate { get; set; }

        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        public string? Comment { get; set; }

        [NotMapped]
        public CompetitionDisplayInfo? DisplayInfo { get; set; }

        public void DefaultTrim()
        {
            CompetitionName = CompetitionName.DefaultTrim();
        }

        public override string ToString()
        {
            return string.Format(
                "{0} '{1}' ({2}) '{3}' '{4}'",
                Organization,
                CompetitionName,
                OrgCompetitionId,
                CompetitionDate,
                CompetitionInfo);
        }
    }
}
