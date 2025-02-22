using AutoMapper;

using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Exceptions;
using DanceCompetitionHelper.Web.Extensions;
using DanceCompetitionHelper.Web.Helper.Request;
using DanceCompetitionHelper.Web.Models.AdjudicatorModels;

using Microsoft.AspNetCore.Mvc;

namespace DanceCompetitionHelper.Web.Controllers
{
    public class AdjudicatorController : ControllerBase<AdjudicatorController>
    {
        public const string RefName = "Adjudicator";
        public const string AdjudicatorLastCreatedAdjudicatorPanelId = nameof(AdjudicatorLastCreatedAdjudicatorPanelId);

        public AdjudicatorController(
            IDanceCompetitionHelper danceCompHelper,
            ILogger<AdjudicatorController> logger,
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
            return GetDefaultRequestHandler<Adjudicator, AdjudicatorOverviewViewModel>()
                .SetOnSuccess(
                    nameof(Index))
                .SetOnNoData(
                    nameof(Index))
                .DefaultIndexAsync(
                    id,
                    async (indexId, dcH, _, _viewData, cToken) =>
                    {
                        var foundComp = await DefaultGetCompetitionAndSetViewDataAsync(
                            dcH,
                            indexId,
                            _viewData,
                            cToken);

                        if (foundComp == null)
                        {
                            return null;
                        }

                        return
                            new AdjudicatorOverviewViewModel()
                            {
                                Competition = await _danceCompHelper.GetCompetitionAsync(
                                    foundComp.CompetitionId,
                                    cToken),
                                OverviewItems = await _danceCompHelper
                                    .GetAdjudicatorsAsync(
                                        foundComp.CompetitionId,
                                        null,
                                        cToken)
                                    .ToListAsync(
                                        cToken),
                            };
                    },
                    cancellationToken);
        }

        [HttpGet]
        public Task<IActionResult> ShowCreateEdit(
            Guid id,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<Adjudicator, AdjudicatorViewModel>()
                .SetOnSuccess(
                    nameof(ShowCreateEdit))
                .SetOnNoData(
                    nameof(Index))
                .DefaultShowAsync(
                    id,
                    async (showId, dcH, mapper, _viewData, cToken) =>
                    {
                        var foundComp = await DefaultGetCompetitionAndSetViewDataAsync(
                            dcH,
                            showId,
                            _viewData,
                            cToken);

                        if (foundComp == null)
                        {
                            return null;
                        }

                        return await FillAdjudicatorViewModel(
                            dcH,
                            foundComp,
                            new AdjudicatorViewModel()
                            {
                                CompetitionId = foundComp.CompetitionId,
                                CompetitionName = foundComp.GetCompetitionName(),
                            },
                            cToken);
                    },
                    cancellationToken);
        }

        [HttpPost]
        public Task<IActionResult> CreateNew(
            AdjudicatorViewModel createAdjudicator,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<Adjudicator, AdjudicatorViewModel>()
                .SetOnSuccess(
                    nameof(Index))
                .SetOnModelStateInvalid(
                    nameof(ShowCreateEdit))
                .SetOnError(
                    nameof(ShowCreateEdit))
                .SetOnFunc(
                    SetOnEnum.OnModelStateInvalid | SetOnEnum.OnError,
                    async (model, dcH, _, _viewData, cToken) =>
                    {
                        await DefaultGetCompetitionAndSetViewDataAsync(
                            dcH,
                            createAdjudicator.CompetitionId,
                            _viewData,
                            cToken);

                        await FillAdjudicatorViewModel(
                            dcH,
                            createAdjudicator.CompetitionId,
                            createAdjudicator,
                            cToken);

                        return null;
                    })
                .DefaultCreateNewAsync(
                    createAdjudicator,
                    _mapper.Map<Adjudicator>(
                        createAdjudicator),
                    async (dcH, newEntity, _, _, cToken) =>
                    {
                        newEntity.Abbreviation = newEntity.Abbreviation.ToUpper();

                        await dcH.CreateAdjudicatorAsync(
                            newEntity,
                            cToken);

                        HttpContext.Session.SetString(
                            AdjudicatorLastCreatedAdjudicatorPanelId,
                            newEntity.AdjudicatorPanelId.ToString());

                        return new
                        {
                            Id = createAdjudicator.CompetitionId,
                        };
                    },
                    cancellationToken);
        }

