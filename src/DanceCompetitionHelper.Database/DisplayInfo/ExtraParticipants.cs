namespace DanceCompetitionHelper.Info
{
    public class ExtraParticipants
    {
        public int ByWinning { get; set; }
        public int ByPromotion { get; set; }

        public int AllExtraParticipants
            => ByWinning + ByPromotion;
    }
}
