using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanceCompetitionHelper.Database.Tables
{
    [Comment("Histroy of an " + nameof(Tables.Adjudicator) + "of a " + nameof(CompetitionClass))]
    [Index(nameof(AdjudicatorHistoryId), nameof(AdjudicatorPanelHistoryId), nameof(AdjudicatorPanelHistoryVersion), IsUnique = true)]
    [Index(nameof(Name), nameof(AdjudicatorPanelHistoryId), nameof(AdjudicatorPanelHistoryVersion), IsUnique = true)]
    [PrimaryKey(nameof(AdjudicatorHistoryId), nameof(Version))]
    public class AdjudicatorHistory : TableBase
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid AdjudicatorHistoryId { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Ref to " + nameof(Tables.AdjudicatorPanelHistory))]
        public Guid AdjudicatorPanelHistoryId { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int AdjudicatorPanelHistoryVersion { get; set; }

        [ForeignKey(nameof(AdjudicatorPanelHistoryId) + "," + nameof(AdjudicatorPanelHistoryVersion))]
        public AdjudicatorPanelHistory AdjudicatorPanelHistory { get; set; } = default!;

        [Required]
        [Range(0, int.MaxValue)]
        public int Version { get; set; }

        [Required]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthOrgId)]
        public string Abbreviation { get; set; } = default!;

        [Required]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsLarge)]
        public string Name { get; set; } = default!;

        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsLarge)]
        public string? Comment { get; set; }
    }
}
