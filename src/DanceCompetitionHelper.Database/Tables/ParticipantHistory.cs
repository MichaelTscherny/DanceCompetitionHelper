using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanceCompetitionHelper.Database.Tables
{
    [Comment("History of Participants of a " + nameof(Competition))]
    [Index(nameof(CompetitionId), nameof(ParticipantId), nameof(Version), IsUnique = true)]
    [Keyless]
    public class ParticipantHistory : TableBase
    {
        [Required]
        public Guid ParticipantId { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Ref to " + nameof(Competition))]
        public Guid CompetitionId { get; set; }

        [ForeignKey(nameof(CompetitionId))]
        public Competition Competition { get; set; } = default!;

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Ref to " + nameof(CompetitionClass))]
        public Guid? CompetitionClassId { get; set; }

        [ForeignKey(nameof(CompetitionClassId))]
        public CompetitionClass? CompetitionClass { get; set; }

        [Required]
        public int Version { get; set; }

        [Required]
        public int StartNumber { get; set; }

        [Required]
        public string NamePartA { get; set; } = default!;

        [Required]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthOrgId)]
        [Comment("'Internal' Org-Id of PartA")]
        public string? OrgIdPartA { get; set; }

        [MaxLength(DanceCompetitionHelperConstants.MaxLengthOrgId)]
        public string? NamePartB { get; set; }

        [MaxLength(DanceCompetitionHelperConstants.MaxLengthOrgId)]
        public string? OrgIdPartB { get; set; } = default!;

        [MaxLength(DanceCompetitionHelperConstants.MaxLengthOrgId)]
        public string? OrgIdClub { get; set; } = default!;

        public int OrgPointsPartA { get; set; }
        public int OrgStartsPartA { get; set; }

        public int? OrgPointsPartB { get; set; }
        public int? OrgStartsPartB { get; set; }
    }
}
