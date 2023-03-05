using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Web.Extensions;
using DanceCompetitionHelper.Web.Models.AdjudicatorModels;
using Microsoft.AspNetCore.Mvc;

namespace DanceCompetitionHelper.Web.Controllers
{
    public class AdjudicatorController : Controller
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

        public IActionResult Index(
            Guid id)
        {
            var foundCompId = _danceCompHelper.FindCompetition(
                id);
            var foundCompClassId = _danceCompHelper.FindCompetitionClass(
                id);

            ViewData["BackTo" + CompetitionClassController.RefName] = foundCompId;
            ViewData[nameof(CompetitionClassController.ShowMultipleStarters)] = foundCompId;
            ViewData[nameof(CompetitionClassController.ShowPossiblePromotions)] = foundCompId;
            ViewData["Show" + AdjudicatorPanelController.RefName] = foundCompId;
            ViewData["Show" + AdjudicatorController.RefName] = foundCompId;

            return View(
                new AdjudicatorOverviewViewModel()
                {
                    Competition = _danceCompHelper.GetCompetition(
                        foundCompId),
                    OverviewItems = _danceCompHelper
                        .GetAdjudicators(
                            foundCompId,
                            null)
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

            ViewData["Show" + ParticipantController.RefName] = foundCompId;
            ViewData["BackTo" + CompetitionClassController.RefName] = foundCompId;
            ViewData[nameof(CompetitionClassController.ShowMultipleStarters)] = foundCompId;
            ViewData[nameof(CompetitionClassController.ShowPossiblePromotions)] = foundCompId;
            ViewData["Show" + AdjudicatorPanelController.RefName] = foundCompId;
            ViewData["Show" + AdjudicatorController.RefName] = foundCompId;

            var helpCompName = string.Empty;

            Guid.TryParse(
                HttpContext.Session.GetString(
                    ParticipantLastCreatedAdjudicatorPanelId),
                out var lastCreatedAdjudicatorPanelId);

            return View(
                new AdjudicatorViewModel()
                {
                    CompetitionId = foundCompId.Value,
                    CompetitionName = foundComp.GetCompetitionName(),
                    AdjudicatorPanels = _danceCompHelper
                        .GetAdjudicatorPanels(
                            foundCompId)
                        .ToSelectListItem(
                            lastCreatedAdjudicatorPanelId),
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNew(
            AdjudicatorViewModel createAdjudicator)
        {
            if (ModelState.IsValid == false)
            {
                createAdjudicator.Errors = ModelState.GetErrorMessages();

                createAdjudicator.AdjudicatorPanels = _danceCompHelper
                    .GetAdjudicatorPanels(
                        createAdjudicator.CompetitionId)
                    .ToSelectListItem(
                        createAdjudicator.AdjudicatorPanelId);

                return View(
                    nameof(ShowCreateEdit),
                    createAdjudicator);
            }

            try
            {
                var useAdjudicatorPanelId = createAdjudicator.AdjudicatorPanelId;

                _danceCompHelper.CreateAdjudicator(
                    useAdjudicatorPanelId,
                    createAdjudicator.Abbreviation,
                    createAdjudicator.Name,
                    createAdjudicator.Comment);

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
                createAdjudicator.Errors = exc.InnerException?.Message ?? exc.Message;

                createAdjudicator.AdjudicatorPanels = _danceCompHelper
                    .GetAdjudicatorPanels(
                        createAdjudicator.CompetitionId)
                    .ToSelectListItem(
                        createAdjudicator.AdjudicatorPanelId);

                return View(
                    nameof(ShowCreateEdit),
                    createAdjudicator);
            }
        }

        public IActionResult ShowEdit(
            Guid id)
        {
            var foundAdjucator = _danceCompHelper.GetAdjudicator(
                id);

            if (foundAdjucator == null)
            {
                return RedirectToAction(
                    nameof(Index));
            }

            var foundComp = _danceCompHelper.GetCompetition(
                foundAdjucator.AdjudicatorPanel.CompetitionId);

            if (foundComp == null)
            {
                return RedirectToAction(
                    nameof(Index));
            }

            return View(
                nameof(ShowCreateEdit),
                new AdjudicatorViewModel()
                {
                    CompetitionId = foundComp.CompetitionId,
                    CompetitionName = foundComp.GetCompetitionName(),
                    AdjudicatorId = foundAdjucator.AdjudicatorId,
                    AdjudicatorPanelId = foundAdjucator.AdjudicatorPanelId,
                    AdjudicatorPanels = _danceCompHelper
                        .GetAdjudicatorPanels(
                            foundComp.CompetitionId)
                        .ToSelectListItem(
                            foundAdjucator.AdjudicatorPanelId),
                    Abbreviation = foundAdjucator.Abbreviation,
                    Name = foundAdjucator.Name,
                    Comment = foundAdjucator.Comment,
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditSave(
            AdjudicatorViewModel editAdjudicator)
        {
            if (ModelState.IsValid == false)
            {
                editAdjudicator.Errors = ModelState.GetErrorMessages();

                return View(
                    nameof(ShowCreateEdit),
                    editAdjudicator);
            }

            try
            {
                _danceCompHelper.EditAdjudicator(
                    editAdjudicator.AdjudicatorId ?? Guid.Empty,
                    editAdjudicator.AdjudicatorPanelId,
                    editAdjudicator.Abbreviation,
                    editAdjudicator.Name,
                    editAdjudicator.Comment);

                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        Id = editAdjudicator.CompetitionId,
                    });
            }
            catch (Exception exc)
            {
                editAdjudicator.Errors = exc.InnerException?.Message ?? exc.Message;

                return View(
                    nameof(ShowCreateEdit),
                    editAdjudicator);
            }
        }

        public IActionResult Delete(
            Guid id)
        {
            var helpCompId = _danceCompHelper.FindCompetition(
                    id);

            _danceCompHelper.RemoveAdjudicator(
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
