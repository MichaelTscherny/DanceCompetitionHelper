using AutoMapper;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Exceptions;
using DanceCompetitionHelper.Web.Extensions;
using DanceCompetitionHelper.Web.Helper.Request;
using DanceCompetitionHelper.Web.Models.CompetitionClassModels;
using Microsoft.AspNetCore.Mvc;

namespace DanceCompetitionHelper.Web.Controllers
{
    public class CompetitionClassController : ControllerBase<CompetitionClassController>
    {
        public const string RefName = "CompetitionClass";
        public const string CompetitionClassLastCreatedAdjudicatorPanelId = nameof(CompetitionClassLastCreatedAdjudicatorPanelId);
        public const string DefaultCompetitionColor = "#ffffff";

        public CompetitionClassController(
            IDanceCompetitionHelper danceCompHelper,
            ILogger<CompetitionClassController> logger,
            IMapper mapper)
            : base(
                danceCompHelper,
                logger,
                mapper)
        {
        }

        public async Task<IActionResult> Index(
            Guid id,
            CancellationToken cancellationToken)
        {
            return await GetDefaultRequestHandler<CompetitionClass, CompetitionClassOverviewViewModel>()
                .SetOnSuccess(
                    nameof(Index))
                .SetOnNoData(
                    nameof(Index))
                .DefaultIndexAsync(
                    id,
                    async (indexId, dcH, _, _viewData, cToken) =>
                    {
                        var foundComp = await dcH.FindCompetitionAsync(
                            indexId,
                            cToken);

                        if (foundComp == null)
                        {
                            return null;
                        }

                        var foundCompId = foundComp.CompetitionId;
                        _viewData["Use" + nameof(CompetitionClass)] = foundCompId;

                        return new CompetitionClassOverviewViewModel()
                        {
                            Competition = foundComp,
                            OverviewItems = await dcH
                                .GetCompetitionClassesAsync(
                                    foundCompId,
                                    cToken,
                                    true)
                                .ToListAsync(
                                    cToken),
                        };
                    },
                    cancellationToken);
        }

        public async Task<IActionResult> DetailedView(
            Guid id,
            CancellationToken cancellationToken)
        {
            return await GetDefaultRequestHandler<CompetitionClass, CompetitionClassOverviewViewModel>()
                .SetOnSuccess(
                    nameof(Index))
                .SetOnNoData(
                    nameof(Index))
                .DefaultShowAsync(
                    id,
                    async (showId, dcH, _, _viewData, cToken) =>
                    {
                        var foundComp = await dcH.FindCompetitionAsync(
                            showId,
                            cToken);

                        if (foundComp == null)
                        {
                            return null;
                        }

                        var foundCompId = foundComp.CompetitionId;
                        ViewData["Use" + nameof(CompetitionClass)] = foundCompId;
                        _viewData["Use" + nameof(CompetitionClass)] = foundCompId;

                        return new CompetitionClassOverviewViewModel()
                        {
                            Competition = foundComp,
                            OverviewItems = await dcH
                                .GetCompetitionClassesAsync(
                                    foundCompId,
                                    cToken,
                                    true,
                                    true)
                                .ToListAsync(
                                    cToken),
                            DetailedView = true,
                        };
                    },
                    cancellationToken);
        }

