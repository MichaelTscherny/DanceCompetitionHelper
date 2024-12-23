﻿namespace DanceCompetitionHelper.Database.Test.Pocos.DanceCompetitionHelper
{
    internal class ParticipantPoco
    {
        public string CompetitionName { get; set; } = default!;
        public string CompetitionClassName { get; set; } = default!;
        public int StartNumber { get; set; }
        public string NamePartA { get; set; } = default!;
        public string? OrgIdPartA { get; set; } = default!;
        public string? NamePartB { get; set; }
        public string? OrgIdPartB { get; set; }
        public string? ClubName { get; set; }
        public string? OrgIdClub { get; set; }
        public int OrgPointsPartA { get; set; }
        public int OrgStartsPartA { get; set; }
        public int? MinStartsForPromotionPartA { get; set; }
        public bool? OrgAlreadyPromotedPartA { get; set; }
        public string? OrgAlreadyPromotedInfoPartA { get; set; }

        public int? OrgPointsPartB { get; set; }
        public int? OrgStartsPartB { get; set; }
        public int? MinStartsForPromotionPartB { get; set; }
        public bool? OrgAlreadyPromotedPartB { get; set; }
        public string? OrgAlreadyPromotedInfoPartB { get; set; }

        public bool Ignore { get; set; }

        public bool MultipleStarts { get; set; }
        public bool PossiblePromotionA { get; set; }
        public bool AlreadyPromotionA { get; set; }
        public string? PossiblePromotionAInfo { get; set; }
        public bool? PossiblePromotionB { get; set; }
        public string? PossiblePromotionBInfo { get; set; }
        public bool? AlreadyPromotionB { get; set; }

        public ParticipantPoco AssertCreate()
        {
            Assert.Multiple(() =>
            {
                Assert.That(
                    CompetitionName,
                    Is.Not.Null
                        .And.No.Empty,
                    nameof(CompetitionName));

                Assert.That(
                    CompetitionClassName,
                    Is.Not.Null
                        .And.No.Empty,
                    nameof(CompetitionClassName));

                Assert.That(
                    NamePartA,
                    Is.Not.Null
                        .And.No.Empty,
                    nameof(NamePartA));
            });

            return this;
        }

        public override string ToString()
        {
            return string.Format(
                "{0} ('{1}'/'{2}'/#'{3}') PartA/B '{4}'/'{5}'",
                CompetitionName,
                CompetitionClassName,
                "CURRENT",
                StartNumber,
                NamePartA,
                NamePartB);
        }
    }
}
