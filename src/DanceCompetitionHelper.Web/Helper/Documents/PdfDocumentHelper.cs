﻿using AutoMapper;

using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Exceptions;
using DanceCompetitionHelper.Web.Controllers;
using DanceCompetitionHelper.Web.Models.Pdfs;

using Microsoft.AspNetCore.Mvc;

using System.Net.Mime;

namespace DanceCompetitionHelper.Web.Helper.Documents
{
    public class PdfDocumentHelper<TLogger>
    {
        private readonly ControllerBase<TLogger> _controllerBase;
        private readonly IDanceCompetitionHelper _danceCompHelper;
        private readonly ILogger<TLogger> _logger;
        private readonly IMapper _mapper;

        public PdfDocumentHelper(
            ControllerBase<TLogger> controllerBase,
            IDanceCompetitionHelper danceCompHelper,
            ILogger<TLogger> logger,
            IMapper mapper)
        {
            _controllerBase = controllerBase
                ?? throw new ArgumentNullException(
                    nameof(controllerBase));
            _danceCompHelper = danceCompHelper
                ?? throw new ArgumentNullException(
                    nameof(danceCompHelper));
            _logger = logger
                ?? throw new ArgumentNullException(
                    nameof(logger));
            _mapper = mapper
                ?? throw new ArgumentNullException(
                    nameof(mapper));
        }

        public async Task<IActionResult> ReturnPdfViewModel<TModel>(
            TModel model,
            Func<TModel, PdfDocumentHelper<TLogger>, PdfGenerator, IDanceCompetitionHelper, IMapper, CancellationToken, Task<TModel>> geneeratePdfFunc,
            CancellationToken cancellationToken)
            where TModel : PdfViewModel
        {
            try
            {
                var pdfView = await _danceCompHelper.RunInReadonlyTransaction<PdfViewModel>(
                    async (dcH, dbCtx, dbTrans, cToken) =>
                    {
                        return await geneeratePdfFunc(
                            model,
                            this,
                            new PdfGenerator(),
                            dcH,
                            _mapper,
                            cToken);
                    },
                    cancellationToken);

                if (pdfView == null)
                {
                    throw new Exception(
                        "No PDF Data returend");
                }

                return _controllerBase.File(
                    pdfView.PdtStream,
                    MediaTypeNames.Application.Pdf,
                    pdfView.FileName);
            }
            catch (Exception exc)
            {
                _logger.LogError(
                    exc,
                    "Error during creation of PDF: {errorMessage}",
                    exc.Message);

                return _controllerBase.Error(
                    string.Format(
                        "Error during creation of PDF: {0}",
                        exc.Message));
            }
        }

        public Task<IActionResult> GetMultipleStarters(
            PdfViewModel pdfViewModel,
            CancellationToken cancellationToken)
        {
            return ReturnPdfViewModel(
                pdfViewModel,
                async (pdfModel, pdfDocHelper, pdfGen, dcH, mapper, cToken) =>
                {
                    var foundComp = await dcH.FindCompetitionAsync(
                        pdfModel.CompetitionId,
                        cToken)
                        ?? throw new NoDataFoundException(
                            string.Format(
                                "{0} with id '{1}' not found!",
                                nameof(Competition),
                                pdfModel.CompetitionId));

                    // pdfGen.DefaultFontSize = 12;

                    return new PdfViewModel()
                    {
                        PdtStream = pdfGen.GetMultipleStarters(
                            foundComp,
                            await dcH
                                .GetMultipleStarterAsync(
                                    foundComp.CompetitionId,
                                    cToken)
                                .ToListAsync(
                                    cToken),
                            pdfModel),
                        FileName = string.Format(
                            "Multiple Starters {0}.pdf",
                            foundComp.OrgCompetitionId),
                    };
                },
                cancellationToken);
        }

