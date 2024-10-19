using AutoMapper;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Exceptions;
using DanceCompetitionHelper.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DanceCompetitionHelper.Web.Helper.Request
{
    public class DefaultRequestHandler<TLogger, TEntity, TModel> : Controller
        where TEntity : TableBase
        where TModel : ViewModelBase
    {
        protected readonly IDanceCompetitionHelper _danceCompHelper;
        protected readonly ILogger<TLogger> _logger;
        protected readonly IMapper _mapper;

        public string? ViewOnSuccess { get; private set; }
        public Func<TModel, IDanceCompetitionHelper, IMapper, CancellationToken, Task<object?>>? FuncOnSuccess { get; private set; }

        public string? ViewOnError { get; private set; }
        public Func<TModel, IDanceCompetitionHelper, IMapper, CancellationToken, Task<object?>>? FuncOnError { get; private set; }

        public string? ViewOnModelStateInvalid { get; private set; }
        public Func<TModel, IDanceCompetitionHelper, IMapper, CancellationToken, Task<object?>>? FuncOnModelStateInvalid { get; private set; }

        public string? ViewOnNoData { get; private set; }
        public object? RouteValuesOnNoData { get; private set; }

        public DefaultRequestHandler(
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

        public DefaultRequestHandler<TLogger, TEntity, TModel> SetOnSuccess(
            string viewOnSuccess,
            Func<TModel, IDanceCompetitionHelper, IMapper, CancellationToken, Task<object?>>? funcOnSuccess = null)
        {
            ViewOnSuccess = viewOnSuccess;
            FuncOnSuccess = funcOnSuccess;

            return this;
        }

        public DefaultRequestHandler<TLogger, TEntity, TModel> SetOnError(
            string viewOnError,
            Func<TModel, IDanceCompetitionHelper, IMapper, CancellationToken, Task<object?>>? funcOnError = null)
        {
            ViewOnError = viewOnError;
            FuncOnError = funcOnError;

            return this;
        }

        public DefaultRequestHandler<TLogger, TEntity, TModel> SetOnModelStateInvalid(
            string viewOnModelStateInvalid,
            Func<TModel, IDanceCompetitionHelper, IMapper, CancellationToken, Task<object?>>? funcOnModelStateInvalid = null)
        {
            ViewOnModelStateInvalid = viewOnModelStateInvalid;
            FuncOnModelStateInvalid = funcOnModelStateInvalid;

            return this;
        }

        public DefaultRequestHandler<TLogger, TEntity, TModel> SetOnNoData(
            string viewOnNoData,
            object? routeValuesOnNoData = null)
        {
            ViewOnNoData = viewOnNoData;
            RouteValuesOnNoData = routeValuesOnNoData;

            return this;
        }

        #region Methods

        public async Task<IActionResult> DefaultIndexAsync(
            Func<IDanceCompetitionHelper, IMapper, CancellationToken, Task<IEnumerable<TEntity>>> funcIndex,
            CancellationToken cancellationToken,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (string.IsNullOrEmpty(
                ViewOnSuccess))
            {
                throw new ArgumentNullException(
                    nameof(ViewOnSuccess));
            }

            if (string.IsNullOrEmpty(
                ViewOnNoData))
            {
                throw new ArgumentNullException(
                    nameof(ViewOnNoData));
            }

            if (funcIndex == null)
            {
                throw new ArgumentNullException(
                    nameof(funcIndex));
            }

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
                               ViewOnNoData,
                               RouteValuesOnNoData);
                        }

                        return View(
                            ViewOnSuccess,
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
                            ViewOnNoData,
                            RouteValuesOnNoData);
                    }
                },
                cancellationToken))
                ?? Error(
                    $"Read for '{ViewOnSuccess}' failed!");
        }

        public async Task<IActionResult> DefaultShowAsync(
            Func<IDanceCompetitionHelper, IMapper, CancellationToken, Task<TEntity>> funcShow,
            CancellationToken cancellationToken,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (string.IsNullOrEmpty(
                ViewOnSuccess))
            {
                throw new ArgumentNullException(
                    nameof(ViewOnSuccess));
            }

            if (string.IsNullOrEmpty(
                ViewOnNoData))
            {
                throw new ArgumentNullException(
                    nameof(ViewOnNoData));
            }

            if (funcShow == null)
            {
                throw new ArgumentNullException(
                    nameof(funcShow));
            }

            return (await _danceCompHelper.RunInReadonlyTransaction<IActionResult>(
                async (dcH, _, _, cToken) =>
                {
                    try
                    {
                        var foundData = await funcShow(
                            dcH,
                            _mapper,
                            cToken);

                        if (foundData == null)
                        {
                            return RedirectToAction(
                               ViewOnNoData,
                               RouteValuesOnNoData);
                        }

                        return View(
                            ViewOnSuccess,
                            _mapper.Map<TModel>(
                                foundData));
                    }
                    catch (NoDataFoundException noDataExc)
                    {
                        _logger.LogWarning(
                            noDataExc,
                            "No Data during {MemberName}(): {Message}",
                            memberName,
                            noDataExc.Message);

                        return RedirectToAction(
                            ViewOnNoData,
                            RouteValuesOnNoData);
                    }
                },
                cancellationToken))
                ?? Error(
                    $"Read for '{ViewOnSuccess}' failed!");
        }

        public async Task<IActionResult> DefaultCreateNewAsync(
            TModel modelView,
            TEntity newEntity,
            /*
            Func<IDanceCompetitionHelper, CancellationToken, Task>? funcOnModelStateInvalid,
            string viewNameModelStateInvalid,
            */
            Func<IDanceCompetitionHelper, TEntity, IMapper, CancellationToken, Task<object?>> funcCreateNew,
            /*
            string viewNameSuccess,
            Func<IDanceCompetitionHelper, TModel, CancellationToken, Task>? funcOnError,
            string viewNameOnError,
            */
            CancellationToken cancellationToken,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (string.IsNullOrEmpty(
                ViewOnSuccess))
            {
                throw new ArgumentNullException(
                    nameof(ViewOnSuccess));
            }
            if (string.IsNullOrEmpty(
                ViewOnError))
            {
                throw new ArgumentNullException(
                    nameof(ViewOnError));
            }

            if (string.IsNullOrEmpty(
                ViewOnModelStateInvalid))
            {
                throw new ArgumentNullException(
                    nameof(ViewOnModelStateInvalid));
            }

            /*
            if (FuncOnModelStateInvalid == null)
            {
                throw new ArgumentNullException(
                    nameof(FuncOnModelStateInvalid));
            }
            */


            if (ModelState.IsValid == false)
            {
                modelView.AddErrors(
                    ModelState);

                if (FuncOnModelStateInvalid != null)
                {
                    await FuncOnModelStateInvalid(
                        modelView,
                        _danceCompHelper,
                        _mapper,
                        cancellationToken);
                }

                return View(
                    ViewOnModelStateInvalid,
                    modelView);
            }

            return (await _danceCompHelper.RunInTransactionWithSaveChangesAndCommit<IActionResult>(
                (dcH, _, _, cToken) => funcCreateNew(
                    dcH,
                    newEntity,
                    _mapper,
                    cToken),
                (routeObjects, cToken) => Task.FromResult<IActionResult>(
                    RedirectToAction(
                        ViewOnSuccess,
                        routeObjects)),
                null,
                async (exc, routeObjects, cToken) =>
                {
                    modelView.AddErrors(
                        exc);

                    if (FuncOnError != null)
                    {
                        var routeObjectsError = await FuncOnError.Invoke(
                            modelView,
                            _danceCompHelper,
                            _mapper,
                            cToken);
                    }

                    return View(
                        ViewOnError,
                        modelView
                        /* TODO: needed?.. 
                        routeObjectsError ?? routeObjects
                        */);
                },
                cancellationToken))
                ?? Error(
                    "Creation failed");
        }

        #endregion Methods

        #region Helper Methods

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

        #endregion Helper Methods
    }
}
