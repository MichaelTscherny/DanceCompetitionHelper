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
        public const string ParticipantLastCreatedAdjudicatorPanelId = nameof(ParticipantLastCreatedAdjudicatorPanelId);

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

        public async Task<IActionResult> Index(
            Guid id,
            CancellationToken cancellationToken)
        {
            var foundCompId = (await _danceCompHelper.FindCompetitionAsync(
                id,
                cancellationToken))
                ?.CompetitionId
                ?? throw new NoDataFoundException(
                    string.Format(
                        "{0} with id '{1}' not found!",
                        nameof(Competition),
                        id));

            ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

            return View(
                new AdjudicatorOverviewViewModel()
                {
                    Competition = await _danceCompHelper.GetCompetitionAsync(
                        foundCompId,
                        cancellationToken),
                    OverviewItems = await _danceCompHelper
                        .GetAdjudicatorsAsync(
                            foundCompId,
                            null,
                            cancellationToken)
                        .ToListAsync(
                            cancellationToken),
                });
        }

        public async Task<IActionResult> ShowCreateEdit(
            Guid id,
            CancellationToken cancellationToken)
        {
            var foundComp = await _danceCompHelper.FindCompetitionAsync(
                id,
                cancellationToken);

            if (foundComp == null)
            {
                return NotFound();
            }

            var foundCompId = foundComp.CompetitionId;
            ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

            var helpCompName = string.Empty;

            _ = Guid.TryParse(
                HttpContext.Session.GetString(
                    ParticipantLastCreatedAdjudicatorPanelId),
                out var lastCreatedAdjudicatorPanelId);

            return View(
                new AdjudicatorViewModel()
                {
                    CompetitionId = foundCompId,
                    CompetitionName = foundComp.GetCompetitionName(),
                    AdjudicatorPanels = await _danceCompHelper
                        .GetAdjudicatorPanelsAsync(
                            foundCompId,
                            cancellationToken)
                        .ToSelectListItemAsync(
                            lastCreatedAdjudicatorPanelId)
                        .ToListAsync(
                            cancellationToken),
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNew(
            AdjudicatorViewModel createAdjudicator,
            CancellationToken cancellationToken)
        {
            if (ModelState.IsValid == false)
            {
                createAdjudicator.AddErrors(
                    ModelState);

                createAdjudicator.AdjudicatorPanels = await _danceCompHelper
                    .GetAdjudicatorPanelsAsync(
                        createAdjudicator.CompetitionId,
                        cancellationToken)
                    .ToSelectListItemAsync(
                        createAdjudicator.AdjudicatorPanelId)
                    .ToListAsync(
                        cancellationToken);

                return View(
                    nameof(ShowCreateEdit),
                    createAdjudicator);
            }

            try
            {
                var useAdjudicatorPanelId = createAdjudicator.AdjudicatorPanelId;

                await _danceCompHelper.CreateAdjudicatorAsync(
                    useAdjudicatorPanelId,
                    createAdjudicator.Abbreviation,
                    createAdjudicator.Name,
                    createAdjudicator.Comment,
                    cancellationToken);

                HttpContext.Session.SetString(
                    ParticipantLastCreatedAdjudicatorPanelId,
                    useAdjudicatorPanelId.ToString());

                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        Id = createAdjudicator.CompetitionId,
                    });
            }
            catch (Exception exc)
            {
                createAdjudicator.AddErrors(
                    exc);

                createAdjudicator.AdjudicatorPanels = await _danceCompHelper
                    .GetAdjudicatorPanelsAsync(
                        createAdjudicator.CompetitionId,
                        cancellationToken)
                    .ToSelectListItemAsync(
                        createAdjudicator.AdjudicatorPanelId)
                    .ToListAsync(
                        cancellationToken);

                return View(
                    nameof(ShowCreateEdit),
                    createAdjudicator);
            }
        }

        public async Task<IActionResult> ShowEdit(
            Guid id,
            CancellationToken cancellationToken)
        {
            var foundAdjucator = await _danceCompHelper.GetAdjudicatorAsync(
                id,
                cancellationToken);

            if (foundAdjucator == null)
            {
                return RedirectToAction(
                    nameof(Index));
            }

            var foundComp = await _danceCompHelper.GetCompetitionAsync(
                foundAdjucator.AdjudicatorPanel.CompetitionId,
                cancellationToken);

            if (foundComp == null)
            {
                return RedirectToAction(
                    nameof(Index));
            }

            ViewData["Use" + nameof(CompetitionClass)] = foundComp.CompetitionId;

            return View(
                nameof(ShowCreateEdit),
                new AdjudicatorViewModel()
                {
                    CompetitionId = foundComp.CompetitionId,
                    CompetitionName = foundComp.GetCompetitionName(),
                    AdjudicatorId = foundAdjucator.AdjudicatorId,
                    AdjudicatorPanelId = foundAdjucator.AdjudicatorPanelId,
                    AdjudicatorPanels = await _danceCompHelper
                        .GetAdjudicatorPanelsAsync(
                            foundComp.CompetitionId,
                            cancellationToken)
                        .ToSelectListItemAsync(
                            foundAdjucator.AdjudicatorPanelId)
                        .ToListAsync(
                            cancellationToken),
                    Abbreviation = foundAdjucator.Abbreviation,
                    Name = foundAdjucator.Name,
                    Comment = foundAdjucator.Comment,
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSave(
            AdjudicatorViewModel editAdjudicator,
            CancellationToken cancellationToken)
        {
            if (ModelState.IsValid == false)
            {
                editAdjudicator.AddErrors(
                    ModelState);

                return View(
                    nameof(ShowCreateEdit),
                    editAdjudicator);
            }

            try
            {
                await _danceCompHelper.EditAdjudicatorAsync(
                    editAdjudicator.AdjudicatorId ?? Guid.Empty,
                    editAdjudicator.AdjudicatorPanelId,
                    editAdjudicator.Abbreviation,
                    editAdjudicator.Name,
                    editAdjudicator.Comment,
                    cancellationToken);

                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        Id = editAdjudicator.CompetitionId,
                    });
            }
            catch (Exception exc)
            {
                editAdjudicator.AddErrors(exc);

                return View(
                    nameof(ShowCreateEdit),
                    editAdjudicator);
            }
        }

        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            var helpComp = await _danceCompHelper.FindCompetitionAsync(
                id,
                cancellationToken);

            await _danceCompHelper.RemoveAdjudicatorAsync(
                id,
                cancellationToken);

            return RedirectToAction(
                nameof(Index),
                new
                {
                    Id = helpComp?.CompetitionId ?? Guid.Empty
                });
        }
    }
}
