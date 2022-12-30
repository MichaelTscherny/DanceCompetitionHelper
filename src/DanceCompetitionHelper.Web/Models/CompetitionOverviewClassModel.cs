using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Web.Models
{
    public class CompetitionOverviewClassModel
    {
        public Competition? Competition { get; set; }
        public IEnumerable<CompetitionClass>? CompetitionClasses { get; set; }
    }
}
