﻿using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Web.Extensions;
using DanceCompetitionHelper.Web.Models.CompetitionClassModels;
using Microsoft.AspNetCore.Mvc;

namespace DanceCompetitionHelper.Web.Controllers
{
    public class CompetitionClassController : Controller
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

        public IActionResult Index(
            Guid id)
        {
            var foundCompId = _danceCompHelper.FindCompetition(
                id);

            ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

            return View(
                new CompetitionClassOverviewViewModel()
                {
                    Competition = _danceCompHelper.GetCompetition(
                        foundCompId),
                    OverviewItems = _danceCompHelper
                        .GetCompetitionClasses(
                            foundCompId,
                            true)
                        .ToList(),
                });
        }

        public IActionResult DetailedView(
            Guid id)
        {
            var foundCompId = _danceCompHelper.FindCompetition(
                id);

            ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

            return View(
                nameof(Index),
                new CompetitionClassOverviewViewModel()
                {
                    Competition = _danceCompHelper.GetCompetition(
                        foundCompId),
                    OverviewItems = _danceCompHelper
                        .GetCompetitionClasses(
                            foundCompId,
                            true,
                            true)
                        .ToList(),
                    DetailedView = true,
                });
        }

        public IActionResult ShowCreateEdit(
            Guid id)
        {
            var foundCompId = _danceCompHelper.FindCompetition(
                id);

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
                    AdjudicatorPanels = _danceCompHelper
                        .GetAdjudicatorPanels(
                            foundCompId)
                        .ToSelectListItem(
                            lastCreatedAdjudicatorPanelId),
                    FollowUpCompetitionClasses = _danceCompHelper
                        .GetCompetitionClasses(
                            foundCompId.Value)
                        .ToSelectListItem(
                            addEmpty: true)
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNew(
            CompetitionClassViewModel createCompetition)
        {
            if (ModelState.IsValid == false)
            {
                createCompetition.Errors = ModelState.GetErrorMessages();

                createCompetition.FollowUpCompetitionClassId = createCompetition.FollowUpCompetitionClassId;
                createCompetition.FollowUpCompetitionClasses = _danceCompHelper
                    .GetCompetitionClasses(
                        createCompetition.CompetitionId)
                    .ToSelectListItem(
                        createCompetition.FollowUpCompetitionClassId,
                        addEmpty: true);

                createCompetition.AdjudicatorPanelId = createCompetition.AdjudicatorPanelId;
                createCompetition.AdjudicatorPanels = _danceCompHelper
                    .GetAdjudicatorPanels(
                        createCompetition.CompetitionId)
                    .ToSelectListItem(
                        createCompetition.AdjudicatorPanelId);

                return View(
                    nameof(ShowCreateEdit),
                    createCompetition);
            }

            try
            {
                var useAdjudicatorPanelId = createCompetition.AdjudicatorPanelId;

                _danceCompHelper.CreateCompetitionClass(
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
                    createCompetition.CompetitionColor);

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
                createCompetition.Errors = exc.InnerException?.Message ?? exc.Message;

                createCompetition.FollowUpCompetitionClassId = createCompetition.FollowUpCompetitionClassId;
                createCompetition.FollowUpCompetitionClasses = _danceCompHelper
                    .GetCompetitionClasses(
                        createCompetition.CompetitionId)
                    .ToSelectListItem(
                        createCompetition.FollowUpCompetitionClassId,
                        addEmpty: true);

                createCompetition.AdjudicatorPanelId = createCompetition.AdjudicatorPanelId;
                createCompetition.AdjudicatorPanels = _danceCompHelper
                    .GetAdjudicatorPanels(
                        createCompetition.CompetitionId)
                    .ToSelectListItem(
                        createCompetition.AdjudicatorPanelId);

                return View(
                    nameof(ShowCreateEdit),
                    createCompetition);
            }
        }

        public IActionResult ShowEdit(
            Guid id)
        {
            var foundCompClass = _danceCompHelper.GetCompetitionClass(
                id);

            if (foundCompClass == null)
            {
                var helpCompClassId = _danceCompHelper.FindCompetition(
                    id);

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
                    FollowUpCompetitionClasses = _danceCompHelper
                        .GetCompetitionClasses(
                            foundCompClass.CompetitionId)
                        .ToSelectListItem(
                            foundCompClass.FollowUpCompetitionClassId,
                            addEmpty: true),
                    AdjudicatorPanelId = foundCompClass.AdjudicatorPanelId,
                    AdjudicatorPanels = _danceCompHelper
                        .GetAdjudicatorPanels(
                            foundCompClass.CompetitionId)
                        .ToSelectListItem(
                            foundCompClass.AdjudicatorPanelId),
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
        public IActionResult EditSave(
            CompetitionClassViewModel editCompetitionClass)
        {
            if (ModelState.IsValid == false)
            {
                editCompetitionClass.Errors = ModelState.GetErrorMessages();

                editCompetitionClass.AdjudicatorPanels = _danceCompHelper
                    .GetAdjudicatorPanels(
                        editCompetitionClass.CompetitionId)
                    .ToSelectListItem(
                        editCompetitionClass.AdjudicatorPanelId);

                return View(
                    nameof(ShowCreateEdit),
                    editCompetitionClass);
            }

            try
            {
                _danceCompHelper.EditCompetitionClass(
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
                    editCompetitionClass.CompetitionColor);

                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        Id = editCompetitionClass.CompetitionId
                    });
            }
            catch (Exception exc)
            {
                editCompetitionClass.Errors = exc.InnerException?.Message ?? exc.Message;

                editCompetitionClass.FollowUpCompetitionClassId = editCompetitionClass.FollowUpCompetitionClassId;
                editCompetitionClass.FollowUpCompetitionClasses = _danceCompHelper
                    .GetCompetitionClasses(
                        editCompetitionClass.CompetitionId)
                    .ToSelectListItem(
                        editCompetitionClass.FollowUpCompetitionClassId,
                        addEmpty: true);

                editCompetitionClass.AdjudicatorPanelId = editCompetitionClass.AdjudicatorPanelId;
                editCompetitionClass.AdjudicatorPanels = _danceCompHelper
                    .GetAdjudicatorPanels(
                        editCompetitionClass.CompetitionId)
                    .ToSelectListItem(
                        editCompetitionClass.AdjudicatorPanelId);

                return View(
                    nameof(ShowCreateEdit),
                    editCompetitionClass);
            }
        }

        public IActionResult Delete(
            Guid id)
        {
            var helpCompClassId = _danceCompHelper.FindCompetition(
                    id);

            _danceCompHelper.RemoveCompetitionClass(
                id);

            return RedirectToAction(
                nameof(Index),
                new
                {
                    Id = helpCompClassId ?? Guid.Empty
                });
        }

        public IActionResult ShowMultipleStarters(
            Guid id)
        {
            var foundCompId = _danceCompHelper.FindCompetition(
                    id);

            if (foundCompId == null)
            {
                return NotFound();
            }

            var helpComp = _danceCompHelper.GetCompetition(
                foundCompId);

            ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

            return View(
                new ShowMultipleStartersOverviewViewModel()
                {
                    Competition = helpComp,
                    OverviewItems = _danceCompHelper
                        .GetMultipleStarter(
                            foundCompId.Value)
                        .ToList(),
                });
        }

        public IActionResult ShowPossiblePromotions(
            Guid id)
        {
            var foundCompId = _danceCompHelper.FindCompetition(
                id);

            if (foundCompId == null)
            {
                return NotFound();
            }

            var helpComp = _danceCompHelper.GetCompetition(
                foundCompId);

            ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

            return View(
                new ShowPossiblePromotionsViewModel()
                {
                    Competition = helpComp,
                    OverviewItems = _danceCompHelper
                        .GetParticipants(
                            foundCompId.Value,
                            null,
                            true)
                        .Where(
                            x => x.DisplayInfo != null
                            && x.DisplayInfo.PromotionInfo != null
                            && x.DisplayInfo.PromotionInfo.PossiblePromotion)
                        .OrderBy(
                            x => x.NamePartA)
                        .ThenBy(
                            x => x.NamePartB)
                        .ToList(),
                    MultipleStarters = _danceCompHelper
                        .GetMultipleStarter(
                            foundCompId.Value)
                        .ToList(),
                });
        }
    }
}
