using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DanceCompetitionHelper.Web.Models
{

    public class CompetitionCreateNew
    {
        [Required]
        [FromForm]
        public string competitionName { get; set; } = default!;

        [Required]
        [FromForm]
        string OrgCompetitionId { get; set; } = default!;

        [FromForm]
        string? CompetitionInfo { get; set; }

        [Required]
        [FromForm]
        DateTime CompetitionDate { get; set; }
    }
}
