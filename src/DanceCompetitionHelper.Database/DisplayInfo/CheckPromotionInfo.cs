using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Database.DisplayInfo
{
    public class CheckPromotionInfo
    {
        public bool PossiblePromotionA { get; init; }
        public string? PossiblePromotionAInfo { get; init; }
        public bool? AlreadyPromotionA { get; init; }
        public string? AlreadyPromotionAInfo { get; init; }

        public bool? PossiblePromotionB { get; init; }
        public string? PossiblePromotionBInfo { get; init; }
        public bool? AlreadyPromotionB { get; init; }
        public string? AlreadyPromotionBInfo { get; init; }

        public bool PossiblePromotion => PossiblePromotionA || (PossiblePromotionB ?? false);
        public bool AlreadyPromoted => (AlreadyPromotionA ?? false) || (AlreadyPromotionB ?? false);

        public List<CompetitionClass> IncludedCompetitionClasses { get; init; } = new List<CompetitionClass>();
    }
}
