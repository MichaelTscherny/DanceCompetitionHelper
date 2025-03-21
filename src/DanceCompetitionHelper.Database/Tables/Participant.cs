﻿using DanceCompetitionHelper.Database.DisplayInfo;
using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanceCompetitionHelper.Database.Tables
{
    [Comment("The Participants of a " + nameof(Competition))]
    [Index(nameof(CompetitionId), nameof(ParticipantId), IsUnique = true)]
    [PrimaryKey(nameof(ParticipantId))]
    public class Participant : TableBase, IDefaultTrim
    {
        [Required]
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

        public void DefaultTrim()
        {
            NamePartA = NamePartA.DefaultTrim();
            OrgIdPartA = OrgIdPartA.DefaultNullableTrim();
            NamePartB = NamePartB.DefaultNullableTrim();
            OrgIdPartB = OrgIdPartB.DefaultNullableTrim();

            ClubName = ClubName.DefaultNullableTrim();
            OrgIdClub = OrgIdClub.DefaultNullableTrim();
        }

        public override string ToString()
        {
            return string.Format(
                "{0} {1}/{2} ({3}/{4}) - {5} ({6}) - P {7}/{8} - S {9}/{10} - Prom {11}/{12} ({13}/{14})",
                StartNumber,
                // 1
                NamePartA,
                NamePartB,
                OrgIdPartA,
                OrgIdPartB,
                // 5
                ClubName,
                OrgIdClub,
                // 6
                OrgPointsPartA,
                OrgPointsPartB,
                // 9
                OrgStartsPartA,
                OrgStartsPartB,
                // 11
                OrgAlreadyPromotedPartA ?? false,
                OrgAlreadyPromotedPartB ?? false,
                OrgAlreadyPromotedInfoPartA ?? "-",
                OrgAlreadyPromotedInfoPartB ?? "-");
        }
    }
}
