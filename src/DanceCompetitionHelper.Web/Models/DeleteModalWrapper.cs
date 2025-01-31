namespace DanceCompetitionHelper.Web.Models
{
    public class DeleteModalWrapper
    {
        public string AspController { get; set; } = default!;
        public string AspAction { get; set; } = default!;
        public string RouteId { get; set; } = default!;

        public string? FormTitle { get; set; }
        public string InfoText { get; set; } = default!;
    }
}
