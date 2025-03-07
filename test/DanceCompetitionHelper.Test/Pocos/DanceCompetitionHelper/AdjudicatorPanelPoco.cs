﻿namespace DanceCompetitionHelper.Test.Pocos.DanceCompetitionHelper
{
    public class AdjudicatorPanelPoco
    {
        public string CompetitionName { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Comment { get; set; }

        public AdjudicatorPanelPoco AssertCreate()
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
