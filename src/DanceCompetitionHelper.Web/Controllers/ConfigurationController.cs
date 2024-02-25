using DanceCompetitionHelper.Database.Enum;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Extensions;
using DanceCompetitionHelper.Web.Extensions;
using DanceCompetitionHelper.Web.Models.ConfigurationModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DanceCompetitionHelper.Web.Controllers
{
    public class ConfigurationController : ControllerBase
    {
        public const string RefName = "Configuration";
        public const string ForAll = "FOR ALL";

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


        public Task<IActionResult> Index(
            Guid id,
            CancellationToken cancellationToken)
        {
            return ShowConfig(
                id,
                cancellationToken);
        }

        private async Task<IActionResult> ShowConfig(
             Guid? id,
             CancellationToken cancellationToken,
             ConfigurationViewModel? showConfiguration = null,
             string? errorsAdd = null,
             string? errorsChange = null)
        {
            var foundCompId = await _danceCompHelper.FindCompetitionAsync(
                id,
                cancellationToken);
            var anyError = string.IsNullOrEmpty(errorsAdd) == false
                || string.IsNullOrEmpty(errorsChange) == false;

            // showConfiguration?.SanityCheck();

            ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

            var (showConfig, foundComp, useComps, useCompClasses, useCompVenues) =
                await _danceCompHelper
                    .GetConfigurationsAsync(
                        foundCompId,
                        cancellationToken);

            List<SelectListItem>? availOrgs = null;
            List<SelectListItem>? availComps = null;
            List<SelectListItem>? availCompClasses = null;
            List<SelectListItem>? availCompVenues = null;

            if (foundComp == null)
            {
                availOrgs = await EnumExtensions
                    .GetValues<OrganizationEnum>()
                    .ToAsyncEnumerable()
                    .ToSelectListItemAsync(
                        anyError
                            ? showConfiguration?.Organization
                            : null)
                    .ToListAsync();

                if (useComps != null)
                {
                    availComps = await useComps
                        .ToAsyncEnumerable()
                        .ToSelectListItemAsync(
                            anyError
                                ? showConfiguration?.CompetitionId
                                : null,
                            addEmpty: true)
                        .ToListAsync();
                }
            }
            else
            {
                availOrgs = await new[]
                    {
                        foundComp.Organization
                    }
                    .ToAsyncEnumerable()
                    .ToSelectListItemAsync(
                        anyError
                            ? showConfiguration?.Organization ?? foundComp.Organization
                            : foundComp.Organization,
                        addEmpty: false)
                    .ToListAsync();
                availComps = await new[]
                    {
                        foundComp
                    }
                    .ToAsyncEnumerable()
                    .ToSelectListItemAsync(
                        anyError
                            ? showConfiguration?.CompetitionId ?? foundComp.CompetitionId
                            : foundComp.CompetitionId,
                        addEmpty: false)
                    .ToListAsync();

                if (useCompClasses != null)
                {
                    availCompClasses = await useCompClasses
                        .ToAsyncEnumerable()
                        .ToSelectListItemAsync(
                            anyError
                                ? showConfiguration?.CompetitionClassId
                                : null,
                            addEmpty: true)
                        .ToListAsync();
                }

                if (useCompVenues != null)
                {
                    availCompVenues = await useCompVenues
                        .ToAsyncEnumerable()
                        .ToSelectListItemAsync(
                            anyError
                                ? showConfiguration?.CompetitionVenueId
                                : null,
                            addEmpty: true)
                        .ToListAsync();
                }
            }

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
                    OverviewItems = showConfig,
                    // for displaying all...
                    AvailableOrganizations = availOrgs,
                    //
                    Competitions = useComps,
                    AvailableCompetitions = availComps,
                    // 
                    CompetitionClasses = useCompClasses,
                    AvailableCompetitionClasses = availCompClasses,
                    //
                    CompetitionVenues = useCompVenues,
                    AvailableCompetitionVenues = availCompVenues,
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNew(
            ConfigurationViewModel createConfiguration,
            CancellationToken cancellationToken)
        {
            if (ModelState.IsValid == false)
            {
                return await ShowConfig(
                    createConfiguration.OriginCompetitionId,
                    cancellationToken,
                    createConfiguration,
                    ModelState.GetErrorMessages());
            }

            try
            {
                createConfiguration.SanityCheck();

                await _danceCompHelper.CreateConfigurationAsync(
                    createConfiguration.Organization,
                    createConfiguration.CompetitionId,
                    createConfiguration.CompetitionClassId,
                    createConfiguration.CompetitionVenueId,
                    createConfiguration.Key,
                    createConfiguration.Value,
                    createConfiguration.Comment,
                    cancellationToken);
            }
            catch (Exception exc)
            {
                return await ShowConfig(
                    createConfiguration.OriginCompetitionId,
                    cancellationToken,
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
        public async Task<IActionResult> Edit(
            ConfigurationViewModel editConfiguration,
            CancellationToken cancellationToken)
        {
            if (ModelState.IsValid == false)
            {
                return await ShowConfig(
                    editConfiguration.OriginCompetitionId,
                    cancellationToken,
                    editConfiguration,
                    errorsChange: ModelState.GetErrorMessages());
            }

            try
            {
                editConfiguration.SanityCheck();

                await _danceCompHelper.EditConfigurationAsync(
                    editConfiguration.Organization,
                    editConfiguration.CompetitionId,
                    editConfiguration.CompetitionClassId,
                    editConfiguration.CompetitionVenueId,
                    editConfiguration.Key,
                    editConfiguration.Value,
                    editConfiguration.Comment,
                    cancellationToken);
            }
            catch (Exception exc)
            {
                return await ShowConfig(
                    editConfiguration.OriginCompetitionId,
                    cancellationToken,
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
        public async Task<IActionResult> Delete(
            ConfigurationViewModel deleteConfiguration,
            CancellationToken cancellationToken)
        {
            if (ModelState.IsValid == false)
            {
                return await ShowConfig(
                    deleteConfiguration.OriginCompetitionId,
                    cancellationToken,
                    deleteConfiguration,
                    errorsChange: ModelState.GetErrorMessages());
            }

            try
            {
                deleteConfiguration.SanityCheck();

                await _danceCompHelper.RemoveConfigurationAsync(
                    deleteConfiguration.Organization,
                    deleteConfiguration.CompetitionId,
                    deleteConfiguration.CompetitionClassId,
                    deleteConfiguration.CompetitionVenueId,
                    deleteConfiguration.Key,
                    cancellationToken);
            }
            catch (Exception exc)
            {
                return await ShowConfig(
                    deleteConfiguration.OriginCompetitionId,
                    cancellationToken,
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
