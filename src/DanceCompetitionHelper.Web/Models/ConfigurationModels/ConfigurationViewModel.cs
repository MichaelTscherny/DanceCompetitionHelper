using DanceCompetitionHelper.Database;
using DanceCompetitionHelper.Database.Enum;
using DanceCompetitionHelper.Database.Tables;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DanceCompetitionHelper.Web.Models.ConfigurationModels
{
    public class ConfigurationViewModel
    {
        [FromForm]
        [HiddenInput]
        public Guid? OriginCompetitionId { get; set; }

        [FromForm]
        public OrganizationEnum? Organization { get; set; }

        [FromForm]
        public Guid? CompetitionId { get; set; }

        [FromForm]
        public Guid? CompetitionClassId { get; set; }

        [FromForm]
        public Guid? CompetitionVenueId { get; set; }

        [FromForm]
        [Required]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsLarge)]
        public string Key { get; set; } = default!;

        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        public string? Value { get; set; }

        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        public string? Comment { get; set; }

        public void SanityCheck()
        {
            new ConfigurationValue()
            {
                Organization = Organization,
                CompetitionId = CompetitionId,
                CompetitionClassId = CompetitionClassId,
                CompetitionVenueId = CompetitionVenueId,
                Key = Key
            }.SanityCheck();
        }
    }
}
