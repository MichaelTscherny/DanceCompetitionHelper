using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Database.DisplayInfo
{
    public class CheckPromotionInfo
    {
        public bool PossiblePromotionA { get; init; }
        public string? PossiblePromotionAInfo { get; init; }
        public bool? PossiblePromotionB { get; init; }
        public string? PossiblePromotionBInfo { get; init; }

        public bool PossiblePromotion => PossiblePromotionA || (PossiblePromotionB ?? false);

        public List<CompetitionClass> IncludedCompetitionClasses { get; init; } = new List<CompetitionClass>();
    }
}
