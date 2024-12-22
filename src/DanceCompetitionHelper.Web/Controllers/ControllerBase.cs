using AutoMapper;

using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Web.Helper.Documents;
using DanceCompetitionHelper.Web.Helper.Request;
using DanceCompetitionHelper.Web.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using System.Diagnostics;
using System.Net.Mime;
using System.Runtime.CompilerServices;

namespace DanceCompetitionHelper.Web.Controllers
{
    public abstract class ControllerBase<TLogger> : Controller
    {
        protected readonly IDanceCompetitionHelper _danceCompHelper;
        protected readonly ILogger<TLogger> _logger;
        protected readonly IMapper _mapper;

        protected ControllerBase(
            IDanceCompetitionHelper danceCompHelper,
            ILogger<TLogger> logger,
            IMapper mapper)
        {
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

        #region Chaining

        public DefaultRequestHandler<TLogger, TType, TModel> GetDefaultRequestHandler<TType, TModel>(
            // CancellationToken cancellationToken,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
            where TType : TableBase
            where TModel : ViewModelBase
        {
            return new DefaultRequestHandler<TLogger, TType, TModel>(
                _danceCompHelper,
                _logger,
                _mapper);
        }

        #endregion Chaining

        public async Task<IActionResult> GetPdfDocument<TModel>(
            TModel pdfInfoModel,
            Func<TModel, IDanceCompetitionHelper, PdfDocumentHelper, CancellationToken, Task<PdfViewModel>> geneeratePdfFunc,
            CancellationToken cancellationToken)
            where TModel : ViewModelBase
        {
            try
            {
                var pdfView = await _danceCompHelper.RunInReadonlyTransaction<PdfViewModel>(
                    async (dcH, dbCtx, dbTrans, cToken) =>
                    {
                        var pdfDoc = new PdfDocumentHelper(
                            dcH, _mapper);

                        try
                        {
                            return await geneeratePdfFunc(
                                pdfInfoModel,
                                dcH,
                                pdfDoc,
                                cToken);
                        }
                        catch (Exception exc)
                        {
                            _logger.LogError(
                                exc,
                                "Error during creation of PDF: {errorMessage}",
                                exc.Message);

                            throw;
                        }
                    },
                    cancellationToken);

                return File(
                    pdfView.PdtStream,
                    MediaTypeNames.Application.Pdf,
                    pdfView.FileName);
            }
            catch (Exception exc)
            {
                return Error(
                    string.Format(
                        "Error during creation of PDF: {0}",
                        exc.Message));
            }
        }

        public async Task<Competition?> DefaultGetCompetitionAndSetViewData(
            IDanceCompetitionHelper danceCompetitionHelper,
            Guid? competitionId,
            ViewDataDictionary? viewData,
            CancellationToken cancellationToken)
        {
            var foundComp = await danceCompetitionHelper.FindCompetitionAsync(
                competitionId ?? Guid.Empty,
                cancellationToken);

            if (foundComp == null)
            {
                return null;
            }

            (viewData ?? ViewData)["Use" + nameof(CompetitionClass)] = foundComp.CompetitionId;

            return foundComp;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(
            string errorMessage)
        {
            return View(
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    Errors = new List<string>()
                    {
                        errorMessage,
                    },
                });
        }

    }
}