        public async Task<IActionResult> ShowCreateEdit(
            Guid id,
            CancellationToken cancellationToken)
        {
            return await GetDefaultRequestHandler<CompetitionClass, CompetitionClassViewModel>()
                .SetOnSuccess(
                    nameof(ShowCreateEdit))
                .SetOnNoData(
                    nameof(Index))
                .DefaultShowAsync(
                    id,
                    async (showId, dcH, mapper, _viewData, cToken) =>
                    {
                        var foundComp = await dcH.FindCompetitionAsync(
                            showId,
                            cToken);

                        if (foundComp == null)
                        {
                            return null;
                        }

                        var foundCompId = foundComp.CompetitionId;
                        ViewData["Use" + nameof(CompetitionClass)] = foundCompId;
                        _viewData["Use" + nameof(CompetitionClass)] = foundCompId;

                        _ = Guid.TryParse(
                            HttpContext.Session.GetString(
                                CompetitionClassLastCreatedAdjudicatorPanelId),
                            out var lastCreatedAdjudicatorPanelId);

                        return await FillCompetitionClassViewModel(
                            dcH,
                            foundComp,
                            new CompetitionClassViewModel()
                            {
                                CompetitionId = foundCompId,
                            },
                            cToken);
                    },
                    cancellationToken);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> CreateNew(
            CompetitionClassViewModel createCompetitionClass,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<CompetitionClass, CompetitionClassViewModel>()
                .SetOnSuccess(
                    nameof(Index))
                .SetOnModelStateInvalid(
                    nameof(ShowCreateEdit))
                .SetOnError(
                    nameof(ShowCreateEdit))
                .SetOnFunc(
                    SetOnEnum.OnModelStateInvalid | SetOnEnum.OnError,
                    async (model, dcH, _, _, cToken) =>
                    {
                        await DefaultGetCompetitionAndSetViewData(
                            dcH,
                            model.CompetitionId,
                            cToken);

                        await FillCompetitionClassViewModel(
                            dcH,
                            model.CompetitionId,
                            model,
                            cToken);

                        return null;
                    })
                .DefaultCreateNewAsync(
                    createCompetitionClass,
                    _mapper.Map<CompetitionClass>(
                        createCompetitionClass),
                    async (dcH, newEntity, _, _, cToken) =>
                    {
                        await dcH.CreateCompetitionClassAsync(
                            newEntity,
                            cToken);

                        HttpContext.Session.SetString(
                            CompetitionClassLastCreatedAdjudicatorPanelId,
                            newEntity.AdjudicatorPanelId.ToString());

                        return new
                        {
                            Id = newEntity.CompetitionId,
                        };
                    },
                    cancellationToken);
        }

        public Task<IActionResult> ShowEdit(
            Guid id,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<CompetitionClass, CompetitionClassViewModel>()
                .SetOnSuccess(
                    nameof(ShowCreateEdit))
                .SetOnNoData(
                    nameof(Index))
                .DefaultShowAsync(
                    id,
                    async (showId, dcH, mapper, _viewData, cToken) =>
                    {
                        var foundCompClass = await dcH.GetCompetitionClassAsync(
                            showId,
                            cToken);

                        if (foundCompClass == null)
                        {
                            return null;
                        }

                        _viewData["Use" + nameof(CompetitionClass)] = foundCompClass.CompetitionId;

                        var ccVm = mapper.Map<CompetitionClassViewModel>(
                            foundCompClass);

                        return await FillCompetitionClassViewModel(
                            dcH,
                            foundCompClass.CompetitionId,
                            ccVm,
                            cToken);
                    },
                    cancellationToken);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> EditSave(
            CompetitionClassViewModel editCompetitionClass,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<CompetitionClass, CompetitionClassViewModel>()
                .SetOnSuccess(
                    nameof(Index))
                .SetOnNoData(
                    nameof(Index))
                .SetOnModelStateInvalid(
                    nameof(ShowCreateEdit))
                .SetOnError(
                    nameof(ShowCreateEdit))
                .SetOnFunc(
                    SetOnEnum.OnError | SetOnEnum.OnModelStateInvalid,
                    async (model, dcH, _, _, cToken) =>
                    {
                        await DefaultGetCompetitionAndSetViewData(
                            dcH,
                            model.CompetitionId,
                            cToken);

                        await FillCompetitionClassViewModel(
                            dcH,
                            model.CompetitionId,
                            model,
                            cToken);

                        return null;
                    })
                .DefaultEditSaveAsync(
                    editCompetitionClass,
                    async (model, dcH, mapper, _, cToken) =>
                    {
                        var foundCompClass = await dcH.GetCompetitionClassAsync(
                            model.CompetitionClassId ?? Guid.Empty,
                            cToken)
                            ?? throw new NoDataFoundException(
                                string.Format(
                                    "{0} with id '{1}' not found!",
                                    nameof(CompetitionClass),
                                    model.CompetitionClassId));

                        // override the values...
                        mapper.Map(
                            model,
                            foundCompClass);

                        // mitigate loops...
                        if (foundCompClass.FollowUpCompetitionClassId == foundCompClass.CompetitionClassId)
                        {
                            foundCompClass.FollowUpCompetitionClassId = null;
                        }

                        return new
                        {
                            Id = model.CompetitionId
                        };
                    },
                    cancellationToken);
        }

        public Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<CompetitionClass, CompetitionClassViewModel>()
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
                        var foundCompClass = await dcH.GetCompetitionClassAsync(
                            delId,
                            cToken)
                            ?? throw new NoDataFoundException(
                                string.Format(
                                    "{0} with id '{1}' not found!",
                                    nameof(CompetitionClass),
                                    delId));

                        await dcH.RemoveCompetitionClassAsync(
                            foundCompClass,
                            cToken);

                        return new
                        {
                            Id = foundCompClass.CompetitionId,
                        };
                    },
                    cancellationToken);
        }

