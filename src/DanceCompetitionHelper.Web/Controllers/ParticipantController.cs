using AutoMapper;

using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Exceptions;
using DanceCompetitionHelper.Web.Extensions;
using DanceCompetitionHelper.Web.Helper.Request;
using DanceCompetitionHelper.Web.Models.ParticipantModels;
using DanceCompetitionHelper.Web.Models.Pdfs;

using Microsoft.AspNetCore.Mvc;

using MigraDoc.DocumentObjectModel;

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

        public Task<IActionResult> Index(
            Guid id,
            CancellationToken cancellationToken,
            bool detailedView = false)
        {
            return GetDefaultRequestHandler<Participant, ParticipantOverviewViewModel>()
                .SetOnSuccess(
                    nameof(Index))
                .SetOnNoData(
                    nameof(Index))
                .DefaultIndexAsync(
                    Guid.Empty,
                    async (_, dcH, _, _viewData, cToken) =>
                    {
                        var foundComp = await dcH.FindCompetitionAsync(
                        id,
                        cToken);

                        if (foundComp == null)
                        {
                            return null;
                        }

                        var foundCompId = foundComp.CompetitionId;
                        _viewData["Use" + nameof(CompetitionClass)] = foundCompId;

                        return new ParticipantOverviewViewModel()
                        {
                            Competition = foundComp,
                            OverviewItems = await _danceCompHelper
                                .GetParticipantsAsync(
                                    foundCompId,
                                    null,
                                    cancellationToken,
                                    true,
                                    detailedView)
                                .ToListAsync(
                                    cancellationToken),
                            DetailedView = detailedView,
                        };
                    },
                    cancellationToken);
        }

        public Task<IActionResult> DetailedView(
            Guid id,
            CancellationToken cancellationToken)
        {
            return Index(
                id,
                cancellationToken,
                true);
        }

        public Task<IActionResult> ShowCreateEdit(
            Guid id,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<Participant, ParticipantViewModel>()
                .SetOnSuccess(
                    nameof(ShowCreateEdit))
                .SetOnNoData(
                    nameof(Index))
                .DefaultShowAsync(
                    id,
                    async (showId, dcH, mapper, _viewData, cToken) =>
                    {
                        var foundComp = await dcH.FindCompetitionAsync(
                            showId,
                            cToken);

                        if (foundComp == null)
                        {
                            return null;
                        }

                        var foundCompId = foundComp.CompetitionId;
                        ViewData["Use" + nameof(CompetitionClass)] = foundCompId;

                        _ = Guid.TryParse(
                            HttpContext.Session.GetString(
                                ParticipantLastCreatedCompetitionClassId),
                            out var lastCreatedClassId);

                        return new ParticipantViewModel()
                        {
                            CompetitionId = foundCompId,
                            CompetitionName = foundComp.GetCompetitionName(),
                            CompetitionClasses = await _danceCompHelper
                                .GetCompetitionClassesAsync(
                                    foundCompId,
                                    cToken)
                                .ToSelectListItemAsync(
                                    lastCreatedClassId)
                                .ToListAsync(
                                    cToken),
                        };
                    },
                    cancellationToken);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> CreateNew(
            ParticipantViewModel createParticipant,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<Participant, ParticipantViewModel>()
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
                        await DefaultGetCompetitionAndSetViewData(
                            dcH,
                            model.CompetitionId,
                            _viewData,
                            cToken);

                        model.CompetitionClasses = await dcH
                            .GetCompetitionClassesAsync(
                                createParticipant.CompetitionId,
                                cancellationToken)
                            .ToSelectListItemAsync(
                                createParticipant.CompetitionClassId)
                            .ToListAsync(
                                cancellationToken);

                        return null;
                    })
                .DefaultCreateNewAsync(
                    createParticipant,
                    _mapper.Map<Participant>(
                        createParticipant),
                    async (dcH, newEntity, _, _, cToken) =>
                    {
                        var useCompetitionClassId = createParticipant.CompetitionClassId ?? Guid.Empty;

                        await dcH.CreateParticipantAsync(
                            newEntity,
                            cancellationToken);

                        HttpContext.Session.SetString(
                            ParticipantLastCreatedCompetitionClassId,
                            useCompetitionClassId.ToString());

                        return new
                        {
                            Id = newEntity.CompetitionId,
                        };
                    },
                    cancellationToken);
        }

        public Task<IActionResult> ShowEdit(
            Guid id,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<Participant, ParticipantViewModel>()
                .SetOnSuccess(
                    nameof(ShowCreateEdit))
                .SetOnNoData(
                    nameof(Index))
                .DefaultShowAsync(
                    id,
                    async (showId, dcH, mapper, _viewData, cToken) =>
                    {
                        var foundPart = await dcH.GetParticipantAsync(
                            showId,
                            cancellationToken)
                            ?? throw new NoDataFoundException(
                                string.Format(
                                    "{0} with id '{1}' not found!",
                                    nameof(Participant),
                                    showId));

                        _viewData["Use" + nameof(CompetitionClass)] = foundPart.CompetitionId;

                        // override the values...
                        var partModel = mapper.Map<ParticipantViewModel>(
                            foundPart);

                        partModel.CompetitionClasses = await dcH
                            .GetCompetitionClassesAsync(
                                foundPart.CompetitionId,
                                cancellationToken)
                            .ToSelectListItemAsync(
                                foundPart.CompetitionClassId)
                            .ToListAsync(
                                cancellationToken);

                        return partModel;
                    },
                    cancellationToken);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> EditSave(
            ParticipantViewModel editParticipant,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<Participant, ParticipantViewModel>()
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
                        await DefaultGetCompetitionAndSetViewData(
                            dcH,
                            model.CompetitionId,
                            _viewData,
                            cToken);

                        model.CompetitionClasses = await dcH
                            .GetCompetitionClassesAsync(
                                model.CompetitionId,
                                cancellationToken)
                            .ToSelectListItemAsync(
                                model.CompetitionClassId)
                            .ToListAsync(
                                cancellationToken);

                        return null;
                    })
                .DefaultEditSaveAsync(
                    editParticipant,
                    async (model, dcH, mapper, _, cToken) =>
                    {
                        var foundPart = await dcH.GetParticipantAsync(
                            model.ParticipantId ?? Guid.Empty,
                            cancellationToken)
                            ?? throw new NoDataFoundException(
                                string.Format(
                                    "{0} with id '{1}' not found!",
                                    nameof(Participant),
                                    model.ParticipantId));

                        // override the values...
                        mapper.Map(
                            model,
                            foundPart);

                        return new
                        {
                            Id = model.CompetitionClassId ?? Guid.Empty
                        };
                    },
                    cancellationToken);
        }

        [HttpGet]
        public Task<IActionResult> Ignore(
            Guid id,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<Participant, ParticipantViewModel>()
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
                        await DefaultGetCompetitionAndSetViewData(
                            dcH,
                            model.CompetitionId,
                            _viewData,
                            cToken);

                        model.CompetitionClasses = await dcH
                            .GetCompetitionClassesAsync(
                                model.CompetitionId,
                                cancellationToken)
                            .ToSelectListItemAsync(
                                model.CompetitionClassId)
                            .ToListAsync(
                                cancellationToken);

                        return null;
                    })
                .DefaultEditSaveAsync(
                    new ParticipantViewModel()
                    {
                        ParticipantId = id,
                    },
                    async (model, dcH, mapper, _, cToken) =>
                    {
                        var foundPart = await dcH.GetParticipantAsync(
                            model.ParticipantId ?? Guid.Empty,
                            cancellationToken)
                            ?? throw new NoDataFoundException(
                                string.Format(
                                    "{0} with id '{1}' not found!",
                                    nameof(Participant),
                                    model.ParticipantId));

                        // override the values...
                        foundPart.Ignore = true;

                        return new
                        {
                            Id = foundPart?.CompetitionClassId ?? Guid.Empty
                        };
                    },
                    cancellationToken);
        }

        public Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            return GetDefaultRequestHandler<Participant, ParticipantViewModel>()
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
                        var helpComp = await _danceCompHelper.FindCompetitionAsync(
                            id,
                            cancellationToken);

                        var foundPart = await dcH.GetParticipantAsync(
                            delId,
                            cToken)
                            ?? throw new NoDataFoundException(
                                string.Format(
                                    "{0} with id '{1}' not found!",
                                    nameof(Participant),
                                    delId));

                        await dcH.RemoveParticipantAsync(
                            foundPart,
                            cToken);

                        return new
                        {
                            Id = helpComp?.CompetitionId ?? Guid.Empty,
                        };
                    },
                    cancellationToken);
        }

        [HttpGet]
        public Task<IActionResult> PdfNumberCards(
            PdfViewModel pdf,
            CancellationToken cancellationToken)
        {
            // CAUTION: only "A4" implemented yet!..
            pdf.PageFormat = PageFormat.A4;
            pdf.PageOrientation = Orientation.Portrait;

            return GetPdfDocumentHelper()
                .GetNumberCards(
                    pdf,
                    cancellationToken);
        }
    }
}
