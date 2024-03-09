using AutoMapper;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Exceptions;
using DanceCompetitionHelper.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DanceCompetitionHelper.Web.Controllers
{
    public abstract class ControllerBase<TLogger> : Controller
    {
        protected readonly IDanceCompetitionHelper _danceCompHelper;
        protected readonly ILogger<TLogger> _logger;
        protected readonly IMapper _mapper;

        protected ControllerBase(
            IDanceCompetitionHelper danceCompHelper,
            ILogger<TLogger> logger,
            IMapper mapper)
        {
            _danceCompHelper = danceCompHelper
                ?? throw new ArgumentNullException(
                    nameof(danceCompHelper));
            _logger = logger
                ?? throw new ArgumentNullException(
                    nameof(logger));
            _mapper = mapper
                ?? throw new ArgumentNullException(
                    nameof(mapper));
        }

        public async Task<IActionResult> DefaultIndexAndShow<T>(
            Func<IDanceCompetitionHelper, IMapper, CancellationToken, Task<T>> funcIndex,
            string viewNameSuccess,
            string viewNameOnNoData,
            CancellationToken cancellationToken,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return (await _danceCompHelper.RunInReadonlyTransaction<IActionResult>(
                async (dcH, _, _, cToken) =>
                {
                    try
                    {
                        var foundData = await funcIndex(
                            dcH,
                            _mapper,
                            cToken);

                        if (foundData == null)
                        {
                            return RedirectToAction(
                               viewNameOnNoData);
                        }

                        return View(
                            viewNameSuccess,
                            foundData);
                    }
                    catch (NoDataFoundException noDataExc)
                    {
                        _logger.LogWarning(
                            noDataExc,
                            "No Data during {MemberName}(): {Message}",
                            memberName,
                            noDataExc.Message);

                        return RedirectToAction(
                            viewNameOnNoData);
                    }
                },
                cancellationToken))
                ?? Error(
                    $"Read for {viewNameSuccess} failed!");
        }

        public async Task<IActionResult> DefaultCreateNew<TVm, TEntity>(
            TVm movelView,
            TEntity newEntity,
            Func<IDanceCompetitionHelper, CancellationToken, Task>? funcOnModelStateInvalid,
            string viewNameModelStateInvalid,
            Func<IDanceCompetitionHelper, TEntity, IMapper, CancellationToken, Task> funcCreateNew,
            string viewNameSuccess,
            string viewNameOnError,
            CancellationToken cancellationToken,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
            where TVm : ViewModelBase
            where TEntity : TableBase
        {
            if (ModelState.IsValid == false)
            {
                movelView.AddErrors(
                    ModelState);

                if (funcOnModelStateInvalid != null)
                {
                    await funcOnModelStateInvalid(
                        _danceCompHelper,
                        cancellationToken);
                }

                return View(
                    viewNameModelStateInvalid,
                    movelView);
            }

            return (await _danceCompHelper.RunInTransactionWithSaveChangesAndCommit<IActionResult>(
                (dcH, _, _, cToken) => funcCreateNew(
                    dcH,
                    newEntity,
                    _mapper,
                    cToken),
                (cToken) => RedirectToAction(
                    viewNameSuccess),
                null,
                (exc, cToken) =>
                {
                    movelView.AddErrors(
                        exc);

                    return View(
                        viewNameOnError,
                        movelView);
                },
                cancellationToken))
                ?? Error(
                    "Creation failed");
        }

        public async Task<IActionResult> DefaultEdit<TVm>(
            TVm movelView,
            Func<IDanceCompetitionHelper, CancellationToken, Task>? funcOnModelStateInvalid,
            string viewNameModelStateInvalid,
            Func<IDanceCompetitionHelper, IMapper, CancellationToken, Task> funcEdit,
            string viewNameSuccess,
            string viewNameOnNoData,
            string viewNameOnError,
            CancellationToken cancellationToken,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
            where TVm : ViewModelBase
        {
            if (ModelState.IsValid == false)
            {
                movelView.AddErrors(
                    ModelState);

                if (funcOnModelStateInvalid != null)
                {
                    await funcOnModelStateInvalid(
                        _danceCompHelper,
                        cancellationToken);
                }

                return View(
                    viewNameModelStateInvalid,
                    movelView);
            }

            return (await _danceCompHelper.RunInTransactionWithSaveChangesAndCommit<IActionResult>(
                (dcH, dbCtx, dbTrans, cToken) => funcEdit(
                    dcH,
                    _mapper,
                    cToken),
                (cToken) => RedirectToAction(
                    viewNameSuccess),
                (cToken) => RedirectToAction(
                    viewNameOnNoData),
                (exc, cToken) =>
                {
                    movelView.AddErrors(
                        exc);

                    return View(
                        viewNameOnError,
                        movelView);
                },
                cancellationToken))
                ?? Error(
                    "Edit failed");
        }

        public async Task<IActionResult> DefaultDelete<TDeleteId>(
            TDeleteId deleteId,
            Func<IDanceCompetitionHelper, TDeleteId, CancellationToken, Task> funcDelete,
            string viewNameSuccess,
            string viewNameOnNoData,
            string viewNameOnError,
            CancellationToken cancellationToken,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return (await _danceCompHelper.RunInTransactionWithSaveChangesAndCommit<IActionResult>(
                (dcH, _, _, cToken) => funcDelete(
                    dcH,
                    deleteId,
                    cToken),
                (cToken) => RedirectToAction(
                    viewNameSuccess),
                (cToken) => RedirectToAction(
                    viewNameOnNoData),
                (exc, cToken) => RedirectToAction(
                    viewNameOnError),
                cancellationToken))
                ?? Error(
                    "Delete failed");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(
            string errorMessage)
        {
            return View(
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    Errors = new List<string>()
                    {
                        errorMessage,
                    },
                });
        }

    }
}
