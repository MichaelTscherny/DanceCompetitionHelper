using DanceCompetitionHelper.Database.DisplayInfo;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanceCompetitionHelper.Database.Tables
{
    [Comment("The classes of a " + nameof(Competition))]
    [Index(nameof(CompetitionId), nameof(OrgClassId), IsUnique = true)]
    [Index(nameof(CompetitionId), nameof(CompetitionClassName), IsUnique = true)]
    public class CompetitionClass : TableBase
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CompetitionClassId { get; set; }

        [Required]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthOrgId)]
        [Comment("'Internal' Org-Id of class of " + nameof(CompetitionClass))]
        public string OrgClassId { get; set; } = default!;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Ref to " + nameof(Tables.Competition))]
        public Guid CompetitionId { get; set; }

        [ForeignKey(nameof(CompetitionId))]
        public Competition Competition { get; set; } = default!;

        [MaxLength(DanceCompetitionHelperConstants.MaxLengthCompetitionClassString)]
        public string CompetitionClassName { get; set; } = default!;

        [MaxLength(DanceCompetitionHelperConstants.MaxLengthCompetitionClassString)]
        public string? Discipline { get; set; }

        [MaxLength(DanceCompetitionHelperConstants.MaxLengthCompetitionClassString)]
        public string? AgeClass { get; set; }

        [MaxLength(DanceCompetitionHelperConstants.MaxLengthCompetitionClassString)]
        public string? AgeGroup { get; set; }

        [MaxLength(DanceCompetitionHelperConstants.MaxLengthCompetitionClassString)]
        public string? Class { get; set; }

        [Range(0, int.MaxValue)]
        public int MinStartsForPromotion { get; set; }
        [Range(0, int.MaxValue)]
        public int MinPointsForPromotion { get; set; }

        [Range(0, int.MaxValue)]
        public int PointsForFirst { get; set; }

        [Range(0, int.MaxValue)]
        public int ExtraManualStarter { get; set; }

        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        public string? Comment { get; set; }

        public bool Ignore { get; set; }

        [NotMapped]
        public CompetitionClassDisplayInfo? DisplayInfo { get; set; }
    }
}
