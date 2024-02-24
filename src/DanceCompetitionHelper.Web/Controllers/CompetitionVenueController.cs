using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Web.Extensions;
using DanceCompetitionHelper.Web.Models.CompetitionVenueModels;
using Microsoft.AspNetCore.Mvc;

namespace DanceCompetitionHelper.Web.Controllers
{
    public class CompetitionVenueController : Controller
    {
        public const string RefName = "CompetitionVenue";

        private readonly IDanceCompetitionHelper _danceCompHelper;
        private readonly ILogger<CompetitionVenueController> _logger;
        private readonly IServiceProvider _serviceProvider;

        public CompetitionVenueController(
            IDanceCompetitionHelper danceCompHelper,
            ILogger<CompetitionVenueController> logger,
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
                new CompetitionVenueOverviewViewModel()
                {
                    Competition = _danceCompHelper.GetCompetitionAsync(
                        foundCompId),
                    OverviewItems = _danceCompHelper
                        .GetCompetitionVenuesAsync(
                            foundCompId)
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

            var foundComp = _danceCompHelper.GetCompetitionAsync(
                foundCompId);

            ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

            return View(
                new CompetitionVenueViewModel()
                {
                    CompetitionId = foundCompId.Value,
                    CompetitionName = foundComp.GetCompetitionName(),
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNew(
            CompetitionVenueViewModel createCompetitionVenue)
        {
            if (ModelState.IsValid == false)
            {
                createCompetitionVenue.Errors = ModelState.GetErrorMessages();

                return View(
                    nameof(ShowCreateEdit),
                    createCompetitionVenue);
            }

            try
            {
                var useCompetitionId = createCompetitionVenue.CompetitionId;

                _danceCompHelper.CreateCompetitionVenue(
                    useCompetitionId,
                    createCompetitionVenue.Name,
                    createCompetitionVenue.LengthInMeter,
                    createCompetitionVenue.WidthInMeter,
                    createCompetitionVenue.Comment);

                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        Id = useCompetitionId,
                    });
            }
            catch (Exception exc)
            {
                createCompetitionVenue.Errors = exc.InnerException?.Message ?? exc.Message;

                return View(
                    nameof(ShowCreateEdit),
                    createCompetitionVenue);
            }
        }

        public IActionResult ShowEdit(
            Guid id)
        {
            var foundCompVenue = _danceCompHelper.GetCompetitionVenue(
                id);

            if (foundCompVenue == null)
            {
                return RedirectToAction(
                    nameof(Index));
            }

            ViewData["Use" + nameof(CompetitionClass)] = foundCompVenue.CompetitionId;

            return View(
                nameof(ShowCreateEdit),
                new CompetitionVenueViewModel()
                {
                    CompetitionVenueId = foundCompVenue.CompetitionVenueId,

                    CompetitionId = foundCompVenue.CompetitionId,
                    CompetitionName = foundCompVenue.Competition.GetCompetitionName(),

                    Name = foundCompVenue.Name,
                    LengthInMeter = foundCompVenue.LengthInMeter,
                    WidthInMeter = foundCompVenue.WidthInMeter,
                    Comment = foundCompVenue.Comment,
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditSave(
            CompetitionVenueViewModel editCompetitionVenue)
        {
            if (ModelState.IsValid == false)
            {
                editCompetitionVenue.Errors = ModelState.GetErrorMessages();

                return View(
                    nameof(ShowCreateEdit),
                    editCompetitionVenue);
            }

            try
            {
                _danceCompHelper.EditCompetitionVenue(
                    editCompetitionVenue.CompetitionVenueId ?? Guid.Empty,
                    editCompetitionVenue.Name,
                    editCompetitionVenue.LengthInMeter,
                    editCompetitionVenue.WidthInMeter,
                    editCompetitionVenue.Comment);

                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        Id = editCompetitionVenue.CompetitionId,
                    });
            }
            catch (Exception exc)
            {
                editCompetitionVenue.Errors = exc.InnerException?.Message ?? exc.Message;

                return View(
                    nameof(ShowCreateEdit),
                    editCompetitionVenue);
            }
        }

        public IActionResult Delete(
            Guid id)
        {
            var helpCompId = _danceCompHelper.FindCompetition(
                    id);

            _danceCompHelper.RemoveCompetitionVenue(
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
