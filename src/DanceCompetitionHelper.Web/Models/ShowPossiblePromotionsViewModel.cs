using DanceCompetitionHelper.Database.DisplayInfo;
using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Web.Models
{
    public class ShowPossiblePromotionsViewModel
    {
        public Competition? Competition { get; init; }
        public List<Participant>? PossiblePromotions { get; init; }
        public List<MultipleStarter>? MultipleStarters { get; init; }
    }
}
