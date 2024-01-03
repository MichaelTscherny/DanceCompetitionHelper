using DanceCompetitionHelper.Database.Tables;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DanceCompetitionHelper.Web.Models.Configuration
{
    public class ConfigurationOverviewViewModel : OverviewModelBase<ConfigurationValue>
    {
        public ConfigurationViewModel ConfigurationViewModel { get; set; } = new ConfigurationViewModel();

        public string? Dummy { get; set; }
        public string? ErrorsAdd { get; set; }
        public string? ErrorsChange { get; set; }

        public List<SelectListItem>? AvailableOrganizations { get; set; } = new List<SelectListItem>();

        public IEnumerable<Competition>? Competitions { get; set; }
        public List<SelectListItem>? AvailableCompetitions { get; set; } = new List<SelectListItem>();

        public IEnumerable<CompetitionClass>? CompetitionClasses { get; set; }
        public List<SelectListItem>? AvailableCompetitionClasses { get; set; } = new List<SelectListItem>();

        public IEnumerable<CompetitionVenue>? CompetitionVenues { get; set; }
        public List<SelectListItem>? AvailableCompetitionVenues { get; set; } = new List<SelectListItem>();
    }
}
