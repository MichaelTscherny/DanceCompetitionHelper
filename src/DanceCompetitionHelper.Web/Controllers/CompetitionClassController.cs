using AutoMapper;

using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Exceptions;
using DanceCompetitionHelper.Web.Enums;
using DanceCompetitionHelper.Web.Extensions;
using DanceCompetitionHelper.Web.Helper.Request;
using DanceCompetitionHelper.Web.Models.CompetitionClassModels;
using DanceCompetitionHelper.Web.Models.Pdfs;

using Microsoft.AspNetCore.Mvc;

using MigraDoc.DocumentObjectModel;

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

        [HttpGet]
        public Task<IActionResult> Index(
            Guid id,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<CompetitionClass, CompetitionClassOverviewViewModel>()
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

                        return new CompetitionClassOverviewViewModel()
                        {
                            Competition = foundComp,
                            CompetitionVenues = await dcH
                                .GetCompetitionVenuesAsync(
                                    foundComp.CompetitionId,
                                    cToken)
                                .ToListAsync(
                                    cToken),
                            OverviewItems = await dcH
                                .GetCompetitionClassesAsync(
                                    foundComp.CompetitionId,
                                    cToken,
                                    true)
                                .ToListAsync(
                                    cToken),
                        };
                    },
                    cancellationToken);
        }

        [HttpGet]
        public Task<IActionResult> DetailedView(
            Guid id,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<CompetitionClass, CompetitionClassOverviewViewModel>()
                .SetOnSuccess(
                    nameof(Index))
                .SetOnNoData(
                    nameof(Index))
                .DefaultShowAsync(
                    id,
                    async (showId, dcH, _, _viewData, cToken) =>
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

                        return new CompetitionClassOverviewViewModel()
                        {
                            Competition = foundComp,
                            OverviewItems = await dcH
                                .GetCompetitionClassesAsync(
                                    foundComp.CompetitionId,
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

        [HttpGet]
        public Task<IActionResult> ShowCreateEdit(
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
                        var foundComp = await DefaultGetCompetitionAndSetViewDataAsync(
                            dcH,
                            showId,
                            _viewData,
                            cToken);

                        if (foundComp == null)
                        {
                            return null;
                        }

                        _ = Guid.TryParse(
                            HttpContext.Session.GetString(
                                CompetitionClassLastCreatedAdjudicatorPanelId),
                            out var lastCreatedAdjudicatorPanelId);

                        return await FillCompetitionClassViewModel(
                            dcH,
                            foundComp,
                            new CompetitionClassViewModel()
                            {
                                CompetitionId = foundComp.CompetitionId,
                            },
                            cToken);
                    },
                    cancellationToken);
        }

        [HttpPost]
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
                    async (model, dcH, _, _viewData, cToken) =>
                    {
                        await DefaultGetCompetitionAndSetViewDataAsync(
                            dcH,
                            model.CompetitionId,
                            _viewData,
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

        [HttpGet]
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

                        var foundComp = await DefaultGetCompetitionAndSetViewDataAsync(
                            dcH,
                            foundCompClass.CompetitionId,
                            _viewData,
                            cToken);

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
                    SetOnEnum.OnModelStateInvalid | SetOnEnum.OnError,
                    async (model, dcH, _, _viewData, cToken) =>
                    {
                        await DefaultGetCompetitionAndSetViewDataAsync(
                            dcH,
                            model.CompetitionId,
                            _viewData,
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

        [HttpGet]
        public Task<IActionResult> Ignore(
            Guid id,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<CompetitionClass, CompetitionClassViewModel>()
                .SetOnSuccess(
                    nameof(Index))
                .SetOnNoData(
                    nameof(Index))
                .SetOnModelStateInvalid(
                    nameof(Index))
                .SetOnError(
                    nameof(Index))
                .SetOnFunc(
                    SetOnEnum.OnModelStateInvalid | SetOnEnum.OnError,
                    async (model, dcH, _, _viewData, cToken) =>
                    {
                        await DefaultGetCompetitionAndSetViewDataAsync(
                            dcH,
                            model.CompetitionId,
                            _viewData,
                            cToken);

                        await FillCompetitionClassViewModel(
                            dcH,
                            model.CompetitionId,
                            model,
                            cToken);

                        return null;
                    })
                .DefaultEditSaveAsync(
                    new CompetitionClassViewModel()
                    {
                        CompetitionClassId = id,
                    },
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
                        foundCompClass.Ignore = true;

                        return new
                        {
                            Id = foundCompClass.CompetitionId
                        };
                    },
                    cancellationToken);
        }

        [HttpGet]
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

        [HttpGet]
        public Task<IActionResult> ShowMultipleStarters(
            Guid id,
            bool groupedClassesView,
            GroupForViewEnum? groupForView,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<CompetitionClass, ShowMultipleStartersOverviewViewModel>()
                .SetOnSuccess(
                    groupedClassesView
                        ? nameof(ShowMultipleStartersDependentClassesView)
                        : nameof(ShowMultipleStarters))
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

                        return new ShowMultipleStartersOverviewViewModel()
                        {
                            Competition = foundComp,
                            OverviewItems = await dcH
                                .GetMultipleStarterAsync(
                                    foundComp.CompetitionId,
                                    cToken)
                                .ToListAsync(
                                    cToken),
                            DependentClassesView = groupedClassesView,
                            GroupForView = groupForView ?? GroupForViewEnum.None,
                        };
                    },
                    cancellationToken);
        }

        [HttpGet]
        public Task<IActionResult> PdfMultipleStarters(
        PdfViewModel pdf,
        CancellationToken cancellationToken)
        {
            return GetPdfDocumentHelper()
                .GetMultipleStarters(
                    pdf,
                    cancellationToken);
        }

        [HttpGet]
        public Task<IActionResult> ShowMultipleStartersDependentClassesView(
            Guid id,
            GroupForViewEnum? groupForView,
            CancellationToken cancellationToken)
        {
            return ShowMultipleStarters(
                id,
                true,
                groupForView,
                cancellationToken);
        }


        [HttpGet]
        public Task<IActionResult> PdfMultipleStartersDependentClassesView(
            PdfViewModel pdf,
            CancellationToken cancellationToken)
        {
            return GetPdfDocumentHelper()
                .GetMultipleStartersDependentClassesView(
                    pdf,
                    cancellationToken);
        }

        [HttpGet]
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
                        var foundComp = await DefaultGetCompetitionAndSetViewDataAsync(
                            dcH,
                            showId,
                            _viewData,
                            cToken);

                        if (foundComp == null)
                        {
                            return null;
                        }

                        return new ShowPossiblePromotionsViewModel()
                        {
                            Competition = foundComp,
                            OverviewItems = await dcH
                                 .GetParticipantsAsync(
                                     foundComp.CompetitionId,
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
                                     foundComp.CompetitionId,
                                     cToken)
                                 .ToListAsync(
                                     cToken),
                        };
                    },
                    cancellationToken);
        }

        [HttpGet]
        public Task<IActionResult> PdfPossiblePromotions(
            PdfViewModel pdf,
            CancellationToken cancellationToken)
        {
            return GetPdfDocumentHelper()
                .GetPossiblePromotions(
                    pdf,
                    cancellationToken);
        }

        [HttpGet]
        public Task<IActionResult> PdfCompetitionClasses(
            PdfViewModel pdf,
            CancellationToken cancellationToken)
        {
            // CAUTION: only "A4" implemented yet!..
            pdf.PageFormat = PageFormat.A4;
            pdf.PageOrientation = Orientation.Landscape;

            return GetPdfDocumentHelper()
                .GetCompetitionClasses(
                    pdf,
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