        public Task<IActionResult> ShowMultipleStarters(
            Guid id,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<CompetitionClass, ShowMultipleStartersOverviewViewModel>()
                .SetOnSuccess(
                    nameof(ShowMultipleStarters))
                .SetOnNoData(
                    nameof(Index))
                .DefaultShowAsync(
                    id,
                    async (showId, dcH, mapper, _viewData, cToken) =>
                    {
                        var foundComp = await dcH.FindCompetitionAsync(
                            showId,
                            cToken);

                        if (foundComp == null)
                        {
                            return null;
                        }

                        var foundCompId = foundComp.CompetitionId;
                        _viewData["Use" + nameof(CompetitionClass)] = foundCompId;

                        return new ShowMultipleStartersOverviewViewModel()
                        {
                            Competition = foundComp,
                            OverviewItems = await dcH
                                .GetMultipleStarterAsync(
                                    foundCompId,
                                    cToken)
                                .ToListAsync(
                                    cToken),
                        };
                    },
                    cancellationToken);
        }

        public Task<IActionResult> ShowPossiblePromotions(
            Guid id,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<CompetitionClass, ShowPossiblePromotionsViewModel>()
                .SetOnSuccess(
                    nameof(ShowPossiblePromotions))
                .SetOnNoData(
                    nameof(Index))
                .DefaultShowAsync(
                    id,
                    async (showId, dcH, mapper, _viewData, cToken) =>
                    {
                        var foundComp = await dcH.FindCompetitionAsync(
                            showId,
                            cancellationToken);

                        if (foundComp == null)
                        {
                            return null;
                        }

                        var foundCompId = foundComp.CompetitionId;
                        _viewData["Use" + nameof(CompetitionClass)] = foundCompId;

                        return new ShowPossiblePromotionsViewModel()
                        {
                            Competition = foundComp,
                            OverviewItems = await dcH
                                 .GetParticipantsAsync(
                                     foundCompId,
                                     null,
                                     cToken,
                                     true)
                                 .Where(
                                     x => x.DisplayInfo != null
                                     && x.DisplayInfo.PromotionInfo != null
                                     && x.DisplayInfo.PromotionInfo.PossiblePromotion)
                                 .OrderBy(
                                     x => x.NamePartA)
                                 .ThenBy(
                                     x => x.NamePartB)
                                 .ToListAsync(
                                     cToken),
                            MultipleStarters = await dcH
                                 .GetMultipleStarterAsync(
                                     foundCompId,
                                     cToken)
                                 .ToListAsync(
                                     cToken),
                        };
                    },
                    cancellationToken);
        }

        #region Helper

        public Task<CompetitionClassViewModel> FillCompetitionClassViewModel(
            IDanceCompetitionHelper dcH,
            Competition? foundComp,
            CompetitionClassViewModel useModel,
            CancellationToken cancellationToken)
        {
            return FillCompetitionClassViewModel(
                dcH,
                foundComp?.CompetitionId ?? Guid.Empty,
                useModel,
                cancellationToken);
        }

        public async Task<CompetitionClassViewModel> FillCompetitionClassViewModel(
            IDanceCompetitionHelper dcH,
            Guid foundCompId,
            CompetitionClassViewModel useModel,
            CancellationToken cancellationToken)
        {
            _ = Guid.TryParse(
                HttpContext.Session.GetString(
                    CompetitionClassLastCreatedAdjudicatorPanelId),
                out var lastCreatedAdjudicatorPanelId);

            useModel.AdjudicatorPanels = await dcH
                .GetAdjudicatorPanelsAsync(
                    foundCompId,
                    cancellationToken)
                .ToSelectListItemAsync(
                    lastCreatedAdjudicatorPanelId)
                .ToListAsync(
                    cancellationToken);
            useModel.FollowUpCompetitionClasses = await dcH
                .GetCompetitionClassesAsync(
                    foundCompId,
                    cancellationToken)
                .Where(
                    x => x.CompetitionClassId != useModel.CompetitionClassId)
                .ToSelectListItemAsync(
                    addEmpty: true)
                .ToListAsync(
                    cancellationToken);

            return useModel;
        }

        #endregion Helper

    }
}
