using DanceCompetitionHelper.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DanceCompetitionHelper.Web.Models.AdjudicatorModels
{
    public class AdjudicatorViewModel
    {
        public string? Errors { get; set; }

        [Required]
        [FromForm]
        [HiddenInput]
        public Guid CompetitionId { get; set; }

        public string? CompetitionName { get; set; }

        [FromForm]
        [HiddenInput]
        public Guid? AdjudicatorId { get; set; }

        [FromForm]
        [Required]
        public Guid AdjudicatorPanelId { get; set; }

        public List<SelectListItem> AdjudicatorPanels { get; set; } = new List<SelectListItem>();

        [Required]
        [MinLength(1)]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthOrgId)]
        [StringLength(DanceCompetitionHelperConstants.MaxLengthOrgId)]
        public string Abbreviation { get; set; } = default!;

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
