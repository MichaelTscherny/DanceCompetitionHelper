using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Web.Extensions;
using DanceCompetitionHelper.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace DanceCompetitionHelper.Web.Controllers
{
    public class ParticipantController : Controller
    {
        public const string RefName = "Participant";

        private readonly IDanceCompetitionHelper _danceCompHelper;
        private readonly ILogger<CompetitionController> _logger;

        public ParticipantController(
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

            ViewData["BackTo" + CompetitionClassController.RefName] = foundCompId;
            ViewData[nameof(CompetitionClassController.ShowMultipleStarters)] = foundCompId;

            return View(
                new ParticipantOverviewViewModel()
                {
                    Competition = _danceCompHelper.GetCompetition(
                        foundCompId ?? Guid.Empty),
                    Participtans = _danceCompHelper
                        .GetParticipants(
                            foundCompId,
                            null,
                            true)
                        .ToList(),
                });
        }

        public IActionResult DetailedView(
            Guid id)
        {
            var foundCompId = _danceCompHelper.FindCompetition(
                id);

            ViewData["BackTo" + CompetitionClassController.RefName] = foundCompId;
            ViewData[nameof(CompetitionClassController.ShowMultipleStarters)] = foundCompId;

            return View(
                nameof(Index),
                new ParticipantOverviewViewModel()
                {
                    Competition = _danceCompHelper.GetCompetition(
                        foundCompId ?? Guid.Empty),
                    Participtans = _danceCompHelper
                        .GetParticipants(
                            foundCompId,
                            null,
                            true)
                        .ToList(),
                    DetailedView = true,
                });
        }

        public const string ParticipantLastCreatedCompetitionClassId = nameof(ParticipantLastCreatedCompetitionClassId);

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
                foundCompId.Value);

            ViewData["Show" + ParticipantController.RefName] = foundCompId;
            ViewData["BackTo" + CompetitionClassController.RefName] = foundCompId;
            ViewData[nameof(CompetitionClassController.ShowMultipleStarters)] = foundCompId;

            var helpCompName = string.Empty;

            Guid.TryParse(
                HttpContext.Session.GetString(
                    ParticipantLastCreatedCompetitionClassId),
                out var lastCreatedClassId);

            return View(
                new ParticipantViewModel()
                {
                    CompetitionId = foundCompId.Value,
                    CompetitionName = foundComp.GetCompetitionName(),
                    CompetitionClasses = _danceCompHelper
                        .GetCompetitionClasses(
                            foundCompId)
                        .ToSelectListItem(
                            lastCreatedClassId),
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNew(
            ParticipantViewModel createParticipant)
        {
            if (ModelState.IsValid == false)
            {
                createParticipant.Errors = ModelState.GetErrorMessages();

                return View(
                    nameof(ShowCreateEdit),
                    createParticipant);
            }

            try
            {
                var useCompetitionClassId = createParticipant.CompetitionClassId ?? Guid.Empty;

                _danceCompHelper.CreateParticipant(
                     createParticipant.CompetitionId,
                     useCompetitionClassId,
                     createParticipant.StartNumber,
                     createParticipant.NamePartA,
                     createParticipant.OrgIdPartA,
                     createParticipant.NamePartB,
                     createParticipant.OrgIdPartB,
                     createParticipant.ClubName,
                     createParticipant.OrgIdClub,
                     createParticipant.OrgPointsPartA,
                     createParticipant.OrgStartsPartA,
                     createParticipant.MinStartsForPromotionPartA,
                     createParticipant.OrgPointsPartB,
                     createParticipant.OrgStartsPartB,
                     createParticipant.MinStartsForPromotionPartB,
                     createParticipant.Comment,
                     createParticipant.Ignore);

                HttpContext.Session.SetString(
                    ParticipantLastCreatedCompetitionClassId,
                    useCompetitionClassId.ToString());

                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        Id = createParticipant.CompetitionId
                    });
            }
            catch (Exception exc)
            {
                createParticipant.Errors = exc.InnerException?.Message ?? exc.Message;

                return View(
                    nameof(ShowCreateEdit),
                    createParticipant);
            }
        }

        public IActionResult ShowEdit(
            Guid id)
        {
            var foundParticipant = _danceCompHelper.GetParticipant(
                id);

            if (foundParticipant == null)
            {
                return RedirectToAction(
                    nameof(Index));
            }

            var foundComp = _danceCompHelper.GetCompetition(
                foundParticipant.CompetitionId);

            return View(
                nameof(ShowCreateEdit),
                new ParticipantViewModel()
                {
                    CompetitionId = foundParticipant.CompetitionId,
                    CompetitionName = foundComp.GetCompetitionName(),
                    CompetitionClassId = foundParticipant.CompetitionClassId,
                    ParticipantId = foundParticipant.ParticipantId,
                    StartNumber = foundParticipant.StartNumber,
                    NamePartA = foundParticipant.NamePartA,
                    OrgIdPartA = foundParticipant.OrgIdPartA,
                    NamePartB = foundParticipant.NamePartB,
                    OrgIdPartB = foundParticipant.OrgIdPartB,
                    ClubName = foundParticipant.ClubName,
                    OrgIdClub = foundParticipant.OrgIdClub,
                    OrgPointsPartA = foundParticipant.OrgPointsPartA,
                    OrgStartsPartA = foundParticipant.OrgStartsPartA,
                    MinStartsForPromotionPartA = foundParticipant.MinStartsForPromotionPartA,
                    OrgPointsPartB = foundParticipant.OrgPointsPartB,
                    OrgStartsPartB = foundParticipant.OrgStartsPartB,
                    MinStartsForPromotionPartB = foundParticipant.MinStartsForPromotionPartB,
                    Comment = foundParticipant.Comment,
                    Ignore = foundParticipant.Ignore,

                    CompetitionClasses = _danceCompHelper
                        .GetCompetitionClasses(
                            foundParticipant.CompetitionId)
                        .ToSelectListItem(
                            foundParticipant.CompetitionClassId),
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditSave(
            ParticipantViewModel editParticipant)
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
                _danceCompHelper.EditParticipant(
                    editParticipant.ParticipantId ?? Guid.Empty,
                    editParticipant.CompetitionClassId ?? Guid.Empty,
                    editParticipant.StartNumber,
                    editParticipant.NamePartA,
                    editParticipant.OrgIdPartA,
                    editParticipant.NamePartB,
                    editParticipant.OrgIdPartB,
                    editParticipant.ClubName,
                    editParticipant.OrgIdClub,
                    editParticipant.OrgPointsPartA,
                    editParticipant.OrgStartsPartA,
                    editParticipant.MinStartsForPromotionPartA,
                    editParticipant.OrgPointsPartB,
                    editParticipant.OrgStartsPartB,
                    editParticipant.MinStartsForPromotionPartB,
                    editParticipant.Comment,
                    editParticipant.Ignore);

                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        Id = editParticipant.CompetitionClassId ?? Guid.Empty
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
            var helpCompClassId = _danceCompHelper.FindCompetitionClass(
                    id);

            _danceCompHelper.RemoveParticipant(
                id);

            return RedirectToAction(
                nameof(Index),
                new
                {
                    Id = helpCompClassId ?? Guid.Empty
                });
        }
    }
}
