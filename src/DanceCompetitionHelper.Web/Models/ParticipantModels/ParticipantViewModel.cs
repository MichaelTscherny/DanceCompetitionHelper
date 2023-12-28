using DanceCompetitionHelper.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DanceCompetitionHelper.Web.Models.ParticipantModels
{
    public class ParticipantViewModel
    {
        public string? Errors { get; set; }

        [Required]
        [FromForm]
        [HiddenInput]
        public Guid CompetitionId { get; set; }

        public string? CompetitionName { get; set; }

        [FromForm]
        [HiddenInput]
        public Guid? CompetitionClassId { get; set; }

        public List<SelectListItem> CompetitionClasses { get; set; } = new List<SelectListItem>();

        [FromForm]
        [HiddenInput]
        public Guid? ParticipantId { get; set; }

        [Required]
        [FromForm]
        [Range(0, int.MaxValue)]
        public int StartNumber { get; set; }

        [Required]
        [FromForm]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        [StringLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        public string NamePartA { get; set; } = default!;

        [FromForm]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthOrgId)]
        [StringLength(DanceCompetitionHelperConstants.MaxLengthOrgId)]
        public string? OrgIdPartA { get; set; }

        [FromForm]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        [StringLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        public string? NamePartB { get; set; }

        [FromForm]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthOrgId)]
        [StringLength(DanceCompetitionHelperConstants.MaxLengthOrgId)]
        public string? OrgIdPartB { get; set; }

        [FromForm]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        [StringLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        public string? ClubName { get; set; }

        [FromForm]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthOrgId)]
        [StringLength(DanceCompetitionHelperConstants.MaxLengthOrgId)]
        public string? OrgIdClub { get; set; }

        [FromForm]
        [Range(0, int.MaxValue)]
        public double OrgPointsPartA { get; set; }
        [FromForm]
        [Range(0, int.MaxValue)]
        public int OrgStartsPartA { get; set; }
        [FromForm]
        [Range(0, int.MaxValue)]
        public int? MinStartsForPromotionPartA { get; set; }
        [FromForm]
        public bool? OrgAlreadyPromotedPartA { get; set; }
        [FromForm]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        public string? OrgAlreadyPromotedInfoPartA { get; set; }

        [FromForm]
        [Range(0, int.MaxValue)]
        public double? OrgPointsPartB { get; set; }
        [FromForm]
        [Range(0, int.MaxValue)]
        public int? OrgStartsPartB { get; set; }
        [FromForm]
        [Range(0, int.MaxValue)]
        public int? MinStartsForPromotionPartB { get; set; }
        [FromForm]
        public bool? OrgAlreadyPromotedPartB { get; set; }
        [FromForm]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        public string? OrgAlreadyPromotedInfoPartB { get; set; }

        [FromForm]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        public string? Comment { get; set; }

        [FromForm]
        public bool Ignore { get; set; }
    }
}
