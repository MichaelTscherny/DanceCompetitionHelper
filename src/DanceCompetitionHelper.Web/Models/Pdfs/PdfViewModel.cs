using MigraDoc.DocumentObjectModel;

using PdfSharp.Pdf;

namespace DanceCompetitionHelper.Web.Models.Pdfs
{
    public class PdfViewModel : ViewModelBase
    {
        public Guid? CompetitionId { get; set; }
        public Guid? CompetitionClassId { get; set; }

        public PdfPageLayout PdfPageLayout { get; set; } = PdfPageLayout.SinglePage;
        public PageFormat PageFormat { get; set; } = PageFormat.A4;
        public Orientation Orientation { get; set; } = Orientation.Portrait;
        public string FileName { get; set; } = default!;
        public Stream PdtStream { get; set; } = default!;

    }
}
