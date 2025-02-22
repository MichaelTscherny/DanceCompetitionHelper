namespace DanceCompetitionHelper.Web.Models.Pdfs
{
    public class PdfViewModelWrapper
    {
        public string AspController { get; set; } = default!;
        public string AspAction { get; set; } = default!;
        public string? FormTitle { get; set; }
        public string? SearchStringText { get; set; }
        public string ButtonText { get; set; } = default!;

        public bool ShowPageLayout { get; set; } = true;
        public bool ShowPageFormat { get; set; } = true;
        public bool ShowPageOrientation { get; set; } = true;
        public bool ShowSearchString { get; set; } = false;
        public bool ShowFromTo { get; set; } = false;
        public bool ShowShading { get; set; } = true;
        public bool ShowGroupForView { get; set; }

        public PdfViewModel PdfViewModel { get; set; } = default!;
    }
}
