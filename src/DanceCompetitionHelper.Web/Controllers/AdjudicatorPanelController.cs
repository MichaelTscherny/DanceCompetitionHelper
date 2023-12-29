using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Web.Extensions;
using DanceCompetitionHelper.Web.Models.AdjudicatorPanelModels;
using Microsoft.AspNetCore.Mvc;

namespace DanceCompetitionHelper.Web.Controllers
{
    public class AdjudicatorPanelController : Controller
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

        public IActionResult Index(
            Guid id)
        {
            var foundCompId = _danceCompHelper.FindCompetition(
                id);

            ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

            return View(
                new AdjudicatorPanelOverviewViewModel()
                {
                    Competition = _danceCompHelper.GetCompetition(
                        foundCompId),
                    OverviewItems = _danceCompHelper
                        .GetAdjudicatorPanels(
                            foundCompId,
                            true)
                        .ToList(),
                });
        }

        public IActionResult ShowCreateEdit(
            Guid id)
        {
            var foundCompId = _danceCompHelper.FindCompetition(
                id);

            if (foundCompId == null)
            {
                return NotFound();
            }

            var foundComp = _danceCompHelper.GetCompetition(
                foundCompId);

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
        public IActionResult CreateNew(
            AdjudicatorPanelViewModel createAdjudicatorPanel)
        {
            if (ModelState.IsValid == false)
            {
                createAdjudicatorPanel.Errors = ModelState.GetErrorMessages();

                return View(
                    nameof(ShowCreateEdit),
                    createAdjudicatorPanel);
            }

            try
            {
                var useCompetitionId = createAdjudicatorPanel.CompetitionId;

                _danceCompHelper.CreateAdjudicatorPanel(
                     createAdjudicatorPanel.CompetitionId,
                     createAdjudicatorPanel.Name,
                     createAdjudicatorPanel.Comment);

                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        Id = useCompetitionId,
                    });
            }
            catch (Exception exc)
            {
                createAdjudicatorPanel.Errors = exc.InnerException?.Message ?? exc.Message;

                return View(
                    nameof(ShowCreateEdit),
                    createAdjudicatorPanel);
            }
        }

        public IActionResult ShowEdit(
            Guid id)
        {
            var foundAdjPanel = _danceCompHelper.GetAdjudicatorPanel(
                id);

            if (foundAdjPanel == null)
            {
                return RedirectToAction(
                    nameof(Index));
            }

            var foundComp = _danceCompHelper.GetCompetition(
                foundAdjPanel.CompetitionId);

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
        public IActionResult EditSave(
            AdjudicatorPanelViewModel editParticipant)
        {
            if (ModelState.IsValid == false)
            {
                editParticipant.Errors = ModelState.GetErrorMessages();

                return View(
                    nameof(ShowCreateEdit),
                    editParticipant);
            }

            try
            {
                var useCompetitionId = editParticipant.CompetitionId;

                _danceCompHelper.EditAdjudicatorPanel(
                    editParticipant.AdjudicatorPanelId ?? Guid.Empty,
                    useCompetitionId,
                    editParticipant.Name,
                    editParticipant.Comment);

                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        Id = useCompetitionId,
                    });
            }
            catch (Exception exc)
            {
                editParticipant.Errors = exc.InnerException?.Message ?? exc.Message;

                return View(
                    nameof(ShowCreateEdit),
                    editParticipant);
            }
        }

        public IActionResult Delete(
            Guid id)
        {
            var helpCompId = _danceCompHelper.FindCompetition(
                id);

            _danceCompHelper.RemoveAdjudicatorPanel(
                id);

            return RedirectToAction(
                nameof(Index),
                new
                {
                    Id = helpCompId ?? Guid.Empty
                });
        }
    }
}
