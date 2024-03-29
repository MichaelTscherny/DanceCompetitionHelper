﻿using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Web.Extensions;
using DanceCompetitionHelper.Web.Models.ParticipantModels;
using Microsoft.AspNetCore.Mvc;

namespace DanceCompetitionHelper.Web.Controllers
{
    public class ParticipantController : Controller
    {
        public const string RefName = "Participant";
        public const string ParticipantLastCreatedCompetitionClassId = nameof(ParticipantLastCreatedCompetitionClassId);

        private readonly IDanceCompetitionHelper _danceCompHelper;
        private readonly ILogger<CompetitionController> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ParticipantController(
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
                new ParticipantOverviewViewModel()
                {
                    Competition = _danceCompHelper.GetCompetition(
                        foundCompId),
                    OverviewItems = _danceCompHelper
                        .GetParticipants(
                            foundCompId,
                            null,
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
                new ParticipantOverviewViewModel()
                {
                    Competition = _danceCompHelper.GetCompetition(
                        foundCompId),
                    OverviewItems = _danceCompHelper
                        .GetParticipants(
                            foundCompId,
                            null,
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

            var foundComp = _danceCompHelper.GetCompetition(
                foundCompId);

            ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

            var helpCompName = string.Empty;

            _ = Guid.TryParse(
                HttpContext.Session.GetString(
                    ParticipantLastCreatedCompetitionClassId),
                out var lastCreatedClassId);

            return View(
                new ParticipantViewModel()
                {
                    CompetitionId = foundCompId.Value,
                    CompetitionName = foundComp.GetCompetitionName(),
                    CompetitionClasses = _danceCompHelper
                        .GetCompetitionClasses(
                            foundCompId)
                        .ToSelectListItem(
                            lastCreatedClassId),
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNew(
            ParticipantViewModel createParticipant)
        {
            if (ModelState.IsValid == false)
            {
                createParticipant.Errors = ModelState.GetErrorMessages();

                createParticipant.CompetitionClasses = _danceCompHelper
                    .GetCompetitionClasses(
                        createParticipant.CompetitionId)
                    .ToSelectListItem(
                        createParticipant.CompetitionClassId);

                return View(
                    nameof(ShowCreateEdit),
                    createParticipant);
            }

            try
            {
                var useCompetitionClassId = createParticipant.CompetitionClassId ?? Guid.Empty;

                _danceCompHelper.CreateParticipant(
                     createParticipant.CompetitionId,
                     useCompetitionClassId,
                     createParticipant.StartNumber,
                     // A
                     createParticipant.NamePartA,
                     createParticipant.OrgIdPartA,
                     // B
                     createParticipant.NamePartB,
                     createParticipant.OrgIdPartB,
                     createParticipant.ClubName,
                     createParticipant.OrgIdClub,
                     // A
                     createParticipant.OrgPointsPartA,
                     createParticipant.OrgStartsPartA,
                     createParticipant.MinStartsForPromotionPartA,
                     createParticipant.OrgAlreadyPromotedPartA,
                     createParticipant.OrgAlreadyPromotedInfoPartA,
                     // B
                     createParticipant.OrgPointsPartB,
                     createParticipant.OrgStartsPartB,
                     createParticipant.MinStartsForPromotionPartB,
                     createParticipant.OrgAlreadyPromotedPartB,
                     createParticipant.OrgAlreadyPromotedInfoPartB,
                     createParticipant.Comment,
                     createParticipant.Ignore);

                HttpContext.Session.SetString(
                    ParticipantLastCreatedCompetitionClassId,
                    useCompetitionClassId.ToString());

                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        Id = createParticipant.CompetitionId
                    });
            }
            catch (Exception exc)
            {
                createParticipant.Errors = exc.InnerException?.Message ?? exc.Message;

                createParticipant.CompetitionClasses = _danceCompHelper
                    .GetCompetitionClasses(
                        createParticipant.CompetitionId)
                    .ToSelectListItem(
                        createParticipant.CompetitionClassId);

                return View(
                    nameof(ShowCreateEdit),
                    createParticipant);
            }
        }

        public IActionResult ShowEdit(
            Guid id)
        {
            var foundParticipant = _danceCompHelper.GetParticipant(
                id);

            if (foundParticipant == null)
            {
                return RedirectToAction(
                    nameof(Index));
            }

            var foundComp = _danceCompHelper.GetCompetition(
                foundParticipant.CompetitionId);

            if (foundComp == null)
            {
                return NotFound();
            }

            ViewData["Use" + nameof(CompetitionClass)] = foundComp.CompetitionId;

            return View(
                nameof(ShowCreateEdit),
                new ParticipantViewModel()
                {
                    CompetitionId = foundParticipant.CompetitionId,
                    CompetitionName = foundComp.GetCompetitionName(),
                    CompetitionClassId = foundParticipant.CompetitionClassId,
                    ParticipantId = foundParticipant.ParticipantId,
                    StartNumber = foundParticipant.StartNumber,
                    // A
                    NamePartA = foundParticipant.NamePartA,
                    OrgIdPartA = foundParticipant.OrgIdPartA,
                    // B
                    NamePartB = foundParticipant.NamePartB,
                    OrgIdPartB = foundParticipant.OrgIdPartB,
                    ClubName = foundParticipant.ClubName,
                    OrgIdClub = foundParticipant.OrgIdClub,
                    // A
                    OrgPointsPartA = foundParticipant.OrgPointsPartA,
                    OrgStartsPartA = foundParticipant.OrgStartsPartA,
                    MinStartsForPromotionPartA = foundParticipant.MinStartsForPromotionPartA,
                    OrgAlreadyPromotedPartA = foundParticipant.OrgAlreadyPromotedPartA,
                    OrgAlreadyPromotedInfoPartA = foundParticipant.OrgAlreadyPromotedInfoPartA,
                    // B
                    OrgPointsPartB = foundParticipant.OrgPointsPartB,
                    OrgStartsPartB = foundParticipant.OrgStartsPartB,
                    MinStartsForPromotionPartB = foundParticipant.MinStartsForPromotionPartB,
                    OrgAlreadyPromotedPartB = foundParticipant.OrgAlreadyPromotedPartB,
                    OrgAlreadyPromotedInfoPartB = foundParticipant.OrgAlreadyPromotedInfoPartB,
                    Comment = foundParticipant.Comment,
                    Ignore = foundParticipant.Ignore,

                    CompetitionClasses = _danceCompHelper
                        .GetCompetitionClasses(
                            foundParticipant.CompetitionId)
                        .ToSelectListItem(
                            foundParticipant.CompetitionClassId),
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditSave(
            ParticipantViewModel editParticipant)
        {
            if (ModelState.IsValid == false)
            {
                editParticipant.Errors = ModelState.GetErrorMessages();

                return View(
                    nameof(ShowCreateEdit),
                    editParticipant);
            }

            try
            {
                _danceCompHelper.EditParticipant(
                    editParticipant.ParticipantId ?? Guid.Empty,
                    editParticipant.CompetitionClassId ?? Guid.Empty,
                    editParticipant.StartNumber,
                    // A
                    editParticipant.NamePartA,
                    editParticipant.OrgIdPartA,
                    // B
                    editParticipant.NamePartB,
                    editParticipant.OrgIdPartB,
                    editParticipant.ClubName,
                    editParticipant.OrgIdClub,
                    // A
                    editParticipant.OrgPointsPartA,
                    editParticipant.OrgStartsPartA,
                    editParticipant.MinStartsForPromotionPartA,
                    editParticipant.OrgAlreadyPromotedPartA,
                    editParticipant.OrgAlreadyPromotedInfoPartA,
                    // B
                    editParticipant.OrgPointsPartB,
                    editParticipant.OrgStartsPartB,
                    editParticipant.MinStartsForPromotionPartB,
                    editParticipant.OrgAlreadyPromotedPartB,
                    editParticipant.OrgAlreadyPromotedInfoPartB,
                    editParticipant.Comment,
                    editParticipant.Ignore);

                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        Id = editParticipant.CompetitionClassId ?? Guid.Empty
                    });
            }
            catch (Exception exc)
            {
                editParticipant.Errors = exc.InnerException?.Message ?? exc.Message;

                return View(
                    nameof(ShowCreateEdit),
                    editParticipant);
            }
        }

        public IActionResult Delete(
            Guid id)
        {
            var helpCompClassId = _danceCompHelper.FindCompetitionClass(
                    id);

            _danceCompHelper.RemoveParticipant(
                id);

            return RedirectToAction(
                nameof(Index),
                new
                {
                    Id = helpCompClassId ?? Guid.Empty
                });
        }
    }
}
