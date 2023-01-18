using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Database.DisplayInfo
{
    public class CheckMultipleStartInfo
    {
        public bool MultipleStarts { get; set; }
        public string? MultipleStartsInfo { get; set; }

        public List<CompetitionClass> IncludedCompetitionClasses { get; init; } = new List<CompetitionClass>();
    }
}
