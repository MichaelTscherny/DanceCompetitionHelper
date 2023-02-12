using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanceCompetitionHelper.Database.Tables
{
    [Comment("Histroy of an " + nameof(Tables.Adjudicator) + "of a " + nameof(CompetitionClass))]
    [Index(nameof(AdjudicatorlId), nameof(AdjudicatorPanelId), IsUnique = true)]
    [Index(nameof(Name), nameof(AdjudicatorPanelId), IsUnique = true)]
    [Keyless]
    public class AdjudicatorHistory : TableBase
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AdjudicatorlId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Ref to " + nameof(Tables.AdjudicatorPanel))]
        public Guid? AdjudicatorPanelId { get; set; }

        [ForeignKey(nameof(AdjudicatorPanelId))]
        public AdjudicatorPanel? AdjudicatorPanel { get; set; } = default!;

        [Required]
        [Range(0, int.MaxValue)]
        public int Version { get; set; }

        [Required]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsLarge)]
        public string Name { get; set; } = default!;

        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsLarge)]
        public string? Comment { get; set; }
    }
}