        [HttpGet]
        public Task<IActionResult> ShowEdit(
            Guid id,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<Adjudicator, AdjudicatorViewModel>()
                .SetOnSuccess(
                    nameof(ShowCreateEdit))
                .SetOnNoData(
                    nameof(Index))
                .DefaultShowAsync(
                    id,
                    async (showId, dcH, mapper, _viewData, cToken) =>
                    {
                        var foundAdjucator = await _danceCompHelper.GetAdjudicatorAsync(
                            id,
                            cToken);

                        if (foundAdjucator == null)
                        {
                            return null;
                        }

                        var foundComp = await DefaultGetCompetitionAndSetViewDataAsync(
                            dcH,
                            foundAdjucator.AdjudicatorPanel.CompetitionId,
                            _viewData,
                            cToken);

                        if (foundComp == null)
                        {
                            return null;
                        }

                        return await FillAdjudicatorViewModel(
                            dcH,
                            foundComp,
                            new AdjudicatorViewModel()
                            {
                                CompetitionId = foundComp.CompetitionId,
                                CompetitionName = foundComp.GetCompetitionName(),
                                AdjudicatorId = foundAdjucator.AdjudicatorId,
                                AdjudicatorPanelId = foundAdjucator.AdjudicatorPanelId,
                                Abbreviation = foundAdjucator.Abbreviation.ToUpper(),
                                Name = foundAdjucator.Name,
                                Comment = foundAdjucator.Comment,
                            },
                            cToken);
                    },
                    cancellationToken);
        }

        [HttpPost]
        public Task<IActionResult> EditSave(
            AdjudicatorViewModel editAdjudicator,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<Adjudicator, AdjudicatorViewModel>()
                .SetOnSuccess(
                    nameof(Index))
                .SetOnNoData(
                    nameof(Index))
                .SetOnModelStateInvalid(
                    nameof(ShowCreateEdit))
                .SetOnError(
                    nameof(ShowCreateEdit))
                .SetOnFunc(
                    SetOnEnum.OnModelStateInvalid | SetOnEnum.OnError,
                    async (model, dcH, _, _viewData, cToken) =>
                    {
                        var foundComp = await DefaultGetCompetitionAndSetViewDataAsync(
                            dcH,
                            model.CompetitionId,
                            _viewData,
                            cToken);

                        await FillAdjudicatorViewModel(
                            dcH,
                            foundComp,
                            model,
                            cToken);

                        return null;
                    })
                .DefaultEditSaveAsync(
                    editAdjudicator,
                    async (model, dcH, mapper, _, cToken) =>
                    {
                        var foundAdjudicator = await dcH.GetAdjudicatorAsync(
                            editAdjudicator.AdjudicatorId ?? Guid.Empty,
                            cToken)
                            ?? throw new NoDataFoundException(
                                string.Format(
                                    "{0} with id '{1}' not found!",
                                    nameof(Adjudicator),
                                    editAdjudicator.AdjudicatorId));

                        // override the values...
                        mapper.Map(
                            editAdjudicator,
                            foundAdjudicator);

                        return new
                        {
                            Id = editAdjudicator.CompetitionId,
                        };
                    },
                    cancellationToken);
        }

        [HttpGet]
        public Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<Adjudicator, AdjudicatorViewModel>()
                .SetOnSuccess(
                    nameof(Index))
                .SetOnNoData(
                    nameof(Index))
                .SetOnError(
                    nameof(Index))
                .DefaultDeleteAsync(
                    id,
                    async (delId, dcH, _, _, cToken) =>
                    {
                        var helpComp = await _danceCompHelper.FindCompetitionAsync(
                            id,
                            cToken);

                        var foundAdj = await dcH.GetAdjudicatorAsync(
                            delId,
                            cToken)
                            ?? throw new NoDataFoundException(
                                string.Format(
                                    "{0} with id '{1}' not found!",
                                    nameof(AdjudicatorPanel),
                                    delId));

                        await dcH.RemoveAdjudicatorAsync(
                            foundAdj,
                            cToken);

                        return new
                        {
                            Id = helpComp?.CompetitionId ?? Guid.Empty,
                        };
                    },
                    cancellationToken);
        }

        #region Helper

        public Task<AdjudicatorViewModel> FillAdjudicatorViewModel(
            IDanceCompetitionHelper dcH,
            Competition? foundComp,
            AdjudicatorViewModel useModel,
            CancellationToken cancellationToken)
        {
            return FillAdjudicatorViewModel(
                dcH,
                foundComp?.CompetitionId ?? Guid.Empty,
                useModel,
                cancellationToken);
        }

        public async Task<AdjudicatorViewModel> FillAdjudicatorViewModel(
            IDanceCompetitionHelper dcH,
            Guid foundCompId,
            AdjudicatorViewModel useModel,
            CancellationToken cancellationToken)
        {
            _ = Guid.TryParse(
                HttpContext.Session.GetString(
                    AdjudicatorLastCreatedAdjudicatorPanelId),
                out var lastCreatedAdjudicatorPanelId);

            useModel.AdjudicatorPanels = await _danceCompHelper
                .GetAdjudicatorPanelsAsync(
                    foundCompId,
                    cancellationToken)
                .ToSelectListItemAsync(
                    lastCreatedAdjudicatorPanelId)
                .ToListAsync(
                    cancellationToken);

            return useModel;
        }

        #endregion Helper
    }
}
