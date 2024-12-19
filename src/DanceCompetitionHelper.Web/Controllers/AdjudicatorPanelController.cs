using AutoMapper;
using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Exceptions;
using DanceCompetitionHelper.Web.Helper.Request;
using DanceCompetitionHelper.Web.Models.AdjudicatorPanelModels;
using Microsoft.AspNetCore.Mvc;

namespace DanceCompetitionHelper.Web.Controllers
{
    public class AdjudicatorPanelController : ControllerBase<AdjudicatorPanelController>
    {
        public const string RefName = "AdjudicatorPanel";

        public AdjudicatorPanelController(
            IDanceCompetitionHelper danceCompHelper,
            ILogger<AdjudicatorPanelController> logger,
            IMapper mapper)
            : base(
                danceCompHelper,
                logger,
                mapper)
        {
        }

        public Task<IActionResult> Index(
            Guid id,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<AdjudicatorPanel, AdjudicatorPanelOverviewViewModel>()
                .SetOnSuccess(
                    nameof(Index))
                .SetOnNoData(
                    nameof(Index))
                .DefaultIndexAsync(
                    id,
                    async (indexId, dcH, _, _viewData, cToken) =>
                    {
                        var foundComp = await _danceCompHelper
                            .FindCompetitionAsync(
                                id,
                                cToken);

                        if (foundComp == null)
                        {
                            return null;
                        }

                        var foundCompId = foundComp.CompetitionId;
                        _viewData["Use" + nameof(CompetitionClass)] = foundCompId;

                        return new AdjudicatorPanelOverviewViewModel()
                        {
                            Competition = foundComp,
                            OverviewItems = await _danceCompHelper
                                .GetAdjudicatorPanelsAsync(
                                    foundCompId,
                                    cToken,
                                    true)
                                .ToListAsync(
                                    cToken),
                        };
                    },
                    cancellationToken);
        }

        public Task<IActionResult> ShowCreateEdit(
            Guid id,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<AdjudicatorPanel, AdjudicatorPanelViewModel>()
                .SetOnSuccess(
                    nameof(ShowCreateEdit))
                .SetOnNoData(
                    nameof(ShowCreateEdit))
                .DefaultShowAsync(
                    id,
                    async (showId, dcH, _, _viewData, cToken) =>
                    {
                        var foundComp = await _danceCompHelper
                            .FindCompetitionAsync(
                                id,
                                cToken);

                        if (foundComp == null)
                        {
                            return null;
                        }

                        var foundCompId = foundComp.CompetitionId;
                        _viewData["Use" + nameof(CompetitionClass)] = foundCompId;

                        return new AdjudicatorPanelViewModel()
                        {
                            CompetitionId = foundCompId,
                            CompetitionName = foundComp.GetCompetitionName(),
                        };
                    },
                    cancellationToken);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> CreateNew(
            AdjudicatorPanelViewModel createAdjudicatorPanel,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<AdjudicatorPanel, AdjudicatorPanelViewModel>()
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
                        await DefaultGetCompetitionAndSetViewData(
                            dcH,
                            createAdjudicatorPanel.CompetitionId,
                            _viewData,
                            cToken);

                        return null;
                    })
                .DefaultCreateNewAsync(
                    createAdjudicatorPanel,
                    _mapper.Map<AdjudicatorPanel>(
                        createAdjudicatorPanel),
                    async (dcH, newEntity, _, _, cToken) =>
                    {
                        await dcH.CreateAdjudicatorPanelAsync(
                            newEntity,
                            cToken);

                        return new
                        {
                            Id = createAdjudicatorPanel.CompetitionId,
                        };
                    },
                    cancellationToken);
        }

        public Task<IActionResult> ShowEdit(
            Guid id,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<AdjudicatorPanel, AdjudicatorPanelViewModel>()
                .SetOnSuccess(
                    nameof(ShowCreateEdit))
                .SetOnNoData(
                    nameof(Index))
                .DefaultShowAsync(
                    id,
                    async (showId, dcH, mapper, _viewData, cToken) =>
                    {
                        var foundAdjPanel = await _danceCompHelper.GetAdjudicatorPanelAsync(
                            id,
                            cToken,
                            true);

                        if (foundAdjPanel == null)
                        {
                            return null;
                        }

                        _viewData["Use" + nameof(CompetitionClass)] = foundAdjPanel.CompetitionId;

                        return new AdjudicatorPanelViewModel()
                        {
                            CompetitionId = foundAdjPanel.CompetitionId,
                            CompetitionName = foundAdjPanel.Competition.GetCompetitionName(),
                            AdjudicatorPanelId = foundAdjPanel.AdjudicatorPanelId,
                            Name = foundAdjPanel.Name,
                            Comment = foundAdjPanel.Comment,
                        };
                    },
                    cancellationToken);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> EditSave(
            AdjudicatorPanelViewModel editAdjudicatorPanel,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<AdjudicatorPanel, AdjudicatorPanelViewModel>()
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
                        await DefaultGetCompetitionAndSetViewData(
                            dcH,
                            model.CompetitionId,
                            _viewData,
                            cToken);

                        return null;
                    })
                .DefaultEditSaveAsync(
                    editAdjudicatorPanel,
                    async (model, dcH, mapper, _, cToken) =>
                    {
                        var foundAdjPanel = await dcH.GetAdjudicatorPanelAsync(
                        editAdjudicatorPanel.AdjudicatorPanelId ?? Guid.Empty,
                        cToken)
                        ?? throw new NoDataFoundException(
                            string.Format(
                                "{0} with id '{1}' not found!",
                                nameof(AdjudicatorPanel),
                                editAdjudicatorPanel.AdjudicatorPanelId));

                        // override the values...
                        mapper.Map(
                            editAdjudicatorPanel,
                            foundAdjPanel);

                        return new
                        {
                            Id = foundAdjPanel.CompetitionId,
                        };
                    },
                    cancellationToken);
        }

        public Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<AdjudicatorPanel, AdjudicatorPanelViewModel>()
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
                        var foundAdjPanel = await dcH.GetAdjudicatorPanelAsync(
                        delId,
                        cToken)
                        ?? throw new NoDataFoundException(
                            string.Format(
                                "{0} with id '{1}' not found!",
                                nameof(AdjudicatorPanel),
                                delId));

                        await dcH.RemoveAdjudicatorPanelAsync(
                            foundAdjPanel,
                            cToken);

                        return new
                        {
                            Id = foundAdjPanel.CompetitionId,
                        };
                    },
                    cancellationToken);
        }
    }
}
