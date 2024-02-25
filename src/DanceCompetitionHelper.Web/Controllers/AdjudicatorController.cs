using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Web.Extensions;
using DanceCompetitionHelper.Web.Models.AdjudicatorModels;
using Microsoft.AspNetCore.Mvc;

namespace DanceCompetitionHelper.Web.Controllers
{
    public class AdjudicatorController : ControllerBase
    {
        public const string RefName = "Adjudicator";
        public const string ParticipantLastCreatedAdjudicatorPanelId = nameof(ParticipantLastCreatedAdjudicatorPanelId);

        private readonly IDanceCompetitionHelper _danceCompHelper;
        private readonly ILogger<AdjudicatorController> _logger;
        private readonly IServiceProvider _serviceProvider;

        public AdjudicatorController(
            IDanceCompetitionHelper danceCompHelper,
            ILogger<AdjudicatorController> logger,
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

        public async Task<IActionResult> Index(
            Guid id,
            CancellationToken cancellationToken)
        {
            var foundCompId = await _danceCompHelper.FindCompetitionAsync(
                id,
                cancellationToken);

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
            var foundCompId = await _danceCompHelper.FindCompetitionAsync(
                id,
                cancellationToken);

            if (foundCompId == null)
            {
                return NotFound();
            }

            var foundComp = await _danceCompHelper.GetCompetitionAsync(
                foundCompId,
                cancellationToken);

            ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

            var helpCompName = string.Empty;

            _ = Guid.TryParse(
                HttpContext.Session.GetString(
                    ParticipantLastCreatedAdjudicatorPanelId),
                out var lastCreatedAdjudicatorPanelId);

            return View(
                new AdjudicatorViewModel()
                {
                    CompetitionId = foundCompId.Value,
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
            var helpCompId = await _danceCompHelper.FindCompetitionAsync(
                id,
                cancellationToken);

            await _danceCompHelper.RemoveAdjudicatorAsync(
                id,
                cancellationToken);

            return RedirectToAction(
                nameof(Index),
                new
                {
                    Id = helpCompId ?? Guid.Empty
                });
        }
    }
}
