using DanceCompetitionHelper.Database;
using DanceCompetitionHelper.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics;

namespace DanceCompetitionHelper.Web.Controllers
{
    public abstract class ControllerBase : Controller
    {
        public async Task<IActionResult> DefaultIndex<T>(
            IDanceCompetitionHelper _danceCompHelper,
            Func<IDanceCompetitionHelper, DanceCompetitionHelperDbContext, IDbContextTransaction, CancellationToken, Task<T>> funcIndex,
            string viewName,
            CancellationToken cancellationToken)
        {
            return (await _danceCompHelper.RunInReadonlyTransaction(
                async (dcH, dbCtx, dbTrans, cToken) =>
                {
                    return View(
                        viewName,
                        await funcIndex(
                            dcH,
                            dbCtx,
                            dbTrans,
                            cToken));
                },
                cancellationToken))
                ?? Error(
                    $"Read for {viewName} failed!");
        }

        public async Task<IActionResult> DefaultCreateNew<TVm>(
            IDanceCompetitionHelper _danceCompHelper,
            TVm movelView,
            Func<IDanceCompetitionHelper, CancellationToken, Task>? funcOnModelStateInvalid,
            string viewNameModelStateInvalid,
            Func<IDanceCompetitionHelper, DanceCompetitionHelperDbContext, IDbContextTransaction, CancellationToken, Task> funcCreateNew,
            string viewNameSuccess,
            string viewNameOnError,
            CancellationToken cancellationToken)
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
                (dcH, dbCtx, dbTrans, cToken) => funcCreateNew(
                    dcH,
                    dbCtx,
                    dbTrans,
                    cToken),
                (cToken) => RedirectToAction(
                    viewNameSuccess),
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
