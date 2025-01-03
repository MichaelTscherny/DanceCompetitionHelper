using DanceCompetitionHelper.Database.DisplayInfo;
using DanceCompetitionHelper.Web.Enums;

namespace DanceCompetitionHelper.Web.Models.CompetitionClassModels
{
    public class ShowMultipleStartersOverviewViewModel : OverviewModelBase<MultipleStarter>
    {
        public bool DependentClassesView { get; set; }
        public GroupForViewEnum GroupForView { get; set; }
    }
}
