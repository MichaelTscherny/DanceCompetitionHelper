namespace DanceCompetitionHelper.Test.Pocos.DanceCompetitionHelper
{
    public class AdjudicatorPoco
    {
        public string CompetitionName { get; set; } = default!;
        public string AdjudicatorPanelName { get; set; } = default!;
        public string Abbreviation { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Comment { get; set; }

        public AdjudicatorPoco AssertCreate()
        {
            Assert.Multiple(() =>
            {
                Assert.That(
                    CompetitionName,
                    Is.Not.Null
                        .And.No.Empty,
                    nameof(CompetitionName));

                Assert.That(
                    AdjudicatorPanelName,
                    Is.Not.Null
                        .And.No.Empty,
                    nameof(AdjudicatorPanelName));

                Assert.That(
                    Abbreviation,
                    Is.Not.Null
                        .And.No.Empty,
                    nameof(Abbreviation));

                Assert.That(
                    Name,
                    Is.Not.Null
                        .And.No.Empty,
                    nameof(Name));
            });

            return this;
        }
    }
}
