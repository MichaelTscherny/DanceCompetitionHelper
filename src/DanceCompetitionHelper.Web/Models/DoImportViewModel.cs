using DanceCompetitionHelper.Database.Enum;
using DanceCompetitionHelper.Web.Enum;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DanceCompetitionHelper.Web.Models
{
    public class DoImportViewModel
    {
        public string? Errors { get; set; }

        [Required]
        [FromForm]
        [HiddenInput]
        public Guid? CompetitionId { get; set; }

        [Required]
        [FromForm]
        [HiddenInput]
        public ImportTypeEnum ImportType { get; set; }

        [Required]
        [FromForm]
        [HiddenInput]
        public OrganizationEnum Organization { get; set; }

        [Required]
        [FromForm]
        public string? OrgCompetitionId { get; set; }
    }
}
