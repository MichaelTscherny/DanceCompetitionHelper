using AutoMapper;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Exceptions;
using DanceCompetitionHelper.Web.Helper.Request;
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

        #region Chaining

        public DefaultRequestHandler<TLogger, TType, TModel> GetDefaultRequestHandler<TType, TModel>(
            // CancellationToken cancellationToken,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
            where TType : TableBase
            where TModel : ViewModelBase
        {
            return new DefaultRequestHandler<TLogger, TType, TModel>(
                _danceCompHelper,
                _logger,
                _mapper);
        }

        #endregion Chaining





        [Obsolete("use GetDefaultRequestHandler()")]
        public async Task<IActionResult> DefaultIndexAndShow<TModel>(
            Func<IDanceCompetitionHelper, IMapper, CancellationToken, Task<TModel>> funcIndex,
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

        [Obsolete("use GetDefaultRequestHandler()")]
        public async Task<IActionResult> DefaultCreateNew<TModel, TEntity>(
            TModel movelView,
            TEntity newEntity,
            Func<IDanceCompetitionHelper, CancellationToken, Task>? funcOnModelStateInvalid,
            string viewNameModelStateInvalid,
            Func<IDanceCompetitionHelper, TEntity, IMapper, CancellationToken, Task<object?>> funcCreateNew,
            string viewNameSuccess,
            Func<IDanceCompetitionHelper, TModel, CancellationToken, Task>? funcOnError,
            string viewNameOnError,
            CancellationToken cancellationToken,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
            where TModel : ViewModelBase
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
                (routeObjects, cToken) => Task.FromResult<IActionResult>(
                    RedirectToAction(
                        viewNameSuccess,
                        routeObjects)),
                null,
                async (exc, routeObjects, cToken) =>
                {
                    movelView.AddErrors(
                        exc);

                    if (funcOnError != null)
                    {
                        await funcOnError.Invoke(
                            _danceCompHelper,
                            movelView,
                            cToken);
                    }

                    return View(
                        viewNameOnError,
                        movelView);
                },
                cancellationToken))
                ?? Error(
                    "Creation failed");
        }

        [Obsolete("use GetDefaultRequestHandler()")]
        public async Task<IActionResult> DefaultEdit<TModel>(
            TModel movelView,
            Func<IDanceCompetitionHelper, CancellationToken, Task>? funcOnModelStateInvalid,
            string viewNameModelStateInvalid,
            Func<IDanceCompetitionHelper, IMapper, CancellationToken, Task<object?>> funcEdit,
            string viewNameSuccess,
            string viewNameOnNoData,
            Func<IDanceCompetitionHelper, TModel, CancellationToken, Task>? funcOnError,
            string viewNameOnError,
            CancellationToken cancellationToken,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
            where TModel : ViewModelBase
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
                (routeObjects, cToken) => Task.FromResult<IActionResult>(
                    RedirectToAction(
                        viewNameSuccess,
                        routeObjects)),
                (routeObjects, cToken) => Task.FromResult<IActionResult>(
                    RedirectToAction(
                        viewNameOnNoData,
                        routeObjects)),
                async (exc, routeObjects, cToken) =>
                {
                    movelView.AddErrors(
                        exc);

                    if (funcOnError != null)
                    {
                        await funcOnError.Invoke(
                            _danceCompHelper,
                            movelView,
                            cToken);
                    }

                    return View(
                        viewNameOnError,
                        movelView);
                },
                cancellationToken))
                ?? Error(
                    "Edit failed");
        }

        [Obsolete("use GetDefaultRequestHandler()")]
        public async Task<IActionResult> DefaultDelete<TDeleteId>(
            TDeleteId deleteId,
            Func<IDanceCompetitionHelper, TDeleteId, CancellationToken, Task<object?>> funcDelete,
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
                (routeObjects, cToken) => Task.FromResult<IActionResult>(
                    RedirectToAction(
                        viewNameSuccess,
                        routeObjects)),
                (routeObjects, cToken) => Task.FromResult<IActionResult>(
                    RedirectToAction(
                        viewNameOnNoData,
                        routeObjects)),
                (exc, routeObjects, cToken) => Task.FromResult<IActionResult>(
                    RedirectToAction(
                        viewNameOnError,
                        routeObjects)),
                cancellationToken))
                ?? Error(
                    "Delete failed");
        }

        public async Task<Competition?> DefaultGetCompetitionAndSetViewData(
            IDanceCompetitionHelper danceCompetitionHelper,
            Guid? competitionId,
            CancellationToken cancellationToken)
        {
            var foundComp = await danceCompetitionHelper.FindCompetitionAsync(
                competitionId ?? Guid.Empty,
                cancellationToken);

            if (foundComp == null)
            {
                return null;
            }

            ViewData["Use" + nameof(CompetitionClass)] = foundComp.CompetitionId;

            return foundComp;
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
