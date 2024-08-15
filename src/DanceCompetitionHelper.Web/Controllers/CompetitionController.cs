using AutoMapper;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Exceptions;
using DanceCompetitionHelper.OrgImpl.Oetsv;
using DanceCompetitionHelper.Web.Models.CompetitionModels;
using Microsoft.AspNetCore.Mvc;

namespace DanceCompetitionHelper.Web.Controllers
{
    public class CompetitionController : ControllerBase<CompetitionController>
    {
        public const string RefName = "Competition";

        private static bool _initialMigrationDone = false;

        private readonly IHostApplicationLifetime _appLifetime;

        public CompetitionController(
            IDanceCompetitionHelper danceCompHelper,
            ILogger<CompetitionController> logger,
            IHostApplicationLifetime appLifetime,
            IMapper mapper)
                : base(
                      danceCompHelper,
                      logger,
                      mapper)
        {
            _appLifetime = appLifetime
                ?? throw new ArgumentNullException(
                    nameof(appLifetime));
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

            return await DefaultIndexAndShow(
                async (dcH, mapper, cToken) =>
                {
                    return await dcH
                        .GetCompetitionsAsync(
                            cToken,
                            true)
                        .ToListAsync();
                },
                // --
                nameof(Index),
                nameof(Index),
                // --
                cancellationToken);
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
                createCompetition,
                _mapper.Map<Competition>(
                    createCompetition),
                // --
                null,
                nameof(ShowCreateEdit),
                // --
                async (dcH, newEntity, mapper, cToken) =>
                {
                    await dcH.CreateCompetitionAsync(
                        newEntity,
                        cToken);

                    return null;
                },
                // --
                nameof(Index),
                nameof(ShowCreateEdit),
                // --
                cancellationToken);
        }

        public Task<IActionResult> ShowEdit(
            Guid id,
            CancellationToken cancellationToken)
        {
            return DefaultIndexAndShow(
                async (dcH, mapper, cToken) =>
                {
                    var foundComp = await dcH.GetCompetitionAsync(
                        id,
                        cToken)
                        ?? throw new NoDataFoundException(
                        string.Format(
                            "{0} with id '{1}' not found!",
                            nameof(Competition),
                            id));

                    return mapper.Map<CompetitionViewModel>(
                        foundComp);
                },
                // --
                nameof(ShowCreateEdit),
                nameof(Index),
                // --
                cancellationToken);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> EditSave(
            CompetitionViewModel editCompetition,
            CancellationToken cancellationToken)
        {
            return DefaultEdit(
                editCompetition,
                // --
                null,
                nameof(ShowCreateEdit),
                // --
                async (dcH, mapper, cToken) =>
                {
                    var foundComp = await dcH.GetCompetitionAsync(
                        editCompetition.CompetitionId,
                        cToken)
                        ?? throw new NoDataFoundException(
                            string.Format(
                                "{0} with id '{1}' not found!",
                                nameof(Competition),
                                editCompetition.CompetitionId));

                    // override the values...
                    mapper.Map(
                        editCompetition,
                        foundComp);

                    return null;
                },
                // --
                nameof(Index),
                nameof(Index),
                nameof(ShowCreateEdit),
                // --
                cancellationToken);
        }

        public Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            return DefaultDelete(
                id,
                // --
                async (dcH, delId, cToken) =>
                {
                    var foundComp = await dcH.GetCompetitionAsync(
                        delId,
                        cToken)
                        ?? throw new NoDataFoundException(
                            string.Format(
                                "{0} with id '{1}' not found!",
                                nameof(Competition),
                                delId));

                    await _danceCompHelper.RemoveCompetitionAsync(
                        foundComp,
                        cToken);

                    return null;
                },
                // --
                nameof(Index),
                nameof(Index),
                nameof(Index),
                // --
                cancellationToken);
        }

        public async Task<IActionResult> CreateTableHistory(
            Guid id,
            CancellationToken cancellationToken)
        {
            return await _danceCompHelper.RunInTransactionWithSaveChangesAndCommit(
                async (dcH, _, _, cToken) =>
                {
                    var foundComp = await _danceCompHelper.GetCompetitionAsync(
                        id,
                        cToken)
                        ?? throw new NoDataFoundException(
                            string.Format(
                                "{0} with id '{1}' not found!",
                                nameof(Competition),
                                id));

                    await _danceCompHelper.CreateTableHistoryAsync(
                        foundComp.CompetitionId,
                        cancellationToken);

                    return null;
                },
                // --                
                (routeObjects, cToken) => RedirectToAction(
                    nameof(Index),
                    routeObjects),
                (routeObjects, cToken) => RedirectToAction(
                    nameof(Index),
                    routeObjects),
                (exc, routeObjects, cToken) => RedirectToAction(
                    nameof(Index),
                    routeObjects),
                cancellationToken)
                ?? Error(
                    "Create History failed");
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