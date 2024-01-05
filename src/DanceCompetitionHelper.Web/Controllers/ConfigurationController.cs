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

            // showConfiguration?.SanityCheck();

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
                    .ToSelectListItem(
                        anyError
                            ? showConfiguration?.Organization
                            : null);
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
            if (ModelState.IsValid == false)
            {
                return ShowConfig(
                    createConfiguration.OriginCompetitionId,
                    createConfiguration,
                    ModelState.GetErrorMessages());
            }

            try
            {
                createConfiguration.SanityCheck();

                _danceCompHelper.AddConfiguration(
                    createConfiguration.Organization,
                    createConfiguration.CompetitionId,
                    createConfiguration.CompetitionClassId,
                    createConfiguration.CompetitionVenueId,
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
            if (ModelState.IsValid == false)
            {
                return ShowConfig(
                    editConfiguration.OriginCompetitionId,
                    editConfiguration,
                    errorsChange: ModelState.GetErrorMessages());
            }

            try
            {
                editConfiguration.SanityCheck();

                _danceCompHelper.EditConfiguration(
                    editConfiguration.Organization,
                    editConfiguration.CompetitionId,
                    editConfiguration.CompetitionClassId,
                    editConfiguration.CompetitionVenueId,
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
            if (ModelState.IsValid == false)
            {
                return ShowConfig(
                    deleteConfiguration.OriginCompetitionId,
                    deleteConfiguration,
                    errorsChange: ModelState.GetErrorMessages());
            }

            try
            {
                deleteConfiguration.SanityCheck();

                _danceCompHelper.RemoveConfiguration(
                    deleteConfiguration.Organization,
                    deleteConfiguration.CompetitionId,
                    deleteConfiguration.CompetitionClassId,
                    deleteConfiguration.CompetitionVenueId,
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
