using DanceCompetitionHelper.Database;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DanceCompetitionHelper.Web.Models.AdjudicatorPanelModels
{
    public class AdjudicatorPanelViewModel : ViewModelBase
    {
        [Required]
        [FromForm]
        [HiddenInput]
        public Guid CompetitionId { get; set; }

        public string? CompetitionName { get; set; }

        [FromForm]
        [HiddenInput]
        public Guid? AdjudicatorPanelId { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsLarge)]
        [StringLength(DanceCompetitionHelperConstants.MaxLengthStringsLarge)]
        public string Name { get; set; } = default!;

        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsLarge)]
        [StringLength(DanceCompetitionHelperConstants.MaxLengthStringsLarge)]
        public string? Comment { get; set; }
    }
}
