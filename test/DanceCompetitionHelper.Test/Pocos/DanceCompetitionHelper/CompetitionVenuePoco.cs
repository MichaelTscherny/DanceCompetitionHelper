namespace DanceCompetitionHelper.Test.Pocos.DanceCompetitionHelper
{
    public class CompetitionVenuePoco
    {
        public string CompetitionName { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Comment { get; set; }

        public CompetitionVenuePoco AssertCreate()
        {
            Assert.Multiple(() =>
            {
                Assert.That(
                    CompetitionName,
                    Is.Not.Null
                        .And.No.Empty,
                    nameof(CompetitionName));

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
