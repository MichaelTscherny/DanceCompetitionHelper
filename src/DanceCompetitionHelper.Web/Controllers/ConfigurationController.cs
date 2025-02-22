using AutoMapper;

using DanceCompetitionHelper.Database.Enum;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Exceptions;
using DanceCompetitionHelper.Web.Extensions;
using DanceCompetitionHelper.Web.Models.CompetitionModels;
using DanceCompetitionHelper.Web.Models.ConfigurationModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DanceCompetitionHelper.Web.Controllers
{
    public class ConfigurationController : ControllerBase<ConfigurationController>
    {
        public const string RefName = "Configuration";
        public const string ForAll = "FOR ALL";

        public ConfigurationController(
            IDanceCompetitionHelper danceCompHelper,
            ILogger<ConfigurationController> logger,
            IMapper mapper)
            : base(
                danceCompHelper,
                logger,
                mapper)
        {
        }

        [HttpGet]
        public Task<IActionResult> Index(
            Guid id,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<ConfigurationValue, ConfigurationOverviewViewModel>()
                .SetOnSuccess(
                    nameof(Index))
                .SetOnNoData(
                    nameof(Index))
                .DefaultIndexAsync(
                    Guid.Empty,
                    async (_, dcH, _, _viewData, cToken) =>
                    {
                        return await ShowConfig(
                            id,
                            _viewData,
                            cToken);
                    },
                    cancellationToken);
        }

        [HttpGet]
        private async Task<ConfigurationOverviewViewModel> ShowConfig(
             Guid? id,
             ViewDataDictionary? viewData,
             CancellationToken cancellationToken,
             ConfigurationViewModel? showConfiguration = null,
             string? errorsAdd = null,
             string? errorsChange = null)
        {
            var foundComp = await DefaultGetCompetitionAndSetViewDataAsync(
                _danceCompHelper,
                id,
                null,
                cancellationToken);

            var foundCompId = foundComp?.CompetitionId ?? Guid.Empty;
            var anyError = string.IsNullOrEmpty(errorsAdd) == false
                || string.IsNullOrEmpty(errorsChange) == false;

            // showConfiguration?.SanityCheck();

            if (foundCompId != Guid.Empty)
            {
                (viewData ?? ViewData)["Use" + nameof(CompetitionClass)] = foundCompId;
            }

            var (showConfig, _, useComps, useCompClasses, useCompVenues) =
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
                availOrgs = BlazorExtensions
                    .ToSelectListItems(
                        (anyError
                            ? showConfiguration?.Organization
                            : null) ?? OrganizationEnum.Any)
                    .ToList();

                if (useComps != null)
                {
                    availComps = await useComps
                        .ToAsyncEnumerable()
                        .ToSelectListItemAsync(
                            anyError
                                ? showConfiguration?.CompetitionId
                                : null,
                            addEmpty: true)
                        .ToListAsync(
                            cancellationToken);
                }
            }
            else
            {
                availOrgs = new[]
                    {
                        foundComp.Organization
                    }
                    .ToSelectListItems(
                        anyError
                            ? showConfiguration?.Organization ?? foundComp.Organization
                            : foundComp.Organization,
                        addEmpty: false)
                    .ToList();
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
                    .ToListAsync(
                        cancellationToken);

                if (useCompClasses != null)
                {
                    availCompClasses = await useCompClasses
                        .ToAsyncEnumerable()
                        .ToSelectListItemAsync(
                            anyError
                                ? showConfiguration?.CompetitionClassId
                                : null,
                            addEmpty: true)
                        .ToListAsync(
                            cancellationToken);
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
                        .ToListAsync(
                            cancellationToken);
                }
            }

            var useCfgViewModel = showConfiguration ?? new ConfigurationViewModel()
            {
                CompetitionId = foundCompId,
            };
            useCfgViewModel.OriginCompetitionId = foundCompId;

            return new ConfigurationOverviewViewModel()
            {
                ConfigurationViewModel = useCfgViewModel,
                ErrorsAdd = errorsAdd,
                ErrorsChange = errorsChange,
                // 
                ShowGlobalConfigOnly = foundCompId == Guid.Empty,
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
            };
        }

        [HttpPost]
        public Task<IActionResult> CreateNew(
            ConfigurationViewModel createConfiguration,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<ConfigurationValue, ConfigurationViewModel>()
                .SetOnSuccess(
                    nameof(Index))
                .SetOnModelStateInvalid(
                    nameof(Index))
                .SetOnError(
                    nameof(Index))
                .DefaultCreateNewAsync(
                    createConfiguration,
                    _mapper.Map<ConfigurationValue>(
                        createConfiguration),
                    // ----
                    async (dcH, newEntity, _, _, cToken) =>
                    {
                        await dcH.CreateConfigurationAsync(
                            newEntity,
                            cToken);

                        return new
                        {
                            Id = createConfiguration.OriginCompetitionId,
                        };
                    },
                    cancellationToken);
        }

        [HttpPost]
        public Task<IActionResult> Edit(
            ConfigurationViewModel editConfiguration,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<ConfigurationValue, ConfigurationViewModel>()
                .SetOnSuccess(
                    nameof(Index))
                .SetOnNoData(
                    nameof(Index))
                .SetOnModelStateInvalid(
                    nameof(Index))
                .SetOnError(
                    nameof(Index))
                .DefaultEditSaveAsync(
                    editConfiguration,
                    async (model, dcH, mapper, _viewData, cToken) =>
                    {
                        model.SanityCheck();

                        var foundConfig = await dcH.GetConfigurationAsync(
                            mapper.Map<ConfigurationValue>(
                                model),
                            cToken)
                            ?? throw new NoDataFoundException(
                                string.Format(
                                    "{0} with id '{1}' not found!",
                                    nameof(ConfigurationValue),
                                    model.CompetitionId));

                        var foundComp = await DefaultGetCompetitionAndSetViewDataAsync(
                            dcH,
                            foundConfig.CompetitionId,
                            _viewData,
                            cToken);

                        // override the values...
                        mapper.Map(
                            model,
                            foundConfig);

                        return new
                        {
                            Id = model.OriginCompetitionId,
                        };
                    },
                    cancellationToken);
        }

        [HttpPost]
        public Task<IActionResult> Delete(
            ConfigurationViewModel deleteConfiguration,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<Competition, CompetitionViewModel>()
                .SetOnSuccess(
                    nameof(Index))
                .SetOnNoData(
                    nameof(Index))
                .SetOnError(
                    nameof(Index))
                .DefaultDeleteAsync(
                    deleteConfiguration,
                    async (delConf, dcH, mapper, _, cToken) =>
                    {
                        var foundConfig = await dcH.GetConfigurationAsync(
                            mapper.Map<ConfigurationValue>(
                                delConf),
                            cToken)
                            ?? throw new NoDataFoundException(
                                string.Format(
                                    "{0} with id '{1}' not found!",
                                    nameof(ConfigurationValue),
                                    delConf));

                        await _danceCompHelper.RemoveConfigurationAsync(
                            foundConfig,
                            cToken);

                        return new
                        {
                            Id = deleteConfiguration.OriginCompetitionId,
                        };
                    },
                    cancellationToken);
        }
    }
}
