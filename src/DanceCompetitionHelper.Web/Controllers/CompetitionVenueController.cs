using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Web.Models.CompetitionVenueModels;
using Microsoft.AspNetCore.Mvc;

namespace DanceCompetitionHelper.Web.Controllers
{
    public class CompetitionVenueController : ControllerBase
    {
        public const string RefName = "CompetitionVenue";

        private readonly IDanceCompetitionHelper _danceCompHelper;
        private readonly ILogger<CompetitionVenueController> _logger;
        private readonly IServiceProvider _serviceProvider;

        public CompetitionVenueController(
            IDanceCompetitionHelper danceCompHelper,
            ILogger<CompetitionVenueController> logger,
            IServiceProvider serviceProvider)
        {
            _danceCompHelper = danceCompHelper
                ?? throw new ArgumentNullException(
                    nameof(danceCompHelper));
            _logger = logger
                ?? throw new ArgumentNullException(
                    nameof(logger));
            _serviceProvider = serviceProvider
                ?? throw new ArgumentNullException(
                    nameof(serviceProvider));
        }

        public async Task<IActionResult> Index(
            Guid id,
            CancellationToken cancellationToken)
        {
            var foundCompId = await _danceCompHelper.FindCompetitionAsync(
                id,
                cancellationToken);

            ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

            return View(
                new CompetitionVenueOverviewViewModel()
                {
                    Competition = await _danceCompHelper.GetCompetitionAsync(
                        foundCompId,
                        cancellationToken),
                    OverviewItems = await _danceCompHelper
                        .GetCompetitionVenuesAsync(
                            foundCompId,
                            cancellationToken)
                        .ToListAsync(
                            cancellationToken),
                });
        }

        public async Task<IActionResult> ShowCreateEdit(
            Guid id,
            CancellationToken cancellationToken)
        {
            var foundCompId = await _danceCompHelper.FindCompetitionAsync(
                id,
                cancellationToken);

            if (foundCompId == null)
            {
                return NotFound();
            }

            var foundComp = await _danceCompHelper.GetCompetitionAsync(
                foundCompId,
                cancellationToken);

            ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

            return View(
                new CompetitionVenueViewModel()
                {
                    CompetitionId = foundCompId.Value,
                    CompetitionName = foundComp.GetCompetitionName(),
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNew(
            CompetitionVenueViewModel createCompetitionVenue,
            CancellationToken cancellationToken)
        {
            if (ModelState.IsValid == false)
            {
                createCompetitionVenue.AddErrors(
                    ModelState);

                return View(
                    nameof(ShowCreateEdit),
                    createCompetitionVenue);
            }

            try
            {
                var useCompetitionId = createCompetitionVenue.CompetitionId;

                await _danceCompHelper.CreateCompetitionVenueAsync(
                    useCompetitionId,
                    createCompetitionVenue.Name,
                    createCompetitionVenue.LengthInMeter,
                    createCompetitionVenue.WidthInMeter,
                    createCompetitionVenue.Comment,
                    cancellationToken);

                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        Id = useCompetitionId,
                    });
            }
            catch (Exception exc)
            {
                createCompetitionVenue.AddErrors(
                    exc);

                return View(
                    nameof(ShowCreateEdit),
                    createCompetitionVenue);
            }
        }

        public async Task<IActionResult> ShowEdit(
            Guid id,
            CancellationToken cancellationToken)
        {
            var foundCompVenue = await _danceCompHelper.GetCompetitionVenueAsync(
                id,
                cancellationToken);

            if (foundCompVenue == null)
            {
                return RedirectToAction(
                    nameof(Index));
            }

            ViewData["Use" + nameof(CompetitionClass)] = foundCompVenue.CompetitionId;

            return View(
                nameof(ShowCreateEdit),
                new CompetitionVenueViewModel()
                {
                    CompetitionVenueId = foundCompVenue.CompetitionVenueId,

                    CompetitionId = foundCompVenue.CompetitionId,
                    CompetitionName = foundCompVenue.Competition.GetCompetitionName(),

                    Name = foundCompVenue.Name,
                    LengthInMeter = foundCompVenue.LengthInMeter,
                    WidthInMeter = foundCompVenue.WidthInMeter,
                    Comment = foundCompVenue.Comment,
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSave(
            CompetitionVenueViewModel editCompetitionVenue,
            CancellationToken cancellationToken)
        {
            if (ModelState.IsValid == false)
            {
                editCompetitionVenue.AddErrors(
                    ModelState);

                return View(
                    nameof(ShowCreateEdit),
                    editCompetitionVenue);
            }

            try
            {
                await _danceCompHelper.EditCompetitionVenueAsync(
                    editCompetitionVenue.CompetitionVenueId ?? Guid.Empty,
                    editCompetitionVenue.Name,
                    editCompetitionVenue.LengthInMeter,
                    editCompetitionVenue.WidthInMeter,
                    editCompetitionVenue.Comment,
                    cancellationToken);

                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        Id = editCompetitionVenue.CompetitionId,
                    });
            }
            catch (Exception exc)
            {
                editCompetitionVenue.AddErrors(
                    exc);

                return View(
                    nameof(ShowCreateEdit),
                    editCompetitionVenue);
            }
        }

        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            var helpCompId = await _danceCompHelper.FindCompetitionAsync(
                id,
                cancellationToken);

            await _danceCompHelper.RemoveCompetitionVenueAsync(
                id,
                cancellationToken);

            return RedirectToAction(
                nameof(Index),
                new
                {
                    Id = helpCompId ?? Guid.Empty
                });
        }
    }
}
