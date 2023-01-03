using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Web.Models
{
    public class CompetitionClassOverviewViewModel
    {
        public Competition? Competition { get; set; }
        public IEnumerable<CompetitionClass>? CompetitionClasses { get; set; }
    }
}
