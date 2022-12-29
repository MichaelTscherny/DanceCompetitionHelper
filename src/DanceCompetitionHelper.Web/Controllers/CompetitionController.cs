using DanceCompetitionHelper.Database.Enum;
using DanceCompetitionHelper.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics;

namespace DanceCompetitionHelper.Web.Controllers
{
    public class CompetitionController : Controller
    {
        private static bool _initialMigrationDone = false;

        private readonly IDanceCompetitionHelper _danceCompHelper;
        private readonly ILogger<CompetitionController> _logger;

        public CompetitionController(
            IDanceCompetitionHelper danceCompHelper,
            ILogger<CompetitionController> logger)
        {
            _danceCompHelper = danceCompHelper
                ?? throw new ArgumentNullException(
                    nameof(danceCompHelper));
            _logger = logger
                ?? throw new ArgumentNullException(
                    nameof(logger));

            if (_initialMigrationDone == false)
            {
                danceCompHelper.Migrate();
                _initialMigrationDone = true;
            }
        }

        public IActionResult Index()
        {
            return View(
                _danceCompHelper.GetCompetitions(
                    true));
        }

        public IActionResult ShowCreateNew(
            string? errorText = null)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNew(
            string competitionName,
            OrganizationEnum organization,
            string orgCompetitionId,
            string? competitionInfo,
            DateTime competitionDate)
        {
            if (ModelState.IsValid == false)
            {
                return View(
                    nameof(Error),
                    new ErrorViewModel
                    {
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                        ModelErrors = ModelState.Values
                            .Where(
                                x => x.ValidationState != ModelValidationState.Valid
                                && x.Errors.Count >= 1)
                            .ToList()
                    });
            }

            try
            {
                _danceCompHelper.CreateCompetition(
                    competitionName,
                    organization,
                    orgCompetitionId,
                    competitionInfo,
                    competitionDate);

                return RedirectToAction(
                    nameof(Index));
            }
            catch (Exception exc)
            {
                ViewData["Error"] = exc.InnerException?.Message ?? exc.Message;
                ViewData["competitionName"] = competitionName;
                ViewData["organization"] = organization;
                ViewData["orgCompetitionId"] = orgCompetitionId;
                ViewData["competitionInfo"] = competitionInfo;
                ViewData["competitionDate"] = competitionDate.ToShortDateString();

                return View(
                    nameof(ShowCreateNew));
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
        }
    }
}