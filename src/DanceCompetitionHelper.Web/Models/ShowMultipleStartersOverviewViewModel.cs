using DanceCompetitionHelper.Database.DisplayInfo;
using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Web.Models
{
    public class ShowMultipleStartersOverviewViewModel
    {
        public Competition? Competition { get; set; }
        public IEnumerable<MultipleStarter>? MultipleStarters { get; set; }
    }
}
