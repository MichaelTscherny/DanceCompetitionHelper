namespace DanceCompetitionHelper.Info
{
    public class ExtraParticipants
    {
        public int ByWinning { get; set; }
        public string? ByWinningInfo { get; set; }

        public int ByPromotion { get; set; }
        public string? ByPromotionInfo { get; set; }

        public int AllExtraParticipants
            => ByWinning + ByPromotion;
    }
}
