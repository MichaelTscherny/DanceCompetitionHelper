using AutoMapper;
using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Web.Extensions;
using DanceCompetitionHelper.Web.Models.ParticipantModels;
using Microsoft.AspNetCore.Mvc;

namespace DanceCompetitionHelper.Web.Controllers
{
    public class ParticipantController : ControllerBase<ParticipantController>
    {
        public const string RefName = "Participant";
        public const string ParticipantLastCreatedCompetitionClassId = nameof(ParticipantLastCreatedCompetitionClassId);

        public ParticipantController(
            IDanceCompetitionHelper danceCompHelper,
            ILogger<ParticipantController> logger,
            IMapper mapper)
            : base(
                danceCompHelper,
                logger,
                mapper)
        {
        }

        public async Task<IActionResult> Index(
            Guid id,
            CancellationToken cancellationToken)
        {
            var foundComp = await _danceCompHelper.FindCompetitionAsync(
                id,
                cancellationToken);

            if (foundComp == null)
            {
                return NotFound();
            }

            var foundCompId = foundComp.CompetitionId;
            ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

            return View(
                new ParticipantOverviewViewModel()
                {
                    Competition = foundComp,
                    OverviewItems = await _danceCompHelper
                        .GetParticipantsAsync(
                            foundCompId,
                            null,
                            cancellationToken,
                            true)
                        .ToListAsync(
                            cancellationToken),
                });
        }

        public async Task<IActionResult> DetailedView(
            Guid id,
            CancellationToken cancellationToken)
        {
            var foundComp = await _danceCompHelper.FindCompetitionAsync(
                id,
                cancellationToken);

            if (foundComp == null)
            {
                return NotFound();
            }

            var foundCompId = foundComp.CompetitionId;
            ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

            return View(
                nameof(Index),
                new ParticipantOverviewViewModel()
                {
                    Competition = foundComp,
                    OverviewItems = await _danceCompHelper
                        .GetParticipantsAsync(
                            foundCompId,
                            null,
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
            var foundComp = await _danceCompHelper.FindCompetitionAsync(
                id,
                cancellationToken);

            if (foundComp == null)
            {
                return NotFound();
            }

            var foundCompId = foundComp.CompetitionId;
            ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

            var helpCompName = string.Empty;

            _ = Guid.TryParse(
                HttpContext.Session.GetString(
                    ParticipantLastCreatedCompetitionClassId),
                out var lastCreatedClassId);

            return View(
                new ParticipantViewModel()
                {
                    CompetitionId = foundCompId,
                    CompetitionName = foundComp.GetCompetitionName(),
                    CompetitionClasses = await _danceCompHelper
                        .GetCompetitionClassesAsync(
                            foundCompId,
                            cancellationToken)
                        .ToSelectListItemAsync(
                            lastCreatedClassId)
                        .ToListAsync(),
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNew(
            ParticipantViewModel createParticipant,
            CancellationToken cancellationToken)
        {
            if (ModelState.IsValid == false)
            {
                createParticipant.AddErrors(
                    ModelState);

                createParticipant.CompetitionClasses = await _danceCompHelper
                    .GetCompetitionClassesAsync(
                        createParticipant.CompetitionId,
                        cancellationToken)
                    .ToSelectListItemAsync(
                        createParticipant.CompetitionClassId)
                    .ToListAsync();

                return View(
                    nameof(ShowCreateEdit),
                    createParticipant);
            }

            try
            {
                var useCompetitionClassId = createParticipant.CompetitionClassId ?? Guid.Empty;

                await _danceCompHelper.CreateParticipantAsync(
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
                     createParticipant.Ignore,
                     cancellationToken);

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
                createParticipant.AddErrors(
                    exc);

                createParticipant.CompetitionClasses = await _danceCompHelper
                    .GetCompetitionClassesAsync(
                        createParticipant.CompetitionId,
                        cancellationToken)
                    .ToSelectListItemAsync(
                        createParticipant.CompetitionClassId)
                    .ToListAsync();

                return View(
                    nameof(ShowCreateEdit),
                    createParticipant);
            }
        }

        public async Task<IActionResult> ShowEdit(
            Guid id,
            CancellationToken cancellationToken)
        {
            var foundParticipant = await _danceCompHelper.GetParticipantAsync(
                id,
                cancellationToken);

            if (foundParticipant == null)
            {
                return RedirectToAction(
                    nameof(Index));
            }

            var foundComp = await _danceCompHelper.GetCompetitionAsync(
                foundParticipant.CompetitionId,
                cancellationToken);

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

                    CompetitionClasses = await _danceCompHelper
                        .GetCompetitionClassesAsync(
                            foundParticipant.CompetitionId,
                            cancellationToken)
                        .ToSelectListItemAsync(
                            foundParticipant.CompetitionClassId)
                        .ToListAsync(),
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSave(
            ParticipantViewModel editParticipant,
            CancellationToken cancellationToken)
        {
            if (ModelState.IsValid == false)
            {
                editParticipant.AddErrors(
                    ModelState);

                return View(
                    nameof(ShowCreateEdit),
                    editParticipant);
            }

            try
            {
                await _danceCompHelper.EditParticipantAsync(
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
                    editParticipant.Ignore,
                    cancellationToken);

                return RedirectToAction(
                    nameof(Index),
                    new
                    {
                        Id = editParticipant.CompetitionClassId ?? Guid.Empty
                    });
            }
            catch (Exception exc)
            {
                editParticipant.AddErrors(
                    exc);

                return View(
                    nameof(ShowCreateEdit),
                    editParticipant);
            }
        }

        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            var helpCompClass = await _danceCompHelper.FindCompetitionClassAsync(
                id,
                cancellationToken);

            await _danceCompHelper.RemoveParticipantAsync(
                id,
                cancellationToken);

            return RedirectToAction(
                nameof(Index),
                new
                {
                    Id = helpCompClass?.CompetitionClassId ?? Guid.Empty
                });
        }
    }
}
