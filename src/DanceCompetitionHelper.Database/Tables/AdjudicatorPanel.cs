using DanceCompetitionHelper.Database.DisplayInfo;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanceCompetitionHelper.Database.Tables
{
    [Comment("An " + nameof(Tables.AdjudicatorPanel) + "of a " + nameof(CompetitionClass))]
    [Index(nameof(AdjudicatorPanelId), nameof(CompetitionId), IsUnique = true)]
    [Index(nameof(Name), nameof(CompetitionId), IsUnique = true)]
    public class AdjudicatorPanel : TableBase
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AdjudicatorPanelId { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Ref to " + nameof(Tables.Competition))]
        public Guid CompetitionId { get; set; }

        [ForeignKey(nameof(CompetitionId))]
        public Competition Competition { get; set; } = default!;

        [Required]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsLarge)]
        public string Name { get; set; } = default!;

        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        public string? Comment { get; set; }

        [NotMapped]
        public AdjudicatorPanelDisplayInfos? DisplayInfo { get; set; }
    }
}
