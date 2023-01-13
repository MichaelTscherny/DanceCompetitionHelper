using DanceCompetitionHelper.Database;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DanceCompetitionHelper.Web.Models
{
    public class CompetitionClassViewModel
    {
        public string? Errors { get; set; }

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
        [Range(0, int.MaxValue)]
        public int MinPointsForPromotion { get; set; }

        [FromForm]
        [Range(0, int.MaxValue)]
        public int PointsForFirst { get; set; }

        [FromForm]
        [Range(0, int.MaxValue)]
        public int PointsForLast { get; set; }

        [FromForm]
        public bool Ignore { get; set; }
    }
}
