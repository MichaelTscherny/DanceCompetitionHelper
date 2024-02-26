using AutoMapper;
using DanceCompetitionHelper.OrgImpl.Oetsv;
using DanceCompetitionHelper.Web.Models.CompetitionModels;
using Microsoft.AspNetCore.Mvc;

namespace DanceCompetitionHelper.Web.Controllers
{
    public class CompetitionController : ControllerBase
    {
        public const string RefName = "Competition";

        private static bool _initialMigrationDone = false;

        private readonly IDanceCompetitionHelper _danceCompHelper;
        private readonly ILogger<CompetitionController> _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly IMapper _mapper;

        public CompetitionController(
            IDanceCompetitionHelper danceCompHelper,
            ILogger<CompetitionController> logger,
            IHostApplicationLifetime appLifetime,
            IMapper mapper)
        {
            _danceCompHelper = danceCompHelper
                ?? throw new ArgumentNullException(
                    nameof(danceCompHelper));
            _logger = logger
                ?? throw new ArgumentNullException(
                    nameof(logger));
            _appLifetime = appLifetime
                ?? throw new ArgumentNullException(
                    nameof(appLifetime));
            _mapper = mapper
                ?? throw new ArgumentNullException(
                    nameof(mapper));
        }

        private async Task CheckInitialMigrationDone(
            IDanceCompetitionHelper danceCompHelper,
            CancellationToken cancellationToken)
        {
            if (_initialMigrationDone == false)
            {
                await danceCompHelper.MigrateAsync();
                await danceCompHelper.CheckMandatoryConfigurationAsync(
                    cancellationToken);
                await danceCompHelper.AddTestDataAsync(
                    cancellationToken);
                _initialMigrationDone = true;
            }
        }

        public async Task<IActionResult> Index(
            CancellationToken cancellationToken)
        {
            await CheckInitialMigrationDone(
                _danceCompHelper,
                CancellationToken.None);

            return await DefaultIndex(
                _danceCompHelper,
                async (dcH, dbCtx, dbTrans, cToken) =>
                {
                    return await dcH
                        .GetCompetitionsAsync(
                            cToken,
                            true)
                        .ToListAsync();
                },
                nameof(Index),
                cancellationToken);

            /*
            return (await _danceCompHelper.RunInReadonlyTransaction(
                async (dcH, dbCtx, dbTrans, cToken) =>
                {
                    return View(
                        await dcH
                            .GetCompetitionsAsync(
                                cToken,
                                true)
                            .ToListAsync());
                },
                cancellationToken))
                ?? Error("Read failed!");
            */
        }

        public IActionResult ShowCreateEdit()
        {
            return View(
                new CompetitionViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> CreateNew(
            CompetitionViewModel createCompetition,
            CancellationToken cancellationToken)
        {
            return DefaultCreateNew(
                _danceCompHelper,
                createCompetition,
                null,
                nameof(ShowCreateEdit),
                async (dcH, dbCtx, dbTrans, cToken) =>
                {
                    await dcH.CreateCompetitionAsync(
                        createCompetition.CompetitionName,
                        createCompetition.Organization,
                        createCompetition.OrgCompetitionId,
                        createCompetition.CompetitionInfo,
                        createCompetition.CompetitionDate ?? DateTime.Now,
                        createCompetition.Comment,
                        cToken);
                },
                nameof(Index),
                nameof(ShowCreateEdit),
                cancellationToken);

            /*
            if (ModelState.IsValid == false)
            {
                createCompetition.AddErrors(
                    ModelState);

                return View(
                    nameof(ShowCreateEdit),
                    createCompetition);
            }

            return (await _danceCompHelper.RunInTransactionWithSaveChangesAndCommit<IActionResult>(
                async (dcH, dbCtx, dbTrans, cToken) =>
                {
                    await dcH.CreateCompetitionAsync(
                        createCompetition.CompetitionName,
                        createCompetition.Organization,
                        createCompetition.OrgCompetitionId,
                        createCompetition.CompetitionInfo,
                        createCompetition.CompetitionDate ?? DateTime.Now,
                        createCompetition.Comment,
                        cToken);
                },
                (cToken) => RedirectToAction(
                    nameof(Index)),
                (exc, cToken) =>
                {
                    createCompetition.AddErrors(
                        exc);

                    return View(
                        nameof(ShowCreateEdit),
                        createCompetition);
                },
                cancellationToken))
                ?? Error(
                    "Save failed");
            */
        }

        public async Task<IActionResult> ShowEdit(
            Guid id,
            CancellationToken cancellationToken)
        {
            var foundComp = await _danceCompHelper.GetCompetitionAsync(
                id,
                cancellationToken);

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
        public async Task<IActionResult> EditSave(
            CompetitionViewModel editCompetition,
            CancellationToken cancellationToken)
        {
            if (ModelState.IsValid == false)
            {
                editCompetition.AddErrors(
                    ModelState);

                return View(
                    nameof(ShowCreateEdit),
                    editCompetition);
            }

            try
            {
                await _danceCompHelper.EditCompetitionAsync(
                    editCompetition.CompetitionId ?? Guid.Empty,
                    editCompetition.CompetitionName,
                    editCompetition.Organization,
                    editCompetition.OrgCompetitionId,
                    editCompetition.CompetitionInfo,
                    editCompetition.CompetitionDate ?? DateTime.Now,
                    editCompetition.Comment,
                    cancellationToken);

                return RedirectToAction(
                    nameof(Index));
            }
            catch (Exception exc)
            {
                editCompetition.AddErrors(
                    exc);

                return View(
                    nameof(ShowCreateEdit),
                    editCompetition);
            }
        }

        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            await _danceCompHelper.RemoveCompetitionAsync(
                id,
                cancellationToken);

            return RedirectToAction(
                nameof(Index));
        }

        public async Task<IActionResult> CreateTableHistory(
            Guid id,
            CancellationToken cancellationToken)
        {
            var foundComp = await _danceCompHelper.GetCompetitionAsync(
                id,
                cancellationToken);

            if (foundComp != null)
            {
                await _danceCompHelper.CreateTableHistoryAsync(
                    foundComp.CompetitionId,
                    cancellationToken);
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

        public async Task<IActionResult> DoImport(
            DoImportViewModel doImportView,
            CancellationToken cancellationToken)
        {
            if (ModelState.IsValid == false)
            {
                doImportView.AddErrors(
                    ModelState); ;

                return View(
                    nameof(ShowImport),
                    doImportView);
            }

            try
            {
                var useParams = new Dictionary<string, string>();

                if (doImportView.FindFollowUpClasses)
                {
                    useParams.Add(
                        nameof(OetsvCompetitionImporter.FindFollowUpClasses),
                        "true");
                };

                if (doImportView.UpdateData)
                {
                    useParams.Add(
                        nameof(OetsvCompetitionImporter.UpdateData),
                        "true");
                };

                // TODO: implement more options/file-uploads/etc...
                var workStatus = await _danceCompHelper.ImportOrUpdateCompetitionAsync(
                    doImportView.Organization,
                    doImportView.OrgCompetitionId,
                    doImportView.ImportType,
                    cancellationToken,
                    null,
                    useParams);

                doImportView.OrgCompetitionId = workStatus.OrgCompetitionId;
                doImportView.CompetitionId = workStatus.CompetitionId;
                doImportView.Errors.AddRange(
                    workStatus.Errors);
                doImportView.WorkInfo.AddRange(
                    workStatus.WorkInfo);
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
    }
}