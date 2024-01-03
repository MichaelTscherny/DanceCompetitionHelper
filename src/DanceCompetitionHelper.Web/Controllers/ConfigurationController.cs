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
             Guid? id,
             ConfigurationViewModel? showConfiguration = null,
             string? errorsAdd = null,
             string? errorsChange = null)
        {
            var foundCompId = _danceCompHelper.FindCompetition(
                id);
            var anyError = string.IsNullOrEmpty(errorsAdd) == false
                || string.IsNullOrEmpty(errorsChange) == false;

            showConfiguration?.SanityCheck();

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
                        anyError
                            ? showConfiguration?.CompetitionId
                            : null,
                        addEmpty: true);
            }
            else
            {
                availOrgs = new[]
                    {
                        foundComp.Organization
                    }
                    .ToSelectListItem(
                        anyError
                            ? showConfiguration?.Organization ?? foundComp.Organization
                            : foundComp.Organization,
                        addEmpty: false);
                availComps = new[]
                    {
                        foundComp
                    }
                    .ToSelectListItem(
                        anyError
                            ? showConfiguration?.CompetitionId ?? foundComp.CompetitionId
                            : foundComp.CompetitionId,
                        addEmpty: false);
            }

            // general...
            availCompClasses = useCompClasses
                ?.ToSelectListItem(
                    anyError
                        ? showConfiguration?.CompetitionClassId
                        : null,
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
                    ErrorsAdd = errorsAdd,
                    ErrorsChange = errorsChange,
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
            createConfiguration.SanityCheck();

            if (ModelState.IsValid == false)
            {
                return ShowConfig(
                    createConfiguration.OriginCompetitionId,
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
                    createConfiguration.OriginCompetitionId,
                    createConfiguration,
                    errorsAdd: exc.InnerException?.Message ?? exc.Message);
            }

            return RedirectToAction(
                nameof(ConfigurationController.Index),
                new
                {
                    Id = createConfiguration.OriginCompetitionId,
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(
            ConfigurationViewModel editConfiguration)
        {
            editConfiguration.SanityCheck();

            if (ModelState.IsValid == false)
            {
                return ShowConfig(
                    editConfiguration.OriginCompetitionId,
                    editConfiguration,
                    errorsChange: ModelState.GetErrorMessages());
            }

            try
            {
                _danceCompHelper.EditConfiguration(
                    editConfiguration.Organization ?? OrganizationEnum.Any,
                    editConfiguration.CompetitionId ?? Guid.Empty,
                    editConfiguration.CompetitionClassId ?? Guid.Empty,
                    editConfiguration.CompetitionVenueId ?? Guid.Empty,
                    editConfiguration.Key,
                    editConfiguration.Value,
                    editConfiguration.Comment);
            }
            catch (Exception exc)
            {
                return ShowConfig(
                    editConfiguration.OriginCompetitionId,
                    editConfiguration,
                    errorsChange: exc.InnerException?.Message ?? exc.Message);
            }

            return RedirectToAction(
                nameof(ConfigurationController.Index),
                new
                {
                    Id = editConfiguration.OriginCompetitionId,
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(
            ConfigurationViewModel deleteConfiguration)
        {
            deleteConfiguration.SanityCheck();

            if (ModelState.IsValid == false)
            {
                return ShowConfig(
                    deleteConfiguration.OriginCompetitionId,
                    deleteConfiguration,
                    errorsChange: ModelState.GetErrorMessages());
            }

            try
            {
                _danceCompHelper.RemoveConfiguration(
                    deleteConfiguration.Organization ?? OrganizationEnum.Any,
                    deleteConfiguration.CompetitionId ?? Guid.Empty,
                    deleteConfiguration.CompetitionClassId ?? Guid.Empty,
                    deleteConfiguration.CompetitionVenueId ?? Guid.Empty,
                    deleteConfiguration.Key);
            }
            catch (Exception exc)
            {
                return ShowConfig(
                    deleteConfiguration.OriginCompetitionId,
                    deleteConfiguration,
                    errorsChange: exc.InnerException?.Message ?? exc.Message);
            }

            return RedirectToAction(
                nameof(ConfigurationController.Index),
                new
                {
                    Id = deleteConfiguration.OriginCompetitionId,
                });
        }
    }
}
