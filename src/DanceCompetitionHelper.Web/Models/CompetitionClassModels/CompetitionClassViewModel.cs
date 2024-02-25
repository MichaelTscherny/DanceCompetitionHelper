using DanceCompetitionHelper.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DanceCompetitionHelper.Web.Models.CompetitionClassModels
{
    public class CompetitionClassViewModel : ViewModelBase
    {
        [Required]
        [FromForm]
        [HiddenInput]
        public Guid CompetitionId { get; set; }

        [FromForm]
        [HiddenInput]
        public Guid? CompetitionClassId { get; set; }

        [Required]
        [FromForm]
        [MinLength(1)]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthCompetitionClassString)]
        [StringLength(DanceCompetitionHelperConstants.MaxLengthCompetitionClassString)]
        public string CompetitionClassName { get; set; } = default!;

        [FromForm]
        public Guid? FollowUpCompetitionClassId { get; set; }

        public List<SelectListItem> FollowUpCompetitionClasses { get; set; } = new List<SelectListItem>();

        [FromForm]
        [Required]
        public Guid AdjudicatorPanelId { get; set; }

        public List<SelectListItem> AdjudicatorPanels { get; set; } = new List<SelectListItem>();

        [Required]
        [FromForm]
        [MinLength(1)]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthOrgId)]
        [StringLength(DanceCompetitionHelperConstants.MaxLengthOrgId)]
        public string OrgClassId { get; set; } = default!;

        [FromForm]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthCompetitionClassString)]
        [StringLength(DanceCompetitionHelperConstants.MaxLengthCompetitionClassString)]
        public string? Discipline { get; set; } = default!;

        [Required]
        [FromForm]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthCompetitionClassString)]
        [StringLength(DanceCompetitionHelperConstants.MaxLengthCompetitionClassString)]
        public string? AgeClass { get; set; } = default!;

        [FromForm]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthCompetitionClassString)]
        [StringLength(DanceCompetitionHelperConstants.MaxLengthCompetitionClassString)]
        public string? AgeGroup { get; set; } = default!;

        [FromForm]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthCompetitionClassString)]
        [StringLength(DanceCompetitionHelperConstants.MaxLengthCompetitionClassString)]
        public string? Class { get; set; } = default!;

        [FromForm]
        [Range(0, int.MaxValue)]
        public int MinStartsForPromotion { get; set; }

        [FromForm]
        [Range(0, double.MaxValue)]
        public double MinPointsForPromotion { get; set; }

        [FromForm]
        [Range(0, double.MaxValue)]
        public double PointsForFirst { get; set; }

        [FromForm]
        [Range(0, int.MaxValue)]
        public int ExtraManualStarter { get; set; }

        [FromForm]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        [StringLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        public string? Comment { get; set; }

        [MaxLength(DanceCompetitionHelperConstants.MaxLengthCreatedBy)]
        [StringLength(DanceCompetitionHelperConstants.MaxLengthCreatedBy)]
        public string? CompetitionColor { get; set; }

        [FromForm]
        public bool Ignore { get; set; }
    }
}
