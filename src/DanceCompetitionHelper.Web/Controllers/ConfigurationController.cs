using DanceCompetitionHelper.Database.Enum;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Extensions;
using DanceCompetitionHelper.Web.Extensions;
using DanceCompetitionHelper.Web.Models.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DanceCompetitionHelper.Web.Controllers
{
    public class ConfigurationController : Controller
    {
        public const string RefName = "Configuration";

        private readonly IDanceCompetitionHelper _danceCompHelper;
        private readonly ILogger<CompetitionController> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ConfigurationController(
            IDanceCompetitionHelper danceCompHelper,
            ILogger<CompetitionController> logger,
            IServiceProvider serviceProvider)
        {
            _danceCompHelper = danceCompHelper
                ?? throw new ArgumentNullException(
                    nameof(danceCompHelper));
            _logger = logger
                ?? throw new ArgumentNullException(
                    nameof(logger));
            _serviceProvider = serviceProvider
                ?? throw new ArgumentNullException(
                    nameof(serviceProvider));
        }


        public IActionResult Index(
            Guid id)
        {
            return ShowConfig(
                id);
        }

        private IActionResult ShowConfig(
             Guid id,
             ConfigurationViewModel? showConfiguration = null,
             string? errors = null)
        {
            var foundCompId = _danceCompHelper.FindCompetition(
                id);

            ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

            var (showConfig, foundComp, useComps, useCompClasses, useCompVenues) =
                _danceCompHelper
                    .GetConfigurations(
                        foundCompId);

            List<SelectListItem>? availOrgs = null;
            List<SelectListItem>? availComps = null;
            List<SelectListItem>? availCompClasses = null;
            List<SelectListItem>? availCompVenues = null;

            if (foundComp == null)
            {
                availOrgs = EnumExtensions
                    .GetValues<OrganizationEnum>()
                    .ToSelectListItem();
                availComps = useComps
                    ?.ToSelectListItem(
                        addEmpty: true);
            }
            else
            {
                availOrgs = new[]
                    {
                        foundComp.Organization
                    }
                    .ToSelectListItem(
                        foundComp.Organization,
                        addEmpty: false);
                availComps = new[]
                    {
                        foundComp
                    }
                    .ToSelectListItem(
                        foundComp.CompetitionId,
                        addEmpty: false);
            }

            // general...
            availCompClasses = useCompClasses
                ?.ToSelectListItem(
                    addEmpty: true);

            var useCfgViewModel = showConfiguration ?? new ConfigurationViewModel()
            {
                CompetitionId = foundCompId,
            };
            useCfgViewModel.OriginCompetitionId = foundCompId;

            return View(
                nameof(Index),
                new ConfigurationOverviewViewModel()
                {
                    ConfigurationViewModel = useCfgViewModel,
                    Errors = errors,
                    // 
                    Competition = foundComp,
                    OverviewItems = showConfig.ToList(),
                    // for displaying all...
                    AvailableOrganizations = availOrgs,
                    //
                    Competitions = useComps,
                    AvailableCompetitions = availComps,
                    // 
                    CompetitionClasses = useCompClasses,
                    AvailableCompetitionClasses = availCompClasses,
                    // TODO: implement when "CompetitionVenues" added
                    CompetitionVenues = useCompVenues,
                    AvailableCompetitionVenues = availCompVenues,
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNew(
            ConfigurationViewModel createConfiguration)
        {
            if (ModelState.IsValid == false)
            {
                return ShowConfig(
                    createConfiguration.OriginCompetitionId ?? Guid.Empty,
                    createConfiguration,
                    ModelState.GetErrorMessages());
            }

            try
            {
                _danceCompHelper.AddConfiguration(
                    createConfiguration.Organization ?? OrganizationEnum.Any,
                    createConfiguration.CompetitionId ?? Guid.Empty,
                    createConfiguration.CompetitionClassId ?? Guid.Empty,
                    createConfiguration.CompetitionVenueId ?? Guid.Empty,
                    createConfiguration.Key,
                    createConfiguration.Value,
                    createConfiguration.Comment);
            }
            catch (Exception exc)
            {
                return ShowConfig(
                    createConfiguration.OriginCompetitionId ?? Guid.Empty,
                    createConfiguration,
                    exc.InnerException?.Message ?? exc.Message);
            }

            return RedirectToAction(
                nameof(ConfigurationController.Index));
        }
    }
}
