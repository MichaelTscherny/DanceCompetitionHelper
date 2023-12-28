using DanceCompetitionHelper.Database.DisplayInfo;
using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Web.Models.CompetitionClassModels
{
    public class ShowPossiblePromotionsViewModel : OverviewModelBase<Participant>
    {
        public List<MultipleStarter>? MultipleStarters { get; init; }
    }
}