        public Task<IActionResult> GetMultipleStartersDependentClassesView(
            PdfViewModel pdfViewModel,
            CancellationToken cancellationToken)
        {
            return ReturnPdfViewModel(
                pdfViewModel,
                async (pdfModel, pdfDocHelper, pdfGen, dcH, mapper, cToken) =>
                {
                    var foundComp = await dcH.FindCompetitionAsync(
                        pdfModel.CompetitionId,
                        cToken)
                        ?? throw new NoDataFoundException(
                            string.Format(
                                "{0} with id '{1}' not found!",
                                nameof(Competition),
                                pdfModel.CompetitionId));

                    return new PdfViewModel()
                    {
                        PdtStream = pdfGen.GetMultipleStartersDependentClassesView(
                            foundComp,
                            await dcH
                                .GetMultipleStarterAsync(
                                    foundComp.CompetitionId,
                                    cToken)
                                .ToListAsync(
                                    cToken),
                            pdfModel),
                        FileName = string.Format(
                            "Multiple Starters Dependent Classes {0}.pdf",
                            foundComp.OrgCompetitionId),
                    };
                },
                cancellationToken);
        }

        public Task<IActionResult> GetPossiblePromotions(
            PdfViewModel pdfViewModel,
            CancellationToken cancellationToken)
        {
            return ReturnPdfViewModel(
                pdfViewModel,
                async (pdfModel, pdfDocHelper, pdfGen, dcH, mapper, cToken) =>
                {
                    var foundComp = await dcH.FindCompetitionAsync(
                        pdfModel.CompetitionId,
                        cToken)
                        ?? throw new NoDataFoundException(
                            string.Format(
                                "{0} with id '{1}' not found!",
                                nameof(Competition),
                                pdfModel.CompetitionId));

                    return new PdfViewModel()
                    {
                        PdtStream = pdfGen.GetPossiblePromotions(
                            foundComp,
                            await dcH
                                 .GetParticipantsAsync(
                                     foundComp.CompetitionId,
                                     null,
                                     cToken,
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
                                     cToken),
                            await dcH
                                .GetMultipleStarterAsync(
                                    foundComp.CompetitionId,
                                    cToken)
                                .ToListAsync(
                                    cToken),
                            pdfModel),
                        FileName = string.Format(
                            "Possible Promotions {0}.pdf",
                            foundComp.OrgCompetitionId),
                    };
                },
                cancellationToken);
        }

        public Task<IActionResult> GetNumberCards(
            PdfViewModel pdfViewModel,
            CancellationToken cancellationToken)
        {
            return ReturnPdfViewModel(
                pdfViewModel,
                async (pdfModel, pdfDocHelper, pdfGen, dcH, mapper, cToken) =>
                {
                    var foundComp = await dcH.FindCompetitionAsync(
                        pdfModel.CompetitionId,
                        cToken)
                        ?? throw new NoDataFoundException(
                            string.Format(
                                "{0} with id '{1}' not found!",
                                nameof(Competition),
                                pdfModel.CompetitionId));

                    var showParticipants = new List<Participant>();
                    if (pdfModel.ParticipantId.HasValue)
                    {
                        var foundPart = await dcH.GetParticipantAsync(
                            pdfModel.ParticipantId.Value,
                            cToken,
                            includeCompetition: true,
                            includeCompetitionClass: true);

                        if (foundComp.CompetitionId == foundPart?.CompetitionId)
                        {
                            showParticipants.Add(
                                foundPart);
                        }
                    }
                    else
                    {
                        showParticipants = await dcH
                            .GetParticipantsAsync(
                                foundComp.CompetitionId,
                                pdfModel.CompetitionClassId,
                                cToken,
                                filter: pdfModel.ToParticipantFilter())
                            .ToListAsync(
                                cToken);
                    }

                    return new PdfViewModel()
                    {
                        PdtStream = pdfGen.GetNumberCards(
                            foundComp,
                            showParticipants,
                            pdfModel),
                        FileName = string.Format(
                            "Number Cards {0}.pdf",
                            foundComp.OrgCompetitionId),
                    };
                },
                cancellationToken);
        }

