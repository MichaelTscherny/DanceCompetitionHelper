using AutoMapper;

using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Exceptions;
using DanceCompetitionHelper.OrgImpl.Oetsv;
using DanceCompetitionHelper.Web.Helper.Request;
using DanceCompetitionHelper.Web.Models;
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

            return await GetDefaultRequestHandler<Competition, CompetitionOverviewViewModel>()
                .SetOnSuccess(
                    nameof(Index))
                .SetOnNoData(
                    nameof(Index))
                .DefaultIndexAsync(
                    Guid.Empty,
                    async (_, dcH, _, _, cToken) =>
                    {
                        return new CompetitionOverviewViewModel()
                        {
                            OverviewItems = await dcH
                                .GetCompetitionsAsync(
                                    cToken,
                                    true)
                                .ToListAsync(
                                    cToken)
                        };
                    },
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
            return GetDefaultRequestHandler<Competition, CompetitionViewModel>()
                .SetOnSuccess(
                    nameof(Index))
                .SetOnModelStateInvalid(
                    nameof(ShowCreateEdit))
                .SetOnError(
                    nameof(ShowCreateEdit))
                .SetOnFunc(
                    SetOnEnum.OnModelStateInvalid | SetOnEnum.OnError,
                    async (model, dcH, _, _viewData, cToken) =>
                    {
                        await DefaultGetCompetitionAndSetViewData(
                            dcH,
                            model.CompetitionId,
                            _viewData,
                            cToken);

                        return null;
                    })
                .DefaultCreateNewAsync(
                    createCompetition,
                    _mapper.Map<Competition>(
                        createCompetition),
                    // ----
                    async (dcH, newEntity, _, _, cToken) =>
                    {
                        await dcH.CreateCompetitionAsync(
                            newEntity,
                            cToken);

                        return null;
                    },
                    cancellationToken);
        }

        public Task<IActionResult> ShowEdit(
            Guid id,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<Competition, CompetitionViewModel>()
                .SetOnSuccess(
                    nameof(ShowCreateEdit))
                .SetOnNoData(
                    nameof(Index))
                .DefaultShowAsync(
                    id,
                    async (showId, dcH, mapper, _, cToken) =>
                    {
                        var foundComp = await dcH.GetCompetitionAsync(
                            showId,
                            cToken)
                            ?? throw new NoDataFoundException(
                                string.Format(
                                    "{0} with id '{1}' not found!",
                                    nameof(Competition),
                                    id));

                        return mapper.Map<CompetitionViewModel>(
                            foundComp);
                    },
                    cancellationToken);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> EditSave(
            CompetitionViewModel editCompetition,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<Competition, CompetitionViewModel>()
                .SetOnSuccess(
                    nameof(Index))
                .SetOnNoData(
                    nameof(Index))
                .SetOnModelStateInvalid(
                    nameof(ShowCreateEdit))
                .SetOnError(
                    nameof(ShowCreateEdit))
                .SetOnFunc(
                    SetOnEnum.OnModelStateInvalid | SetOnEnum.OnError,
                    async (model, dcH, _, _viewData, cToken) =>
                    {
                        await DefaultGetCompetitionAndSetViewData(
                            dcH,
                            model.CompetitionId,
                            _viewData,
                            cToken);

                        return null;
                    })
                .DefaultEditSaveAsync(
                    editCompetition,
                    async (model, dcH, mapper, _, cToken) =>
                    {
                        var foundComp = await dcH.GetCompetitionAsync(
                            model.CompetitionId,
                            cToken)
                            ?? throw new NoDataFoundException(
                                string.Format(
                                    "{0} with id '{1}' not found!",
                                    nameof(Competition),
                                    model.CompetitionId));

                        // override the values...
                        mapper.Map(
                            model,
                            foundComp);

                        return null;
                    },
                    cancellationToken);
        }

        public Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<Competition, CompetitionViewModel>()
                .SetOnSuccess(
                    nameof(Index))
                .SetOnNoData(
                    nameof(Index))
                .SetOnError(
                    nameof(Index))
                .DefaultDeleteAsync(
                    id,
                    async (delId, dcH, _, _, cToken) =>
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
                (routeObjects, cToken) => Task.FromResult<IActionResult>(
                    RedirectToAction(
                        nameof(Index),
                        routeObjects)),
                (routeObjects, cToken) => Task.FromResult<IActionResult>(
                    RedirectToAction(
                        nameof(Index),
                        routeObjects)),
                (exc, routeObjects, cToken) => Task.FromResult<IActionResult>(
                    RedirectToAction(
                        nameof(Index),
                        routeObjects)),
                cancellationToken)
                ?? Error(
                    "Create History failed");
        }

        public Task<IActionResult> ShowImport(
            Guid? id,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<Competition, DoImportViewModel>()
                .SetOnSuccess(
                    nameof(ShowImport))
                .SetOnNoData(
                    nameof(Index))
                .DefaultShowAsync(
                    id,
                    async (showId, dcH, mapper, _, cToken) =>
                    {
                        var retDoImport = new DoImportViewModel()
                        {
                            // OrgCompetitionId = "1524",
                            FindFollowUpClasses = true,
                            UpdateData = true,
                        };

                        var foundComp = await dcH.GetCompetitionAsync(
                            showId,
                            cToken);

                        if (foundComp != null
                            && string.IsNullOrEmpty(
                                foundComp.OrgCompetitionId) == false)
                        {
                            retDoImport.OrgCompetitionId = foundComp.OrgCompetitionId;
                            retDoImport.FindFollowUpClasses = false;
                        }

                        return retDoImport;
                    },
                    cancellationToken);
        }

        public async Task<IActionResult> DoImport(
            DoImportViewModel doImportView,
            CancellationToken cancellationToken)
        {
            if (ModelState.IsValid == false)
            {
                doImportView.AddErrors(
                    ModelState);

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

        public Task<IActionResult> DownloadDummyFile(
            CancellationToken cancellationToken)
        {
            return GetPdfDocumentHelper()
                .GetDummyPdf(
                    new PdfViewModel()
                    {
                        CompetitionId = Guid.Parse("0623cd95-0929-4d3f-8767-daa17b65d63b"),
                        CompetitionClassId = Guid.Parse("0c2d36f9-31f8-48ce-86ab-67915bda2df3"),

                        // PageFormat = MigraDoc.DocumentObjectModel.PageFormat.A5,
                        // Orientation = MigraDoc.DocumentObjectModel.Orientation.Landscape,
                    },
                    cancellationToken);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}