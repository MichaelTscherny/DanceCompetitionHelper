using AutoMapper;

using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;

using PdfSharp.Pdf;

namespace DanceCompetitionHelper.Web.Helper.Documents
{
    public class PdfDocumentHelper
    {
        private readonly IDanceCompetitionHelper _danceCompHelper;
        private readonly IMapper _mapper;

        static PdfDocumentHelper()
        {
#if CORE
            // Core build does not use Windows fonts, so set a FontResolver that handles the fonts our samples need.
            GlobalFontSettings.FontResolver = new SamplesFontResolver();
#endif
        }

        public PdfDocumentHelper(
            IDanceCompetitionHelper danceCompHelper,
            IMapper mapper)
        {
            _danceCompHelper = danceCompHelper
                ?? throw new ArgumentNullException(
                    nameof(danceCompHelper));
            _mapper = mapper
                ?? throw new ArgumentNullException(
                    nameof(mapper));
        }

        public Stream GetDummyPdf()
        {
            var memStream = new MemoryStream();
            var document = new Document
            {
                Info =
                {
                    Title = "Hello, MigraDoc",
                    Subject = "Demonstrates an excerpt of the capabilities of MigraDoc.",
                    Author = "Stefan Lange"
                },

            };

            document.LastSection.AddParagraph(
                "Test",
                StyleNames.Heading1);

            /*
            document.LastSection.PageSetup.PageFormat = PageFormat.A3;
            document.LastSection.PageSetup.Orientation = Orientation.Landscape;
            */

            var table = new Table
            {
                Borders =
                {
                    Width = 0.75,
                },
            };

            document.LastSection.PageSetup = document.DefaultPageSetup.Clone();
            var usePageSetup = document.LastSection.PageSetup;

            var tableWidth = usePageSetup.EffectivePageWidth - usePageSetup.LeftMargin - usePageSetup.RightMargin;

            var column = table.AddColumn(tableWidth * 0.33);
            column.Format.Alignment = ParagraphAlignment.Center;

            table.AddColumn(tableWidth * 0.66);

            var row = table.AddRow();
            row.Shading.Color = Colors.PaleGoldenrod;
            row.HeadingFormat = true;
            var cell = row.Cells[0];
            cell.AddParagraph("Itemus");
            cell = row.Cells[1];
            cell.AddParagraph("Descriptum");

            row = table.AddRow();
            cell = row.Cells[0];
            cell.AddParagraph("1");
            cell = row.Cells[1];
            cell.AddParagraph("FillerText.ShortText");

            row = table.AddRow();
            cell = row.Cells[0];
            cell.AddParagraph("2");
            cell = row.Cells[1];
            cell.AddParagraph("FillerText.Text");

            table.SetEdge(
                0,
                0,
                2,
                3,
                Edge.Box,
                BorderStyle.Single,
                1,
                Colors.Black);

            document.LastSection.Add(table);

            var pdfRenderer = new PdfDocumentRenderer
            {
                // Associate the MigraDoc document with a renderer.
                Document = document,
                PdfDocument =
                {
                    // Change some settings before rendering the MigraDoc document.
                    PageLayout = PdfPageLayout.SinglePage,
                    ViewerPreferences =
                    {
                        FitWindow = true,
                    }
                }
            };

            // Layout and render document to PDF.
            pdfRenderer.RenderDocument();

            pdfRenderer.PdfDocument.Save(
                memStream);

            return memStream;
        }
    }
}
