using DanceCompetitionHelper.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace DanceCompetitionHelper.Web.Controllers
{
    public class ParticipantController : Controller
    {
        public const string RefName = "Participant";

        private readonly IDanceCompetitionHelper _danceCompHelper;
        private readonly ILogger<CompetitionController> _logger;

        public ParticipantController(
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
            var foundCompId = _danceCompHelper.FindCompetition(
                id);

            ViewData["BackTo" + CompetitionClassController.RefName] = foundCompId;
            ViewData[nameof(CompetitionClassController.ShowMultipleStarters)] = foundCompId;

            return View(
                new ParticipantOverviewViewModel()
                {
                    Competition = _danceCompHelper.GetCompetition(
                        foundCompId ?? Guid.Empty),
                    Participtans = _danceCompHelper
                        .GetParticipants(
                            foundCompId,
                            null,
                            true)
                        .ToList(),
                });
        }
    }
}
