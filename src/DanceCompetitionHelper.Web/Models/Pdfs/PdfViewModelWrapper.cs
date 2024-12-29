namespace DanceCompetitionHelper.Web.Models.Pdfs
{
    public class PdfViewModelWrapper
    {
        public string AspController { get; set; } = default!;
        public string AspAction { get; set; } = default!;
        public string? FormTitle { get; set; }
        public string ButtonText { get; set; } = default!;

        public PdfViewModel PdfViewModel { get; set; } = default!;
    }
}
