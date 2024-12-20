using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Web.Models
{
    public abstract class OverviewModelBase<T> : ViewModelBase
        where T : class
    {
        public Competition? Competition { get; set; }
        public IEnumerable<T>? OverviewItems { get; set; }
        public bool DetailedView { get; set; }
    }
}
