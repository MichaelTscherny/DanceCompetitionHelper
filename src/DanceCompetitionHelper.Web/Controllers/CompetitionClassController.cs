using DanceCompetitionHelper.Web.Extensions;
using DanceCompetitionHelper.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace DanceCompetitionHelper.Web.Controllers
{
    public class CompetitionClassController : Controller
    {
        public const string RefName = "CompetitionClass";

        private readonly IDanceCompetitionHelper _danceCompHelper;
        private readonly ILogger<CompetitionController> _logger;

        public CompetitionClassController(
            IDanceCompetitionHelper danceCompHelper,
            ILogger<CompetitionController> logger)
        {
            _danceCompHelper = danceCompHelper
                ?? throw new ArgumentNullException(
                    nameof(danceCompHelper));
            _logger = logger
                ?? throw new ArgumentNullException(
                    nameof(logger));
        }

        public IActionResult Index(
            Guid id)
        {
            var foundCompId = _danceCompHelper.FindCompetition(
                id);

            ViewData[nameof(CompetitionClassController.ShowMultipleStarters)] = foundCompId;

            return View(
                new CompetitionClassOverviewViewModel()
                {
                    Competition = _danceCompHelper.GetCompetition(
                        foundCompId ?? Guid.Empty),
                    CompetitionClasses = _danceCompHelper
                        .GetCompetitionClasses(
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

            ViewData["BackTo" + RefName] = foundCompId;

            return View(
                new CompetitionClassViewModel()
                {
                    CompetitionId = foundCompId.Value,
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNew(
            CompetitionClassViewModel createCompetition)
        {
            if (ModelState.IsValid == false)
            {
                createCompetition.Errors = ModelState.GetErrorMessages();

                return View(
                    nameof(ShowCreateEdit),
                    createCompetition);
            }

            try
            {
                _danceCompHelper.CreateCompetitionClass(
                    createCompetition.CompetitionId,
                    createCompetition.CompetitionClassName,
                    createCompetition.OrgClassId,
                    createCompetition.Discipline,
                    createCompetition.AgeClass,
                    createCompetition.AgeGroup,
                    createCompetition.Class,
                    createCompetition.MinStartsForPromotion,
                    createCompetition.MinPointsForPromotion,
                    createCompetition.Ignore);

                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        Id = createCompetition.CompetitionId
                    });
            }
            catch (Exception exc)
            {
                createCompetition.Errors = exc.InnerException?.Message ?? exc.Message;

                return View(
                    nameof(ShowCreateEdit),
                    createCompetition);
            }
        }

        public IActionResult ShowEdit(
            Guid id)
        {
            var foundCompClass = _danceCompHelper.GetCompetitionClass(
                id);

            if (foundCompClass == null)
            {
                var helpCompClassId = _danceCompHelper.FindCompetition(
                    id);

                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        Id = helpCompClassId ?? Guid.Empty
                    });
            }

            ViewData["BackTo" + RefName] = foundCompClass.CompetitionId;

            return View(
                nameof(ShowCreateEdit),
                new CompetitionClassViewModel()
                {
                    CompetitionId = foundCompClass.CompetitionId,
                    CompetitionClassId = foundCompClass.CompetitionClassId,
                    CompetitionClassName = foundCompClass.CompetitionClassName,
                    OrgClassId = foundCompClass.OrgClassId,
                    Discipline = foundCompClass.Discipline,
                    AgeClass = foundCompClass.AgeClass,
                    AgeGroup = foundCompClass.AgeGroup,
                    Class = foundCompClass.Class,
                    MinStartsForPromotion = foundCompClass.MinStartsForPromotion,
                    MinPointsForPromotion = foundCompClass.MinPointsForPromotion,
                    Ignore = foundCompClass.Ignore,
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditSave(
            CompetitionClassViewModel editCompetitionClass)
        {
            if (ModelState.IsValid == false)
            {
                editCompetitionClass.Errors = ModelState.GetErrorMessages();

                return View(
                    nameof(ShowCreateEdit),
                    editCompetitionClass);
            }

            try
            {
                _danceCompHelper.EditCompetitionClass(
                    editCompetitionClass.CompetitionClassId ?? Guid.Empty,
                    editCompetitionClass.CompetitionClassName,
                    editCompetitionClass.OrgClassId,
                    editCompetitionClass.Discipline,
                    editCompetitionClass.AgeClass,
                    editCompetitionClass.AgeGroup,
                    editCompetitionClass.Class,
                    editCompetitionClass.MinStartsForPromotion,
                    editCompetitionClass.MinPointsForPromotion,
                    editCompetitionClass.Ignore);

                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        Id = editCompetitionClass.CompetitionId
                    });
            }
            catch (Exception exc)
            {
                editCompetitionClass.Errors = exc.InnerException?.Message ?? exc.Message;

                return View(
                    nameof(ShowCreateEdit),
                    editCompetitionClass);
            }
        }

        public IActionResult Delete(
            Guid id)
        {
            var helpCompClassId = _danceCompHelper.FindCompetition(
                    id);

            _danceCompHelper.RemoveCompetitionClass(
                id);

            return RedirectToAction(
                nameof(Index),
                new
                {
                    Id = helpCompClassId ?? Guid.Empty
                });
        }

        public IActionResult ShowMultipleStarters(
            Guid id)
        {
            var helpCompClassId = _danceCompHelper.FindCompetition(
                    id);

            return View();
        }
    }
}
