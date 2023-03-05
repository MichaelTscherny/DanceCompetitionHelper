using DanceCompetitionHelper.Web.Extensions;
using DanceCompetitionHelper.Web.Models;
using DanceCompetitionHelper.Web.Models.CompetitionModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DanceCompetitionHelper.Web.Controllers
{
    public class CompetitionController : Controller
    {
        public const string RefName = "Competition";

        private static bool _initialMigrationDone = false;

        private readonly IDanceCompetitionHelper _danceCompHelper;
        private readonly ILogger<CompetitionController> _logger;
        private readonly IServiceProvider _serviceProvider;

        public CompetitionController(
            IDanceCompetitionHelper danceCompHelper,
            ILogger<CompetitionController> logger,
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

            if (_initialMigrationDone == false)
            {
                danceCompHelper.Migrate();
                danceCompHelper.AddTestData();
                _initialMigrationDone = true;
            }
        }

        public IActionResult Index()
        {
            return View(
                _danceCompHelper
                    .GetCompetitions(
                        true)
                    .ToList());
        }

        public IActionResult ShowCreateEdit()
        {
            return View(
                new CompetitionViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNew(
            CompetitionViewModel createCompetition)
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
                _danceCompHelper.CreateCompetition(
                    createCompetition.CompetitionName,
                    createCompetition.Organization,
                    createCompetition.OrgCompetitionId,
                    createCompetition.CompetitionInfo,
                    createCompetition.CompetitionDate ?? DateTime.Now,
                    createCompetition.Comment);

                return RedirectToAction(
                    nameof(Index));
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
            var foundComp = _danceCompHelper.GetCompetition(
                id);

            if (foundComp == null)
            {
                return RedirectToAction(
                    nameof(Index));
            }

            return View(
                nameof(ShowCreateEdit),
                new CompetitionViewModel()
                {
                    CompetitionId = foundComp.CompetitionId,
                    CompetitionName = foundComp.CompetitionName,
                    Organization = foundComp.Organization,
                    OrgCompetitionId = foundComp.OrgCompetitionId,
                    CompetitionInfo = foundComp.CompetitionInfo,
                    CompetitionDate = foundComp.CompetitionDate,
                    Comment = foundComp.Comment,
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditSave(
            CompetitionViewModel editCompetition)
        {
            if (ModelState.IsValid == false)
            {
                editCompetition.Errors = ModelState.GetErrorMessages();

                return View(
                    nameof(ShowCreateEdit),
                    editCompetition);
            }

            try
            {
                _danceCompHelper.EditCompetition(
                    editCompetition.CompetitionId ?? Guid.Empty,
                    editCompetition.CompetitionName,
                    editCompetition.Organization,
                    editCompetition.OrgCompetitionId,
                    editCompetition.CompetitionInfo,
                    editCompetition.CompetitionDate ?? DateTime.Now,
                    editCompetition.Comment);

                return RedirectToAction(
                    nameof(Index));
            }
            catch (Exception exc)
            {
                editCompetition.Errors = exc.InnerException?.Message ?? exc.Message;

                return View(
                    nameof(ShowCreateEdit),
                    editCompetition);
            }
        }

        public IActionResult Delete(
            Guid id)
        {
            _danceCompHelper.RemoveCompetition(
                id);

            return RedirectToAction(
                nameof(Index));
        }

        public IActionResult CreateTableHistory(
            Guid id)
        {
            var foundComp = _danceCompHelper.GetCompetition(
                id);

            if (foundComp != null)
            {
                _danceCompHelper.CreateTableHistory(
                    foundComp.CompetitionId);
            }

            return RedirectToAction(
                nameof(Index));
        }

        public IActionResult ShowImport()
        {
            return View(
                new DoImportViewModel()
                {
                    OrgCompetitionId = "1524",
                });
        }

        public IActionResult DoImport(
            DoImportViewModel doImportView)
        {
            if (ModelState.IsValid == false)
            {
                doImportView.Errors.AddRange(
                    ModelState.GetErrorMessages()
                    .Split(
                        new[] { "\r\n" },
                        StringSplitOptions.RemoveEmptyEntries));

                return View(
                    nameof(ShowImport),
                    doImportView);
            }

            try
            {
                // TODO: implement more options/file-uploads/etc...
                var executionErrors = _danceCompHelper.ImportOrUpdateCompetition(
                    doImportView.Organization,
                    doImportView.OrgCompetitionId,
                    doImportView.ImportType,
                    null);

                doImportView.Errors.AddRange(
                    executionErrors);
            }
            catch (Exception exc)
            {
                doImportView.Errors.Add(
                    exc.Message);
            }

            return View(
                doImportView);
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