using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanceCompetitionHelper.Database.Tables
{
    [Comment("History of an " + nameof(Tables.AdjudicatorPanel) + "of a " + nameof(CompetitionClass))]
    [Index(nameof(AdjudicatorPanelId), nameof(CompetitionId), IsUnique = true)]
    [Index(nameof(Name), nameof(CompetitionId), IsUnique = true)]
    [Keyless]
    public class AdjudicatorPanelHistory : TableBase
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AdjudicatorPanelId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Ref to " + nameof(Tables.Competition))]
        public Guid? CompetitionId { get; set; }

        [ForeignKey(nameof(CompetitionId))]
        public Competition? Competition { get; set; } = default!;

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
