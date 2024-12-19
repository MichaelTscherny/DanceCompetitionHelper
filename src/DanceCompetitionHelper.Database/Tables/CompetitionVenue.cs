using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanceCompetitionHelper.Database.Tables
{
    [PrimaryKey(nameof(CompetitionVenueId))]
    [Index(nameof(CompetitionId), nameof(Name), IsUnique = true)]
    public class CompetitionVenue : TableBase, IDefaultTrim
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CompetitionVenueId { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Ref to " + nameof(Tables.Competition))]
        public Guid CompetitionId { get; set; }

        [ForeignKey(nameof(CompetitionId))]
        public Competition Competition { get; set; } = default!;

        [Required]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        public string Name { get; set; } = default!;

        [Required]
        [Range(0, int.MaxValue)]
        public int LengthInMeter { get; set; } = default!;

        [Required]
        [Range(0, int.MaxValue)]
        public int WidthInMeter { get; set; } = default!;

        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        public string? Comment { get; set; }

        public void DefaultTrim()
        {
            Name = Name.DefaultTrim();
        }

        public override string ToString()
        {
            return string.Format(
                "{0} ({1}x{2} - '{3}')",
                Name,
                LengthInMeter,
                WidthInMeter,
                Competition);
        }
    }
}
