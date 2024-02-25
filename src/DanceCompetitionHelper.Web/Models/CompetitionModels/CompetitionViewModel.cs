using DanceCompetitionHelper.Database;
using DanceCompetitionHelper.Database.Enum;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DanceCompetitionHelper.Web.Models.CompetitionModels
{
    public class CompetitionViewModel : ViewModelBase
    {
        [FromForm]
        [HiddenInput]
        public Guid? CompetitionId { get; set; }

        [Required]
        [FromForm]
        [MinLength(2)]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsLarge)]
        [StringLength(DanceCompetitionHelperConstants.MaxLengthStringsLarge)]
        public string CompetitionName { get; set; } = default!;

        [Required]
        [FromForm]
        public OrganizationEnum Organization { get; set; }

        [Required]
        [FromForm]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthOrgId)]
        [StringLength(DanceCompetitionHelperConstants.MaxLengthOrgId)]
        public string OrgCompetitionId { get; set; } = default!;

        [FromForm]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsLarge)]
        [StringLength(DanceCompetitionHelperConstants.MaxLengthStringsLarge)]
        public string? CompetitionInfo { get; set; }

        [Required]
        [FromForm]
        [DataType(DataType.Date)]
        public DateTime? CompetitionDate { get; set; }

        [FromForm]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        public string? Comment { get; set; }
    }
}
