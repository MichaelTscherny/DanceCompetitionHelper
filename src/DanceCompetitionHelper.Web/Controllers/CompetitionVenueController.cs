using AutoMapper;

using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Exceptions;
using DanceCompetitionHelper.Web.Helper.Request;
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

        [HttpGet]
        public Task<IActionResult> Index(
            Guid id,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<CompetitionVenue, CompetitionVenueOverviewViewModel>()
                .SetOnSuccess(
                    nameof(Index))
                .SetOnNoData(
                    nameof(Index))
                .DefaultIndexAsync(
                    id,
                    async (indexId, dcH, _, _viewData, cToken) =>
                    {
                        var foundComp = await DefaultGetCompetitionAndSetViewDataAsync(
                            dcH,
                            indexId,
                            _viewData,
                            cToken);

                        if (foundComp == null)
                        {
                            return null;
                        }

                        return new CompetitionVenueOverviewViewModel()
                        {
                            Competition = foundComp,
                            OverviewItems = await _danceCompHelper
                            .GetCompetitionVenuesAsync(
                                foundComp.CompetitionId,
                                cToken)
                            .ToListAsync(
                                cToken),
                        };
                    },
                    cancellationToken);
        }

        [HttpGet]
        public Task<IActionResult> ShowCreateEdit(
            Guid id,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<CompetitionVenue, CompetitionVenueViewModel>()
                .SetOnSuccess(
                    nameof(ShowCreateEdit))
                .SetOnNoData(
                    nameof(Index))
                .DefaultShowAsync(
                    id,
                    async (showId, dcH, mapper, _viewData, cToken) =>
                    {
                        var foundComp = await DefaultGetCompetitionAndSetViewDataAsync(
                            dcH,
                            showId,
                            _viewData,
                            cToken);

                        if (foundComp == null)
                        {
                            return null;
                        }

                        return new CompetitionVenueViewModel()
                        {
                            CompetitionId = foundComp.CompetitionId,
                            CompetitionName = foundComp.GetCompetitionName(),
                        };
                    },
                    cancellationToken);
        }

        [HttpPost]
        public Task<IActionResult> CreateNew(
            CompetitionVenueViewModel createCompetitionVenue,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<CompetitionVenue, CompetitionVenueViewModel>()
                .SetOnSuccess(
                    nameof(Index))
                .SetOnModelStateInvalid(
                    nameof(ShowCreateEdit))
                .SetOnError(
                    nameof(ShowCreateEdit))
                .SetOnFunc(
                    SetOnEnum.OnModelStateInvalid | SetOnEnum.OnError,
                    async (model, dcH, _, _viewData, cToken) =>
                    {
                        await DefaultGetCompetitionAndSetViewDataAsync(
                            dcH,
                            model.CompetitionId,
                            _viewData,
                            cToken);

                        return null;
                    })
                .DefaultCreateNewAsync(
                    createCompetitionVenue,
                    _mapper.Map<CompetitionVenue>(
                        createCompetitionVenue),
                    async (dcH, newEntity, _, _, cToken) =>
                    {
                        await dcH.CreateCompetitionVenueAsync(
                        newEntity,
                        cToken);

                        return new
                        {
                            Id = newEntity.CompetitionId,
                        };
                    },
                    cancellationToken);
        }

        [HttpGet]
        public Task<IActionResult> ShowEdit(
            Guid id,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<CompetitionVenue, CompetitionVenueViewModel>()
                .SetOnSuccess(
                    nameof(ShowCreateEdit))
                .SetOnNoData(
                    nameof(Index))
                .DefaultShowAsync(
                    id,
                    async (showId, dcH, mapper, _viewData, cToken) =>
                    {
                        var foundCompVenue = await dcH
                            .GetCompetitionVenueAsync(
                                id,
                                cancellationToken,
                                true);
                        await DefaultGetCompetitionAndSetViewDataAsync(
                            dcH,
                            foundCompVenue?.CompetitionId,
                            _viewData,
                            cToken);

                        if (foundCompVenue == null)
                        {
                            return null;
                        }

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
                    cancellationToken);
        }

        [HttpPost]
        public Task<IActionResult> EditSave(
            CompetitionVenueViewModel editCompetitionVenue,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<CompetitionVenue, CompetitionVenueViewModel>()
                .SetOnSuccess(
                    nameof(Index))
                .SetOnNoData(
                    nameof(Index))
                .SetOnModelStateInvalid(
                    nameof(ShowCreateEdit))
                .SetOnError(
                    nameof(ShowCreateEdit))
                .SetOnFunc(
                    SetOnEnum.OnModelStateInvalid | SetOnEnum.OnError,
                    async (model, dcH, _, _viewData, cToken) =>
                    {
                        await DefaultGetCompetitionAndSetViewDataAsync(
                            dcH,
                            model.CompetitionId,
                            _viewData,
                            cToken);

                        return null;
                    })
                .DefaultEditSaveAsync(
                    editCompetitionVenue,
                    async (model, dcH, mapper, _, cToken) =>
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
                    cancellationToken);
        }

        [HttpGet]
        public Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<CompetitionVenue, CompetitionVenueViewModel>()
                .SetOnSuccess(
                    nameof(Index))
                .SetOnNoData(
                    nameof(Index))
                .SetOnError(
                    nameof(Index))
                .DefaultDeleteAsync(
                    id,
                    async (delId, dcH, _, _, cToken) =>
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
                    cancellationToken);
        }
    }
}
