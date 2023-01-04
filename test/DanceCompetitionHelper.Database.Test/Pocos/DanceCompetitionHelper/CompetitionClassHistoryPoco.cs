namespace DanceCompetitionHelper.Database.Test.Pocos.DanceCompetitionHelper
{
    public class CompetitionClassHistoryPoco
    {
        public string CompetitionName { get; set; } = default!;
        public string OrgClassId { get; set; } = default!;
        public int Version { get; set; }
        public string CompetitionClassName { get; set; } = default!;
        public string? Discipline { get; set; }
        public string? AgeClass { get; set; }
        public string? AgeGroup { get; set; }
        public string? Class { get; set; }
        public int MinStartsForPromotion { get; set; }
        public int MinPointsForPromotion { get; set; }
        public int PointsForWinning { get; set; }

        public bool Ignore { get; set; }

        public override string ToString()
        {
            return string.Format(
                "{0} ('{1}'/'{2}') D/Ac/Ag/C: '{3}'/'{4}'/'{5}'/'{6}' - S/P: {7}/{8}",
                CompetitionName,
                OrgClassId,
                Version,
                Discipline,
                AgeClass,
                AgeGroup,
                Class,
                MinStartsForPromotion,
                MinPointsForPromotion);
        }
    }
}
