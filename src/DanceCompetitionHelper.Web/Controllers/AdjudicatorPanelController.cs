using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Web.Models.AdjudicatorPanelModels;
using Microsoft.AspNetCore.Mvc;

namespace DanceCompetitionHelper.Web.Controllers
{
    public class AdjudicatorPanelController : ControllerBase
    {
        public const string RefName = "AdjudicatorPanel";

        private readonly IDanceCompetitionHelper _danceCompHelper;
        private readonly ILogger<AdjudicatorPanelController> _logger;
        private readonly IServiceProvider _serviceProvider;

        public AdjudicatorPanelController(
            IDanceCompetitionHelper danceCompHelper,
            ILogger<AdjudicatorPanelController> logger,
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
                new AdjudicatorPanelOverviewViewModel()
                {
                    Competition = await _danceCompHelper.GetCompetitionAsync(
                        foundCompId,
                        cancellationToken),
                    OverviewItems = await _danceCompHelper
                        .GetAdjudicatorPanelsAsync(
                            foundCompId,
                            cancellationToken,
                            true)
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

            return View(
                new AdjudicatorPanelViewModel()
                {
                    CompetitionId = foundCompId.Value,
                    CompetitionName = foundComp.GetCompetitionName(),
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNew(
            AdjudicatorPanelViewModel createAdjudicatorPanel,
            CancellationToken cancellationToken)
        {
            if (ModelState.IsValid == false)
            {
                createAdjudicatorPanel.AddErrors(
                    ModelState);

                return View(
                    nameof(ShowCreateEdit),
                    createAdjudicatorPanel);
            }

            try
            {
                var useCompetitionId = createAdjudicatorPanel.CompetitionId;

                await _danceCompHelper.CreateAdjudicatorPanelAsync(
                     createAdjudicatorPanel.CompetitionId,
                     createAdjudicatorPanel.Name,
                     createAdjudicatorPanel.Comment,
                     cancellationToken);

                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        Id = useCompetitionId,
                    });
            }
            catch (Exception exc)
            {
                createAdjudicatorPanel.AddErrors(
                    exc);

                return View(
                    nameof(ShowCreateEdit),
                    createAdjudicatorPanel);
            }
        }

        public async Task<IActionResult> ShowEdit(
            Guid id,
            CancellationToken cancellationToken)
        {
            var foundAdjPanel = await _danceCompHelper.GetAdjudicatorPanelAsync(
                id,
                cancellationToken);

            if (foundAdjPanel == null)
            {
                return RedirectToAction(
                    nameof(Index));
            }

            var foundComp = await _danceCompHelper.GetCompetitionAsync(
                foundAdjPanel.CompetitionId,
                cancellationToken);

            ViewData["Use" + nameof(CompetitionClass)] = foundAdjPanel.CompetitionId;

            return View(
                nameof(ShowCreateEdit),
                new AdjudicatorPanelViewModel()
                {
                    CompetitionId = foundAdjPanel.CompetitionId,
                    CompetitionName = foundComp.GetCompetitionName(),
                    AdjudicatorPanelId = foundAdjPanel.AdjudicatorPanelId,
                    Name = foundAdjPanel.Name,
                    Comment = foundAdjPanel.Comment,
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSave(
            AdjudicatorPanelViewModel editParticipant,
            CancellationToken cancellationToken)
        {
            if (ModelState.IsValid == false)
            {
                editParticipant.AddErrors(
                    ModelState);

                return View(
                    nameof(ShowCreateEdit),
                    editParticipant);
            }

            try
            {
                var useCompetitionId = editParticipant.CompetitionId;

                await _danceCompHelper.EditAdjudicatorPanelAsync(
                    editParticipant.AdjudicatorPanelId ?? Guid.Empty,
                    useCompetitionId,
                    editParticipant.Name,
                    editParticipant.Comment,
                    cancellationToken);

                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        Id = useCompetitionId,
                    });
            }
            catch (Exception exc)
            {
                editParticipant.AddErrors(
                    exc);

                return View(
                    nameof(ShowCreateEdit),
                    editParticipant);
            }
        }

        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            var helpCompId = await _danceCompHelper.FindCompetitionAsync(
                id,
                cancellationToken);

            await _danceCompHelper.RemoveAdjudicatorPanelAsync(
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
