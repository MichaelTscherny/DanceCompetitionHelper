﻿using AutoMapper;

using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Exceptions;
using DanceCompetitionHelper.Web.Controllers;
using DanceCompetitionHelper.Web.Models;

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

        static PdfDocumentHelper()
        {
#if CORE
            // Core build does not use Windows fonts, so set a FontResolver that handles the fonts our samples need.
            GlobalFontSettings.FontResolver = new SamplesFontResolver();
#endif
        }

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
