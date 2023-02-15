namespace DanceCompetitionHelper.Test.Pocos.DanceCompetitionHelper
{
    public class AdjudicatorHistoryPoco
    {
        public string CompetitionName { get; set; } = default!;
        public string AdjudicatorPanelName { get; set; } = default!;
        public int AdjudicatorPanelVersion { get; set; }
        public int Version { get; set; }
        public string Abbreviation { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Comment { get; set; }
    }
}
