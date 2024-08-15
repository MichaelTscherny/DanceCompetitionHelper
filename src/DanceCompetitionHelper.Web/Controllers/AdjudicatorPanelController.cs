using AutoMapper;
using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Exceptions;
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

                    return new AdjudicatorPanelOverviewViewModel()
                    {
                        Competition = foundComp,
                        OverviewItems = await _danceCompHelper
                            .GetAdjudicatorPanelsAsync(
                                foundCompId,
                                cToken,
                                true)
                            .ToListAsync(),
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

                    return new AdjudicatorPanelViewModel()
                    {
                        CompetitionId = foundCompId,
                        CompetitionName = foundComp.GetCompetitionName(),
                    };
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
            AdjudicatorPanelViewModel createAdjudicatorPanel,
            CancellationToken cancellationToken)
        {
            return DefaultCreateNew(
                createAdjudicatorPanel,
                _mapper.Map<AdjudicatorPanel>(
                    createAdjudicatorPanel),
                // --
                null,
                nameof(ShowCreateEdit),
                // --
                async (dcH, newEntity, mapper, cToken) =>
                {
                    await dcH.CreateAdjudicatorPanelAsync(
                        newEntity,
                        cToken);

                    return new
                    {
                        Id = createAdjudicatorPanel.CompetitionId,
                    };
                },
                // -- on success
                nameof(Index),
                // -- on error
                null,
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
                    var foundAdjPanel = await _danceCompHelper.GetAdjudicatorPanelAsync(
                        id,
                        cToken,
                        true);

                    if (foundAdjPanel == null)
                    {
                        return null;
                    }

                    ViewData["Use" + nameof(CompetitionClass)] = foundAdjPanel.CompetitionId;

                    return new AdjudicatorPanelViewModel()
                    {
                        CompetitionId = foundAdjPanel.CompetitionId,
                        CompetitionName = foundAdjPanel.Competition.GetCompetitionName(),
                        AdjudicatorPanelId = foundAdjPanel.AdjudicatorPanelId,
                        Name = foundAdjPanel.Name,
                        Comment = foundAdjPanel.Comment,
                    };
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
            AdjudicatorPanelViewModel editAdjudicatorPanel,
            CancellationToken cancellationToken)
        {
            return DefaultEdit(
                editAdjudicatorPanel,
                // --
                null,
                nameof(ShowCreateEdit),
                // -- on success
                async (dcH, mapper, cToken) =>
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
                nameof(Index),
                // -- on no data
                nameof(Index),
                // -- on error
                async (dcH, model, cToken) =>
                {
                    var foundAdjPanel = await dcH.GetAdjudicatorPanelAsync(
                        model.CompetitionId,
                        cToken);

                    if (foundAdjPanel == null)
                    {
                        return;
                    }

                    ViewData["Use" + nameof(CompetitionClass)] = foundAdjPanel.CompetitionId;
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
                // --
                nameof(Index),
                nameof(Index),
                nameof(Index),
                // --
                cancellationToken);
        }
    }
}
