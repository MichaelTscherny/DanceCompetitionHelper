namespace DanceCompetitionHelper.Database.Test.Pocos.DanceCompetitionHelper
{
    internal class ParticipantPoco
    {
        public string CompetitionName { get; set; } = default!;
        public string CompetitionClassName { get; set; } = default!;
        public int StartNumber { get; set; }
        public string NamePartA { get; set; } = default!;
        public string? OrgIdPartA { get; set; }
        public string? NamePartB { get; set; }
        public string? OrgIdPartB { get; set; }
        public string? ClubName { get; set; }
        public string? OrgIdClub { get; set; }
        public int OrgPointsPartA { get; set; }
        public int OrgStartsPartA { get; set; }

        public int? OrgPointsPartB { get; set; }
        public int? OrgStartsPartB { get; set; }
        public bool Ignore { get; set; }

        public bool MultipleStarts { get; set; }
        public bool PossiblePromotionA { get; set; }
        public bool? PossiblePromotionB { get; set; }

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
