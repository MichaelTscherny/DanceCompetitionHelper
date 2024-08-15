using AutoMapper;
using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Exceptions;
using DanceCompetitionHelper.Web.Models.CompetitionVenueModels;
using Microsoft.AspNetCore.Mvc;

namespace DanceCompetitionHelper.Web.Controllers
{
    public class CompetitionVenueController : ControllerBase<CompetitionVenueController>
    {
        public const string RefName = "CompetitionVenue";

        public CompetitionVenueController(
            IDanceCompetitionHelper danceCompHelper,
            ILogger<CompetitionVenueController> logger,
            IMapper mapper)
            : base(
                danceCompHelper,
                logger,
                mapper)
        {
        }

        public Task<IActionResult> Index(
            Guid id,
            CancellationToken cancellationToken)
        {
            return DefaultIndexAndShow(
                async (dcH, mapper, cToken) =>
                {
                    var foundComp = await _danceCompHelper
                        .FindCompetitionAsync(
                            id,
                            cToken);

                    if (foundComp == null)
                    {
                        return null;
                    }

                    var foundCompId = foundComp.CompetitionId;
                    ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

                    return new CompetitionVenueOverviewViewModel()
                    {
                        Competition = foundComp,
                        OverviewItems = await _danceCompHelper
                        .GetCompetitionVenuesAsync(
                            foundCompId,
                            cToken)
                        .ToListAsync(),
                    };
                },
                // --
                nameof(Index),
                nameof(Index),
                // --
                cancellationToken);
        }

        public Task<IActionResult> ShowCreateEdit(
            Guid id,
            CancellationToken cancellationToken)
        {
            return DefaultIndexAndShow(
               async (dcH, mapper, cToken) =>
               {
                   var foundComp = await _danceCompHelper
                       .FindCompetitionAsync(
                           id,
                           cToken);

                   if (foundComp == null)
                   {
                       return null;
                   }

                   var foundCompId = foundComp.CompetitionId;
                   ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

                   return new CompetitionVenueViewModel()
                   {
                       CompetitionId = foundCompId,
                       CompetitionName = foundComp.GetCompetitionName(),
                   };
               },
               // --
               nameof(ShowCreateEdit),
               nameof(ShowCreateEdit),
               // --
               cancellationToken);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> CreateNew(
            CompetitionVenueViewModel createCompetitionVenue,
            CancellationToken cancellationToken)
        {
            return DefaultCreateNew(
                createCompetitionVenue,
                _mapper.Map<CompetitionVenue>(
                    createCompetitionVenue),
                // --
                async (dcH, cToken) =>
                {
                    await DefaultGetCompetitionAndSetViewData(
                        dcH,
                        createCompetitionVenue.CompetitionVenueId,
                        cToken);
                },
                nameof(ShowCreateEdit),
                // --
                async (dcH, newEntity, mapper, cToken) =>
                {
                    await dcH.CreateCompetitionVenueAsync(
                        newEntity,
                        cToken);

                    return new
                    {
                        Id = newEntity.CompetitionId,
                    };
                },
                // -- on success
                nameof(Index),
                // -- on error
                async (dcH, model, cToken) =>
                {
                    await DefaultGetCompetitionAndSetViewData(
                        dcH,
                        model.CompetitionId,
                        cToken);
                },
                nameof(ShowCreateEdit),
                // --
                cancellationToken);
        }

        public Task<IActionResult> ShowEdit(
            Guid id,
            CancellationToken cancellationToken)
        {
            return DefaultIndexAndShow(
                async (dcH, mapper, cToken) =>
                {
                    var foundCompVenue = await _danceCompHelper.GetCompetitionVenueAsync(
                        id,
                        cancellationToken,
                        true);

                    if (foundCompVenue == null)
                    {
                        return null;
                    }

                    ViewData["Use" + nameof(CompetitionClass)] = foundCompVenue.CompetitionId;

                    return new CompetitionVenueViewModel()
                    {
                        CompetitionVenueId = foundCompVenue.CompetitionVenueId,

                        CompetitionId = foundCompVenue.CompetitionId,
                        CompetitionName = foundCompVenue.Competition.GetCompetitionName(),

                        Name = foundCompVenue.Name,
                        LengthInMeter = foundCompVenue.LengthInMeter,
                        WidthInMeter = foundCompVenue.WidthInMeter,
                        Comment = foundCompVenue.Comment,
                    };
                },
                // --
                nameof(ShowCreateEdit),
                nameof(Index),
                // --
                cancellationToken);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> EditSave(
            CompetitionVenueViewModel editCompetitionVenue,
            CancellationToken cancellationToken)
        {
            return DefaultEdit(
                editCompetitionVenue,
                // --
                async (dcH, cToken) =>
                {
                    await DefaultGetCompetitionAndSetViewData(
                        dcH,
                        editCompetitionVenue.CompetitionId,
                        cToken);
                },
                nameof(ShowCreateEdit),
                // -- on success
                async (dcH, mapper, cToken) =>
                {
                    var foundAdjPanel = await dcH.GetCompetitionVenueAsync(
                        editCompetitionVenue.CompetitionVenueId ?? Guid.Empty,
                        cToken)
                        ?? throw new NoDataFoundException(
                            string.Format(
                                "{0} with id '{1}' not found!",
                                nameof(CompetitionVenue),
                                editCompetitionVenue.CompetitionVenueId));

                    // override the values...
                    mapper.Map(
                        editCompetitionVenue,
                        foundAdjPanel);

                    return new
                    {
                        Id = foundAdjPanel.CompetitionId,
                    };
                },
                nameof(Index),
                // -- on no data
                nameof(Index),
                // -- on error
                async (dcH, model, cToken) =>
                {
                    await DefaultGetCompetitionAndSetViewData(
                        dcH,
                        model.CompetitionId,
                        cToken);
                },
                nameof(ShowCreateEdit),
                // --
                cancellationToken);
        }

        public Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            return DefaultDelete(
                id,
                // --
                async (dcH, delId, cToken) =>
                {
                    var foundCompVenue = await dcH.GetCompetitionVenueAsync(
                        delId,
                        cToken)
                        ?? throw new NoDataFoundException(
                            string.Format(
                                "{0} with id '{1}' not found!",
                                nameof(CompetitionVenue),
                                delId));

                    await _danceCompHelper.RemoveCompetitionVenueAsync(
                        foundCompVenue,
                        cToken);

                    return new
                    {
                        Id = foundCompVenue.CompetitionId,
                    };
                },
                // --
                nameof(Index),
                nameof(Index),
                nameof(Index),
                // --
                cancellationToken);
        }
    }
}
