using AutoMapper;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Exceptions;
using DanceCompetitionHelper.Web.Extensions;
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

        public Task<IActionResult> Index(
            Guid id,
            CancellationToken cancellationToken)
        {
            return DefaultIndexAndShow(
                async (dcH, mapper, cToken) =>
                {
                    var foundComp = await dcH.FindCompetitionAsync(
                        id,
                        cToken);

                    if (foundComp == null)
                    {
                        return null;
                    }

                    var foundCompId = foundComp.CompetitionId;
                    ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

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
                // --
                nameof(Index),
                nameof(Index),
                // --
                cancellationToken);
        }

        public Task<IActionResult> DetailedView(
            Guid id,
            CancellationToken cancellationToken)
        {
            return DefaultIndexAndShow(
                async (dcH, mapper, cToken) =>
                {
                    var foundComp = await dcH.FindCompetitionAsync(
                        id,
                        cToken);

                    if (foundComp == null)
                    {
                        return null;
                    }

                    var foundCompId = foundComp.CompetitionId;
                    ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

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
                            cancellationToken),
                        DetailedView = true,
                    };
                },
                // --
                nameof(Index),
                nameof(Index),
                // --
                cancellationToken);
        }

        public Task<IActionResult> ShowCreateEdit(
            Guid id,
            CancellationToken cancellationToken)
        {
            return DefaultIndexAndShow(
                async (dcH, mapper, cToken) =>
                {
                    var foundComp = await dcH.FindCompetitionAsync(
                        id,
                        cToken);

                    if (foundComp == null)
                    {
                        return null;
                    }

                    var foundCompId = foundComp.CompetitionId;
                    ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

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
                // --
                nameof(ShowCreateEdit),
                nameof(Index),
                // --
                cancellationToken);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> CreateNew(
            CompetitionClassViewModel createCompetitionClass,
            CancellationToken cancellationToken)
        {
            return DefaultCreateNew(
                createCompetitionClass,
                _mapper.Map<CompetitionClass>(
                    createCompetitionClass),
                // --
                async (dcH, cToken) =>
                {
                    await DefaultGetCompetitionAndSetViewData(
                        dcH,
                        createCompetitionClass.CompetitionId,
                        cToken);

                    await FillCompetitionClassViewModel(
                        dcH,
                        createCompetitionClass.CompetitionId,
                        createCompetitionClass,
                        cToken);
                },
                nameof(ShowCreateEdit),
                // --
                async (dcH, newEntity, mapper, cToken) =>
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
                // -- on success
                nameof(Index),
                // -- on error
                async (dcH, model, cToken) =>
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
                },
                nameof(ShowCreateEdit),
                // --
                cancellationToken);
        }

        public Task<IActionResult> ShowEdit(
            Guid id,
            CancellationToken cancellationToken)
        {
            return DefaultIndexAndShow(
                async (dcH, mapper, cToken) =>
                {
                    var foundCompClass = await dcH.GetCompetitionClassAsync(
                        id,
                        cToken);

                    if (foundCompClass == null)
                    {
                        return null;
                    }

                    ViewData["Use" + nameof(CompetitionClass)] = foundCompClass.CompetitionId;

                    var ccVm = mapper.Map<CompetitionClassViewModel>(
                        foundCompClass);

                    return await FillCompetitionClassViewModel(
                        dcH,
                        foundCompClass.CompetitionId,
                        ccVm,
                        cToken);
                },
                // --
                nameof(ShowCreateEdit),
                nameof(Index),
                // --
                cancellationToken);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> EditSave(
            CompetitionClassViewModel editCompetitionClass,
            CancellationToken cancellationToken)
        {
            return DefaultEdit(
                editCompetitionClass,
                // --
                async (dcH, cToken) =>
                {
                    await DefaultGetCompetitionAndSetViewData(
                        dcH,
                        editCompetitionClass.CompetitionId,
                        cToken);

                    await FillCompetitionClassViewModel(
                        dcH,
                        editCompetitionClass.CompetitionId,
                        editCompetitionClass,
                        cToken);
                },
                nameof(ShowCreateEdit),
                // -- 
                async (dcH, mapper, cToken) =>
                {
                    var foundCompClass = await dcH.GetCompetitionClassAsync(
                        editCompetitionClass.CompetitionClassId ?? Guid.Empty,
                        cToken)
                        ?? throw new NoDataFoundException(
                            string.Format(
                                "{0} with id '{1}' not found!",
                                nameof(CompetitionClass),
                                editCompetitionClass.CompetitionClassId));

                    // override the values...
                    mapper.Map(
                        editCompetitionClass,
                        foundCompClass);

                    return new
                    {
                        Id = editCompetitionClass.CompetitionId
                    };
                },
                // -- on success
                nameof(Index),
                // -- on no data
                nameof(Index),
                // -- on error
                async (dcH, model, cToken) =>
                {
                    var foundComp = await DefaultGetCompetitionAndSetViewData(
                        dcH,
                        model.CompetitionId,
                        cToken);

                    await FillCompetitionClassViewModel(
                        dcH,
                        foundComp,
                        model,
                        cToken);
                },
                nameof(ShowCreateEdit),
                // --
                cancellationToken);
        }

        public Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            return DefaultDelete(
                id,
                // --
                async (dcH, delId, cToken) =>
                {
                    var foundCompClass = await dcH.GetCompetitionClassAsync(
                        id,
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
                // --
                nameof(Index),
                nameof(Index),
                nameof(Index),
                // --
                cancellationToken);
        }

        public Task<IActionResult> ShowMultipleStarters(
            Guid id,
            CancellationToken cancellationToken)
        {
            return DefaultIndexAndShow(
                async (dcH, mapper, cToken) =>
                {
                    var foundComp = await dcH.FindCompetitionAsync(
                        id,
                        cToken);

                    if (foundComp == null)
                    {
                        return null;
                    }

                    var foundCompId = foundComp.CompetitionId;
                    ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

                    return new ShowMultipleStartersOverviewViewModel()
                    {
                        Competition = foundComp,
                        OverviewItems = await dcH
                            .GetMultipleStarterAsync(
                                foundCompId,
                                cToken,
                                useTransaction: false)
                            .ToListAsync(
                                cToken),
                    };
                },
                // --
                nameof(ShowMultipleStarters),
                nameof(Index),
                // --
                cancellationToken);
        }

        public Task<IActionResult> ShowPossiblePromotions(
            Guid id,
            CancellationToken cancellationToken)
        {
            return DefaultIndexAndShow(
               async (dcH, mapper, cToken) =>
               {
                   var foundComp = await dcH.FindCompetitionAsync(
                       id,
                       cancellationToken);

                   if (foundComp == null)
                   {
                       return null;
                   }

                   var foundCompId = foundComp.CompetitionId;
                   ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

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
                                cToken,
                                useTransaction: false)
                            .ToListAsync(
                                cToken),
                   };
               },
               // --
               nameof(ShowPossiblePromotions),
               nameof(Index),
               // --
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
                    cancellationToken,
                    useTransaction: false)
                .ToSelectListItemAsync(
                    lastCreatedAdjudicatorPanelId)
                .ToListAsync(
                    cancellationToken);
            useModel.FollowUpCompetitionClasses = await dcH
                .GetCompetitionClassesAsync(
                    foundCompId,
                    cancellationToken)
                .ToSelectListItemAsync(
                    addEmpty: true)
                .ToListAsync(
                    cancellationToken);

            return useModel;
        }

        #endregion Helper

    }
}
