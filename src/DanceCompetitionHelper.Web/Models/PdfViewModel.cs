namespace DanceCompetitionHelper.Web.Models
{
    public class PdfViewModel : ViewModelBase
    {
        public string FileName { get; set; } = default!;
        public Stream PdtStream { get; set; } = default!;

    }
}
