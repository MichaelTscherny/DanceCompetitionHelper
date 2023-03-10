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

            ViewData["Show" + ParticipantController.RefName] = foundCompId;
            ViewData[nameof(CompetitionClassController.ShowMultipleStarters)] = foundCompId;
            ViewData[nameof(CompetitionClassController.ShowPossiblePromotions)] = foundCompId;

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

        public IActionResult DetailedView(
            Guid id)
        {
            var foundCompId = _danceCompHelper.FindCompetition(
                id);

            ViewData["Show" + ParticipantController.RefName] = foundCompId;
            ViewData[nameof(CompetitionClassController.ShowMultipleStarters)] = foundCompId;
            ViewData[nameof(CompetitionClassController.ShowPossiblePromotions)] = foundCompId;

            return View(
                nameof(Index),
                new CompetitionClassOverviewViewModel()
                {
                    Competition = _danceCompHelper.GetCompetition(
                        foundCompId ?? Guid.Empty),
                    CompetitionClasses = _danceCompHelper
                        .GetCompetitionClasses(
                            foundCompId,
                            true)
                        .ToList(),
                    DetailedView = true,
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

            ViewData["Show" + ParticipantController.RefName] = foundCompId;
            ViewData["BackTo" + CompetitionClassController.RefName] = foundCompId;

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
                    createCompetition.PointsForFirst,
                    createCompetition.ExtraManualStarter,
                    createCompetition.Comment,
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

            ViewData["Show" + ParticipantController.RefName] = foundCompClass.CompetitionId;
            ViewData["BackTo" + CompetitionClassController.RefName] = foundCompClass.CompetitionId;

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
                    PointsForFirst = foundCompClass.PointsForFirst,
                    ExtraManualStarter = foundCompClass.ExtraManualStarter,
                    Comment = foundCompClass.Comment,
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
                    editCompetitionClass.PointsForFirst,
                    editCompetitionClass.ExtraManualStarter,
                    editCompetitionClass.Comment,
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
            var foundCompId = _danceCompHelper.FindCompetition(
                    id);

            if (foundCompId == null)
            {
                return NotFound();
            }

            var helpComp = _danceCompHelper.GetCompetition(
                foundCompId.Value);

            ViewData["Show" + ParticipantController.RefName] = foundCompId;
            ViewData["BackTo" + CompetitionClassController.RefName] = foundCompId;
            ViewData[nameof(CompetitionClassController.ShowPossiblePromotions)] = foundCompId;

            return View(
                new ShowMultipleStartersOverviewViewModel()
                {
                    Competition = helpComp,
                    MultipleStarters = _danceCompHelper
                        .GetMultipleStarter(
                            foundCompId.Value)
                        .ToList(),
                });
        }

        public IActionResult ShowPossiblePromotions(
            Guid id)
        {
            var foundCompId = _danceCompHelper.FindCompetition(
                id);

            if (foundCompId == null)
            {
                return NotFound();
            }

            var helpComp = _danceCompHelper.GetCompetition(
                foundCompId.Value);

            ViewData["Show" + ParticipantController.RefName] = foundCompId;
            ViewData["BackTo" + CompetitionClassController.RefName] = foundCompId;
            ViewData[nameof(CompetitionClassController.ShowMultipleStarters)] = foundCompId;

            return View(
                new ShowPossiblePromotionsViewModel()
                {
                    Competition = helpComp,
                    PossiblePromotions = _danceCompHelper
                        .GetParticipants(
                            foundCompId.Value,
                            null,
                            true)
                        .Where(
                            x => x.DisplayInfo != null
                            && x.DisplayInfo.PromotionInfo != null
                            && x.DisplayInfo.PromotionInfo.PossiblePromotion)
                        .OrderBy(
                            x => x.NamePartA)
                        .ThenBy(
                            x => x.NamePartB)
                        .ToList(),
                    MultipleStarters = _danceCompHelper
                        .GetMultipleStarter(
                            foundCompId.Value)
                        .ToList(),
                });
        }
    }
}
