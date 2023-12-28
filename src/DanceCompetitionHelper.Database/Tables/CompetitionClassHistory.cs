using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanceCompetitionHelper.Database.Tables
{
    [Comment("History of Classes of a " + nameof(Competition))]
    [Index(nameof(CompetitionId), nameof(OrgClassId), nameof(Version), IsUnique = true)]
    [Index(nameof(CompetitionId), nameof(CompetitionClassName), nameof(Version), IsUnique = true)]
    [Index(nameof(AdjudicatorPanelHistoryId), nameof(AdjudicatorPanelHistoryVersion), IsUnique = false)]
    [PrimaryKey(nameof(CompetitionClassHistoryId), nameof(Version))]
    public class CompetitionClassHistory : TableBase
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid CompetitionClassHistoryId { get; set; }

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
        [Comment("Ref to follow up " + nameof(Tables.CompetitionClassHistory))]
        public Guid? FollowUpCompetitionClassHistoryId { get; set; }

        [ForeignKey(nameof(FollowUpCompetitionClassHistoryId) + "," + nameof(Version))]
        public CompetitionClassHistory? FollowUpCompetitionClassHistory { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Ref to " + nameof(AdjudicatorPanelHistory))]
        public Guid AdjudicatorPanelHistoryId { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int AdjudicatorPanelHistoryVersion { get; set; }

        [ForeignKey(nameof(AdjudicatorPanelHistoryId) + "," + nameof(AdjudicatorPanelHistoryVersion))]
        public AdjudicatorPanelHistory AdjudicatorPanelHistory { get; set; } = default!;

        [Required]
        [Range(0, int.MaxValue)]
        public int Version { get; set; }

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
    }
}
