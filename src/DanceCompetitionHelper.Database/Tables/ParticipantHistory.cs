using DanceCompetitionHelper.Database.DisplayInfo;

using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanceCompetitionHelper.Database.Tables
{
    [Comment("History of Participants of a " + nameof(Competition))]
    [Index(nameof(CompetitionId), nameof(ParticipantHistoryId), nameof(Version), IsUnique = true)]
    [PrimaryKey(nameof(ParticipantHistoryId), nameof(Version))]
    public class ParticipantHistory : TableBase
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ParticipantHistoryId { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Ref to " + nameof(Competition))]
        public Guid CompetitionId { get; set; }

        [ForeignKey(nameof(CompetitionId))]
        public Competition Competition { get; set; } = default!;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Ref to " + nameof(CompetitionClassHistory))]
        public Guid CompetitionClassHistoryId { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int CompetitionClassHistoryVersion { get; set; }

        [ForeignKey(nameof(CompetitionClassHistoryId) + "," + nameof(CompetitionClassHistoryVersion))]
        public CompetitionClassHistory CompetitionClassHistory { get; set; } = default!;

        [Required]
        [Range(0, int.MaxValue)]
        public int Version { get; set; }

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

        [Range(0, double.MaxValue)]
        public double OrgPointsPartA { get; set; }
        [Range(0, int.MaxValue)]
        public int OrgStartsPartA { get; set; }
        [Range(0, int.MaxValue)]
        public int? MinStartsForPromotionPartA { get; set; }
        public bool? OrgAlreadyPromotedPartA { get; set; }
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        public string? OrgAlreadyPromotedInfoPartA { get; set; }

        [Range(0, double.MaxValue)]
        public double? OrgPointsPartB { get; set; }
        [Range(0, int.MaxValue)]
        public int? OrgStartsPartB { get; set; }
        [Range(0, int.MaxValue)]
        public int? MinStartsForPromotionPartB { get; set; }
        public bool? OrgAlreadyPromotedPartB { get; set; }
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        public string? OrgAlreadyPromotedInfoPartB { get; set; }

        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        public string? Comment { get; set; }

        public bool Ignore { get; set; }

        [NotMapped]
        public ParticipantDisplayInfo? DisplayInfo { get; set; }
    }
}
