using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Web.Extensions;
using DanceCompetitionHelper.Web.Models.CompetitionClassModels;
using Microsoft.AspNetCore.Mvc;

namespace DanceCompetitionHelper.Web.Controllers
{
    public class CompetitionClassController : ControllerBase
    {
        public const string RefName = "CompetitionClass";
        public const string CompetitionClassLastCreatedAdjudicatorPanelId = nameof(CompetitionClassLastCreatedAdjudicatorPanelId);
        public const string DefaultCompetitionColor = "#ffffff";

        private readonly IDanceCompetitionHelper _danceCompHelper;
        private readonly ILogger<CompetitionController> _logger;
        private readonly IServiceProvider _serviceProvider;

        public CompetitionClassController(
            IDanceCompetitionHelper danceCompHelper,
            ILogger<CompetitionController> logger,
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
                new CompetitionClassOverviewViewModel()
                {
                    Competition = await _danceCompHelper.GetCompetitionAsync(
                        foundCompId,
                        cancellationToken),
                    OverviewItems = await _danceCompHelper
                        .GetCompetitionClassesAsync(
                            foundCompId,
                            cancellationToken,
                            true)
                        .ToListAsync(),
                });
        }

        public async Task<IActionResult> DetailedView(
            Guid id,
            CancellationToken cancellationToken)
        {
            var foundCompId = await _danceCompHelper.FindCompetitionAsync(
                id,
                cancellationToken);

            ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

            return View(
                nameof(Index),
                new CompetitionClassOverviewViewModel()
                {
                    Competition = await _danceCompHelper.GetCompetitionAsync(
                        foundCompId,
                        cancellationToken),
                    OverviewItems = await _danceCompHelper
                        .GetCompetitionClassesAsync(
                            foundCompId,
                            cancellationToken,
                            true,
                            true)
                        .ToListAsync(
                            cancellationToken),
                    DetailedView = true,
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

            ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

            _ = Guid.TryParse(
                HttpContext.Session.GetString(
                    CompetitionClassLastCreatedAdjudicatorPanelId),
                out var lastCreatedAdjudicatorPanelId);

            return View(
                new CompetitionClassViewModel()
                {
                    CompetitionId = foundCompId.Value,
                    AdjudicatorPanels = await _danceCompHelper
                        .GetAdjudicatorPanelsAsync(
                            foundCompId,
                            cancellationToken)
                        .ToSelectListItemAsync(
                            lastCreatedAdjudicatorPanelId)
                        .ToListAsync(
                            cancellationToken),
                    FollowUpCompetitionClasses = await _danceCompHelper
                        .GetCompetitionClassesAsync(
                            foundCompId.Value,
                            cancellationToken)
                        .ToSelectListItemAsync(
                            addEmpty: true)
                        .ToListAsync(
                            cancellationToken)
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNew(
            CompetitionClassViewModel createCompetition,
            CancellationToken cancellationToken)
        {
            if (ModelState.IsValid == false)
            {
                createCompetition.AddErrors(
                    ModelState);

                createCompetition.FollowUpCompetitionClassId = createCompetition.FollowUpCompetitionClassId;
                createCompetition.FollowUpCompetitionClasses = await _danceCompHelper
                    .GetCompetitionClassesAsync(
                        createCompetition.CompetitionId,
                        cancellationToken)
                    .ToSelectListItemAsync(
                        createCompetition.FollowUpCompetitionClassId,
                        addEmpty: true)
                    .ToListAsync();

                createCompetition.AdjudicatorPanelId = createCompetition.AdjudicatorPanelId;
                createCompetition.AdjudicatorPanels = await _danceCompHelper
                    .GetAdjudicatorPanelsAsync(
                        createCompetition.CompetitionId,
                        cancellationToken)
                    .ToSelectListItemAsync(
                        createCompetition.AdjudicatorPanelId)
                    .ToListAsync(
                        cancellationToken);

                return View(
                    nameof(ShowCreateEdit),
                    createCompetition);
            }

            try
            {
                var useAdjudicatorPanelId = createCompetition.AdjudicatorPanelId;

                await _danceCompHelper.CreateCompetitionClassAsync(
                    createCompetition.CompetitionId,
                    createCompetition.CompetitionClassName,
                    createCompetition.FollowUpCompetitionClassId,
                    useAdjudicatorPanelId,
                    createCompetition.OrgClassId,
                    createCompetition.Discipline,
                    createCompetition.AgeClass,
                    createCompetition.AgeGroup,
                    createCompetition.Class,
                    createCompetition.MinStartsForPromotion,
                    createCompetition.MinPointsForPromotion,
                    createCompetition.PointsForFirst,
                    createCompetition.ExtraManualStarter,
                    createCompetition.Comment,
                    createCompetition.Ignore,
                    createCompetition.CompetitionColor,
                    cancellationToken);

                HttpContext.Session.SetString(
                    CompetitionClassLastCreatedAdjudicatorPanelId,
                    useAdjudicatorPanelId.ToString());

                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        Id = createCompetition.CompetitionId
                    });
            }
            catch (Exception exc)
            {
                createCompetition.AddErrors(
                    exc);

                createCompetition.FollowUpCompetitionClassId = createCompetition.FollowUpCompetitionClassId;
                createCompetition.FollowUpCompetitionClasses = await _danceCompHelper
                    .GetCompetitionClassesAsync(
                        createCompetition.CompetitionId,
                        cancellationToken)
                    .ToSelectListItemAsync(
                        createCompetition.FollowUpCompetitionClassId,
                        addEmpty: true)
                    .ToListAsync();

                createCompetition.AdjudicatorPanelId = createCompetition.AdjudicatorPanelId;
                createCompetition.AdjudicatorPanels = await _danceCompHelper
                    .GetAdjudicatorPanelsAsync(
                        createCompetition.CompetitionId,
                        cancellationToken)
                    .ToSelectListItemAsync(
                        createCompetition.AdjudicatorPanelId)
                    .ToListAsync(
                        cancellationToken);

                return View(
                    nameof(ShowCreateEdit),
                    createCompetition);
            }
        }

        public async Task<IActionResult> ShowEdit(
            Guid id,
            CancellationToken cancellationToken)
        {
            var foundCompClass = await _danceCompHelper.GetCompetitionClassAsync(
                id,
                cancellationToken);

            if (foundCompClass == null)
            {
                var helpCompClassId = await _danceCompHelper.FindCompetitionAsync(
                    id,
                    cancellationToken);

                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        Id = helpCompClassId ?? Guid.Empty
                    });
            }

            ViewData["Use" + nameof(CompetitionClass)] = foundCompClass.CompetitionId;

            return View(
                nameof(ShowCreateEdit),
                new CompetitionClassViewModel()
                {
                    CompetitionId = foundCompClass.CompetitionId,
                    CompetitionClassId = foundCompClass.CompetitionClassId,
                    CompetitionClassName = foundCompClass.CompetitionClassName,

                    FollowUpCompetitionClassId = foundCompClass.FollowUpCompetitionClassId,
                    FollowUpCompetitionClasses = await _danceCompHelper
                        .GetCompetitionClassesAsync(
                            foundCompClass.CompetitionId,
                            cancellationToken)
                        .ToSelectListItemAsync(
                            foundCompClass.FollowUpCompetitionClassId,
                            addEmpty: true)
                        .ToListAsync(),
                    AdjudicatorPanelId = foundCompClass.AdjudicatorPanelId,
                    AdjudicatorPanels = await _danceCompHelper
                        .GetAdjudicatorPanelsAsync(
                            foundCompClass.CompetitionId,
                            cancellationToken)
                        .ToSelectListItemAsync(
                            foundCompClass.AdjudicatorPanelId)
                        .ToListAsync(
                            cancellationToken),
                    OrgClassId = foundCompClass.OrgClassId,
                    Discipline = foundCompClass.Discipline,
                    AgeClass = foundCompClass.AgeClass,
                    AgeGroup = foundCompClass.AgeGroup,
                    Class = foundCompClass.Class,
                    MinStartsForPromotion = foundCompClass.MinStartsForPromotion,
                    MinPointsForPromotion = foundCompClass.MinPointsForPromotion,
                    PointsForFirst = foundCompClass.PointsForFirst,
                    ExtraManualStarter = foundCompClass.ExtraManualStarter,
                    Comment = foundCompClass.Comment,
                    Ignore = foundCompClass.Ignore,
                    CompetitionColor = foundCompClass.CompetitionColor ?? DefaultCompetitionColor,
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSave(
            CompetitionClassViewModel editCompetitionClass,
            CancellationToken cancellationToken)
        {
            if (ModelState.IsValid == false)
            {
                editCompetitionClass.AddErrors(
                    ModelState);

                editCompetitionClass.AdjudicatorPanels = await _danceCompHelper
                    .GetAdjudicatorPanelsAsync(
                        editCompetitionClass.CompetitionId,
                        cancellationToken)
                    .ToSelectListItemAsync(
                        editCompetitionClass.AdjudicatorPanelId)
                    .ToListAsync(
                        cancellationToken);

                return View(
                    nameof(ShowCreateEdit),
                    editCompetitionClass);
            }

            try
            {
                await _danceCompHelper.EditCompetitionClassAsync(
                    editCompetitionClass.CompetitionClassId ?? Guid.Empty,
                    editCompetitionClass.CompetitionClassName,
                    editCompetitionClass.FollowUpCompetitionClassId,
                    editCompetitionClass.AdjudicatorPanelId,
                    editCompetitionClass.OrgClassId,
                    editCompetitionClass.Discipline,
                    editCompetitionClass.AgeClass,
                    editCompetitionClass.AgeGroup,
                    editCompetitionClass.Class,
                    editCompetitionClass.MinStartsForPromotion,
                    editCompetitionClass.MinPointsForPromotion,
                    editCompetitionClass.PointsForFirst,
                    editCompetitionClass.ExtraManualStarter,
                    editCompetitionClass.Comment,
                    editCompetitionClass.Ignore,
                    editCompetitionClass.CompetitionColor,
                    cancellationToken);

                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        Id = editCompetitionClass.CompetitionId
                    });
            }
            catch (Exception exc)
            {
                editCompetitionClass.AddErrors(
                    exc);

                editCompetitionClass.FollowUpCompetitionClassId = editCompetitionClass.FollowUpCompetitionClassId;
                editCompetitionClass.FollowUpCompetitionClasses = await _danceCompHelper
                    .GetCompetitionClassesAsync(
                        editCompetitionClass.CompetitionId,
                        cancellationToken)
                    .ToSelectListItemAsync(
                        editCompetitionClass.FollowUpCompetitionClassId,
                        addEmpty: true)
                    .ToListAsync();

                editCompetitionClass.AdjudicatorPanelId = editCompetitionClass.AdjudicatorPanelId;
                editCompetitionClass.AdjudicatorPanels = await _danceCompHelper
                    .GetAdjudicatorPanelsAsync(
                        editCompetitionClass.CompetitionId,
                        cancellationToken)
                    .ToSelectListItemAsync(
                        editCompetitionClass.AdjudicatorPanelId)
                    .ToListAsync(
                        cancellationToken);

                return View(
                    nameof(ShowCreateEdit),
                    editCompetitionClass);
            }
        }

        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            var helpCompClassId = await _danceCompHelper.FindCompetitionAsync(
                id,
                cancellationToken);

            await _danceCompHelper.RemoveCompetitionClassAsync(
                id,
                cancellationToken);

            return RedirectToAction(
                nameof(Index),
                new
                {
                    Id = helpCompClassId ?? Guid.Empty
                });
        }

        public async Task<IActionResult> ShowMultipleStarters(
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

            var helpComp = await _danceCompHelper.GetCompetitionAsync(
                foundCompId,
                cancellationToken);

            ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

            return View(
                new ShowMultipleStartersOverviewViewModel()
                {
                    Competition = helpComp,
                    OverviewItems = await _danceCompHelper
                        .GetMultipleStarterAsync(
                            foundCompId.Value,
                            cancellationToken)
                        .ToListAsync(),
                });
        }

        public async Task<IActionResult> ShowPossiblePromotions(
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

            var helpComp = await _danceCompHelper.GetCompetitionAsync(
                foundCompId,
                cancellationToken);

            ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

            return View(
                new ShowPossiblePromotionsViewModel()
                {
                    Competition = helpComp,
                    OverviewItems = await _danceCompHelper
                        .GetParticipantsAsync(
                            foundCompId.Value,
                            null,
                            cancellationToken,
                            true)
                        .Where(
                            x => x.DisplayInfo != null
                            && x.DisplayInfo.PromotionInfo != null
                            && x.DisplayInfo.PromotionInfo.PossiblePromotion)
                        .OrderBy(
                            x => x.NamePartA)
                        .ThenBy(
                            x => x.NamePartB)
                        .ToListAsync(
                            cancellationToken),
                    MultipleStarters = await _danceCompHelper
                        .GetMultipleStarterAsync(
                            foundCompId.Value,
                            cancellationToken)
                        .ToListAsync(
                            cancellationToken),
                });
        }
    }
}
