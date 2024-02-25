using DanceCompetitionHelper.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DanceCompetitionHelper.Web.Controllers
{
    public abstract class ControllerBase : Controller
    {
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
