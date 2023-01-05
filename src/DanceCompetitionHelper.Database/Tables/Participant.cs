using DanceCompetitionHelper.Database.DisplayInfo;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanceCompetitionHelper.Database.Tables
{
    [Comment("The Participants of a " + nameof(Competition))]
    public class Participant : TableBase
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ParticipantId { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Ref to " + nameof(Competition))]
        public Guid CompetitionId { get; set; }

        [ForeignKey(nameof(CompetitionId))]
        public Competition Competition { get; set; } = default!;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Ref to " + nameof(CompetitionClass))]
        public Guid CompetitionClassId { get; set; }

        [ForeignKey(nameof(CompetitionClassId))]
        public CompetitionClass CompetitionClass { get; set; } = default!;

        [Required]
        [Range(0, int.MaxValue)]
        public int StartNumber { get; set; }

        [Required]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        public string NamePartA { get; set; } = default!;

        [Required]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthOrgId)]
        [Comment("'Internal' Org-Id of PartA")]
        public string? OrgIdPartA { get; set; }

        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        public string? NamePartB { get; set; }

        [MaxLength(DanceCompetitionHelperConstants.MaxLengthOrgId)]
        public string? OrgIdPartB { get; set; }

        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        public string? ClubName { get; set; }

        [MaxLength(DanceCompetitionHelperConstants.MaxLengthOrgId)]
        public string? OrgIdClub { get; set; }

        [Range(0, int.MaxValue)]
        public int OrgPointsPartA { get; set; }
        [Range(0, int.MaxValue)]
        public int OrgStartsPartA { get; set; }

        [Range(0, int.MaxValue)]
        public int? OrgPointsPartB { get; set; }
        [Range(0, int.MaxValue)]
        public int? OrgStartsPartB { get; set; }

        public bool Ignore { get; set; }

        [NotMapped]
        public ParticipantDisplayInfo? DisplayInfo { get; set; }
    }
}
