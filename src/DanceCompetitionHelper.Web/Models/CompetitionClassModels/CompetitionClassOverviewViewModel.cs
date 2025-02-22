using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Web.Models.CompetitionClassModels
{
    public class CompetitionClassOverviewViewModel : OverviewModelBase<CompetitionClass>
    {
        public IEnumerable<CompetitionVenue>? CompetitionVenues { get; set; }

    }
}
