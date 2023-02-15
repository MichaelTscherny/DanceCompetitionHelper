using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanceCompetitionHelper.Database.Tables
{
    [Comment("History of an " + nameof(Tables.AdjudicatorPanel) + "of a " + nameof(Competition))]
    [Index(nameof(AdjudicatorPanelHistoryId), nameof(CompetitionId), nameof(Version), IsUnique = true)]
    [Index(nameof(Name), nameof(CompetitionId), nameof(Version), IsUnique = true)]
    [PrimaryKey(nameof(AdjudicatorPanelHistoryId), nameof(Version))]
    public class AdjudicatorPanelHistory : TableBase
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid AdjudicatorPanelHistoryId { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Ref to " + nameof(Tables.Competition))]
        public Guid CompetitionId { get; set; }

        [ForeignKey(nameof(CompetitionId))]
        public Competition Competition { get; set; } = default!;

        [Required]
        [Range(0, int.MaxValue)]
        public int Version { get; set; }

        [Required]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsLarge)]
        public string Name { get; set; } = default!;

        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        public string? Comment { get; set; }
    }
}
