using AutoMapper;
using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Exceptions;
using DanceCompetitionHelper.Web.Extensions;
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

        public Task<IActionResult> Index(
            Guid id,
            CancellationToken cancellationToken)
        {
            return DefaultIndexAndShow(
                async (dcH, mapper, cToken) =>
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
                    ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

                    return
                        new AdjudicatorOverviewViewModel()
                        {
                            Competition = await _danceCompHelper.GetCompetitionAsync(
                                foundCompId,
                                cToken),
                            OverviewItems = await _danceCompHelper
                                .GetAdjudicatorsAsync(
                                    foundCompId,
                                    null,
                                    cToken)
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

        public Task<IActionResult> ShowCreateEdit(
            Guid id,
            CancellationToken cancellationToken)
        {
            return DefaultIndexAndShow(
                async (dcH, mapper, cToken) =>
                {
                    var foundComp = await _danceCompHelper.FindCompetitionAsync(
                        id,
                        cancellationToken);

                    if (foundComp == null)
                    {
                        return null;
                    }

                    var foundCompId = foundComp.CompetitionId;
                    ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

                    return await FillAdjudicatorViewModel(
                        dcH,
                        foundComp,
                        new AdjudicatorViewModel()
                        {
                            CompetitionId = foundCompId,
                            CompetitionName = foundComp.GetCompetitionName(),
                        },
                        cToken);
                },
                // --
                nameof(ShowCreateEdit),
                nameof(ShowCreateEdit),
                // --
                cancellationToken);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> CreateNew(
            AdjudicatorViewModel createAdjudicator,
            CancellationToken cancellationToken)
        {
            return DefaultCreateNew(
                createAdjudicator,
                _mapper.Map<Adjudicator>(
                    createAdjudicator),
                // --
                async (dcH, cToken) =>
                {
                    await DefaultGetCompetitionAndSetViewData(
                        dcH,
                        createAdjudicator.CompetitionId,
                        cToken);

                    await FillAdjudicatorViewModel(
                        dcH,
                        createAdjudicator.CompetitionId,
                        createAdjudicator,
                        cToken);
                },
                nameof(ShowCreateEdit),
                // --
                async (dcH, newEntity, mapper, cToken) =>
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
                // -- on success
                nameof(Index),
                //
                async (dcH, model, cToken) =>
                {
                    var foundComp = await DefaultGetCompetitionAndSetViewData(
                        dcH,
                        model.CompetitionId,
                        cToken);

                    await FillAdjudicatorViewModel(
                        dcH,
                        foundComp,
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
                    var foundAdjucator = await _danceCompHelper.GetAdjudicatorAsync(
                        id,
                        cancellationToken);

                    if (foundAdjucator == null)
                    {
                        return null;
                    }

                    var foundComp = await _danceCompHelper.GetCompetitionAsync(
                        foundAdjucator.AdjudicatorPanel.CompetitionId,
                        cancellationToken);

                    if (foundComp == null)
                    {
                        return null;
                    }

                    ViewData["Use" + nameof(CompetitionClass)] = foundComp.CompetitionId;

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
                // -- on no data
                nameof(ShowCreateEdit),
                nameof(Index),
                // --
                cancellationToken);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> EditSave(
            AdjudicatorViewModel editAdjudicator,
            CancellationToken cancellationToken)
        {
            return DefaultEdit(
                editAdjudicator,
                // --
                null,
                nameof(ShowCreateEdit),
                // -- on success
                async (dcH, mapper, cToken) =>
                {
                    var foundAdjPanel = await dcH.GetAdjudicatorAsync(
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
                        foundAdjPanel);

                    return new
                    {
                        Id = editAdjudicator.CompetitionId,
                    };
                },
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

                    await FillAdjudicatorViewModel(
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
                async (dcH, delId, cToken) =>
                {
                    var helpComp = await _danceCompHelper.FindCompetitionAsync(
                        id,
                        cancellationToken);

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
                // --
                nameof(Index),
                nameof(Index),
                nameof(Index),
                // --
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
