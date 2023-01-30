using DanceCompetitionHelper.Database.DisplayInfo;
using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Web.Models
{
    public class ShowPossiblePromotionsViewModel
    {
        public Competition? Competition { get; set; }
        public IEnumerable<MultipleStarter>? MultipleStarters { get; set; }
        public IEnumerable<Participant>? PossiblePromotions { get; set; }
    }
}
