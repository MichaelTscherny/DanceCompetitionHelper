using DanceCompetitionHelper.Database;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DanceCompetitionHelper.Web.Models.CompetitionVenueModels
{
    public class CompetitionVenueViewModel : ViewModelBase
    {
        [Required]
        [FromForm]
        [HiddenInput]
        public Guid CompetitionId { get; set; }

        public string? CompetitionName { get; set; }

        [FromForm]
        [HiddenInput]
        public Guid? CompetitionVenueId { get; set; }

        [Required]
        [FromForm]
        [MinLength(1)]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        [StringLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        public string Name { get; set; } = default!;

        [Required]
        [FromForm]
        [Range(0, int.MaxValue)]
        public int LengthInMeter { get; set; } = default!;

        [Required]
        [FromForm]
        [Range(0, int.MaxValue)]
        public int WidthInMeter { get; set; } = default!;

        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsLarge)]
        [StringLength(DanceCompetitionHelperConstants.MaxLengthStringsLarge)]
        public string? Comment { get; set; }
    }
}
