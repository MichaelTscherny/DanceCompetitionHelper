using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Web.Models
{
    public class ParticipantOverviewViewModel
    {
        public Competition? Competition { get; set; }
        public IEnumerable<Participant>? Participtans { get; set; }
    }
}
