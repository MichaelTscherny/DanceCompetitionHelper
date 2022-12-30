using DanceCompetitionHelper.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace DanceCompetitionHelper.Web.Controllers
{
    public class CompetitionClassController : Controller
    {
        public const string RefName = "CompetitionClass";

        private readonly IDanceCompetitionHelper _danceCompHelper;
        private readonly ILogger<CompetitionController> _logger;

        public CompetitionClassController(
            IDanceCompetitionHelper danceCompHelper,
            ILogger<CompetitionController> logger)
        {
            _danceCompHelper = danceCompHelper
                ?? throw new ArgumentNullException(
                    nameof(danceCompHelper));
            _logger = logger
                ?? throw new ArgumentNullException(
                    nameof(logger));
        }

        public IActionResult Index(
            Guid id)
        {
            return View(
                new CompetitionOverviewClassModel()
                {
                    Competition = _danceCompHelper.GetCompetition(
                        id),
                    CompetitionClasses = _danceCompHelper.GetCompetitionClasses(
                        // CAUTION: that's the CompetitionId!
                        id),
                });
        }
    }
}
