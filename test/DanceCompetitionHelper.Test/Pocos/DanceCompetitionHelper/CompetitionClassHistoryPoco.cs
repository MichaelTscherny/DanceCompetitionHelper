namespace DanceCompetitionHelper.Database.Test.Pocos.DanceCompetitionHelper
{
    public class CompetitionClassHistoryPoco
    {
        public string CompetitionName { get; set; } = default!;
        public string OrgClassId { get; set; } = default!;
        public int Version { get; set; }
        public string CompetitionClassName { get; set; } = default!;
        public string AdjudicatorPanelName { get; set; } = default!;
        public int AdjudicatorPanelVersion { get; set; }
        public string? Discipline { get; set; }
        public string? AgeClass { get; set; }
        public string? AgeGroup { get; set; }
        public string? Class { get; set; }
        public int MinStartsForPromotion { get; set; }
        public int MinPointsForPromotion { get; set; }

        public int PointsForFirst { get; set; }
        public int ExtraManualStarter { get; set; }
        public string? Comment { get; set; }

        public bool Ignore { get; set; }

        public CompetitionClassHistoryPoco AssertCreate()
        {
            Assert.Multiple(() =>
            {
                Assert.That(
                    CompetitionName,
                    Is.Not.Null
                        .And.No.Empty,
                    nameof(CompetitionName));

                Assert.That(
                    OrgClassId,
                    Is.Not.Null
                        .And.No.Empty,
                    nameof(OrgClassId));

                Assert.That(
                    CompetitionClassName,
                    Is.Not.Null
                        .And.No.Empty,
                    nameof(CompetitionClassName));

                Assert.That(
                    AdjudicatorPanelName,
                    Is.Not.Null
                        .And.No.Empty,
                    nameof(AdjudicatorPanelName));
            });

            return this;
        }

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
