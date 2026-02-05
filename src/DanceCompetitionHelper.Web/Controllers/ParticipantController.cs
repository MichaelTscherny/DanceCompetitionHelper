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
            ILogger<ParticipantController> logger)
            : base(
                danceCompHelper,
                logger)
        {
        }

        [HttpGet]
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
                    id,
                    async (showId, dcH, _viewData, cToken) =>
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

                        return new ParticipantOverviewViewModel()
                        {
                            Competition = foundComp,
                            OverviewItems = await _danceCompHelper
                                .GetParticipantsAsync(
                                    foundComp.CompetitionId,
                                    null,
                                    cToken,
                                    true,
                                    detailedView)
                                .ToListAsync(
                                    cToken),
                            DetailedView = detailedView,
                        };
                    },
                    cancellationToken);
        }

        [HttpGet]
        public Task<IActionResult> DetailedView(
            Guid id,
            CancellationToken cancellationToken)
        {
            return Index(
                id,
                cancellationToken,
                true);
        }

        [HttpGet]
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
                    async (showId, dcH, _viewData, cToken) =>
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

                        _ = Guid.TryParse(
                            HttpContext.Session.GetString(
                                ParticipantLastCreatedCompetitionClassId),
                            out var lastCreatedClassId);

                        return new ParticipantViewModel()
                        {
                            CompetitionId = foundComp.CompetitionId,
                            CompetitionName = foundComp.GetCompetitionName(),
                            CompetitionClasses = await _danceCompHelper
                                .GetCompetitionClassesAsync(
                                    foundComp.CompetitionId,
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
                    async (model, dcH, _viewData, cToken) =>
                    {
                        await DefaultGetCompetitionAndSetViewDataAsync(
                            dcH,
                            model.CompetitionId,
                            _viewData,
                            cToken);

                        model.CompetitionClasses = await dcH
                            .GetCompetitionClassesAsync(
                                createParticipant.CompetitionId,
                                cToken)
                            .ToSelectListItemAsync(
                                createParticipant.CompetitionClassId)
                            .ToListAsync(
                                cToken);

                        return null;
                    })
                .DefaultCreateNewAsync(
                    createParticipant,
                    createParticipant.Map()!,
                    async (dcH, newEntity, _, cToken) =>
                    {
                        var useCompetitionClassId = createParticipant.CompetitionClassId ?? Guid.Empty;

                        await dcH.CreateParticipantAsync(
                            newEntity,
                            cToken);

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

        [HttpGet]
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
                    async (showId, dcH, _viewData, cToken) =>
                    {
                        var foundPart = await dcH.GetParticipantAsync(
                            showId,
                            cToken)
                            ?? throw new NoDataFoundException(
                                string.Format(
                                    "{0} with id '{1}' not found!",
                                    nameof(Participant),
                                    showId));

                        var foundComp = await DefaultGetCompetitionAndSetViewDataAsync(
                            dcH,
                            foundPart.CompetitionId,
                            _viewData,
                            cToken);

                        // override the values...
                        var partModel = foundPart.Map();
                        if (partModel != null)
                        {
                            partModel.CompetitionClasses = await dcH
                                .GetCompetitionClassesAsync(
                                    foundPart.CompetitionId,
                                    cToken)
                                .ToSelectListItemAsync(
                                    foundPart.CompetitionClassId)
                                .ToListAsync(
                                    cToken);
                        }

                        return partModel;
                    },
                    cancellationToken);
        }

        [HttpPost]
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
                    async (model, dcH, _viewData, cToken) =>
                    {
                        await DefaultGetCompetitionAndSetViewDataAsync(
                            dcH,
                            model.CompetitionId,
                            _viewData,
                            cToken);

                        model.CompetitionClasses = await dcH
                            .GetCompetitionClassesAsync(
                                model.CompetitionId,
                                cToken)
                            .ToSelectListItemAsync(
                                model.CompetitionClassId)
                            .ToListAsync(
                                cToken);

                        return null;
                    })
                .DefaultEditSaveAsync(
                    editParticipant,
                    async (model, dcH, _, cToken) =>
                    {
                        var foundPart = await dcH.GetParticipantAsync(
                            model.ParticipantId ?? Guid.Empty,
                            cToken)
                            ?? throw new NoDataFoundException(
                                string.Format(
                                    "{0} with id '{1}' not found!",
                                    nameof(Participant),
                                    model.ParticipantId));

                        // override the values...
                        model.Map(
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
                    async (model, dcH, _viewData, cToken) =>
                    {
                        await DefaultGetCompetitionAndSetViewDataAsync(
                            dcH,
                            model.CompetitionId,
                            _viewData,
                            cToken);

                        model.CompetitionClasses = await dcH
                            .GetCompetitionClassesAsync(
                                model.CompetitionId,
                                cToken)
                            .ToSelectListItemAsync(
                                model.CompetitionClassId)
                            .ToListAsync(
                                cToken);

                        return null;
                    })
                .DefaultEditSaveAsync(
                    new ParticipantViewModel()
                    {
                        ParticipantId = id,
                    },
                    async (model, dcH, _, cToken) =>
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
                        model.Map(
                            foundPart);
                        // foundPart.Ignore = true;

                        return new
                        {
                            Id = foundPart?.CompetitionClassId ?? Guid.Empty
                        };
                    },
                    cancellationToken);
        }

        [HttpGet]
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
                    async (delId, dcH, _, cToken) =>
                    {
                        var helpComp = await _danceCompHelper.FindCompetitionAsync(
                            id,
                            cToken);

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

        [HttpGet]
        public Task<IActionResult> PdfParticipants(
            PdfViewModel pdf,
            CancellationToken cancellationToken)
        {
            return GetPdfDocumentHelper()
                .GetParticipants(
                    pdf,
                    cancellationToken);
        }
    }
}
