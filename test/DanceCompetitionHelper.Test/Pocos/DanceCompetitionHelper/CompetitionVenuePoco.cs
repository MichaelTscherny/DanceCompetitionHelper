namespace DanceCompetitionHelper.Test.Pocos.DanceCompetitionHelper
{
    public class CompetitionVenuePoco
    {
        public string CompetitionName { get; set; } = default!;
        public string Name { get; set; } = default!;
        public int LengthInMeter { get; set; }
        public int WidthInMeter { get; set; }
        public string? Comment { get; set; }

        public CompetitionVenuePoco AssertCreate()
        {
            using (Assert.EnterMultipleScope())
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
            }

            return this;
        }
    }
}
