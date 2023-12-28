using DanceCompetitionHelper.Database.DisplayInfo;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanceCompetitionHelper.Database.Tables
{
    [Comment("The classes of a " + nameof(Competition))]
    [Index(nameof(CompetitionId), nameof(OrgClassId), IsUnique = true)]
    [Index(nameof(CompetitionId), nameof(CompetitionClassName), IsUnique = true)]
    [Index(nameof(AdjudicatorPanelId), IsUnique = false)]
    [PrimaryKey(nameof(CompetitionClassId))]
    public class CompetitionClass : TableBase
    {
        [Required]
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

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Ref to follow up " + nameof(Tables.CompetitionClass))]
        public Guid? FollowUpCompetitionClassId { get; set; }

        [ForeignKey(nameof(FollowUpCompetitionClassId))]
        public CompetitionClass? FollowUpCompetitionClass { get; set; }

        [NotMapped]
        public CompetitionClass? PreviousCompetitionClass { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Ref to " + nameof(AdjudicatorPanel))]
        public Guid AdjudicatorPanelId { get; set; }

        [ForeignKey(nameof(AdjudicatorPanelId))]
        public AdjudicatorPanel AdjudicatorPanel { get; set; } = default!;

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
        [Range(0, double.MaxValue)]
        public double MinPointsForPromotion { get; set; }

        [Range(0, double.MaxValue)]
        public double PointsForFirst { get; set; }

        [Range(0, int.MaxValue)]
        public int ExtraManualStarter { get; set; }

        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        public string? Comment { get; set; }

        [MaxLength(DanceCompetitionHelperConstants.MaxLengthCreatedBy)]
        public string? CompetitionColor { get; set; }

        public bool Ignore { get; set; }

        [NotMapped]
        public CompetitionClassDisplayInfo? DisplayInfo { get; set; }

        public override string ToString()
        {
            return string.Format(
                "{0} ({1}/{2}/{3}/{4}/{5} - {6}/{7}/{8}/+{9}/'{10}')",
                CompetitionClassName,
                OrgClassId,
                AgeClass,
                AgeGroup,
                Discipline,
                Class,
                MinPointsForPromotion,
                MinStartsForPromotion,
                PointsForFirst,
                ExtraManualStarter,
                Comment);
        }
    }
}
