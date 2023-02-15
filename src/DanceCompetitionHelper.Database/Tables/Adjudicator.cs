using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanceCompetitionHelper.Database.Tables
{
    [Comment("An " + nameof(Tables.Adjudicator) + "of a " + nameof(CompetitionClass))]
    [Index(nameof(AdjudicatorId), nameof(AdjudicatorPanelId), IsUnique = true)]
    [Index(nameof(Name), nameof(AdjudicatorPanelId), IsUnique = true)]
    [PrimaryKey(nameof(AdjudicatorId))]
    public class Adjudicator : TableBase
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AdjudicatorId { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Ref to " + nameof(Tables.AdjudicatorPanel))]
        public Guid AdjudicatorPanelId { get; set; }

        [ForeignKey(nameof(AdjudicatorPanelId))]
        public AdjudicatorPanel AdjudicatorPanel { get; set; } = default!;

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