        public Task<IActionResult> GetParticipants(
            PdfViewModel pdfViewModel,
            CancellationToken cancellationToken)
        {
            return ReturnPdfViewModel(
                pdfViewModel,
                async (pdfModel, pdfDocHelper, pdfGen, dcH, mapper, cToken) =>
                {
                    var foundComp = await dcH.FindCompetitionAsync(
                        pdfModel.CompetitionId,
                        cToken)
                        ?? throw new NoDataFoundException(
                            string.Format(
                                "{0} with id '{1}' not found!",
                                nameof(Competition),
                                pdfModel.CompetitionId));

                    var extraToFileName = string.Empty;
                    if (pdfModel.CompetitionClassId.HasValue
                        && Guid.Empty != pdfModel.CompetitionClassId)
                    {
                        extraToFileName = " Class ";
                    }

                    return new PdfViewModel()
                    {
                        PdtStream = pdfGen.GetParticipants(
                            foundComp,
                            await dcH
                                .GetParticipantsAsync(
                                    foundComp.CompetitionId,
                                    pdfModel.CompetitionClassId,
                                    cToken)
                                .ToListAsync(
                                    cToken),
                            pdfModel),
                        FileName = string.Format(
                            "Participants {0}{1}.pdf",
                            extraToFileName,
                            foundComp.OrgCompetitionId),
                    };
                },
                cancellationToken);
        }

        public Task<IActionResult> GetCompetitionClasses(
            PdfViewModel pdfViewModel,
            CancellationToken cancellationToken)
        {
            return ReturnPdfViewModel(
                pdfViewModel,
                async (pdfModel, pdfDocHelper, pdfGen, dcH, mapper, cToken) =>
                {
                    var foundComp = await dcH.FindCompetitionAsync(
                        pdfModel.CompetitionId,
                        cToken)
                        ?? throw new NoDataFoundException(
                            string.Format(
                                "{0} with id '{1}' not found!",
                                nameof(Competition),
                                pdfModel.CompetitionId));

                    var compClassVenues = new Dictionary<Guid, HashSet<Guid>>();

                    var extraToFileName = string.Empty;
                    if (pdfModel.CompetitionClassId.HasValue
                        && Guid.Empty != pdfModel.CompetitionClassId)
                    {
                        extraToFileName = " Venue ";

                        /* TODO: linke comps with venues
                        compClassVenues
                        */
                    }

                    return new PdfViewModel()
                    {
                        PdtStream = pdfGen.GetCompetitionClasses(
                            foundComp,
                            await dcH
                                .GetCompetitionClassesAsync(
                                    foundComp.CompetitionId,
                                    cToken,
                                    true,
                                    false)
                                .ToListAsync(
                                    cToken),
                            await dcH
                                .GetCompetitionVenuesAsync(
                                    foundComp.CompetitionId,
                                    cToken)
                                // TODO: just for testing
                                .OrderBy(
                                    x => x.Name)
                                .ToListAsync(
                                    cToken),
                            compClassVenues,
                            pdfModel),
                        FileName = string.Format(
                            "Competition Classes {0}{1}.pdf",
                            extraToFileName,
                            foundComp.OrgCompetitionId),
                    };
                },
                cancellationToken);
        }

        [Obsolete("do not use! Just a test dummy!")]
        public Task<IActionResult> GetDummyPdf(
            PdfViewModel pdfViewModel,
            CancellationToken cancellationToken)
        {
            return ReturnPdfViewModel(
                pdfViewModel,
                async (pdfModel, pdfDocHelper, pdfGen, dcH, mapper, cToken) =>
                {
                    return new PdfViewModel()
                    {
                        PdtStream = pdfGen.GetDummyPdf(
                            await dcH.GetCompetitionAsync(
                                pdfModel.CompetitionId ?? Guid.Empty,
                                cToken),
                            await dcH.GetCompetitionClassAsync(
                                pdfModel.CompetitionClassId ?? Guid.Empty,
                                cToken),
                            pdfModel),
                        FileName = "dummy.pdf"
                    };
                },
                cancellationToken);
        }
    }
}
